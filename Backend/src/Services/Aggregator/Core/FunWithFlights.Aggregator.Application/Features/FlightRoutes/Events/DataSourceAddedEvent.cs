using MediatR;
using FunWithFlights.Messaging;

namespace FunWithFlights.Aggregator.Application.Features.FlightRoutes.Events;

public record DataSourceAddedEvent(DataSource Payload, string Version, string Type, DateTime OccurredAt) : IIntegrationEvent, INotification;
public record DataSource(string Name, string? Description, string Url);
