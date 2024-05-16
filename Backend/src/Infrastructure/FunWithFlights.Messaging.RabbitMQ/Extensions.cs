using RabbitMQ.Client;
using System.Text;

namespace FunWithFlights.Messaging.RabbitMQ;

public static class Extensions
{
    public static IBasicProperties CreateEventAttributes<T>(this IModel channel, IIntegrationEvent<T> integrationEvent)
    {
        var properties = channel.CreateBasicProperties();

        properties.Headers = new Dictionary<string, object>
        {
            ["type"] = integrationEvent.Type,
            ["version"] = integrationEvent.Version
        };

        return properties;
    }

    public static string GetAttribute(this IBasicProperties properties, string header)
    {
        var attribute = properties.Headers[header];

        return attribute is byte[] attributeBytes
            ? Encoding.UTF8.GetString(attributeBytes)
            : throw new ArgumentNullException("Header: {headerName} is missing", header);
    }
}
