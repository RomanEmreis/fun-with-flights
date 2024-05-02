namespace FunWithFlights.Aggregator.Application;

internal static class CommonHelpers
{
    internal static class Cache
    {
        internal static string CreateCacheKey(string payload) => $"{CommonConstants.Cache.Namespace}:{payload}";
    }
}
