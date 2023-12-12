using FunWithFlights.Aggregator.Application.Features.FlightRoutes.Responses;
using FunWithFlights.Aggregator.Application.Services.FlightsProvider;
using System.Net.Http.Json;

namespace FunWithFlights.Aggregator.Infrastructure.Services.FlightsProvider;

public class FlightsProviderService(HttpClient httpClient) : IFlightsProviderService
{
    public Task<FlightRouteResponse[]?> GetFlightRoutesAsync(string url, CancellationToken cancellationToken = default) =>
        string.IsNullOrWhiteSpace(url)
            ? throw new ArgumentException($"'{nameof(url)}' cannot be null or whitespace.", nameof(url))
            : httpClient.GetFromJsonAsync<FlightRouteResponse[]?>(url, cancellationToken);
}
