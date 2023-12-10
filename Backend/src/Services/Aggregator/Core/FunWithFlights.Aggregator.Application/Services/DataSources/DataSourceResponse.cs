namespace FunWithFlights.Aggregator.Application.Services.DataSources
{
    public sealed class DataSourcesResponse
    {
        public IEnumerable<DataSourceResponse>? Results { get; set; }
    }

    public sealed class DataSourceResponse
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Url { get; set; }
    }
}
