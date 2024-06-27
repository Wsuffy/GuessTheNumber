using GuessTheNumber.Application.Models.Requests;

namespace GuessTheNumber.Abstractions.Interfaces;

public interface IGameManager
{
    public Task<Guid> InitializeGame(InitializeGameRequest request, CancellationToken cancellationToken);
    public Task<string> GuessNumber(Guid sessionId, int userGuess, CancellationToken cancellationToken);

    /*public int GetTargetValue();*/
}