using FunWithFlights.DataSources.Application.Features.DataSources.Responses;
using FunWithFlights.DataSources.Domain.Entities;

namespace FunWithFlights.DataSources.Application.Features.DataSources.Extensions;

internal static class DataSourceExtensions
{
    internal static DataSourceResponse ToResponse(this DataSource dataSource) => new()
    {
        Id = dataSource.Id,
        Name = dataSource.Name,
        Description = dataSource.Description,
        Url = dataSource.Url
    };
}
