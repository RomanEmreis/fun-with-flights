using FunWithFlights.DataSources.Application.Data;
using FunWithFlights.DataSources.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FunWithFlights.DataSources.Infrastructure.Data;

public class ApplicationContext(DbContextOptions<ApplicationContext> options) : DbContext(options), IApplicationContext
{
    public DbSet<DataSource> DataSources { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default) => base.SaveChangesAsync(cancellationToken);
}
