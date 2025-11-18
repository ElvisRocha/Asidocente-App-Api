using Asidocente.Application.Features.Attendance.Commands.RecordAttendance;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Asidocente.Api.Controllers;

/// <summary>
/// Attendance management endpoints
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AttendanceController : ControllerBase
{
    private readonly IMediator _mediator;

    public AttendanceController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Record student attendance
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Record([FromBody] RecordAttendanceCommand command)
    {
        var result = await _mediator.Send(command);

        if (result.IsSuccess)
        {
            return CreatedAtAction(nameof(Record), result.Value);
        }

        return BadRequest(new { errors = result.Errors });
    }
}
