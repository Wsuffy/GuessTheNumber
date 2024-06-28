using GuessTheNumber.Abstractions.Interfaces;
using GuessTheNumber.Application.Models.DTOs;
using GuessTheNumber.Application.Models.Requests;
using GuessTheNumber.Application.Models.Responses;
using GuessTheNumber.Domain.Exceptions;
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
    private Mock<IMediator> _mediatorMock;

    [SetUp]
    public void Setup()
    {
        _mediatorMock = new Mock<IMediator>();
        _mockRandomNumberGenerator = new Mock<IGuessRandomNumberGenerator>();
        _gameManager = new GameManager(_mediatorMock.Object);
    }

    /// <summary>
    /// Проверка на то, чтобы инициализированная игра, вернула sessionId
    /// </summary>
    /// <param name="leftBorder">Левая граница</param>
    /// <param name="rightBorder">Правая граница</param>
    /// <param name="attemptCount">Количество попыток</param>
    /// <param name="targetValue">Ожидаемое значение</param>
    [Test]
    [TestCase(1, 100, 10, 50)]
    [TestCase(int.MinValue, int.MaxValue, 50, 0)]
    [TestCase(100, 1000, int.MaxValue, 100)]
    [TestCase(-100, -10, 2, -50)]
    public async Task InitializeGame_ShouldReturnSessionId(int leftBorder, int rightBorder, int attemptCount,
        int targetValue)
    {
        var request = new InitializeGameRequest(leftBorder, rightBorder, attemptCount);
        var expectedSessionId = Guid.NewGuid();

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<AddGameConfigurationCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GameConfigurationDto(expectedSessionId, targetValue, attemptCount));

        var result = await _gameManager.InitializeGame(request, CancellationToken.None);

        Assert.That(result, Is.EqualTo(expectedSessionId));
    }

    /// <summary>
    /// Проверка на то, что manager выдаст BadRequestException, если сессия пуста
    /// </summary>
    /// <param name="userGuess">Пользовательское предположение</param>
    /// <param name="expectedString">Ожидаемая строка в ошибке</param>
    /// <exception cref="BadRequestException">Bad Request Exception</exception>
    [Test]
    [TestCase(50, "Вы не установили правила игры, чтобы в нее играть")]
    [TestCase(int.MinValue, "Вы не установили правила игры, чтобы в нее играть")]
    [TestCase(int.MaxValue, "Вы не установили правила игры, чтобы в нее играть")]
    public void GuessNumber_ShouldThrowException_WhenSessionIsNull(int userGuess, string expectedString)
    {
        var sessionId = Guid.NewGuid();
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetGameSessionByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((GetGameSessionResponse)null);

        Func<Task> act = async () => await _gameManager.GuessNumber(sessionId, userGuess, CancellationToken.None);

        Assert.Throws<BadRequestException>(() =>
            throw new BadRequestException(expectedString));
    }

    /// <summary>
    /// Проверка на то, чтобы успешно завершать игру, когда, количество попыток опускается до 0
    /// </summary>
    /// <param name="targetValue">Пользовательское предположение</param>
    /// <param name="attemptCount">Количество попыток</param>
    /// <param name="expectedString">Ожидаемая строка</param>
    [Test]
    [TestCase(50, 0, "Игра окончена. Вы проиграли. Загаданное число было 50.")]
    [TestCase(1000, 0, "Игра окончена. Вы проиграли. Загаданное число было 1000.")]
    [TestCase(-1000, 0, $"Игра окончена. Вы проиграли. Загаданное число было -1000.")]
    public async Task GuessNumber_ShouldReturnCorrectMessage_WhenAttemptsAreZero(int targetValue, int attemptCount,
        string expectedString)
    {
        var sessionId = Guid.NewGuid();
        var sessionResponse = new GetGameSessionResponse(targetValue, attemptCount);

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetGameSessionByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(sessionResponse);

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<RemoveGameConfigurationCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GameConfigurationDto(sessionId, targetValue, attemptCount));

        var result = await _gameManager.GuessNumber(sessionId, targetValue, CancellationToken.None);

        Assert.That(result, Is.EqualTo(expectedString));
    }

    /// <summary>
    /// Проверка на то, что если пользователь угадал число корректно завершить ее
    /// </summary>
    /// <param name="targetValue">Пользовательское предположение</param>
    /// <param name="attemptCount">Количество попыток</param>
    /// <param name="expectedString">Ожидаемая строка</param>
    [Test]
    [TestCase(50, 5, "Поздравляем! Вы угадали число.")]
    [TestCase(int.MaxValue, 5, "Поздравляем! Вы угадали число.")]
    [TestCase(int.MinValue, 5, "Поздравляем! Вы угадали число.")]
    public async Task GuessNumber_ShouldReturnCorrectMessage_WhenGuessIsCorrect(int targetValue, int attemptCount,
        string expectedString)
    {
        var sessionId = Guid.NewGuid();
        var sessionResponse = new GetGameSessionResponse(targetValue, attemptCount);

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetGameSessionByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(sessionResponse);

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<RemoveGameConfigurationCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GameConfigurationDto(sessionId, targetValue, attemptCount));

        var result = await _gameManager.GuessNumber(sessionId, targetValue, CancellationToken.None);

        Assert.That(result, Is.EqualTo("Поздравляем! Вы угадали число."));
    }

    
    /// <summary>
    /// Проверка на то, чтобы успешно продолжить игру игру, когда, количество пользователь предполагает, что число выше
    /// </summary>
    /// <param name="targetValue">Пользовательское предположение</param>
    /// <param name="attemptCount">Количество попыток</param>
    /// <param name="expectedString">Ожидаемая строка</param>
    [Test]
    [TestCase(50, 5, "Не верно, число меньше 51. Осталось 4 попыток.")]
    [TestCase(1000, 1, "Не верно, число меньше 1001. Осталось 0 попыток.")]
    [TestCase(-1000, 100, "Не верно, число меньше -999. Осталось 99 попыток.")]
    public async Task GuessNumber_ShouldReturnCorrectMessage_WhenGuessIsTooHigh(int targetValue, int attemptCount,
        string expectedString)
    {
        var sessionId = Guid.NewGuid();
        var sessionResponse = new GetGameSessionResponse(targetValue, attemptCount);

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetGameSessionByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(sessionResponse);

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<DecreaseAmountOfAttemptsCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(attemptCount - 1);

        var result = await _gameManager.GuessNumber(sessionId, targetValue + 1, CancellationToken.None);

        Assert.That(result, Is.EqualTo(expectedString));
    }

    /// <summary>
    /// Проверка на то, чтобы успешно продолжить игру игру, когда, количество пользователь предполагает, что число ниже
    /// </summary>
    /// <param name="targetValue">Пользовательское предположение</param>
    /// <param name="attemptCount">Количество попыток</param>
    /// <param name="expectedString">Ожидаемая строка</param>
    [Test]
    [TestCase(50, 5, "Не верно, число больше 49. Осталось 4 попыток.")]
    [TestCase(-1000, 1, "Не верно, число больше -1001. Осталось 0 попыток.")]
    [TestCase(50000, 1000, "Не верно, число больше 49999. Осталось 999 попыток.")]
    public async Task GuessNumber_ShouldReturnCorrectMessage_WhenGuessIsTooLow(int targetValue, int attemptCount,
        string expectedString)
    {
        var sessionId = Guid.NewGuid();
        var sessionResponse = new GetGameSessionResponse(targetValue, attemptCount);

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetGameSessionByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(sessionResponse);

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<DecreaseAmountOfAttemptsCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(attemptCount - 1);

        var result = await _gameManager.GuessNumber(sessionId, targetValue - 1, CancellationToken.None);

        Assert.That(result, Is.EqualTo(expectedString));
    }
}