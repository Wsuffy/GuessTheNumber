namespace GuessTheNumber.Infrastructure.Random;

public class RandomProvider
{
    private static readonly ThreadLocal<System.Random> RandomWrapper = new(() => new System.Random(Guid.NewGuid().GetHashCode()));

    public static System.Random GetThreadRandom()
    {
        return RandomWrapper.Value!;
    }
}