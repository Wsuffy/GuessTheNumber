namespace GuessTheNumber.Application.Models.Requests;

public sealed record GuessRequest(int SessionId, int GuessedNumber);