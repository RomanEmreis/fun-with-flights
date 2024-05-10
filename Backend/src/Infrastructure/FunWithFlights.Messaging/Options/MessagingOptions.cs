namespace FunWithFlights.Messaging;

public sealed class MessagingOptions
{
    public string? Exchange { get; set; }
    public string? Queue { get; set; }
    public string[]? Subscriptions { get; set; }
}
