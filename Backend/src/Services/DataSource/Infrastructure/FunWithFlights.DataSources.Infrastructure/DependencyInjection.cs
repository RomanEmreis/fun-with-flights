using FunWithFlights.DataSources.Application.Data;
using FunWithFlights.DataSources.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;

namespace FunWithFlights.DataSources.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddDataSources(this IServiceCollection services)
    {
        services.AddMediatR(config =>
            config.RegisterServicesFromAssemblyContaining<IApplicationContext>());

        services.AddDataBase();

        return services;
    }

    public static IServiceCollection AddDataBase(this IServiceCollection services)
    {
        services.AddScoped<IApplicationContext>(provider => 
            provider.GetRequiredService<ApplicationContext>());

        return services;
    }
}
