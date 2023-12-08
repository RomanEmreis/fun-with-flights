using FunWithFlights.DataSources.Application.Data;
using FunWithFlights.DataSources.Domain.Entities;
using MediatR;

namespace FunWithFlights.DataSources.Application.Features.DataSources.Commands;

public record AddNewDataSource(string Name, string? Description, string Url): IRequest;

internal sealed class AddNewDataSourceHandler(IApplicationContext context) : IRequestHandler<AddNewDataSource>
{
    public async Task Handle(AddNewDataSource request, CancellationToken cancellationToken)
    {
        var newDataSource = new DataSource(
            request.Name,
            request.Description,
            request.Url);

        context.DataSources.Add(newDataSource);

        await context.SaveChangesAsync(cancellationToken);
    }
}