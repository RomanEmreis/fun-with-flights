using FunWithFlights.AppHost.Extensions;

var builder = DistributedApplication.CreateBuilder(args);

// DataSources
var dataSourcesDb = builder.AddPostgresContainer("data-sources").AddDatabase("datasourcesdb");
builder.AddProject<Projects.FunWithFlights_DataSources_DatabaseManager>("datasources-databasemanager")
    .WithReference(dataSourcesDb);

var dataSourcesApi = builder.AddProject<Projects.FunWithFlights_DataSources_API>("datasources-api")
    .WithReference(dataSourcesDb);


builder.AddNpmApp("frontend", "../../../../frontend/FunWithFlightsUI")
    .WithReference(dataSourcesApi)
    .WithServiceBinding(scheme: "http");

builder.Build().Run();
