namespace FootballSolution.Tests.Controllers;

using API.Controllers;
using Application.Common.Models;
using Application.Common.Paging;
using Application.Features.Query.GetPlayersByTeam;
using FluentAssertions;
using global::Domain.Common;
using global::Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

public class TeamsControllerTests
{
    private readonly Mock<IMediator> _mediator;
    private readonly TeamsController _controller;

    public TeamsControllerTests()
    {
        _mediator = new Mock<IMediator>();
        _controller = new TeamsController(_mediator.Object);
    }

    [Fact]
    public async Task GetTeamPlayers_WithValidParameters_ReturnsPagedResult()
    {
        // Arrange
        var teamId = Guid.NewGuid();
        var playerResponse = new PlayerResponse(
            Guid.NewGuid(),
            "John",
            "Doe",
            new DateOnly(2000, 1, 1),
            1.80m,
            Position.Forward,
            1000000m);

        var pagedResult = new PagedResult<PlayerResponse>
        {
            CurrentPage = 1,
            PageSize = 10,
            RowCount = 1,
            Results = new List<PlayerResponse> { playerResponse }
        };

        _mediator.Setup(m => m.Send(
                It.IsAny<GetPlayersByTeamQuery>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<PagedResult<PlayerResponse>>.Success(pagedResult));

        // Act
        var result = await _controller.GetTeamPlayers(
            teamId,
            1,
            10,
            "search",
            "name",
            false);

        // Assert
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        var value = okResult!.Value as Result<PagedResult<PlayerResponse>>;
        value!.Value.Results.Should().HaveCount(1);
        value.Value.CurrentPage.Should().Be(1);
    }
}