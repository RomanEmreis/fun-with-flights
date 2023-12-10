using FunWithFlights.Aggregator.Application.Features.FlightRoutes.Responses;

namespace FunWithFlights.Aggregator.Application.Services.FlightsProvider;

public interface IFlightsProviderService
{
    Task<FlightRouteResponse[]?> GetFlightRoutesAsync(string url, CancellationToken cancellationToken = default);
}
