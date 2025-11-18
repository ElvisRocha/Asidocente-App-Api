using Asidocente.Application.Features.Students.Commands.CreateStudent;
using Asidocente.Application.Features.Students.Queries.GetStudent;
using Asidocente.Application.Features.Students.Queries.GetStudentsList;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Asidocente.Api.Controllers;

/// <summary>
/// Students management endpoints
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public StudentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Create a new student
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateStudentCommand command)
    {
        var result = await _mediator.Send(command);

        if (result.IsSuccess)
        {
            return CreatedAtAction(nameof(Get), new { id = result.Value }, result.Value);
        }

        return BadRequest(new { errors = result.Errors });
    }

    /// <summary>
    /// Get student by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(int id)
    {
        var result = await _mediator.Send(new GetStudentQuery { Id = id });

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return NotFound(new { errors = result.Errors });
    }

    /// <summary>
    /// Get paginated list of students
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetList([FromQuery] GetStudentsListQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
