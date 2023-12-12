using FunWithFlights.Aggregator.Application.Data;
using FunWithFlights.Aggregator.Application.Features.FlightRoutes.Common;
using FunWithFlights.Aggregator.Application.Features.FlightRoutes.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FunWithFlights.Aggregator.Application.Features.FlightRoutes.Queries;

public record FindRoutes(
    string SourceAirport,
    string DestinationAirport,
    DateOnly DateOfFlight,
    DateOnly? DateOfReturn) : IRequest<FlightRoutesResponse>;

internal sealed class FindRouteHandler(IApplicationContext context) : IRequestHandler<FindRoutes, FlightRoutesResponse>
{
    public async Task<FlightRoutesResponse> Handle(FindRoutes request, CancellationToken cancellationToken)
    {
        var (source, destination, _, _) = request; // ignore DateOfFlight and DateOfReturn for now

        var flightRoutes = await context.Routes
            .Where(route => 
                route.SourceAirport == source &&
                route.DestinationAirport == destination)
            .Select(route => ResponseHelper.ConvertToResponse(route))
            .ToListAsync(cancellationToken);

        return new FlightRoutesResponse(flightRoutes);
    }
}
