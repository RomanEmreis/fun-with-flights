using FunWithFlights.Aggregator.Application;
using FunWithFlights.Messaging.RabbitMQ.Publisher;
using FunWithFlights.Messaging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace FunWithFlights.Aggregator.Infrastructure.Messaging;

internal sealed class AggregatorEventPublisher(
    IConnection connection,
    ILogger<AggregatorEventPublisher> logger,
    IOptions<MessagingOptions> options)
    : RabbitMqEventPublisher(connection, logger, options)
{
    public override string RoutingKeyPrefix => CommonConstants.MessageRouting.Namespace;
}
