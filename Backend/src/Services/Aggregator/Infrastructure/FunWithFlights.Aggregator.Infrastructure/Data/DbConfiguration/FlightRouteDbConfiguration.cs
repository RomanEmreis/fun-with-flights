using FunWithFlights.Aggregator.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FunWithFlights.Aggregator.Infrastructure.Data.DbConfiguration;

internal class FlightRouteDbConfiguration : IEntityTypeConfiguration<FlightRoute>
{
    public void Configure(EntityTypeBuilder<FlightRoute> builder)
    {
        builder.HasKey(route => route.Id);
        builder.HasIndex(route => new { route.SourceAirport, route.DestinationAirport }).IsUnique(false);
    }
}
