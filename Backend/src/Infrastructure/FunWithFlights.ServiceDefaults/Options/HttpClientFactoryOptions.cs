namespace FunWithFlights.ServiceDefaults.Options;

public sealed class HttpClientFactoryOptions
{
    public string? BaseUrl { get; set; }
    public int DefaultHttpMessageHandlerLifeTime { get; set; } = 5; //minutes
}
