using GuessTheNumber.Application.Models.Requests;

namespace GuessTheNumber.Abstractions.Interfaces;

public interface IGameManager
{
    public void InitializeGame(InitializeGameRequest request);
    public string GuessNumber(int userGuess);

    public int GetTargetValue();
}