using FunWithFlights.Aggregator.Application.Data;
using FunWithFlights.Aggregator.Application.Features.FlightRoutes.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FunWithFlights.Aggregator.Application.Features.FlightRoutes.Queries;

public record FindRoutes(
    string SourceAirport,
    string DestinationAirport,
    DateOnly DateOfFlight,
    DateOnly DateOfReturn) : IRequest<FlightRoutesResponse>;

internal sealed class FindRouteHandler(IApplicationContext context) : IRequestHandler<FindRoutes, FlightRoutesResponse>
{
    public async Task<FlightRoutesResponse> Handle(FindRoutes request, CancellationToken cancellationToken)
    {
        var (source, destination, _, _) = request;

        var flightRoutes = await context.Routes
            .Where(route => 
                route.SourceAirport == source &&
                route.DestinationAirport == destination)
            .Select(route => new FlightRouteResponse
            {
                SourceAirport = route.SourceAirport,
                DestinationAirport = route.DestinationAirport,
                Airline = route.Airline,
                CodeShare = route.CodeShare,
                Equipment = route.Equipment,
                Stops = route.Stops,
            })
            .ToListAsync(cancellationToken);

        return new FlightRoutesResponse(flightRoutes);
    }
}
