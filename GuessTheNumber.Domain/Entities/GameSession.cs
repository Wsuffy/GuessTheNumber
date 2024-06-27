namespace GuessTheNumber.Domain.Entities;

public class GameSession
{
    public Guid SessionId { get; set; }
    public int TargetValue { get; set; }
    public int AttemptCount { get; set; }
}