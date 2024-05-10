using FunWithFlights.Aggregator.Application.Features.FlightRoutes.Events;
using FunWithFlights.Messaging;
using MediatR;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace FunWithFlights.Aggregator.FlightsScanner;

public class EventListener(
    IConnection connection,
    IServiceScopeFactory serviceScopeFactory,
    IOptions<MessagingOptions> options,
    ILogger<EventListener> logger) : BackgroundService
{
    private IModel? _channel;

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        _channel = connection.CreateModel();

        _channel.ExchangeDeclare(options.Value.Exchange, ExchangeType.Direct, durable: true);
        foreach (var subscription in options.Value.Subscriptions ?? [])
        {
            _channel.QueueDeclare(subscription, exclusive: false, durable: true);
            _channel.QueueBind(subscription, options.Value.Exchange, subscription, null);
        }

        await base.StartAsync(cancellationToken);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.Received += async (ch, ea) =>
        {
            try
            {
                // received message
                var @event = DeserializeEvent(ea);

                if (@event is not null)
                {
                    logger.LogInformation("Received event: {name}, version: {version}, payload: {payload}",
                        @event.Type,
                        @event.Version,
                        Encoding.UTF8.GetString(ea.Body.Span));

                    using var scope = serviceScopeFactory.CreateScope();
                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                    if (@event is INotification notification) await mediator.Publish(notification); // implement cancellation
                    _channel?.BasicAck(ea.DeliveryTag, false);

                    logger.LogInformation("Event successfully handled: {name}, version: {version}",
                        nameof(DataSourceAddedEvent),
                        @event.Version);
                }
                else
                {
                    logger.LogError("Unable to read the body of the event: {name}", nameof(DataSourceAddedEvent));
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An Exception occurred:");
            }
        };

        foreach (var subscription in options.Value.Subscriptions ?? []) 
        {
            _channel.BasicConsume(subscription, false, consumer);
        }

        return Task.CompletedTask;
    }

    private IIntegrationEvent? DeserializeEvent(BasicDeliverEventArgs eventArgs)
    {
        var eventType = GetAttribute(eventArgs.BasicProperties, "type");
        ArgumentException.ThrowIfNullOrWhiteSpace(eventType);

        var eventVersion = GetAttribute(eventArgs.BasicProperties, "version");
        ArgumentException.ThrowIfNullOrWhiteSpace(eventVersion);

        var version = Version.Parse(eventVersion);

        if (version.Major != 1) throw new NotSupportedException($"Unsupported event version: {eventVersion}");

        return eventType switch
        {
            nameof(DataSourceAddedEvent) => JsonSerializer.Deserialize<DataSourceAddedEvent>(eventArgs.Body.Span),
            _ => throw new NotSupportedException($"Unsupported event type: {eventType}")
        };
    }

    private static string GetAttribute(IBasicProperties properties, string header)
    {
        var attribute = properties.Headers[header];

        return attribute is byte[] attributeBytes
            ? Encoding.UTF8.GetString(attributeBytes)
            : throw new ArgumentNullException("Header: {headerName} is missing", header);
    }

    public override void Dispose()
    {
        _channel?.Close();
        _channel?.Dispose();

        base.Dispose();
    }
}
