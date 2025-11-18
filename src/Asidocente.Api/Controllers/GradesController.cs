using Asidocente.Application.Features.Grades.Commands.RegisterGrade;
using Asidocente.Application.Features.Grades.Queries.GetGradesByStudent;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Asidocente.Api.Controllers;

/// <summary>
/// Grades management endpoints
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class GradesController : ControllerBase
{
    private readonly IMediator _mediator;

    public GradesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Register a new grade
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterGradeCommand command)
    {
        var result = await _mediator.Send(command);

        if (result.IsSuccess)
        {
            return CreatedAtAction(nameof(GetByStudent), new { studentId = command.StudentId }, result.Value);
        }

        return BadRequest(new { errors = result.Errors });
    }

    /// <summary>
    /// Get grades by student
    /// </summary>
    [HttpGet("student/{studentId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByStudent(int studentId, [FromQuery] int? academicPeriodId = null)
    {
        var result = await _mediator.Send(new GetGradesByStudentQuery
        {
            StudentId = studentId,
            AcademicPeriodId = academicPeriodId
        });

        return Ok(result);
    }
}
