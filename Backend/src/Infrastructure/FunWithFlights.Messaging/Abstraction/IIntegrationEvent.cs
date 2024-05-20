namespace FunWithFlights.Messaging;

public interface IIntegrationEvent
{
    DateTime OccurredAt { get; init; }
    string Type { get; init; }
    string Version { get; init; }
}

public interface IIntegrationEvent<T> : IIntegrationEvent
{
    T Payload { get; init; }
}
