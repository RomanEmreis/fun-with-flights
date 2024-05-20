namespace FunWithFlights.Messaging;

public interface IEventPublisher : IDisposable
{
    ValueTask PublishAsync(IIntegrationEvent integrationEvent);
    ValueTask PublishAsync<T>(IIntegrationEvent<T> integrationEvent);
}
