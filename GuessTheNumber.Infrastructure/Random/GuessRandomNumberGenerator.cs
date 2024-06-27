using GuessTheNumber.Abstractions.Interfaces;

namespace GuessTheNumber.Infrastructure.Random;

public class GuessRandomNumberGenerator : IGuessRandomNumberGenerator
{
    private readonly System.Random _random = RandomProvider.GetThreadRandom();

    public int Generate(int minValue, int maxValue)
    {
        return _random.Next(minValue, maxValue + 1);
    }
}