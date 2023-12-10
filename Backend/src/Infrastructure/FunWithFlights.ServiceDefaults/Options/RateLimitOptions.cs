namespace FunWithFlights.ServiceDefaults.Options;

public sealed class RateLimitOptions
{
    public int PermitLimit { get; set; } = 5; // 5 requests per window
    public int Window { get; set; } = 15; // 15 seconds window
    public int QueueLimit { get; set; } = 2;
}
