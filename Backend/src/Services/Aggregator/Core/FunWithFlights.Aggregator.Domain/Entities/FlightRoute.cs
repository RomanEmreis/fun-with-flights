namespace FunWithFlights.Aggregator.Domain.Entities
{
    public sealed class FlightRoute(
        string dataSourceName,
        string airline,
        string sourceAirport,
        string destinationAirport,
        string? codeShare,
        int stops,
        string? equipment)
    {
        public int Id { get; set; }

        public string DataSourceName { get; private set; } = dataSourceName;

        public string Airline { get; private set; } = airline;
        public string SourceAirport { get; private set; } = sourceAirport;
        public string DestinationAirport { get; private set; } = destinationAirport;
        public string? CodeShare { get; private set; } = codeShare;
        public int Stops { get; private set; } = stops;
        public string? Equipment { get; private set; } = equipment;
    }
}
