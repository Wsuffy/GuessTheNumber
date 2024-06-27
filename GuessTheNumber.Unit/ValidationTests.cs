using FluentValidation.TestHelper;
using GuessTheNumber.Application.Models.Requests;
using GuessTheNumber.Application.Models.Validators;

namespace GuessTheNumber.Unit;

[TestFixture]
public class ValidationTests
{
    private InitializeGameRequestValidator _gameRequestValidator;

    [SetUp]
    public void Setup()
    {
        _gameRequestValidator = new InitializeGameRequestValidator();
    }

    /// <summary>
    /// Тест, который проверяет что при создании InitializeGameRequest с неккоректными данными произойдет ошибка по (LeftBroder)
    /// </summary>
    /// <param name="leftBorder">Левая граница</param>
    /// <param name="rightBorder">Правая граница</param>
    /// <param name="attemptCount"></param>
    [Test]
    [TestCase(10, 0, 3)]
    [TestCase(int.MinValue, int.MinValue, 2)]
    [TestCase(-10, -100, 50)]
    public void InitializeGameRequestValidator_WhenLeftBorderMoreThenRight(int leftBorder, int rightBorder,
        int attemptCount)
    {
        var gameRequest = new InitializeGameRequest(leftBorder, rightBorder, attemptCount);

        var result = _gameRequestValidator.TestValidate(gameRequest);

        result.ShouldHaveValidationErrorFor(x => x.LeftBorder);
    }

    /// <summary>
    /// Тест, который проверяет что при создании InitializeGameRequest с неккоректными данными произойдет ошибка по (AttemptCount)
    /// </summary>
    /// <param name="leftBorder">Левая граница</param>
    /// <param name="rightBorder">Правая граница</param>
    /// <param name="attemptCount"></param>
    [Test]
    [TestCase(0, 10, -1)]
    [TestCase(int.MinValue, int.MaxValue, 0)]
    [TestCase(0, 150, -100)]
    [TestCase(-100, -10, int.MinValue)]
    public void EquipmentValidator_WhenNameIsEmpty_ShouldHaveValidationError(int leftBorder, int rightBorder,
        int attemptCount)
    {
        var gameRequest = new InitializeGameRequest(leftBorder, rightBorder, attemptCount);

        var result = _gameRequestValidator.TestValidate(gameRequest);

        result.ShouldHaveValidationErrorFor(x => x.AttemptCount);
    }
}