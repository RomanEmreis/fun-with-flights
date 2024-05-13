using FunWithFlights.Aggregator.Application.Data;
using FunWithFlights.Aggregator.Application.Features.FlightRoutes.Common;
using FunWithFlights.Aggregator.Application.Features.FlightRoutes.Responses;
using LinqToDB;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace FunWithFlights.Aggregator.Application.Features.FlightRoutes.Queries;

public record GetRoutes(int Start, int Limit = 10) : IRequest<FlightRoutesResponse>;

public sealed class GetRoutesHandler(IApplicationContext context, IDistributedCache cache) : IRequestHandler<GetRoutes, FlightRoutesResponse>
{
    private const int DefaultSlidingExpirationSeconds = 120;

    public async Task<FlightRoutesResponse> Handle(GetRoutes request, CancellationToken cancellationToken)
    {
        var cacheKey           = CreateCacheKey(request);
        var cachedFlightRoutes = await cache.GetAsync(cacheKey, cancellationToken);

        if (cachedFlightRoutes is null) 
        {
            var (start, limit) = request;
            var flightRoutes   = await context.Routes
                .Skip(start)
                .Take(limit)
                .Select(route => ResponseHelper.ConvertToResponse(route))
                .ToListAsync(cancellationToken);

            var response       = new FlightRoutesResponse(flightRoutes);
            var cachingOptions = CreateOptions();

            await cache.SetAsync(
                cacheKey,
                JsonSerializer.SerializeToUtf8Bytes(response),
                cachingOptions,
                cancellationToken);

            return response;
        }

        return JsonSerializer.Deserialize<FlightRoutesResponse>(cachedFlightRoutes) ?? new([]);
    }

    private static string CreateCacheKey(GetRoutes request) => CommonHelpers.Cache.CreateCacheKey($"{nameof(GetRoutes)}:s:{request.Start}:l:{request.Limit}");
    private static DistributedCacheEntryOptions CreateOptions() => new() { SlidingExpiration = TimeSpan.FromSeconds(DefaultSlidingExpirationSeconds) };
}
