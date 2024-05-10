using FunWithFlights.Aggregator.Application.Data;
using FunWithFlights.Aggregator.Application.Data.Extensions;
using FunWithFlights.Aggregator.Application.Features.FlightRoutes.Events;
using FunWithFlights.Aggregator.Application.Features.FlightRoutes.Responses;
using FunWithFlights.Aggregator.Application.Services.FlightsProvider;
using FunWithFlights.Aggregator.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace FunWithFlights.Aggregator.Application.Features.FlightRoutes.EventHandlers;

internal sealed class DataSourcesAddedEventHandler(
    IFlightsProviderService flightsProviderService,
    IApplicationContext context,
    IDistributedCache cache,
    ILogger<DataSourceAddedEvent> logger) : INotificationHandler<DataSourceAddedEvent>
{
    public async Task Handle(DataSourceAddedEvent notification, CancellationToken cancellationToken)
    {
        var dataSource = notification.Payload;
        var flightRoutes = await flightsProviderService.GetFlightRoutesAsync(dataSource.Url!, cancellationToken);

        if (flightRoutes is null)
        {
            logger.LogWarning("Couldn't able to read flight routes of data source: {dataSourceName}", dataSource.Name);
            return;
        }

        await context.UseTransaction(async (
            IApplicationContext context,
            CancellationToken cancellationToken) => 
        {
            await context.Routes.ExecuteInsertAsync(
                flightRoutes
                    .Where(IsValidFlightRoute)
                    .Select(flightRoute => ConvertToDomain(dataSource.Name, flightRoute)),
                cancellationToken);
        }, 
        errorMessage: "An exception occurred while scanning flight routes",
        cancellationToken);

        await cache.RemoveAsync($"{CommonConstants.Cache.Namespace}:*", cancellationToken);
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
