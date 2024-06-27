namespace GuessTheNumber.Application.Models.DTOs;

public class GameConfigurationDto
{
    public Guid SessionId { get; set; }
    public int TargetValue { get; set; }
    public int AttemptCount { get; set; }

    public GameConfigurationDto(Guid sessionId, int targetValue, int attemptCount)
    {
        SessionId = sessionId;
        TargetValue = targetValue;
        AttemptCount = attemptCount;
    }
}