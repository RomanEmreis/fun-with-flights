using FunWithFlights.DataSources.Application.Data;
using FunWithFlights.DataSources.Application.Features.DataSources.Extensions;
using FunWithFlights.DataSources.Application.Features.DataSources.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace FunWithFlights.DataSources.Application.Features.DataSources.Queries;

public sealed class GetDataSources() : IRequest<DataSourcesResponse>;

internal sealed class GetDataSourcesHandler(IApplicationContext context, IDistributedCache cache) : IRequestHandler<GetDataSources, DataSourcesResponse>
{
    private const int DefaultSlidingExpirationSeconds = 60;

    public async Task<DataSourcesResponse> Handle(GetDataSources request, CancellationToken cancellationToken)
    {
        var cacheKey          = CreateCacheKey();
        var cachedDataSources = await cache.GetAsync(cacheKey, cancellationToken);

        if (cachedDataSources is null)
        {
            var dataSources = await context.DataSources
                .Select(dataSource => dataSource.ToResponse())
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

    private static DistributedCacheEntryOptions CreateOptions() => new()
    {
        SlidingExpiration = TimeSpan.FromSeconds(DefaultSlidingExpirationSeconds),
        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(DefaultSlidingExpirationSeconds * 3)
    };

    private static string CreateCacheKey() => CommonHelpers.Cache.CreateCacheKey($"{nameof(GetDataSources)}");
}
