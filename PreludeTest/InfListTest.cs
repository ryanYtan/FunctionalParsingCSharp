using Prelude;

namespace PreludeTest;

[TestFixture]
public class InfListTest
{
    [Test]
    public void Cycle_NormalCollection_ReturnsCorrectly()
    {
        var expected = new[]
        {
            1, 2, 3, 4, 5, 1, 2, 3, 4, 5, 1, 2, 3
        };
        var actual = new[] { 1, 2, 3, 4, 5 }
            .Cycle()
            .Take(13)
            .ToArray();
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void Cycle_EmptyCollection_ThrowsException()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            var _ = Array.Empty<int>().Cycle().ToArray();
        });
    }

    [Test]
    public void Replicate_NormalCount_ReturnsCorrectly()
    {
        var expected = new[] { 2, 2, 2, 2, 2 };
        var actual = InfList.Replicate(2, 5).ToArray();
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void Replicate_ZeroCount_ReturnsZeroEnumerable()
    {
        var expected = Array.Empty<int>();
        var actual = InfList.Replicate(1, 0).ToArray();
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void Iterate_ReturnsCorrectly()
    {
        var expected = new[] { 1, 2, 4, 8, 16 };
        var actual = InfList.Iterate(1, x => x * 2)
            .Take(5)
            .ToArray();
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void Generate_ReturnsCorrectly()
    {
        var expected = new[] { 5, 5, 5, 5, 5 };
        var actual = InfList.Generate(() => 5)
            .Take(5)
            .ToArray();
        Assert.That(actual, Is.EqualTo(expected));
    }
}