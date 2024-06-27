namespace GuessTheNumber.Application.Models.Responses;

public sealed record GetGameSessionResponse(int TargetValue, int Attempts);