namespace FunWithFlights.Aggregator.Application.Services.DataSources;

public interface IDataSourcesService
{
    Task<DataSourcesResponse?> GetAvailableDataSourcesAsync(CancellationToken cancellationToken = default);
}
