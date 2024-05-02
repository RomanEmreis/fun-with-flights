using FunWithFlights.DataSources.Application.Data;
using FunWithFlights.DataSources.Application.Features.DataSources.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace FunWithFlights.DataSources.Application.Features.DataSources.Commands;

public record RemoveDataSource(int Id) : IRequest;

internal sealed class RemoveDataSourceHandler(IApplicationContext context, IDistributedCache cache) : IRequestHandler<RemoveDataSource>
{
    public async Task Handle(RemoveDataSource request, CancellationToken cancellationToken)
    {
        var dataSourceId = request.Id;
        await context.DataSources
            .Where(dataSource => dataSource.Id == dataSourceId)
            .ExecuteDeleteAsync(cancellationToken);

        await cache.RemoveAsync(nameof(GetDataSources), cancellationToken);
    }
}

