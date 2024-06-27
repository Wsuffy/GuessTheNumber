using GuessTheNumber.Abstractions.Interfaces;
using GuessTheNumber.Application.Models.DTOs;
using GuessTheNumber.Application.Models.Requests;
using GuessTheNumber.Application.Models.Validators;
using GuessTheNumber.Domain.Exceptions;
using GuessTheNumber.Implementation;
using GuessTheNumber.Implementation.Commands;
using GuessTheNumber.Implementation.Managers;
using GuessTheNumber.Implementation.Queries;
using MediatR;
using Moq;

namespace GuessTheNumber.Unit;

[TestFixture]
public class GameManagerTests
{
    private Mock<IGuessRandomNumberGenerator> _mockRandomNumberGenerator;
    private IGameManager _gameManager;
    private Mock<IMediator> _mockMediator;

    [SetUp]
    public void Setup()
    {
        _mockMediator = new Mock<IMediator>();
        _mockRandomNumberGenerator = new Mock<IGuessRandomNumberGenerator>();
        _gameManager = new GameManager(_mockMediator.Object);
    }

    [Test]
    public async Task InitializeGame_ShouldReturnValidSessionId()
    {
        var request = new InitializeGameRequest(1, 10, 3);
        var expectedSessionId = Guid.NewGuid();
        var configurationDto = new GameConfigurationDto(expectedSessionId, 5, 3);

        _mockMediator.Setup(m => m.Send(It.IsAny<AddGameConfigurationCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(configurationDto);

        var result = await _gameManager.InitializeGame(request, CancellationToken.None);

        Assert.That(result, Is.EqualTo(expectedSessionId));
    }
}