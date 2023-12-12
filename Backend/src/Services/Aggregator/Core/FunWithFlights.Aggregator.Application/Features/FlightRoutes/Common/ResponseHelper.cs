using FunWithFlights.Aggregator.Application.Features.FlightRoutes.Responses;
using FunWithFlights.Aggregator.Domain.Entities;

namespace FunWithFlights.Aggregator.Application.Features.FlightRoutes.Common;

internal static class ResponseHelper
{
    internal static FlightRouteResponse ConvertToResponse(FlightRoute route) => new()
    {
        SourceAirport = route.SourceAirport,
        DestinationAirport = route.DestinationAirport,
        Airline = route.Airline,
        CodeShare = route.CodeShare,
        Equipment = route.Equipment,
        Stops = route.Stops,
    };
}
