namespace FunWithFlights.Messaging;

public interface IEventPublisher : IDisposable
{
    Task PublishAsync<T>(IIntegrationEvent<T> integrationEvent);
}
