using FunWithFlights.Aggregator.Application.Data;
using FunWithFlights.Aggregator.Domain.Entities;
using LinqToDB.Data;
using LinqToDB.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FunWithFlights.Aggregator.Infrastructure.Data;

public class ApplicationContext(DbContextOptions<ApplicationContext> options, ILogger<ApplicationContext> logger) : DbContext(options), IApplicationContext
{
    public DbSet<FlightRoute> Routes { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);
    }

    public Task UseTransaction(
        Func<IApplicationContext, CancellationToken, Task> operation,
        string errorMessage,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(operation);

        var executionStrategy = Database.CreateExecutionStrategy();
        var context = this;
        return executionStrategy.ExecuteAsync(async (CancellationToken cancellationToken) => 
        {
            using var transaction = context.Database.BeginTransaction();

            try
            {
                await operation(context, cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, errorMessage);
                await transaction.RollbackAsync(cancellationToken);
            }
        },
        cancellationToken);
    }

    public new Task SaveChangesAsync(CancellationToken cancellationToken = default) => base.SaveChangesAsync(cancellationToken);
}
