using FunWithFlights.Aggregator.Application.Data;
using FunWithFlights.Aggregator.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace FunWithFlights.Aggregator.Infrastructure.Data;

public class ApplicationContext(DbContextOptions<ApplicationContext> options) : DbContext(options), IApplicationContext
{
    public DbSet<FlightRoute> Routes { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);
    }

    public IDbContextTransaction BeginTransaction() => Database.BeginTransaction();
    public IExecutionStrategy CreateExecutionStrategy() => Database.CreateExecutionStrategy();

    public new Task SaveChangesAsync(CancellationToken cancellationToken = default) => base.SaveChangesAsync(cancellationToken);
}
