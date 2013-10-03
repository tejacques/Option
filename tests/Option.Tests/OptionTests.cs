using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Option;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [TestFixture]
    public class OptionTests
    {
        [Test]
        public void TestCreate()
        {
            string s = "string";
            Option<string> option = s;

            Assert.IsTrue(option.HasValue);
            Assert.AreEqual(s, option.Value);

            Option<object> objectOption = (object)null;

            Assert.IsFalse(objectOption.HasValue);
            Assert.IsTrue(Option.None == objectOption);

            Option<object> nullOption = Option.Some<object>(null);

            Assert.IsTrue(nullOption.HasValue);
            Assert.AreEqual(null, nullOption.Value);
        }

        [Test]
        public void TestValue()
        {
            Option<object> objectOption = Option.None;
            Assert.Throws(
                typeof(InvalidOperationException),
                () => { var v = objectOption.Value; });

            object o = new object();
            objectOption = o;
            object o2 = objectOption;
        }

        [Test]
        public void TestTryGetValue()
        {
            Option<object> objectOption = Option.None;
            object o;
            Assert.IsFalse(objectOption.TryGetValue(out o));

            objectOption = new object();

            Assert.IsTrue(objectOption.TryGetValue(out o));
        }

        [Test]
        public void TestValueOrDefault()
        {
            Option<object> objectOption = Option.None;
            Assert.AreEqual(null, objectOption.ValueOrDefault);

            object o = new object();
            objectOption = o;

            Assert.AreEqual(o, objectOption.Value);

            var optionEqual = objectOption;
            var optionNotEqual = new object();
            Assert.IsTrue(objectOption.Equals(optionEqual));
            Assert.IsFalse(objectOption.Equals(optionNotEqual));
            Assert.IsFalse(objectOption.Equals(Option<object>.None));
        }

        [Test]
        public void TestOptionPatternMatcherT()
        {
            Option<int> optionNone = Option.None;
            Option<int> optionSome = 10;
            Option<int> optionOne = 1;

            bool matchedNone = false;
            bool matchedSome = false;
            bool matchedOne = false;

            Action resetMatchVars = () =>
            {
                matchedNone = false;
                matchedSome = false;
                matchedOne = false;
            };

            var staticMatcher = Option<int>.PatternMatch()
                .None(() => matchedNone = true)
                .Some(1, () => matchedOne = true)
                .Some((x) => matchedSome = true);

            var matcher = optionSome.Match()
                .None(() => matchedNone = true)
                .Some(1, () => matchedOne = true)
                .Some((x) => matchedSome = true);

            staticMatcher.Result();
            
            Assert.IsTrue(matchedNone);
            Assert.IsFalse(matchedSome);
            Assert.IsFalse(matchedOne);

            resetMatchVars();

            staticMatcher.Result(null);

            Assert.IsFalse(matchedNone);
            Assert.IsFalse(matchedSome);
            Assert.IsFalse(matchedOne);

            resetMatchVars();

            matcher.Result();
            Assert.IsFalse(matchedNone);
            Assert.IsTrue(matchedSome);
            Assert.IsFalse(matchedOne);

            resetMatchVars();

            matcher.Result(optionNone);
            Assert.IsTrue(matchedNone);
            Assert.IsFalse(matchedSome);
            Assert.IsFalse(matchedOne);

            resetMatchVars();

            matcher.Result(optionSome);
            Assert.IsFalse(matchedNone);
            Assert.IsTrue(matchedSome);
            Assert.IsFalse(matchedOne);

            resetMatchVars();

            matcher.Result(optionOne);
            Assert.IsFalse(matchedNone);
            Assert.IsFalse(matchedSome);
            Assert.IsTrue(matchedOne);

            Assert.Throws(typeof(InvalidOperationException),
                () => matcher.None(() => { }));
            Assert.Throws(typeof(InvalidOperationException),
                () => matcher.Some((x) => { }));
            Assert.Throws(typeof(InvalidOperationException),
                () => matcher.Some(1, () => { }));
        }

        [Test]
        public void TestOptionPatternMatcherTInTOut()
        {
            Option<int> optionNone = Option.None;
            Option<int> optionSome = 10;
            Option<int> optionOne = 1;

            var staticMatcher = Option<int>.PatternMatch<int>();

            var matcher =  optionSome.Match<int>()
                .None(() => 0)
                .Some((x) => 1)
                .Some(1, () => 2);

            Assert.AreEqual(default(int), staticMatcher.Result());
            Assert.AreEqual(default(int), staticMatcher.Result(null));
            Assert.AreEqual(1, matcher.Result());
            Assert.AreEqual(0, matcher.Result(optionNone));
            Assert.AreEqual(1, matcher.Result(optionSome));
            Assert.AreEqual(2, matcher.Result(optionOne));

            Assert.Throws(typeof(InvalidOperationException),
                () => matcher.None(() => 0));
            Assert.Throws(typeof(InvalidOperationException),
                () => matcher.Some((x) => 1));
            Assert.Throws(typeof(InvalidOperationException),
                () => matcher.Some(1, () => 2));
        }

        [Test]
        public void TestImplicitOperators()
        {
            Option<int?> o1 = 1;                        // Some<int?>(1)
            Option<int?> o2 = Option.Some<int?>(null);  // Some<int?>(null)
            Option<int?> o3 = (int?)null;               // None<int?>

            Assert.AreEqual(true, o1.HasValue);
            Assert.AreEqual(true, o2.HasValue);
            Assert.AreEqual(false, o3.HasValue);

            int? i1 = o1;
            int? i2 = o2;

            Assert.Throws(typeof(InvalidOperationException), () =>
            {
                int? i3 = o3;
            });
        }

        [Test]
        public void TestEquals()
        {
            Option<int> o = 1;
            Option<int> oSame = 1;
            Option<bool> oDifferent = false;

            Assert.IsTrue(o.Equals((object)oSame));
            Assert.IsFalse(o.Equals(oDifferent));
            Assert.IsFalse(o.Equals(null));
        }

        [Test]
        public void TestEqualsOperators()
        {
            Option<int> o = 1;
            Option<int> oSame = 1;
            Option<int> oDifferent = 2;

            Option<int> oNull = null;

            Assert.IsTrue(o == oSame);
            Assert.IsFalse(o == oDifferent);
            Assert.IsTrue(o != oDifferent);
            Assert.IsTrue(oNull != o);
            Assert.IsTrue(null != o);
            Assert.IsFalse(oNull != null);
            Assert.IsFalse(null != oNull);
        }

        [Test]
        public void TestHashCode()
        {
            int i = 1;
            Option<int> o1 = i;
            Option<int> o2 = Option<int>.None;

            Assert.AreEqual(i.GetHashCode(), o1.GetHashCode());
            Assert.AreEqual(0, o2.GetHashCode());
        }

        [Test]
        public void TestSome()
        {
            Some<int> s = Option.Some(0);
            s.Value = 1;

            Assert.AreEqual(1, s.Value);
        }
    }
}
