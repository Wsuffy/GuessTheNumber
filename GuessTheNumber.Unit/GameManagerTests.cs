using GuessTheNumber.Abstractions.Interfaces;
using GuessTheNumber.Application.Models.Requests;
using GuessTheNumber.Application.Models.Validators;
using GuessTheNumber.Domain.Exceptions;
using GuessTheNumber.Implementation;
using Moq;

namespace GuessTheNumber.Unit;

[TestFixture]
public class GameManagerTests
{
    private Mock<IGuessRandomNumberGenerator> _mockRandomNumberGenerator;
    private IGameManager _gameManager;
    private InitializeGameRequestValidator _validator;

    [SetUp]
    public void Setup()
    {
        _mockRandomNumberGenerator = new Mock<IGuessRandomNumberGenerator>();
        _gameManager = new GameManager(_mockRandomNumberGenerator.Object);
        _validator = new InitializeGameRequestValidator();
    }

    /// <summary>
    /// Метод GuessNumber должен выдать сообщение о победе, если число угадано правильно
    /// </summary>
    /// <param name="target">Угадываемое число</param>
    /// <param name="guess">Наше предположение</param>
    /// <param name="messageString">Ожидаемое сообщение</param>
    [Test]
    [TestCase(5, 5, "Поздравляем! Вы угадали число.")]
    [TestCase(7, 7, "Поздравляем! Вы угадали число.")]
    [TestCase(3, 3, "Поздравляем! Вы угадали число.")]
    public void GuessNumber_ShouldReturnCorrectResult_WhenGuessIsCorrect(int target, int guess, string messageString)
    {
        var request = new InitializeGameRequest(1, 10, 3);
        _mockRandomNumberGenerator.Setup(x => x.Generate(It.IsAny<int>(), It.IsAny<int>())).Returns(target);
        _gameManager.InitializeGame(request);

        var result = _gameManager.GuessNumber(guess);

        Assert.That(result, Is.EqualTo(messageString));
    }

    /// <summary>
    /// Метод GuessNumber должен правильно отрабатывать, когда предположение слишком велико
    /// </summary>
    /// <param name="target">Угадываемое число</param>
    /// <param name="guess">Наше предположение</param>
    /// <param name="attempts">Количество попыток</param>
    /// <param name="expectedString">Ожидаемая ошибка</param>
    [Test]
    [TestCase(5, 7, 3, "Не верно, число меньше 7. Осталось 2 попыток.")]
    [TestCase(5, 10, 3, "Не верно, число меньше 10. Осталось 2 попыток.")]
    public void GuessNumber_ShouldReturnCorrectResult_WhenGuessIsTooHigh(int target, int guess, int attempts,
        string expectedString)
    {
        var request = new InitializeGameRequest(1, 10, attempts);
        _mockRandomNumberGenerator.Setup(x => x.Generate(It.IsAny<int>(), It.IsAny<int>())).Returns(target);
        _gameManager.InitializeGame(request);

        var result = _gameManager.GuessNumber(guess);

        Assert.That(result, Is.EqualTo(expectedString));
    }

    /// <summary>
    /// Метод GuessNumber должен правильно отрабатывать, когда предположение слишком мало
    /// </summary>
    /// <param name="target">Угадываемое число</param>
    /// <param name="guess">Наше предположение</param>
    /// <param name="attempts">Количество попыток</param>
    /// <param name="expectedString">Ожидаемая ошибка</param>
    [Test]
    [TestCase(5, 3, 230, "Не верно, число больше 3. Осталось 229 попыток.")]
    [TestCase(4, 1, 3, "Не верно, число больше 1. Осталось 2 попыток.")]
    [TestCase(9, 1, 10, "Не верно, число больше 1. Осталось 9 попыток.")]
    public void GuessNumber_ShouldReturnCorrectResult_WhenGuessIsTooLow(int target, int guess, int attempts,
        string expectedString)
    {
        var request = new InitializeGameRequest(1, 10, attempts);
        _mockRandomNumberGenerator.Setup(x => x.Generate(It.IsAny<int>(), It.IsAny<int>())).Returns(target);
        _gameManager.InitializeGame(request);

        var result = _gameManager.GuessNumber(guess);

        Assert.That(result, Is.EqualTo(expectedString));
    }

    /// <summary>
    /// Метод GuessNumber должен правильно отрабатывать, когда игра окончена
    /// </summary>
    /// <param name="target">Угадываемое число</param>
    /// <param name="guess">Наше предположение</param>
    /// <param name="expectedString">Ожидаемая ошибка</param>
    [Test]
    [TestCase(5, 3, "Игра окончена. Вы проиграли. Загаданное число было 5.")]
    [TestCase(2, int.MaxValue, "Игра окончена. Вы проиграли. Загаданное число было 2.")]
    [TestCase(1, int.MinValue, "Игра окончена. Вы проиграли. Загаданное число было 1.")]
    public void GuessNumber_ShouldReturnGameOver_WhenAttemptsAreExhausted(int target, int guess, string expectedString)
    {
        // Arrange
        var request = new InitializeGameRequest(1, 10, 1);
        _mockRandomNumberGenerator.Setup(x => x.Generate(It.IsAny<int>(), It.IsAny<int>())).Returns(target);
        _gameManager.InitializeGame(request);
        _gameManager.GuessNumber(guess);

        // Act
        var result = _gameManager.GuessNumber(guess);

        // Assert
        Assert.That(result, Is.EqualTo(expectedString));
    }

    /// <summary>
    /// Тест который проверяет, правильность работы метода GetTargetValue, если настройки существуют
    /// </summary>
    /// <param name="targetReturn">Необходимое число</param>
    [Test]
    [TestCase(42)]
    [TestCase(int.MinValue)]
    [TestCase(int.MaxValue)]
    public void GetTargetValue_ReturnsCorrectValue_AfterInitialization(int targetReturn)
    {
        var initializeRequest = new InitializeGameRequest(1, 100, 5);

        _mockRandomNumberGenerator.Setup(x => x.Generate(It.IsAny<int>(), It.IsAny<int>())).Returns(targetReturn);

        _gameManager.InitializeGame(initializeRequest);
        var targetValue = _gameManager.GetTargetValue();

        Assert.That(targetValue, Is.EqualTo(targetReturn));
    }

    /// <summary>
    /// Тест который проверяет, что при обращении к методу GetTargetValue может выпасть NullReferenceException, если настроек нет
    /// </summary>
    [Test]
    public void GetTargetValue_ReturnsException_WithoutInitialization()
    {
        var targetValue = 0;
        var exceptionThrown = false;

        try
        {
            targetValue = _gameManager.GetTargetValue();
        }
        catch (NullReferenceException)
        {
            exceptionThrown = true;
        }

        Assert.That(exceptionThrown, Is.True, "Expected NullReferenceException was not thrown.");
    }

    /// <summary>
    /// Тест который проверяет, что при обращении к методу GuessNumber может выпасть BadRequestException, если настроек нет
    /// </summary>
    [Test]
    public void GuessNumber_ReturnsException_WithoutInitialization()
    {
        var guessValue = 5;
        var exceptionThrown = false;

        try
        {
            _gameManager.GuessNumber(guessValue);
        }
        catch (BadRequestException)
        {
            exceptionThrown = true;
        }

        Assert.That(exceptionThrown, Is.True);
    }
}