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

    [Test]
    public void Result_NormalUsage_ReturnsCorrectly()
    {
        {
            var r = Result<string, string>.Of("hello")
                .SelectOk(x => x + " world!")
                .SelectErr(e => e + "NOTHING SHOULD HAPPEN")
                .SelectMany(x => x == "helloworld!"
                    ? Result<string, string>.Of("hello again!")
                    : Result<string, string>.Err("no more hello..."));
            Assert.That(r.IsErr(), Is.True);
            Assert.That(r.IsOk(), Is.False);
            Assert.That(r, Is.EqualTo(Result<string, string>.Err("no more hello...")));
        }
    }
}