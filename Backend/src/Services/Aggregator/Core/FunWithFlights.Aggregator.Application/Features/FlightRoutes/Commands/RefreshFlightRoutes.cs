﻿using FunWithFlights.Aggregator.Application.Data;
using FunWithFlights.Aggregator.Application.Data.Extensions;
using FunWithFlights.Aggregator.Application.Features.FlightRoutes.Responses;
using FunWithFlights.Aggregator.Application.Services.DataSources;
using FunWithFlights.Aggregator.Application.Services.FlightsProvider;
using FunWithFlights.Aggregator.Domain.Entities;
using FunWithFlights.Messaging;
using LinqToDB.EntityFrameworkCore;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace FunWithFlights.Aggregator.Application.Features.FlightRoutes.Commands;

public class RefreshFlightRoutesAsync() : IRequest;
public class RefreshFlightRoutes(DateTime occurredAt) : INotification, IIntegrationEvent
{
    public DateTime OccurredAt { get; init; } = occurredAt;
    public string Type { get; init; } = nameof(RefreshFlightRoutes);
    public string Version { get; init; } = "1.0.0";
}

internal sealed class RefreshFlightRoutesAsyncHandler(IEventPublisher publisher) : IRequestHandler<RefreshFlightRoutesAsync>
{
    public async Task Handle(RefreshFlightRoutesAsync request, CancellationToken cancellationToken) => 
        await publisher.PublishAsync(new RefreshFlightRoutes(DateTime.Now));
}

internal sealed class RefreshFlightRoutesHandler(
    IDataSourcesService dataSourcesService,
    IFlightsProviderService flightsProviderService,
    IApplicationContext context,
    ILogger<RefreshFlightRoutes> logger) : INotificationHandler<RefreshFlightRoutes>
{
    public async Task Handle(RefreshFlightRoutes request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Fetching data sources...");

        var dataSources = await dataSourcesService.GetAvailableDataSourcesAsync(cancellationToken);
        if (dataSources?.Results is null || !dataSources.Results.Any())
        {
            logger.LogWarning("Couldn't find any data sources");
            return;
        }

        logger.LogInformation("Found {Count} data sources.", dataSources.Results.Count());
        logger.LogInformation("Scanning routes...");
        
        var sw = Stopwatch.StartNew();

        var routesByDataSources = dataSources.Results
            .Where(dataSource => 
                !string.IsNullOrWhiteSpace(dataSource.Url) && 
                !string.IsNullOrWhiteSpace(dataSource.Name))
            .ToDictionary(
                dataSource => dataSource.Name!,
                dataSource => flightsProviderService.GetFlightRoutesAsync(dataSource.Url!, cancellationToken));

        // waiting once all flight routes will be loaded
        await Task.WhenAll(routesByDataSources.Values);

        await context.UseTransaction(async (
            IApplicationContext context,
            CancellationToken cancellationToken) => 
        {
            // removing all routes since they're expired
            await context.Routes.ExecuteDeleteAsync(cancellationToken);

            LinqToDBForEFTools.Initialize();

            foreach (var (dataSourceName, flightRoutesTask) in routesByDataSources)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var flightRoutes = await flightRoutesTask;
                if (flightRoutes is null)
                {
                    logger.LogWarning("Couldn't able to read flight routes of data source: {dataSourceName}", dataSourceName);
                    continue;
                }

                await context.Routes.ExecuteInsertAsync(
                    flightRoutes
                        .Where(IsValidFlightRoute)
                        .Select(flightRoute => ConvertToDomain(dataSourceName, flightRoute)),
                    cancellationToken);
            }
        }, 
        errorMessage: "An exception occurred while scanning flight routes",
        cancellationToken);

        logger.LogInformation("Scanning finished in {ElapsedMilliseconds}ms", sw.ElapsedMilliseconds);
    }

    private static bool IsValidFlightRoute(FlightRouteResponse flightRoute) =>
        !string.IsNullOrWhiteSpace(flightRoute.SourceAirport) &&
        !string.IsNullOrWhiteSpace(flightRoute.DestinationAirport) &&
        !string.IsNullOrWhiteSpace(flightRoute.Airline);

    private static FlightRoute ConvertToDomain(string dataSourceName, FlightRouteResponse flightRoute) => new(
        dataSourceName,
        flightRoute.Airline!,
        flightRoute.SourceAirport!,
        flightRoute.DestinationAirport!,
        flightRoute.CodeShare,
        flightRoute.Stops,
        flightRoute.Equipment);
}
