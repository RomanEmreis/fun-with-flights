using FunWithFlights.Aggregator.Application.Data;
using FunWithFlights.Aggregator.Application.Features.FlightRoutes.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FunWithFlights.Aggregator.Application.Features.FlightRoutes.Queries;

public class GetAirports() : IRequest<AirportsResponse>;

internal sealed class GetAirportsHandler(IApplicationContext context) : IRequestHandler<GetAirports, AirportsResponse>
{
    public async Task<AirportsResponse> Handle(GetAirports request, CancellationToken cancellationToken)
    {
        var sourceAirports = await context.Routes
            .Select(route => new AirportResponse(route.SourceAirport))
            .Distinct()
            .ToListAsync(cancellationToken);

        var destinationAirports = await context.Routes
            .Select(route => new AirportResponse(route.DestinationAirport))
            .Distinct()
            .ToListAsync(cancellationToken);

        return new AirportsResponse(sourceAirports, destinationAirports);
    }
}
