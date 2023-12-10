using FunWithFlights.Aggregator.Application.Services.DataSources;
using System.Net.Http.Json;

namespace FunWithFlights.Aggregator.Infrastructure.Services.DataSources;

internal sealed class DataSourcesService(HttpClient httpClient) : IDataSourcesService
{
    public Task<DataSourcesResponse?> GetAvailableDataSourcesAsync(CancellationToken cancellationToken = default) => 
        httpClient.GetFromJsonAsync<DataSourcesResponse?>($"api/data-sources/all", cancellationToken);
}
