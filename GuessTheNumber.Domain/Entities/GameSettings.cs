namespace GuessTheNumber.Domain.Entities;

/// <summary>
/// Сущность, использумая для настройки игры
/// NumberToGuess - Число, которое нужно отгадать
/// AttemptCount - количество попыток для того чтобы угадать
/// </summary>
public class GameSettings
{
    public int NumberToGuess { get; set; }
    public int AttemptCount { get; set; }

    public GameSettings(int numberToGuess, int attemptCount)
    {
        NumberToGuess = numberToGuess;
        AttemptCount = attemptCount;
    }
}