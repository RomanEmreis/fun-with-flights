using FunWithFlights.Aggregator.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace FunWithFlights.Aggregator.Application.Data;

public interface IApplicationContext
{
    DbSet<FlightRoute> Routes { get; }

    public Task UseTransaction(
        Func<IApplicationContext, CancellationToken, Task> operation,
        string errorMessage,
        CancellationToken cancellationToken = default);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
