using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace FunWithFlights.Messaging.RabbitMQ;

public abstract class RabbitMqEventListener(IConnection connection, ILogger logger, IOptions<MessagingOptions> options) : BackgroundService
{
    private readonly MessagingOptions _messagingOptions = options.Value;
    private readonly IConnection _connection = connection;
    private readonly ILogger _logger = logger;

    private readonly IModel _channel = connection.CreateModel();
    private AsyncEventingBasicConsumer? _consumer;

    protected virtual ValueTask StartListen(CancellationToken cancellationToken = default)
    {
        try
        {
            _channel.ExchangeDeclare(_messagingOptions.Exchange, ExchangeType.Direct, durable: true);
            _channel.QueueDeclare(_messagingOptions.EventQueue, exclusive: false, durable: true);

            foreach (var subscription in _messagingOptions.Subscriptions ?? [])
            {
                _channel.QueueBind(_messagingOptions.EventQueue, _messagingOptions.Exchange, subscription, null);
            }

            _consumer = new AsyncEventingBasicConsumer(_channel);
            _consumer.Received += OnReceived;
            _channel.BasicConsume(_messagingOptions.EventQueue, false, _consumer);

            cancellationToken.Register(OnCancel);
        }
        catch (TaskCanceledException)
        {
            _logger.LogWarning("Operation has been canceled");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An exception occurred:");
        }

        return ValueTask.CompletedTask;
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        if (_consumer is not null) _consumer.Received -= OnReceived;
        
        _channel?.Close();
        return base.StopAsync(cancellationToken);
    }

    private void OnCancel()
    {
        if (_consumer is null) return;

        foreach (var consumerTag in _consumer.ConsumerTags)
        {
            _channel.BasicCancel(consumerTag);
        }
    }

    private async Task OnReceived(object sender, BasicDeliverEventArgs eventArgs)
    {
        try
        {
            // received message
            var @event = DeserializeEvent(eventArgs);
            if (@event is not null)
            {
                _logger.LogInformation("Received event: {name}, version: {version}, payload: {payload}",
                    @event.Type,
                    @event.Version,
                    Encoding.UTF8.GetString(eventArgs.Body.Span));

                await HandleEventAsync(@event);

                _channel?.BasicAck(eventArgs.DeliveryTag, false);

                _logger.LogInformation("Event successfully handled: {name}, version: {version}",
                    @event.Type,
                    @event.Version);
            }
            else
            {
                _logger.LogError("Unable to read the body of the event");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An Exception occurred:");
        }
    }

    protected abstract Task HandleEventAsync(IIntegrationEvent integrationEvent, CancellationToken cancellationToken = default);
    protected abstract IIntegrationEvent? DeserializeEvent(BasicDeliverEventArgs eventArgs);

    protected (string EventType, string EventVersion) GetEventAttributes(IBasicProperties basicProperties, int supportedVersion)
    {
        var eventType = basicProperties.GetAttribute("type");
        ArgumentException.ThrowIfNullOrWhiteSpace(eventType);

        var eventVersion = basicProperties.GetAttribute("version");
        ArgumentException.ThrowIfNullOrWhiteSpace(eventVersion);

        var version = Version.Parse(eventVersion);
        if (version.Major != supportedVersion) throw new NotSupportedException($"Unsupported event version: {eventVersion}");

        return (eventType, eventVersion);
    }

    public override void Dispose()
    {
        _consumer = null;
        _channel.Dispose();

        base.Dispose();
    }
}
