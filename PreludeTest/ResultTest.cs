using Prelude;

namespace PreludeTest;

[TestFixture]
public class ResultTest
{
    [Test]
    public void Result_ReturnsCorrectly()
    {
        var r = Result<string, string>.Of("hello")
            .SelectOk(x => x + " world!")
            .OkValue()
            .Unwrap();
        Assert.That(r, Is.EqualTo("hello world!"));
    }
}