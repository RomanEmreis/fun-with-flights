using FunWithFlights.AppHost.Extensions;

var builder = DistributedApplication.CreateBuilder(args);

// DataSources
var dataSourcesDb = builder.AddPostgresContainer("data-sources").AddDatabase("datasourcesdb");
builder.AddProject<Projects.FunWithFlights_DataSources_DatabaseManager>("datasources-databasemanager")
    .WithReference(dataSourcesDb);

var dataSourcesApi = builder.AddProject<Projects.FunWithFlights_DataSources_API>("datasources-api")
    .WithReference(dataSourcesDb);

// Aggregator
var flightsDb = builder.AddPostgresContainer("aggregator").AddDatabase("flightsdb");
var aggregatorApi = builder.AddProject<Projects.FunWithFlights_Aggregator_API>("aggregator-api")
    .WithReference(flightsDb);

builder.AddProject<Projects.FunWIthFlights_Aggregator_DatabaseManager>("aggregator-databasemanager")
    .WithReference(flightsDb);

builder.AddProject<Projects.FunWithFlights_Aggregator_FlightsScanner>("flightsscanner")
    .WithReference(flightsDb)
    .WithReference(dataSourcesApi);

// Frontend
builder.AddNpmApp("frontend", "../../../../frontend/FunWithFlightsUI")
    .WithReference(dataSourcesApi)
    .WithReference(aggregatorApi)
    .WithServiceBinding(scheme: "http");

builder.Build().Run();
