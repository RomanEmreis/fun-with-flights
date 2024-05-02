using FunWithFlights.DataSources.Application.Data;
using FunWithFlights.DataSources.Application.Features.DataSources.Responses;
using FunWithFlights.DataSources.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace FunWithFlights.DataSources.Application.Features.DataSources.Queries;

public sealed class GetDataSources() : IRequest<DataSourcesResponse>;

internal sealed class GetDataSourcesHandler(IApplicationContext context, IDistributedCache cache) : IRequestHandler<GetDataSources, DataSourcesResponse>
{
    private const int DefaultSlidingExpirationHours = 2;

    public async Task<DataSourcesResponse> Handle(GetDataSources request, CancellationToken cancellationToken)
    {
        var cacheKey          = CreateCacheKey();
        var cachedDataSources = await cache.GetAsync(cacheKey, cancellationToken);

        if (cachedDataSources is null)
        {
            var dataSources = await context.DataSources
                .Select(dataSource => ConvertToResponse(dataSource))
                .ToListAsync(cancellationToken);

            var response = new DataSourcesResponse(dataSources);
            var cachingOptions = CreateOptions();

            await cache.SetAsync(
                cacheKey,
                JsonSerializer.SerializeToUtf8Bytes(response),
                cachingOptions,
                cancellationToken);

            return response;
        }

        return JsonSerializer.Deserialize<DataSourcesResponse>(cachedDataSources) ?? new([]);
    }

    private static DataSourceResponse ConvertToResponse(DataSource dataSource) => new()
    {
        Id = dataSource.Id,
        Name = dataSource.Name,
        Description = dataSource.Description,
        Url = dataSource.Url
    };

    private static DistributedCacheEntryOptions CreateOptions() => new() { SlidingExpiration = TimeSpan.FromHours(DefaultSlidingExpirationHours) };
    private static string CreateCacheKey() => CommonHelpers.Cache.CreateCacheKey($"{nameof(GetDataSources)}");
}
