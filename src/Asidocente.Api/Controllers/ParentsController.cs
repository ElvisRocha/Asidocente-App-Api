using Asidocente.Application.Features.Parents.Commands.CreateParent;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Asidocente.Api.Controllers;

/// <summary>
/// Parents management endpoints
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ParentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ParentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Create a new parent
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateParentCommand command)
    {
        var result = await _mediator.Send(command);

        if (result.IsSuccess)
        {
            return CreatedAtAction(nameof(Create), result.Value);
        }

        return BadRequest(new { errors = result.Errors });
    }
}
