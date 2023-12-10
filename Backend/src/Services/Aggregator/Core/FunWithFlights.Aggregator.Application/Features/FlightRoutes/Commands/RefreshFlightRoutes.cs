using FunWithFlights.Aggregator.Application.Data;
using FunWithFlights.Aggregator.Application.Features.FlightRoutes.Responses;
using FunWithFlights.Aggregator.Application.Services.DataSources;
using FunWithFlights.Aggregator.Application.Services.FlightsProvider;
using FunWithFlights.Aggregator.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;
using System.Diagnostics;

namespace FunWithFlights.Aggregator.Application.Features.FlightRoutes.Commands;

public class RefreshFlightRoutes() : INotification;

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
        if (dataSources?.Results is null)
        {
            logger.LogWarning("Couldn't find any data sources");
            return;
        }

        logger.LogInformation($"Found {dataSources.Results.Count()} data sources.");
        logger.LogInformation("Scanning routes...");
        
        var sw = Stopwatch.StartNew();

        var routesByDataSources = dataSources.Results
            .Where(dataSource => 
                !string.IsNullOrWhiteSpace(dataSource.Url) && 
                !string.IsNullOrWhiteSpace(dataSource.Name))
            .ToDictionary(
                dataSource => dataSource.Name!,
                dataSource => flightsProviderService.GetFlightRoutesAsync(dataSource.Url!, cancellationToken));

        // waiting once all flight routes will be ready
        await Task.WhenAll(routesByDataSources.Values);

        var executionStrategy = context.CreateExecutionStrategy();
        await executionStrategy.ExecuteAsync(async () => 
        {
            using var transaction = context.BeginTransaction();

            try
            {
                // removing all routes since they're expired
                await context.Routes.ExecuteDeleteAsync(cancellationToken);

                foreach (var (dataSourceName, flightRoutesTask) in routesByDataSources)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var flightRoutes = await flightRoutesTask;
                    if (flightRoutes is null)
                    {
                        logger.LogWarning($"Couldn't able to read flight routes of data source: {dataSourceName}");
                        continue;
                    }

                    context.Routes.AddRange(
                        flightRoutes
                            .Where(IsValidFlightRoute)
                            .Select(flightRoute => ConvertToDomain(dataSourceName, flightRoute)));

                    await context.SaveChangesAsync(cancellationToken);
                }

                await transaction.CommitAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                logger.LogError(ex, "An exception occurred while scanning flight routes");
            }
        });

        logger.LogInformation($"Scanning finished in {sw.ElapsedMilliseconds}ms");
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
