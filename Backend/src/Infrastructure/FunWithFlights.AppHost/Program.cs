var builder = DistributedApplication.CreateBuilder(args);

// FlightRouter v1 - mock service that provides flight routes
builder.AddProject<Projects.FunWithFlights_Aggregator_FlightRouter>("flightrouter");

var messageBus = builder.AddRabbitMQ("message-bus")
    .WithManagementPlugin();

// DataSources
var dataSourcesRedis = builder
    .AddRedis("datasources-cache")
    .WithRedisCommander();

var dataSourcesDb = builder
    .AddPostgres("data-sources")
    .WithPgAdmin()
    .AddDatabase("datasources-db");

builder.AddProject<Projects.FunWithFlights_DataSources_DatabaseManager>("datasources-databasemanager")
    .WithReference(dataSourcesDb);

var dataSourcesApi = builder.AddProject<Projects.FunWithFlights_DataSources_API>("datasources-api")
    .WithReference(dataSourcesDb)
    .WithReference(dataSourcesRedis)
    .WithReference(messageBus);

// Aggregator
var aggregatorRedis = builder
    .AddRedis("aggregator-cache")
    .WithRedisCommander();

var flightsDb = builder
    .AddPostgres("aggregator")
    .WithPgAdmin()
    .AddDatabase("flights-db");

var aggregatorApi = builder.AddProject<Projects.FunWithFlights_Aggregator_API>("aggregator-api")
    .WithReference(flightsDb)
    .WithReference(aggregatorRedis)
    .WithReference(messageBus);

builder.AddProject<Projects.FunWIthFlights_Aggregator_DatabaseManager>("aggregator-databasemanager")
    .WithReference(flightsDb);

builder.AddProject<Projects.FunWithFlights_Aggregator_FlightsScanner>("flightsscanner")
    .WithReference(flightsDb)
    .WithReference(dataSourcesApi)
    .WithReference(aggregatorRedis)
    .WithReference(messageBus);

// Frontend
builder.AddNpmApp("frontend", "../../../../frontend/FunWithFlightsUI")
    .WithReference(dataSourcesApi)
    .WithReference(aggregatorApi)
    .WithHttpsEndpoint(env: "PORT");

builder.Build().Run();
