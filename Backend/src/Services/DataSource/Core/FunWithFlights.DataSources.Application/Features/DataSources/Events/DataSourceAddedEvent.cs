using FunWithFlights.DataSources.Domain.Entities;
using FunWithFlights.Messaging;

namespace FunWithFlights.DataSources.Application.Features.DataSources.Events;

public sealed class DataSourceAddedEvent(DateTime occurredAt, DataSource payload) : IIntegrationEvent<DataSource>
{
    public DateTime OccurredAt { get; init; } = occurredAt;
    public string Type { get; init; } = nameof(DataSourceAddedEvent);
    public string Version { get; init; } = "1.0.0";
    public DataSource Payload { get; init; } = payload;
}
