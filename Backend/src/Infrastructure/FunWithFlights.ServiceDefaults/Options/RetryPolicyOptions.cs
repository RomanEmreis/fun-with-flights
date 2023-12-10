namespace FunWithFlights.ServiceDefaults.Options;

public sealed class RetryPolicyOptions
{
    public int RetryCount { get; set; } = 5;
    public int MedianFirstRetryDelay { get; set; } = 1; // seconds
}
