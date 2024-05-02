using FunWithFlights.DataSources.Infrastructure;
using FunWithFlights.DataSources.Infrastructure.Data;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace FunWithFlights.DataSources.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.AddServiceDefaults();

        // Add services to the container.
        builder.AddNpgsqlDbContext<ApplicationContext>("datasources-db");

        builder.AddRedisDistributedCache("datasources-cache");

        builder.Services.AddCors();
        builder.Services.AddControllers();
        builder.Services.AddApiVersioning(headerName: "X-Version");
        builder.Services.AddResponseCompression(options => options.EnableForHttps = true);
        builder.Services.AddRateLimiting(builder.Configuration);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddProblemDetails();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Data Sources API",
                Description = "Web API for managing data sources",
                TermsOfService = new Uri("https://example.com/terms")
            });

            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });

        builder.Services.AddDataSources();

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

        app.UseResponseCompression();

        app.UseRateLimiter();

        app.MapControllers().RequireRateLimiting("fixed");

        app.Run();
    }
}
