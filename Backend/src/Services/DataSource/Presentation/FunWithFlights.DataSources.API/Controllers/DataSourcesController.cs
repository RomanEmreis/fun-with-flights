using Asp.Versioning;
using FunWithFlights.DataSources.Application.Features.DataSources.Commands;
using FunWithFlights.DataSources.Application.Features.DataSources.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FunWithFlights.DataSources.API.Controllers;

[ApiVersion("1.0")]
[Route("api")]
[ApiController]
public class DataSourcesController(IMediator mediator) : ControllerBase
{
    [HttpGet("data-sources/all")]
    public async Task<IActionResult> GetDataSources(CancellationToken cancellationToken)
    {
        var results = await mediator.Send(new GetDataSources(), cancellationToken);
        return Ok(results);
    }

    [HttpPost("data-sources/add")]
    public async Task<IActionResult> AddDataSource(AddNewDataSource command, CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);
        return Ok();
    }

    [HttpPut("data-sources/update-url")]
    public async Task<IActionResult> ChangeDataSourceUrl(ChangeDataSourceUrl command, CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);
        return Ok();
    }

    [HttpDelete("data-sources/{dataSourceId:int}/remove")]
    public async Task<IActionResult> RemoveDataSource(int dataSourceId, CancellationToken cancellationToken)
    {
        await mediator.Send(new RemoveDataSource(dataSourceId), cancellationToken);
        return Ok();
    }
}
