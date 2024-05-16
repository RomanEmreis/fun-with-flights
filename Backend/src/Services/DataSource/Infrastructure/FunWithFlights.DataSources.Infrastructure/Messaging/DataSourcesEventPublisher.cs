using FunWithFlights.Messaging;
using FunWithFlights.Messaging.RabbitMQ.Publisher;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace FunWithFlights.DataSources.Infrastructure.Messaging;

internal sealed class DataSourcesEventPublisher(
    IConnection connection, 
    ILogger<DataSourcesEventPublisher> logger,
    IOptions<MessagingOptions> options)
    : RabbitMqEventPublisher(connection, logger, options)
{
}
