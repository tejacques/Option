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
    public class OptionBenchmarks
    {
        public static int loops = 1000000;
        [Test]
        public void _Jit()
        {
            var tmp = loops;
            loops = 1;
            BenchmarkMatchT();
            BenchmarkMatchTInTOut();
            BenchmarkOptionPatternMatcherT();
            BenchmarkOptionPatternMatcherTInTOut();
            loops = tmp;
        }

        [Test]
        public void BenchmarkMatchT()
        {
            Option<int> o = Option.None;

            for (int i = 0; i < loops; i++)
            {
                o.Match(None: () => { }, Some: x => { });
            }
        }

        [Test]
        public void BenchmarkMatchTInTOut()
        {
            Option<int> o = Option.None;

            for (int i = 0; i < loops; i++)
            {
                var v = o.Match(
                    None: () => 0,
                    Some: x => x);
            }
        }

        [Test]
        public void BenchmarkOptionPatternMatcherT()
        {
            Option<int> o = Option.None;

            for (int i = 0; i < loops; i++)
            {
                o.Match()
                    .None(() => { })
                    .Some((x) => { })
                    .Result();
            }
        }

        [Test]
        public void BenchmarkOptionPatternMatcherTCached()
        {
            Option<int> o = Option.None;

            var matcher = Option<int>.PatternMatch()
                .None(() => { })
                .Some((x) => { });

            for (int i = 0; i < loops; i++)
            {
                matcher.Result(o);
            }
        }

        [Test]
        public void BenchmarkOptionPatternMatcherTInTOut()
        {
            Option<int> o = Option.None;

            for (int i = 0; i < loops; i++)
            {
                var v = o.Match<int>()
                    .None(() => 0)
                    .Some((x) => 1)
                    .Result();
            }
        }

        [Test]
        public void BenchmarkOptionPatternMatcherTInTOutCached()
        {
            Option<int> o = Option.None;
            var matcher = Option<int>.PatternMatch<int>()
                .None(() => 0)
                .Some((x) => 1);

            for (int i = 0; i < loops; i++)
            {
                var v = matcher.Result(o);
            }
        }
    }
}
