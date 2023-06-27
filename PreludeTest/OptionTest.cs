using Prelude;

namespace PreludeTest;

[TestFixture]
public class OptionTest
{
    [Test]
    public void Option_Select_Success()
    {
        {
            var opt = Option<int>.Some(1)
                .Select(x => x + 1)
                .Select(x => x + 2);
            Assert.True(opt.IsSome());
            Assert.False(opt.IsNone());
            Assert.That(opt, Is.EqualTo(Option<int>.Some(4)));
        }
        {
            var opt = Option<int>.None()
                .Select(x => x + 1)
                .Select(x => x + 2);
            Assert.True(opt.IsNone());
            Assert.False(opt.IsSome());
            Assert.That(opt, Is.EqualTo(Option<int>.None()));
        }
    }

    [Test]
    public void Option_SelectMany_Success()
    {
        {
            var opt = Option<int>.Some(1)
                .Select(x => x + 1)
                .Select(x => x + 2)
                .SelectMany(x => x == 4
                    ? Option<string>.Some("is 4")
                    : Option<string>.None())
                .SelectMany(x => x == "is 4"
                    ? Option<string>.Some("is actually 4")
                    : Option<string>.None());
            Assert.True(opt.IsSome());
            Assert.False(opt.IsNone());
            Assert.That(opt, Is.EqualTo(Option<string>.Some("is actually 4")));
        }
        {
            var opt = Option<int>.Some(1)
                .Select(x => x + 1)
                .Select(x => x + 2)
                .SelectMany(x => x == 5
                    ? Option<string>.Some("is 5")
                    : Option<string>.None())
                .SelectMany(x => x == "is 5"
                    ? Option<string>.Some("revert...")
                    : Option<string>.None());
            Assert.True(opt.IsNone());
            Assert.False(opt.IsSome());
            Assert.That(opt, Is.EqualTo(Option<string>.None()));
        }
    }

    [Test]
    public void Option_Where_Success()
    {
        {
            var opt = Option<int>.Some(10)
                .Where(x => x == 10)
                .Where(x => x == 10);
            Assert.True(opt.IsSome());
            Assert.False(opt.IsNone());
            Assert.That(opt, Is.EqualTo(Option<int>.Some(10)));
        }
        {
            var opt = Option<int>.Some(10)
                .Where(x => x == 11)
                .Where(x => x == 10);
            Assert.True(opt.IsNone());
            Assert.False(opt.IsSome());
            Assert.That(opt, Is.EqualTo(Option<int>.None()));
        }
        {
            var opt = Option<int>.Some(10)
                .Where(x => x == 10)
                .Where(x => x == 11);
            Assert.True(opt.IsNone());
            Assert.False(opt.IsSome());
            Assert.That(opt, Is.EqualTo(Option<int>.None()));
        }
    }

    [Test]
    public void Option_OrElse_Success()
    {
        {
            var opt = Option<int>.Some(1)
                .OrElse(() => Option<int>.Some(100));
            Assert.True(opt.IsSome());
            Assert.False(opt.IsNone());
            Assert.That(opt, Is.EqualTo(Option<int>.Some(1)));
        }
        {
            var opt = Option<int>.None()
                .OrElse(() => Option<int>.Some(100));
            Assert.True(opt.IsSome());
            Assert.False(opt.IsNone());
            Assert.That(opt, Is.EqualTo(Option<int>.Some(100)));
        }
    }

    [Test]
    public void Option_OrElseThrow_Success()
    {
        {
            var opt = Option<int>.Some(1)
                .OrElseThrow(() => new Exception(""));
            Assert.True(opt.IsSome());
            Assert.False(opt.IsNone());
            Assert.That(opt, Is.EqualTo(Option<int>.Some(1)));
        }
        {
            Assert.Throws<Exception>(() =>
            {
                var _ = Option<int>.None()
                    .OrElseThrow(() => new Exception(""));
            });
        }
    }
}
