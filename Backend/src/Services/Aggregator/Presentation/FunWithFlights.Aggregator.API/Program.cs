using FunWithFlights.Aggregator.Infrastructure;
using FunWithFlights.Aggregator.Infrastructure.Data;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.AddNpgsqlDbContext<ApplicationContext>("flightsdb");

builder.Services.AddCors();
builder.Services.AddControllers();
builder.Services.AddApiVersioning(headerName: "X-Version");
builder.Services.AddRateLimiting(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddProblemDetails();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Aggregator API",
        Description = "Web API provides the information about flight routes, airports and airlines",
        TermsOfService = new Uri("https://example.com/terms")
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.AddAggregator(builder.Configuration);

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // allow any origin
    .AllowCredentials()); // allow credentials

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseRateLimiter();

app.MapControllers().RequireRateLimiting("fixed");

app.Run();
