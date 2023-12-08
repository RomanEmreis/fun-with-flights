using FunWithFlights.DataSources.Application.Data;
using FunWithFlights.DataSources.Application.Features.DataSources.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FunWithFlights.DataSources.Application.Features.DataSources.Queries;

public sealed class GetDataSources() : IRequest<DataSourcesResponse>;

internal sealed class GetDataSourcesHandler(IApplicationContext context) : IRequestHandler<GetDataSources, DataSourcesResponse>
{
    public async Task<DataSourcesResponse> Handle(GetDataSources request, CancellationToken cancellationToken)
    {
        var dataSources = await context.DataSources
            .Select(dataSource => new DataSourceResponse 
            {
                Id = dataSource.Id,
                Name = dataSource.Name,
                Description = dataSource.Description,
                Url = dataSource.Url,
            }).ToListAsync(cancellationToken);

        return new DataSourcesResponse(dataSources);
    }
}
