using Asp.Versioning;
using FunWithFlights.Aggregator.Application.Features.FlightRoutes.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FunWithFlights.Aggregator.API.Controllers;

[ApiVersion("1.0")]
[ApiController]
[Route("api/flight-routes")]
public class FlightRoutesController(IMediator mediator) : ControllerBase
{
    /// <summary>
    ///     Returns a list of available flight routes
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /api/flight-routes/list?start=0<![CDATA[&limit=10]]>
    ///
    /// </remarks>
    /// <param name="start">Start number, can be any number above or equal zero</param>
    /// <param name="limit">Number of flight routes to return, can be any number from 10 to 100</param>
    /// <param name="cancellationToken"></param>
    /// <returns>
    ///     A page that includes a list of flight routes
    /// </returns>
    [HttpGet("list")]
    public async Task<IActionResult> FindFlightRoutes(
        [Range(0, int.MaxValue)] int start,
        [Range(10, 100)] int limit,
        CancellationToken cancellationToken)
    {
        var results = await mediator.Send(new GetRoutes(start, limit), cancellationToken);
        return Ok(results);
    }

    /// <summary>
    ///     Returns available flight routes by given destination
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     PUT /api/flight-routes/find
    ///     {
    ///         sourceAirport: "KZN",
    ///         destinationAirport: "ASF",
    ///         dateOfFlight: "2023-12-20",
    ///         dateOfReturn: "2024-01-02"
    ///     }
    ///
    /// </remarks>
    /// <returns>
    ///     A list of flight routes
    /// </returns>
    [HttpPut("find")]
    public async Task<IActionResult> FindFlightRoutes(FindRoutes command, CancellationToken cancellationToken)
    {
        var results = await mediator.Send(command, cancellationToken);
        return Ok(results);
    }

    /// <summary>
    ///     Returns a list of available airports
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /api/flight-routes/airports
    ///
    /// </remarks>
    /// <returns>
    ///     A list of airports
    /// </returns>
    [HttpGet("airports")]
    public async Task<IActionResult> GetAirports(CancellationToken cancellationToken)
    {
        var results = await mediator.Send(new GetAirports(), cancellationToken);
        return Ok(results);
    }
}
