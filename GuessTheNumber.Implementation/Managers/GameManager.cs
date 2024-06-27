using GuessTheNumber.Abstractions.Interfaces;
using GuessTheNumber.Application.Models.Enums;
using GuessTheNumber.Application.Models.Requests;
using GuessTheNumber.Application.Models.Responses;
using GuessTheNumber.Domain.Exceptions;
using GuessTheNumber.Implementation.Commands;
using GuessTheNumber.Implementation.Queries;
using MediatR;

namespace GuessTheNumber.Implementation.Managers;

public class GameManager : IGameManager
{
    private readonly IMediator _mediator;

    public GameManager(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<Guid> InitializeGame(InitializeGameRequest request, CancellationToken cancellationToken)
    {
        var addGameConfigurationResult = await _mediator.Send(
            new AddGameConfigurationCommand(request.LeftBorder, request.RightBorder, request.AttemptCount),
            cancellationToken);

        return addGameConfigurationResult.SessionId;
    }

    public async Task<string> GuessNumber(Guid sessionId, int userGuess, CancellationToken cancellationToken)
    {
        var session = await _mediator.Send(new GetGameSessionByIdQuery(sessionId), cancellationToken);

        if (session == null)
            throw new BadRequestException("Вы не установили правила игры, чтобы в нее играть");

        if (session.Attempts <= 0)
        {
            await _mediator.Send(new RemoveGameConfigurationCommand(sessionId), cancellationToken);
            return $"Игра окончена. Вы проиграли. Загаданное число было {session.TargetValue}.";
        }

        var result = await CheckGuess(sessionId, userGuess, session.TargetValue, cancellationToken);
        return GetMessageForResult(result.Attempts, result.GuessResult, userGuess);
    }

    private async Task<CheckGuessResponse> CheckGuess(Guid sessionId, int userGuess, int numberToGuess,
        CancellationToken cancellationToken)
    {
        if (userGuess == numberToGuess)
        {
            var correctResult = await _mediator.Send(new RemoveGameConfigurationCommand(sessionId), cancellationToken);
            return new CheckGuessResponse(GuessResult.Correct, correctResult.AttemptCount);
        }

        if (userGuess > numberToGuess)
        {
            var toHighResult = await _mediator.Send(new DecreaseAmountOfAttemptsCommand(sessionId), cancellationToken);
            return new CheckGuessResponse(GuessResult.TooHigh, toHighResult);
        }

        var toLowResult = await _mediator.Send(new DecreaseAmountOfAttemptsCommand(sessionId), cancellationToken);
        return new CheckGuessResponse(GuessResult.TooLow, toLowResult);
    }

    private string GetMessageForResult(int attemptCount, GuessResult result, int userGuess)
    {
        var messages = new Dictionary<GuessResult, string>
        {
            { GuessResult.Correct, "Поздравляем! Вы угадали число." },
            { GuessResult.TooHigh, $"Не верно, число меньше {userGuess}. Осталось {attemptCount} попыток." },
            { GuessResult.TooLow, $"Не верно, число больше {userGuess}. Осталось {attemptCount} попыток." }
        };

        return messages[result];
    }
}