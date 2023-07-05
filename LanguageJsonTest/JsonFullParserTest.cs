using FunctionalParser;
using LanguageJson;
using Prelude;

namespace LanguageJsonTest;

using static JsonFullParser;

public class Tests
{
    [Test]
    public void TestNumber()
    {
        var cases = new[]
        {
            "0",
            "123",
            "123902",
        };

        foreach (var number in cases)
        {
            var actual = Number(number);
            var expectedValue = Option<IEnumerable<string>>.Some(new List<string> { number });
            Assert.That(actual.Remaining, Is.EqualTo(""));
            Assert.That(actual.Value.PredicateEquals(expectedValue, Enumerable.SequenceEqual), Is.True);
            Assert.That(actual.IsSuccess, Is.True);
        }
    }

    [Test]
    public void TestString()
    {
        var cases = new string[]
        {
            "\"asdf\"",
            "\"pog1champ\"",
            "\"hey5guys\"",
        };
        foreach (var str in cases)
        {
            var actual = Str(str);
            var expectedValue = Option<IEnumerable<string>>.Some(new List<string> { str });
            Assert.That(actual.Remaining, Is.EqualTo(""));
            Assert.That(actual.Value.PredicateEquals(expectedValue, Enumerable.SequenceEqual), Is.True);
            Assert.That(actual.IsSuccess, Is.True);
        }
    }
    
    [Test]
    public void TestJsonFullParse()
    {
        var json = "{\"test\":123}";
        Json(json);
    }
}