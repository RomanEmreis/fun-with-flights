namespace FunWithFlights.Messaging;

public sealed class MessagingOptions
{
    public string? Exchange { get; set; }
    public string? EventQueue { get; set; }
    public string[]? Subscriptions { get; set; }
}
