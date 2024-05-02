var builder = DistributedApplication.CreateBuilder(args);

// FlightRouter - mock service that provides flight routes
builder.AddProject<Projects.FunWithFlights_Aggregator_FlightRouter>("flightrouter");

// DataSources
var dataSourcesRedis = builder.AddRedis("datasources-cache");
var dataSourcesDb = builder.AddPostgres("data-sources").AddDatabase("datasources-db");
builder.AddProject<Projects.FunWithFlights_DataSources_DatabaseManager>("datasources-databasemanager")
    .WithReference(dataSourcesDb);

var dataSourcesApi = builder.AddProject<Projects.FunWithFlights_DataSources_API>("datasources-api")
    .WithReference(dataSourcesDb)
    .WithReference(dataSourcesRedis);

// Aggregator
var aggregatorRedis = builder.AddRedis("aggregator-cache");
var flightsDb = builder.AddPostgres("aggregator").AddDatabase("flights-db");
var aggregatorApi = builder.AddProject<Projects.FunWithFlights_Aggregator_API>("aggregator-api")
    .WithReference(flightsDb)
    .WithReference(aggregatorRedis);

builder.AddProject<Projects.FunWIthFlights_Aggregator_DatabaseManager>("aggregator-databasemanager")
    .WithReference(flightsDb);

builder.AddProject<Projects.FunWithFlights_Aggregator_FlightsScanner>("flightsscanner")
    .WithReference(flightsDb)
    .WithReference(dataSourcesApi)
    .WithReference(aggregatorRedis);

// Frontend
builder.AddNpmApp("frontend", "../../../../frontend/FunWithFlightsUI")
    .WithReference(dataSourcesApi)
    .WithReference(aggregatorApi)
    .WithHttpsEndpoint(env: "PORT");


builder.Build().Run();
