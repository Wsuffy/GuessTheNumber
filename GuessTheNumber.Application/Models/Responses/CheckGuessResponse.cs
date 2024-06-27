using GuessTheNumber.Application.Models.Enums;

namespace GuessTheNumber.Application.Models.Responses;

public sealed record CheckGuessResponse(GuessResult GuessResult, int Attempts);