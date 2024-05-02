using FunWithFlights.DataSources.DatabaseManager;
using FunWithFlights.DataSources.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddNpgsqlDbContext<ApplicationContext>("datasources-db", null,
    optionsBuilder => optionsBuilder.UseNpgsql(npgsqlBuilder =>
        npgsqlBuilder.MigrationsAssembly(typeof(Program).Assembly.GetName().Name)));

// Add services to the container.
builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing.AddSource(DataSourcesInitializer.ActivitySourceName));

builder.Services.AddSingleton<DataSourcesInitializer>();
builder.Services.AddHostedService(sp => sp.GetRequiredService<DataSourcesInitializer>());
builder.Services.AddHealthChecks()
    .AddCheck<DataSourcesDatabaseHealthCheck>("DbInitializer", null);

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();


app.Run();
