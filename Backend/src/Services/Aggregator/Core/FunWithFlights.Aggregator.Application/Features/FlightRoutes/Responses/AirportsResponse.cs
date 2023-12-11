namespace FunWithFlights.Aggregator.Application.Features.FlightRoutes.Responses;

public sealed class AirportsResponse(IEnumerable<AirportResponse> source, IEnumerable<AirportResponse> destination)
{
    public IEnumerable<AirportResponse> AvailableSourceAirports { get; set; } = source;
    public IEnumerable<AirportResponse> AvailableDestinationAirports { get; set; } = destination;
}

public record AirportResponse(string Name);
