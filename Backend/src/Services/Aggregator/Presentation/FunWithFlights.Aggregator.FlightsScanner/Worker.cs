using FunWithFlights.Aggregator.Application.Features.FlightRoutes.Commands;
using FunWithFlights.Aggregator.FlightsScanner.Options;
using MediatR;
using Microsoft.Extensions.Options;

namespace FunWithFlights.Aggregator.FlightsScanner;

public class Worker(
    IServiceScopeFactory serviceScopeFactory,
    ILogger<Worker> logger,
    IOptions<FlightScanningOptions> options) : BackgroundService
{
    private readonly PeriodicTimer _timer = new(TimeSpan.FromHours(options.Value.Period));

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        do
        {
            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }

            using var scope = serviceScopeFactory.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            await mediator.Publish(new RefreshFlightRoutes(), stoppingToken);
        }
        while (
            await _timer.WaitForNextTickAsync(stoppingToken) &&
            !stoppingToken.IsCancellationRequested);
    }
}
