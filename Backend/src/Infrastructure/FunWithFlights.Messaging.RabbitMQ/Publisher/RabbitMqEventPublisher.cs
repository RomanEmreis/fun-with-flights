using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text.Json;

namespace FunWithFlights.Messaging.RabbitMQ.Publisher;

public abstract class RabbitMqEventPublisher : IEventPublisher
{
    private readonly IModel _channel;
    private readonly ILogger _logger;
    private readonly MessagingOptions _messagingOptions;

    protected RabbitMqEventPublisher(IConnection connection, ILogger logger, IOptions<MessagingOptions> options)
    {
        _channel = connection.CreateModel();
        _logger = logger;
        _messagingOptions = options.Value;

        _channel.ExchangeDeclare(_messagingOptions.Exchange, ExchangeType.Direct, durable: true);
    }

    public abstract string RoutingKeyPrefix { get; }

    public ValueTask PublishAsync(IIntegrationEvent integrationEvent)
    {
        _logger.LogInformation(
            "Publishing: {name}, version: {version}",
            integrationEvent.Type,
            integrationEvent.Version);

        var properties = _channel.CreateEventAttributes(integrationEvent);
        var body = JsonSerializer.SerializeToUtf8Bytes(integrationEvent);

        _channel.BasicPublish(
            exchange: _messagingOptions.Exchange,
            routingKey: $"{RoutingKeyPrefix}:{integrationEvent.Type}",
            properties,
            body);

        _logger.LogInformation(
            "Event: {name}, version: {version} has been successfully published",
            integrationEvent.Type,
            integrationEvent.Version);

        return ValueTask.CompletedTask;
    }

    public ValueTask PublishAsync<T>(IIntegrationEvent<T> integrationEvent)
    {
        _logger.LogInformation(
            "Publishing: {name}, version: {version}",
            integrationEvent.Type,
            integrationEvent.Version);

        var properties = _channel.CreateEventAttributes(integrationEvent);
        var body = JsonSerializer.SerializeToUtf8Bytes(integrationEvent);

        _channel.BasicPublish(
            exchange: _messagingOptions.Exchange,
            routingKey: $"{RoutingKeyPrefix}:{integrationEvent.Type}",
            properties,
            body);

        _logger.LogInformation(
            "Event: {name}, version: {version} has been successfully published",
            integrationEvent.Type,
            integrationEvent.Version);

        return ValueTask.CompletedTask;
    }

    public void Dispose()
    {
        _channel.Close();
        _channel.Dispose();
    }
}
