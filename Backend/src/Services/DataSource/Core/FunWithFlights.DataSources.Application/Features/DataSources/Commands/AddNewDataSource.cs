using FunWithFlights.DataSources.Application.Data;
using FunWithFlights.DataSources.Application.Features.DataSources.Queries;
using FunWithFlights.DataSources.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;

namespace FunWithFlights.DataSources.Application.Features.DataSources.Commands;

public record AddNewDataSource(string Name, string? Description, string Url): IRequest;

internal sealed class AddNewDataSourceHandler(IApplicationContext context, IDistributedCache cache) : IRequestHandler<AddNewDataSource>
{
    public async Task Handle(AddNewDataSource request, CancellationToken cancellationToken)
    {
        var newDataSource = new DataSource(
            request.Name,
            request.Description,
            request.Url);

        context.DataSources.Add(newDataSource);

        await context.SaveChangesAsync(cancellationToken);
        await cache.RemoveAsync(nameof(GetDataSources), cancellationToken);
    }
}