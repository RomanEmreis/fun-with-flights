var builder = DistributedApplication.CreateBuilder(args);

// DataSources
var dataSourcesDb = builder.AddPostgresContainer("data-sources").AddDatabase("datasourcesdb");
builder.AddProject<Projects.FunWithFlights_DataSources_API>("funwithflights.datasources.api")
    .WithReference(dataSourcesDb);

builder.AddProject<Projects.FunWithFlights_DataSources_DatabaseManager>("funwithflights.datasources.databasemanager")
    .WithReference(dataSourcesDb);

builder.Build().Run();
