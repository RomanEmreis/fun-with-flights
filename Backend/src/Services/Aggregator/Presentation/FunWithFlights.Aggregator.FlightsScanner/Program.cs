using FunWithFlights.Aggregator.FlightsScanner;
using FunWithFlights.Aggregator.FlightsScanner.Options;
using FunWithFlights.Aggregator.Infrastructure;
using FunWithFlights.Aggregator.Infrastructure.Data;
using FunWithFlights.Messaging;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.AddRedisDistributedCache("aggregator-cache");
builder.AddNpgsqlDbContext<ApplicationContext>("flights-db");
builder.AddRabbitMQClient(
    "message-bus",
    configureConnectionFactory: options => options.DispatchConsumersAsync = true);
builder.Services.Configure<MessagingOptions>(
    builder.Configuration.GetSection(nameof(MessagingOptions)));

builder.Services.Configure<FlightScanningOptions>(
    builder.Configuration.GetSection(nameof(FlightScanningOptions)));

builder.Services.AddAggregator(builder.Configuration);

builder.Services.AddHostedService<Worker>();
builder.Services.AddHostedService<EventListener>();

var host = builder.Build();
host.Run();
