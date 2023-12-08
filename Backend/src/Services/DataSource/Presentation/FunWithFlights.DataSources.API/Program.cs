using Asp.Versioning;
using FunWithFlights.DataSources.Infrastructure;
using FunWithFlights.DataSources.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FunWithFlights.DataSources.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.AddServiceDefaults();

        // Add services to the container.
        builder.AddNpgsqlDbContext<ApplicationContext>("datasourcesdb");

        builder.Services.AddApiVersioning(setup =>
        {
            setup.AssumeDefaultVersionWhenUnspecified = true;
            setup.DefaultApiVersion = new ApiVersion(1, 0);
            setup.ReportApiVersions = true;
            setup.ApiVersionReader = new HeaderApiVersionReader("X-Version");
        });

        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddProblemDetails();
        builder.Services.AddSwaggerGen();

        builder.Services.AddDataSources();

        var app = builder.Build();

        app.MapDefaultEndpoints();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
