namespace FunWithFlights.Messaging;

public interface IEventPublisher : IDisposable
{
    ValueTask PublishAsync<T>(IIntegrationEvent<T> integrationEvent);
}
