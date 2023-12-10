using Asp.Versioning;
using FunWithFlights.Aggregator.Application.Features.FlightRoutes.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FunWithFlights.Aggregator.API.Controllers;

[ApiVersion("1.0")]
[ApiController]
[Route("api/flight-routes")]
public class FlightRoutesController(IMediator mediator) : ControllerBase
{
    [HttpPut("find")]
    public async Task<IActionResult> FindFlightRoutes(FindRoutes command, CancellationToken cancellationToken)
    {
        var results = await mediator.Send(command, cancellationToken);
        return Ok(results);
    }
}
