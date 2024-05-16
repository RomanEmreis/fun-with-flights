using FunWithFlights.Aggregator.Application.Data;
using FunWithFlights.Aggregator.Application.Features.FlightRoutes.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace FunWithFlights.Aggregator.Application.Features.FlightRoutes.Queries;

public class GetAirports() : IRequest<AirportsResponse>;

internal sealed class GetAirportsHandler(IApplicationContext context, IDistributedCache cache) : IRequestHandler<GetAirports, AirportsResponse>
{
    private const int DefaultSlidingExpirationSeconds = 60;

    public async Task<AirportsResponse> Handle(GetAirports request, CancellationToken cancellationToken)
    {
        var cacheKey       = CreateCacheKey();
        var cachedAirports = await cache.GetAsync(cacheKey, cancellationToken);
        
        if (cachedAirports is null)
        {
            var sourceAirports = await context.Routes
                .Select(route => new AirportResponse(route.SourceAirport))
                .Distinct()
                .ToListAsync(cancellationToken);

            var destinationAirports = await context.Routes
                .Select(route => new AirportResponse(route.DestinationAirport))
                .Distinct()
                .ToListAsync(cancellationToken);

            var response = new AirportsResponse(sourceAirports, destinationAirports);

            if (sourceAirports.Count != 0 && destinationAirports.Count != 0)
            {
                var cachingOptions = CreateOptions();
                await cache.SetAsync(
                    cacheKey,
                    JsonSerializer.SerializeToUtf8Bytes(response),
                    cachingOptions,
                    cancellationToken);
            }

            return response;
        }

        return JsonSerializer.Deserialize<AirportsResponse>(cachedAirports) ?? new([], []);
    }

    private static string CreateCacheKey() => CommonHelpers.Cache.CreateCacheKey(nameof(GetAirports));
    private static DistributedCacheEntryOptions CreateOptions() => new()
    {
        SlidingExpiration = TimeSpan.FromSeconds(DefaultSlidingExpirationSeconds),
        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(DefaultSlidingExpirationSeconds * 3)
    };
}
