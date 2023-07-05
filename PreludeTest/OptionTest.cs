using System.Collections;
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

    [Test]
    public void Option_Equals_Success()
    {
        {
            var opt1 = Option<int>.Some(1);
            var opt2 = Option<int>.Some(1);
            Assert.That(opt1, Is.EqualTo(opt2));
        }
        {
            var sameList = new List<string> { "0", "1", "asdf" };
            var opt1 = Option<IEnumerable<string>>.Some(sameList);
            var opt2 = Option<IEnumerable<string>>.Some(sameList);
            Assert.That(opt1, Is.EqualTo(opt2));
        }
        {
            var opt1 = Option<int>.Some(1);
            var opt2 = Option<int>.Some(2);
            Assert.That(opt1, Is.Not.EqualTo(opt2));
        }
        {
            var sameList1 = new List<string> { "0", "1", "asdf" };
            var sameList2 = new List<string> { "0", "1", "asdf" };
            var opt1 = Option<IEnumerable<string>>.Some(sameList1);
            var opt2 = Option<IEnumerable<string>>.Some(sameList2);
            Assert.That(opt1, Is.Not.EqualTo(opt2));
        }
    }
    
    [Test]
    public void Option_PredicateEquals_Success()
    {
        {
            Func<int, int, bool> comparer = (i, j) => i == j;
            var opt1 = Option<int>.Some(1);
            var opt2 = Option<int>.Some(1);
            Assert.That(opt1.PredicateEquals(opt2, comparer), Is.True);
        }
        {
            Func<IEnumerable<string>, IEnumerable<string>, bool> comparer = (x, y) => x.SequenceEqual(y);
            var sameList = new List<string> { "0", "1", "asdf" };
            var opt1 = Option<IEnumerable<string>>.Some(sameList);
            var opt2 = Option<IEnumerable<string>>.Some(sameList);
            Assert.That(opt1.PredicateEquals(opt2, comparer), Is.True);
        }
        {
            Func<IEnumerable<string>, IEnumerable<string>, bool> comparer = (x, y) => x.SequenceEqual(y);
            var sameList1 = new List<string> { "0", "1", "asdf" };
            var sameList2 = new List<string> { "0", "1", "asdf" };
            var opt1 = Option<IEnumerable<string>>.Some(sameList1);
            var opt2 = Option<IEnumerable<string>>.Some(sameList2);
            Assert.That(opt1.PredicateEquals(opt2, comparer), Is.True);
        }
        {
            Func<int, int, bool> comparer = (i, j) => i == j;
            var opt1 = Option<int>.Some(1);
            var opt2 = Option<int>.Some(2);
            Assert.That(opt1.PredicateEquals(opt2, comparer), Is.Not.True);
        }
        {
            Func<IEnumerable<string>, IEnumerable<string>, bool> comparer = (x, y) => x.SequenceEqual(y);
            var list1 = new List<string> { "0", "1", "asdf" };
            var list2 = new List<string> { "0", "1", "asdf", "2" };
            var opt1 = Option<IEnumerable<string>>.Some(list1);
            var opt2 = Option<IEnumerable<string>>.Some(list2);
            Assert.That(opt1.PredicateEquals(opt2, comparer), Is.Not.True);
        }
    }
}
