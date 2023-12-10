using FunWithFlights.Aggregator.Infrastructure.Data;
using FunWIthFlights.Aggregator.DatabaseManager;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddNpgsqlDbContext<ApplicationContext>("flightsdb", null,
    optionsBuilder => optionsBuilder.UseNpgsql(npgsqlBuilder =>
        npgsqlBuilder.MigrationsAssembly(typeof(Program).Assembly.GetName().Name)));

// Add services to the container.
builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing.AddSource(FlightsDatabaseInitializer.ActivitySourceName));

builder.Services.AddSingleton<FlightsDatabaseInitializer>();
builder.Services.AddHostedService(sp => sp.GetRequiredService<FlightsDatabaseInitializer>());
builder.Services.AddHealthChecks()
    .AddCheck<FlightsDatabaseHealthCheck>("DbInitializer", null);

var app = builder.Build();

app.MapDefaultEndpoints();

app.UseHttpsRedirection();

app.Run();
