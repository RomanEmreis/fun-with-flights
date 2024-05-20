using FunWithFlights.Aggregator.Application.Data;
using FunWithFlights.Aggregator.Application.Features.FlightRoutes.Commands;
using FunWithFlights.Aggregator.Application.Features.FlightRoutes.Responses;
using FunWithFlights.Aggregator.Application.Services.DataSources;
using FunWithFlights.Aggregator.Application.Services.FlightsProvider;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FunWithFlights.Aggregator.Application.Tests.Features.FlightRoutes.Commands
{
    public class RefreshFlightRoutesTests : TestEnvironment
    {
        [Fact]
        public async Task RefreshFlightRoutes_PopulatesDbWithNewRoutes()
        {
            _ = MockService<ILogger<RefreshFlightRoutes>>();
            var dataSourcesProvider = MockService<IDataSourcesService>();
            dataSourcesProvider.Setup(provider => provider.GetAvailableDataSourcesAsync(CancellationToken.None))
                .ReturnsAsync(new DataSourcesResponse 
                {
                    Results = new[] 
                    {
                        new DataSourceResponse { Id = 1, Name = "Test 1", Url = "http://test/1" },
                        new DataSourceResponse { Id = 2, Name = "Test 2", Url = "http://test/2" }
                    }
                });

            var flightsProvider = MockService<IFlightsProviderService>();
            flightsProvider.Setup(provider => provider.GetFlightRoutesAsync("http://test/1", CancellationToken.None))
                .ReturnsAsync(new[] 
                {
                    new FlightRouteResponse { Airline = "1", SourceAirport = "1", DestinationAirport = "1.1" },
                    new FlightRouteResponse { Airline = "2", SourceAirport = "2", DestinationAirport = "2.1" }
                });
            flightsProvider.Setup(provider => provider.GetFlightRoutesAsync("http://test/2", CancellationToken.None))
                .ReturnsAsync(new[]
                {
                    new FlightRouteResponse { Airline = "1", SourceAirport = "1", DestinationAirport = "1.2" },
                    new FlightRouteResponse { Airline = "2", SourceAirport = "2", DestinationAirport = "2.2" }
                });

            var appContext = MockService<IApplicationContext>();
            appContext
                .Setup(context =>
                    context.UseTransaction(It.IsAny<Func<IApplicationContext, CancellationToken, Task>>(), It.IsAny<string>(), CancellationToken.None));

            var command = new RefreshFlightRoutes(DateTime.Now);

            await Publish(command);

            appContext
                .Verify(context =>
                    context.UseTransaction(It.IsAny<Func<IApplicationContext, CancellationToken, Task>>(), It.IsAny<string>(), CancellationToken.None),
                    Times.Once);
        }

        [Fact]
        public async Task RefreshFlightRoutes_NoDataSources_LogsWarningAndExits()
        {
            _ = MockService<IApplicationContext>();
            _ = MockService<IFlightsProviderService>();
            var dataSourcesProvider = MockService<IDataSourcesService>();
            dataSourcesProvider.Setup(provider => provider.GetAvailableDataSourcesAsync(CancellationToken.None))
                .ReturnsAsync(new DataSourcesResponse
                {
                    Results = new DataSourceResponse[0]
                });

            var logger = MockService<ILogger<RefreshFlightRoutes>>();
            logger.Setup(logger => logger.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => string.Equals("Couldn't find any data sources", o.ToString(), StringComparison.InvariantCultureIgnoreCase)),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()));

            var command = new RefreshFlightRoutes(DateTime.Now);

            await Publish(command);

            logger.Verify(logger => logger.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => string.Equals("Couldn't find any data sources", o.ToString(), StringComparison.InvariantCultureIgnoreCase)),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                    Times.Once);
        }
    }
}
