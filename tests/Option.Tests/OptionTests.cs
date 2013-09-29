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
            Option<string> option = s.ToOption();

            Assert.IsTrue(option.HasValue);
            Assert.AreEqual(s, option.Value);

            Option<object> objectOption = (object)null;

            Assert.IsFalse(objectOption.HasValue);
            Assert.IsTrue(Option.None == objectOption);

            Option<object> nullOption = Option.From<object>(null);

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
            var optionNotEqual = (new object()).ToOption();
            Assert.IsTrue(objectOption.Equals(optionEqual));
            Assert.IsFalse(objectOption.Equals(optionNotEqual));
            Assert.IsFalse(objectOption.Equals(Option<object>.None));
        }
    }
}
