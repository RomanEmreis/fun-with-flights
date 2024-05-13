using FunWithFlights.Aggregator.Domain.Entities;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateSlimBuilder(args);

builder.AddServiceDefaults();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

var app = builder.Build();

app.MapGet("/flight-routes", () =>
{
    string sourceName = "FlightRouter";
    string[] equipment = ["Airbus", "Boeing", "Embraer", "SuperJet"];

    var flightRoutes = Enumerable.Range(0, 3000).Select(i => 
    {
        var sourceAirport = $"Airport {Random.Shared.Next(1, 100)}";
        var destinationAirport = $"Airport {Random.Shared.Next(1, 100)}";
        
        while (sourceAirport == destinationAirport)
        {
            destinationAirport = $"Airport {Random.Shared.Next(1, 100)}";
        }

        var flightRoute = new FlightRoute(
            sourceName,
            $"Airline {Random.Shared.Next(1, 150)}",
            sourceAirport,
            destinationAirport,
            $"Code {i}",
            0,
            equipment[Random.Shared.Next(0, equipment.Length - 1)]);

        flightRoute.Id = i;

        return flightRoute;
    }).ToArray();

    return flightRoutes;
})
.WithName("GetWeatherForecast");

app.Run();

[JsonSerializable(typeof(FlightRoute[]))]
internal partial class AppJsonSerializerContext : JsonSerializerContext {}
