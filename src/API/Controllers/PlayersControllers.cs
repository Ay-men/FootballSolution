namespace API.Controllers;

using Application.Common.Models;
using Application.Features.Command.CreatePlayer;
using Application.Features.Command.UpdatePlayer;
using Application.Features.Query.GetPlayerById;
using Attributes;
using Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[ApiKeyAuth]
[Route("api/[controller]")]
[Produces("application/json")]
public class PlayersController : ControllerBase
{
    private readonly IMediator _mediator;

    public PlayersController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }
    
 /// <summary>
    /// Creates a new player
    /// </summary>
    /// <param name="command">Player creation details</param>
    /// <returns>The created player</returns>
    [HttpPost]
    [ProducesResponseType(typeof(Result<PlayerResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Result<PlayerResponse>>> Create(
        [FromBody] CreatePlayerCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(result);

        return CreatedAtAction(
            nameof(GetById), 
            new { id = result.Value }, 
            result);
    }

    /// <summary>
    /// Gets a specific player by id
    /// </summary>
    /// <param name="id">The player's ID</param>
    /// <returns>The player details</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(Result<PlayerResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Result<PlayerResponse>>> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetPlayerByIdQuery(id));

        if (!result.IsSuccess)
            return NotFound(result);

        return Ok(result);
    }

    /// <summary>
    /// Updates an existing player
    /// </summary>
    /// <param name="id">The player's ID</param>
    /// <param name="command">Update details</param>
    /// <returns>The updated player</returns>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(Result<PlayerResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Result<PlayerResponse>>> Update(
        Guid id, 
        [FromBody] UpdatePlayerCommand command)
    {
        if (id != command.PlayerId)
            return BadRequest(new { error = "ID mismatch" });

        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return result.Error.Message.Contains("not found") 
                ? NotFound(result) 
                : BadRequest(result);

        return Ok(result);
    }
}