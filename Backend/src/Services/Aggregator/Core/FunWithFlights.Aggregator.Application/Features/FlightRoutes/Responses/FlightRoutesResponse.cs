namespace FunWithFlights.Aggregator.Application.Features.FlightRoutes.Responses;

public sealed class FlightRoutesResponse(IEnumerable<FlightRouteResponse> results)
{
    public IEnumerable<FlightRouteResponse> Results { get; set; } = results;
}

public sealed class FlightRouteResponse
{
    public string? Airline { get; set; }
    public string? SourceAirport { get; set; }
    public string? DestinationAirport { get; set; }
    public string? CodeShare { get; set; }
    public int Stops { get; set; }
    public string? Equipment { get; set; }
}
