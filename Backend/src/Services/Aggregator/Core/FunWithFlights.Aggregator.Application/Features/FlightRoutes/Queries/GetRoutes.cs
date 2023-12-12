using FunWithFlights.Aggregator.Application.Data;
using FunWithFlights.Aggregator.Application.Features.FlightRoutes.Common;
using FunWithFlights.Aggregator.Application.Features.FlightRoutes.Responses;
using LinqToDB;
using MediatR;

namespace FunWithFlights.Aggregator.Application.Features.FlightRoutes.Queries;

public record GetRoutes(int Start, int Limit = 10) : IRequest<FlightRoutesResponse>;

public sealed class GetRoutesHandler(IApplicationContext context) : IRequestHandler<GetRoutes, FlightRoutesResponse>
{
    public async Task<FlightRoutesResponse> Handle(GetRoutes request, CancellationToken cancellationToken)
    {
        var (start, limit) = request;
        
        var flightRoutes = await context.Routes
            .Skip(start)
            .Take(limit)
            .Select(route => ResponseHelper.ConvertToResponse(route))
            .ToListAsync(cancellationToken);

        return new FlightRoutesResponse(flightRoutes);
    }
}
