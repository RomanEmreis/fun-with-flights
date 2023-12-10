using FunWithFlights.Aggregator.Application.Features.FlightRoutes.Responses;
using FunWithFlights.Aggregator.Application.Services.FlightsProvider;
using System.Net.Http.Json;

namespace FunWithFlights.Aggregator.Infrastructure.Services.FlightsProvider;

internal class FlightsProviderService(HttpClient httpClient) : IFlightsProviderService
{
    public Task<FlightRouteResponse[]?> GetFlightRoutesAsync(string url, CancellationToken cancellationToken = default) =>
        httpClient.GetFromJsonAsync<FlightRouteResponse[]?>(url, cancellationToken);
}
