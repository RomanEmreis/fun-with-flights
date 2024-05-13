using FunWithFlights.Aggregator.Application.Data;
using FunWithFlights.Aggregator.Application.Features.FlightRoutes.Common;
using FunWithFlights.Aggregator.Application.Features.FlightRoutes.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace FunWithFlights.Aggregator.Application.Features.FlightRoutes.Queries;

public record FindRoutes(
    string SourceAirport,
    string DestinationAirport,
    DateOnly DateOfFlight,
    DateOnly? DateOfReturn) : IRequest<FlightRoutesResponse>;

internal sealed class FindRouteHandler(IApplicationContext context, IDistributedCache cache) : IRequestHandler<FindRoutes, FlightRoutesResponse>
{
    private const int DefaultSlidingExpirationSeconds = 120;

    public async Task<FlightRoutesResponse> Handle(FindRoutes request, CancellationToken cancellationToken)
    {
        var cacheKey            = CreateCacheKey(request);
        var  cachedFlightRoutes = await cache.GetAsync(cacheKey, cancellationToken);

        if (cachedFlightRoutes is null)
        {
            var (source, destination, _, _) = request; // ignore DateOfFlight and DateOfReturn for now
            var flightRoutes                = await context.Routes
                .Where(route =>
                    route.SourceAirport == source &&
                    route.DestinationAirport == destination)
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

    private static DistributedCacheEntryOptions CreateOptions() => new() { SlidingExpiration = TimeSpan.FromSeconds(DefaultSlidingExpirationSeconds) };
    private static string CreateCacheKey(FindRoutes request) =>
        CommonHelpers.Cache.CreateCacheKey(
            $"{nameof(FindRoutes)}:sa:{request.SourceAirport}:da:{request.DestinationAirport}:dof:{request.DateOfFlight:yyyy-MM-dd}:dor:{request.DateOfReturn:yyyy-MM-dd}");
}
