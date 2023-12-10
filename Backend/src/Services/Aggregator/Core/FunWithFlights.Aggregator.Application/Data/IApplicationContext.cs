using FunWithFlights.Aggregator.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace FunWithFlights.Aggregator.Application.Data;

public interface IApplicationContext
{
    DbSet<FlightRoute> Routes { get; }

    IDbContextTransaction BeginTransaction();
    IExecutionStrategy CreateExecutionStrategy();
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
