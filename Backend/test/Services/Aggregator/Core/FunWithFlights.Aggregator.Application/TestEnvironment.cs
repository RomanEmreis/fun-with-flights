using FunWithFlights.Aggregator.Application.Data;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;

namespace FunWithFlights.Aggregator.Application.Tests
{
    public abstract class TestEnvironment
    {
        protected IServiceCollection _services = new ServiceCollection();

        public TestEnvironment()
        {
            Setup();
        }

        public void Setup()
        {
            _services.AddMediatR(config =>
                config.RegisterServicesFromAssemblyContaining<IApplicationContext>());
        }

        protected Mock<T> MockService<T>()
             where T : class
        {
            var mock = new Mock<T>();
            _services.TryAddScoped(provider => mock.Object);

            return mock;
        }

        protected async Task Send<T>(T command)
            where T : IRequest
        {
            using var scope = _services.BuildServiceProvider().CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            
            await mediator.Send(command);
        }

        protected async Task Publish<T>(T command)
            where T : INotification
        {
            using var scope = _services.BuildServiceProvider().CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            await mediator.Publish(command);
        }
    }
}
