namespace GuessTheNumber.Abstractions.Interfaces;

public interface IGuessRandomNumberGenerator
{
    public int Generate(int minValue, int maxValue);
}