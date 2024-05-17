using Asp.Versioning;
using FunWithFlights.DataSources.Application.Features.DataSources.Commands;
using FunWithFlights.DataSources.Application.Features.DataSources.Queries;
using FunWithFlights.DataSources.Application.Features.DataSources.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FunWithFlights.DataSources.API.Controllers;

[ApiVersion("1.0")]
[ApiController]
[Route("api/data-sources")]
public class DataSourcesController(IMediator mediator) : ControllerBase
{
    /// <summary>
    ///     Provides the information about all available data sources
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /api/data-sources/all
    ///
    /// </remarks>
    /// <returns>
    ///     A list of available data sources
    /// </returns>
    [HttpGet("all")]
    [Produces<DataSourcesResponse>]
    public async Task<IActionResult> GetDataSources(CancellationToken cancellationToken)
    {
        var results = await mediator.Send(new GetDataSources(), cancellationToken);
        return Ok(results);
    }

    /// <summary>
    ///     Adds a new data source
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /api/data-sources/add
    ///     {
    ///         name: "New Data Source",
    ///         description: "Some description",
    ///         url: "https://url-to-provide.com/flights"
    ///     }
    ///
    /// </remarks>
    [HttpPost("add")]
    public async Task<IActionResult> AddDataSource(AddNewDataSource command, CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);
        return Created();
    }

    /// <summary>
    ///     Updates a give data source's url
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     PUT /api/data-sources/update-url
    ///     {
    ///         id: "New Data Source",
    ///         newUrl: "https://url-to-provide.com/flights"
    ///     }
    ///
    /// </remarks>
    [HttpPut("update-url")]
    public async Task<IActionResult> ChangeDataSourceUrl(ChangeDataSourceUrl command, CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);
        return Ok();
    }

    /// <summary>
    ///     Removes a data source by Id
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     DELETE /api/data-sources/dataSourceId/remove
    ///
    /// </remarks>
    [HttpDelete("{dataSourceId:int}/remove")]
    public async Task<IActionResult> RemoveDataSource(int dataSourceId, CancellationToken cancellationToken)
    {
        await mediator.Send(new RemoveDataSource(dataSourceId), cancellationToken);
        return Ok();
    }
}
