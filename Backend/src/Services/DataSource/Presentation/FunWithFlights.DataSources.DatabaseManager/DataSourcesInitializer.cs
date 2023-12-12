using FunWithFlights.DataSources.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace FunWithFlights.DataSources.DatabaseManager;

internal class DataSourcesInitializer(IServiceProvider serviceProvider, ILogger<DataSourcesInitializer> logger) : BackgroundService
{
    public const string ActivitySourceName = "Migrations";

    private readonly ActivitySource _activitySource = new(ActivitySourceName);

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

        await InitializeDatabaseAsync(dbContext, cancellationToken);
    }

    private async Task InitializeDatabaseAsync(ApplicationContext dbContext, CancellationToken cancellationToken)
    {
        using var activity = _activitySource.StartActivity("Initializing data sources database", ActivityKind.Client);

        var sw = Stopwatch.StartNew();

        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(dbContext.Database.MigrateAsync, cancellationToken);

        logger.LogInformation("Database initialization completed after {ElapsedMilliseconds}ms", sw.ElapsedMilliseconds);
    }

    public override void Dispose()
    {
        _activitySource.Dispose();
        base.Dispose();
    }
}
