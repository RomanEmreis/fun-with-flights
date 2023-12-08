using FunWithFlights.DataSources.Application.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FunWithFlights.DataSources.Application.Features.DataSources.Commands;

public record RemoveDataSource(int Id) : IRequest;

internal sealed class RemoveDataSourceHandler(IApplicationContext context) : IRequestHandler<RemoveDataSource>
{
    public async Task Handle(RemoveDataSource request, CancellationToken cancellationToken)
    {
        var dataSourceId = request.Id;
        await context.DataSources
            .Where(dataSource => dataSource.Id == dataSourceId)
            .ExecuteDeleteAsync(cancellationToken);
    }
}

