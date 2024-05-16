using FunWithFlights.Aggregator.Application.Features.FlightRoutes.Events;
using FunWithFlights.Messaging;
using FunWithFlights.Messaging.RabbitMQ;
using MediatR;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text.Json;

namespace FunWithFlights.Aggregator.FlightsScanner;

public sealed class EventListener(
    IServiceScopeFactory serviceScopeFactory,
    IConnection connection,
    IOptions<MessagingOptions> options,
    ILogger<EventListener> logger) : RabbitMqEventListener(connection, logger, options)
{
    public override async Task StartAsync(CancellationToken cancellationToken) => await base.StartAsync(cancellationToken);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        await StartListen(stoppingToken);
    }

    protected override async Task HandleEventAsync(IIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
    {
        using var scope = serviceScopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        if (integrationEvent is INotification notification) await mediator.Publish(notification, cancellationToken);
    }

    protected override IIntegrationEvent? DeserializeEvent(BasicDeliverEventArgs eventArgs)
    {
        var (type, version) = GetEventAttributes(eventArgs.BasicProperties, supportedVersion: 1);
        return type switch
        {
            nameof(DataSourceAddedEvent) => JsonSerializer.Deserialize<DataSourceAddedEvent>(eventArgs.Body.Span),
            _ => throw new NotSupportedException($"Unsupported event type: {type}")
        };
    }

    public override void Dispose() => base.Dispose();
}
