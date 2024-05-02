using FunWithFlights.Aggregator.FlightsScanner;
using FunWithFlights.Aggregator.FlightsScanner.Options;
using FunWithFlights.Aggregator.Infrastructure;
using FunWithFlights.Aggregator.Infrastructure.Data;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.AddRedisDistributedCache("aggregator-cache");
builder.AddNpgsqlDbContext<ApplicationContext>("flights-db");

builder.Services.Configure<FlightScanningOptions>(
    builder.Configuration.GetSection(nameof(FlightScanningOptions)));

builder.Services.AddAggregator(builder.Configuration);

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
