using FunWithFlights.AppHost.Extensions;

var builder = DistributedApplication.CreateBuilder(args);

// DataSources
var dataSourcesDb = builder.AddPostgresContainer("data-sources").AddDatabase("datasourcesdb");
builder.AddProject<Projects.FunWithFlights_DataSources_DatabaseManager>("funwithflights.datasources.databasemanager")
    .WithReference(dataSourcesDb);

var dataSourcesApi = builder.AddProject<Projects.FunWithFlights_DataSources_API>("funwithflights.datasources.api")
    .WithReference(dataSourcesDb);


builder.AddNpmApp("frontend", "../../../../frontend/FunWithFlightsUI", "dev")
    .WithReference(dataSourcesApi)
    .WithServiceBinding(scheme: "http", hostPort: 5173);

builder.Build().Run();
