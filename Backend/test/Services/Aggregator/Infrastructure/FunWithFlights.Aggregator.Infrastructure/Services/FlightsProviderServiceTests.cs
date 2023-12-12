using FluentAssertions;
using FunWithFlights.Aggregator.Infrastructure.Services.FlightsProvider;
using Moq;
using Xunit;

namespace FunWithFlights.Aggregator.Infrastructure.Tests.Services;

public class FlightsProviderServiceTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void GetFlightRoutesAsync_NullUrl_ThrowsArgumentException(string? url)
    {
        var fakeHttpClient = new Mock<HttpClient>();
        var service = new FlightsProviderService(fakeHttpClient.Object);

        Func<Task> act = () => service.GetFlightRoutesAsync(url);

        act.Should().ThrowExactlyAsync<ArgumentException>();
    }
}
