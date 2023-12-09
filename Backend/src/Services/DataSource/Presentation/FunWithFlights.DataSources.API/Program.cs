using FunWithFlights.DataSources.Infrastructure;
using FunWithFlights.DataSources.Infrastructure.Data;

namespace FunWithFlights.DataSources.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.AddServiceDefaults();

        // Add services to the container.
        builder.AddNpgsqlDbContext<ApplicationContext>("datasourcesdb");

        builder.Services.AddCors();
        builder.Services.AddControllers();
        builder.Services.AddApiVersioning(headerName: "X-Version");
        builder.Services.AddRateLimiting(builder.Configuration);

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
            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true) // allow any origin
                .AllowCredentials()); // allow credentials
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.UseRateLimiter();

        app.MapControllers().RequireRateLimiting("fixed");

        app.Run();
    }
}
