using FunWithFlights.Messaging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text.Json;

namespace FunWithFlights.DataSources.Infrastructure.Messaging;

internal sealed class DataSourcesEventPublisher : IEventPublisher
{
    private readonly IModel _channel;
    private readonly MessagingOptions _messagingOptions;
    private readonly ILogger<DataSourcesEventPublisher> _logger;

    public DataSourcesEventPublisher(IConnection connection, ILogger<DataSourcesEventPublisher> logger, IOptions<MessagingOptions> options)
    {
        _channel = connection.CreateModel();
        _logger = logger;
        _messagingOptions = options.Value;

        _channel.ExchangeDeclare(_messagingOptions.Exchange, ExchangeType.Direct, durable: true);
        //_channel.QueueDeclare(_messagingOptions.Queue, exclusive: false, durable: true);
        //_channel.QueueBind(_messagingOptions.Queue, _messagingOptions.Exchange, _messagingOptions.Queue, null);
    }

    public Task PublishAsync<T>(IIntegrationEvent<T> @event)
    {
        var properties = _channel.CreateBasicProperties();

        properties.Headers = new Dictionary<string, object> 
        {
            ["type"]    = @event.Type,
            ["version"] = @event.Version
        };

        _channel.BasicPublish(
            _messagingOptions.Exchange,
            _messagingOptions.Queue,
            properties,
            body: JsonSerializer.SerializeToUtf8Bytes(@event));

        _logger.LogInformation(
            "Event: {name}, version: {version} has been successfully published",
            @event.Type,
            @event.Version);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _channel.Close();
        _channel.Dispose();
    }
}
