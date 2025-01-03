namespace API.Controllers;

using API.Attributes;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Common.Models;
using Application.Features.Command.SignContract;
using Application.Features.Query.GetContractById;
using Domain.Common;

[ApiController]
[Route("api/[controller]")]
[ApiKeyAuth]
public class TransfersController : ControllerBase
{
  private readonly IMediator _mediator;

  public TransfersController(IMediator mediator)
  {
    _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
  }

  
  /// <summary>
  /// Initiates a player transfer between teams
  /// </summary>
  /// <param name="command">Transfer details</param>
  /// <returns>Result of the transfer operation</returns>
  [HttpPost]
  [ProducesResponseType(typeof(Result<TransferResponse>), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<ActionResult<Result<Guid>>> TransferPlayer(
    [FromBody] SignContractCommand command)
  {
    var result = await _mediator.Send(command);

    
    if (!result.IsSuccess)
    {
      // Check if the error is a NotFound error
      if (result.Error.Message.Contains("not found", StringComparison.OrdinalIgnoreCase))
      {
        return NotFound(result.Error.Message);
      }

      return BadRequest(result.Error.Message);
    }
    return Ok(result);
  }
  
  /// <summary>
  /// Gets the details of a signed contract
  /// </summary>
  /// <param name="id">The contract ID</param>
  /// <returns>The contract details</returns>
  [HttpGet("{id:guid}")]
  [ProducesResponseType(typeof(Result<SignContractResponse>), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<ActionResult<Result<SignContractResponse>>> GetContract(Guid id)
  {
    var query = new GetContractByIdQuery(id);
    var result = await _mediator.Send(query);

    if (!result.IsSuccess)
    {
      return NotFound(result);
    }

    return Ok(result);
  }
}