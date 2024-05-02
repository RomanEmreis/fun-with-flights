using FunWithFlights.Aggregator.Domain.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

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
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();
