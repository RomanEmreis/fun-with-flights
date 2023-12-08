namespace FunWithFlights.DataSources.Application.Features.DataSources.Responses;

public sealed class DataSourcesResponse(IEnumerable<DataSourceResponse> results)
{
    public IEnumerable<DataSourceResponse> Results { get; } = results;
}

public sealed class DataSourceResponse
{
    public int Id { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
    public string? Url { get; init; }
}
