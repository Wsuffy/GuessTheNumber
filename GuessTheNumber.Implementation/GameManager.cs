using GuessTheNumber.Abstractions.Enums;
using GuessTheNumber.Abstractions.Interfaces;
using GuessTheNumber.Application.Models.Requests;
using GuessTheNumber.Domain.Entities;
using GuessTheNumber.Domain.Exceptions;

namespace GuessTheNumber.Implementation;

public class GameManager : IGameManager
{
    private GameSettings _settings;

    private readonly IGuessRandomNumberGenerator _randomNumberGenerator;

    public GameManager(IGuessRandomNumberGenerator randomNumberGenerator)
    {
        _randomNumberGenerator = randomNumberGenerator;
    }

    public void InitializeGame(InitializeGameRequest request)
    {
        var targetValue = _randomNumberGenerator.Generate(request.LeftBorder, request.RightBorder);

        var settings = new GameSettings(targetValue, request.AttemptCount);
        _settings = settings;
    }

    public string GuessNumber(int userGuess)
    {
        if (_settings == null)
            throw new BadRequestException("Вы не установили правила игры, чтобы в нее играть");

        if (_settings.AttemptCount <= 0)
            return $"Игра окончена. Вы проиграли. Загаданное число было {_settings.NumberToGuess}.";

        _settings.AttemptCount--;

        var result = CheckGuess(userGuess);
        return GetMessageForResult(result, userGuess);
    }

    public int GetTargetValue()
    {
        return _settings.NumberToGuess;
    }

    private GuessResult CheckGuess(int userGuess)
    {
        if (userGuess == _settings.NumberToGuess)
        {
            return GuessResult.Correct;
        }
        else if (userGuess > _settings.NumberToGuess)
        {
            return GuessResult.TooHigh;
        }
        else
        {
            return GuessResult.TooLow;
        }
    }

    private string GetMessageForResult(GuessResult result, int userGuess)
    {
        var messages = new Dictionary<GuessResult, string>
        {
            { GuessResult.Correct, "Поздравляем! Вы угадали число." },
            { GuessResult.TooHigh, $"Не верно, число меньше {userGuess}. Осталось {_settings.AttemptCount} попыток." },
            { GuessResult.TooLow, $"Не верно, число больше {userGuess}. Осталось {_settings.AttemptCount} попыток." }
        };

        return messages[result];
    }
}