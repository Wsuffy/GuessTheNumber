using GuessTheNumber.Abstractions.Interfaces;
using GuessTheNumber.Infrastructure.Random;

namespace GuessTheNumber.Unit;

[TestFixture]
public class RandomProviderTests
{
    private IGuessRandomNumberGenerator _generator;

    [SetUp]
    public void SetUp()
    {
        _generator = new GuessRandomNumberGenerator();
    }

    /// <summary>
    /// Метод GetThreadRandom Должен вернуть случайный инстанс
    /// </summary>
    [Test]
    public void GetThreadRandom_ShouldReturnRandomInstance()
    {
        var random = RandomProvider.GetThreadRandom();

        Assert.That(random, Is.Not.Null);
    }

    /// <summary>
    /// GetThreadRandom должен вернуть разные инстансы для разных Тредов
    /// </summary>
    [Test]
    public void GetThreadRandom_ShouldReturnDifferentInstancesForDifferentThreads()
    {
        Random random1 = null;
        Random random2 = null;

        var task1 = Task.Run(() => random1 = RandomProvider.GetThreadRandom());
        var task2 = Task.Run(() => random2 = RandomProvider.GetThreadRandom());

        Task.WaitAll(task1, task2);
        Assert.Multiple(() =>
        {
            Assert.That(random1, Is.Not.Null);
            Assert.That(random2, Is.Not.Null);
            Assert.That(random2, Is.Not.EqualTo(random1));
        });
    }

    /// <summary>
    /// Метод Generate должен вернуть значение которое находится между левым и правым значением
    /// </summary>
    /// <param name="minValue">Минимальное значение</param>
    /// <param name="maxValue">Максимальное значение</param>
    [Test]
    [TestCase(1, 10)]
    [TestCase(0, 1)]
    [TestCase(-100, 100)]
    [TestCase(int.MinValue, int.MaxValue)]
    public void Generate_ShouldReturnNumberWithinRange(int minValue, int maxValue)
    {
        var result = _generator.Generate(minValue, maxValue);

        Assert.That(result, Is.GreaterThanOrEqualTo(minValue).And.LessThanOrEqualTo(maxValue));
    }

    /// <summary>
    /// Метод Generate должен вернуть одно единственное значение, если левая и правая граница равны
    /// </summary>
    /// <param name="minValue"></param>
    /// <param name="maxValue"></param>
    [Test]
    [TestCase(0, 0)]
    [TestCase(-1, -1)]
    [TestCase(100, 100)]
    public void Generate_ShouldReturnMinValueWhenMinAndMaxAreEqual(int minValue, int maxValue)
    {
        var result = _generator.Generate(minValue, maxValue);

        Assert.That(result, Is.EqualTo(minValue));
    }

    /// <summary>
    /// Метод Generate должен вернуть разные значения из Range
    /// </summary>
    /// <param name="minValue"></param>
    /// <param name="maxValue"></param>
    /// <param name="iterations"></param>
    [Test]
    [TestCase(1, 10, 1000)]
    [TestCase(-100, 100, 1000)]
    public void Generate_ShouldReturnDifferentNumbersWithinRange(int minValue, int maxValue, int iterations)
    {
        var results = new HashSet<int>();

        for (var i = 0; i < iterations; i++)
            results.Add(_generator.Generate(minValue, maxValue));

        Assert.Multiple(() =>
        {
            Assert.That(results, Has.Count.GreaterThan(1),
                "Generate should produce multiple distinct numbers within the range.");
            Assert.That(results, Is.All.InRange(minValue, maxValue));
        });
    }
}