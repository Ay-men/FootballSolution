namespace API.Controllers;

using Application.Common.Models;
using Application.Features.Command.CreateTeam;
using Application.Features.Query.GetPlayersByTeam;
using Application.Features.Query.GetTeamDetails;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Common.Paging;
using Attributes;
using Domain.Common;

[ApiKeyAuth]
[ApiController]
[Route("api/[controller]")]
// [Authorize(Policy = "ManagerOnly")]
public class TeamsController : ControllerBase
{
  private readonly IMediator _mediator;

  public TeamsController(IMediator mediator)
  {
    _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
  }

    /// <summary>
    /// Creates a new team
    /// </summary>
    /// <param name="command">Team creation details</param>
    /// <returns>The created team</returns>
    [HttpPost]
    [ProducesResponseType(typeof(Result<TeamResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Result<Guid>>> Create(
        [FromBody] CreateTeamCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Gets detailed information about a team
    /// </summary>
    /// <param name="id">Team ID</param>
    /// <returns>Detailed team information</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(Result<TeamResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Result<TeamResponse>>> GetTeamDetails([FromRoute] Guid id)
    {
        var result = await _mediator.Send(new GetTeamDetailsQuery(id));

        if (!result.IsSuccess)
            return NotFound(result);

        return Ok(result);
    }

    /// <summary>
    /// Gets paginated list of players in a team
    /// </summary>
    /// <param name="id">Team ID</param>
    /// <param name="pageNumber">Page number</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="searchTerm">Search term</param>
    /// <param name="sortBy">Sort field</param>
    /// <param name="sortDescending">Sort direction</param>
    /// <returns>Paginated list of players</returns>
    [HttpGet("{id:guid}/players")]
    [ProducesResponseType(typeof(Result<PagedResult<PlayerResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Result<PagedResult<PlayerResponse>>>> GetTeamPlayers(
        Guid id,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null,
        [FromQuery] string? sortBy = null,
        [FromQuery] bool sortDescending = false)
    {
        var query = new GetPlayersByTeamQuery(
            id,
            pageNumber,
            pageSize,
            searchTerm,
            sortBy,
            sortDescending);

        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
            return NotFound(result);

        return Ok(result);
    }
  
  
  
}