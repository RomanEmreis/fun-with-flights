using FunWithFlights.DataSources.Application.Data;
using FunWithFlights.DataSources.Application.Features.DataSources.Responses;
using FunWithFlights.DataSources.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FunWithFlights.DataSources.Application.Features.DataSources.Queries;

public sealed class GetDataSources() : IRequest<DataSourcesResponse>;

internal sealed class GetDataSourcesHandler(IApplicationContext context) : IRequestHandler<GetDataSources, DataSourcesResponse>
{
    public async Task<DataSourcesResponse> Handle(GetDataSources request, CancellationToken cancellationToken)
    {
        var dataSources = await context.DataSources
            .Select(dataSource => ConvertToResponse(dataSource))
            .ToListAsync(cancellationToken);

        return new DataSourcesResponse(dataSources);
    }

    private static DataSourceResponse ConvertToResponse(DataSource dataSource) => new()
    {
        Id = dataSource.Id,
        Name = dataSource.Name,
        Description = dataSource.Description,
        Url = dataSource.Url
    };
}
