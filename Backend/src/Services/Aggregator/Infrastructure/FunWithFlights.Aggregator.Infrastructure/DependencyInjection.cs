using FunWithFlights.Aggregator.Application.Data;
using FunWithFlights.Aggregator.Application.Services.DataSources;
using FunWithFlights.Aggregator.Infrastructure.Services.DataSources;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using FunWithFlights.Aggregator.Application.Services.FlightsProvider;
using FunWithFlights.Aggregator.Infrastructure.Services.FlightsProvider;
using FunWithFlights.ServiceDefaults.Options;
using FunWithFlights.Aggregator.Infrastructure.Data;

namespace FunWithFlights.Aggregator.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddAggregator(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(config =>
            config.RegisterServicesFromAssemblyContaining<IApplicationContext>());

        services.AddDataBase();

        var retryPolicyOptions = new RetryPolicyOptions();
        configuration.GetSection(nameof(RetryPolicyOptions)).Bind(retryPolicyOptions);

        services.AddDataSources(configuration, retryPolicyOptions);
        services.AddFlightsProvider(configuration, retryPolicyOptions);

        return services;
    }

    public static IServiceCollection AddDataSources(
        this IServiceCollection services,
        IConfiguration configuration,
        RetryPolicyOptions retryPolicyOptions)
    {
        var httpClientFactoryOptions = new HttpClientFactoryOptions();
        configuration.GetSection("DataSourcesHttpClientFactoryOptions").Bind(httpClientFactoryOptions);

        services.AddHttpClient<IDataSourcesService, DataSourcesService>(httpClientFactoryOptions, retryPolicyOptions);

        return services;
    }

    public static IServiceCollection AddFlightsProvider(
        this IServiceCollection services,
        IConfiguration configuration,
        RetryPolicyOptions retryPolicyOptions)
    {
        var httpClientFactoryOptions = new HttpClientFactoryOptions();
        configuration.GetSection("FlightProviderHttpClientFactoryOptions").Bind(httpClientFactoryOptions);

        services.AddHttpClient<IFlightsProviderService, FlightsProviderService>(httpClientFactoryOptions, retryPolicyOptions);

        return services;
    }

    public static IServiceCollection AddDataBase(this IServiceCollection services)
    {
        services.AddScoped<IApplicationContext>(provider =>
            provider.GetRequiredService<ApplicationContext>());

        return services;
    }
}
