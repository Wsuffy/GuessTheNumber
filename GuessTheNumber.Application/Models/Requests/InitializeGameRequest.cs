namespace GuessTheNumber.Application.Models.Requests;

public sealed record InitializeGameRequest(int LeftBorder, int RightBorder, int AttemptCount);