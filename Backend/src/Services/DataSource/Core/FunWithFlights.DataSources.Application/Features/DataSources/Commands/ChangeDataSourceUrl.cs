﻿using FunWithFlights.DataSources.Application.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace FunWithFlights.DataSources.Application.Features.DataSources.Commands;

public record ChangeDataSourceUrl(int Id, string NewUrl) : IRequest;

internal sealed class ChangeDataSourceUrlHandler(IApplicationContext context, IDistributedCache cache) : IRequestHandler<ChangeDataSourceUrl>
{
    public async Task Handle(ChangeDataSourceUrl request, CancellationToken cancellationToken)
    {
        var (dataSourceId, newUrl) = request;
        var dataSourceToUpdate = await context.DataSources
            .FirstOrDefaultAsync(x => x.Id == dataSourceId, cancellationToken)
            ?? throw new InvalidOperationException($"Couldn't find data source with given Id: {dataSourceId}");

        dataSourceToUpdate.ChangeUrl(newUrl);
        await context.SaveChangesAsync(cancellationToken);
        await cache.RemoveAsync($"{CommonConstants.Cache.Namespace}:*", cancellationToken);
    }
}
