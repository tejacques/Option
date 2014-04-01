using Functional.Option;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tests
{
    [TestFixture]
    public class OptionBenchmarks
    {
        public static int loops = 10000000;
        [Test]
        public void _Jit()
        {
            var tmp = loops;
            loops = 1000;

            BenchmarkCreate();

            BenchmarkTryGet();
            BenchmarkTryGetCached();

            BenchmarkMatchT();
            BenchmarkMatchTCached();

            BenchmarkMatchTInTOut();
            BenchmarkMatchTInTOutCached();

            BenchmarkOptionPatternMatcherT();
            BenchmarkOptionPatternMatcherTInTOut();

            BenchmarkOptionPatternMatcherTInTOutValues();
            BenchmarkOptionPatternMatcherTInTOutValuesCached();


            BenchmarkEnumerateOption();
            BenchmarkEnumerableForEachOption();
            BenchmarkEnumerableSelectOption();
            BenchmarkEnumerableWhereOption();
            BenchmarkEnumerableSelectManyOption();
            BenchmarkEnumerableFlattenOption();

            BenchmarkEnumerateArray();
            BenchmarkEnumerableSelectArray();
            BenchmarkEnumerableWhereArray();

            BenchmarkEnumerateList();
            BenchmarkEnumerableForEachList();
            BenchmarkEnumerableSelectList();
            BenchmarkEnumerableWhereList();
            loops = tmp;
        }

        [Test]
        public void BenchmarkCreate()
        {
            for (int i = 0; i < loops; i++)
            {
                Option<int> o = i;
            }
        }

        [Test]
        public void BenchmarkTryGet()
        {
            int sum = 0;
            for (int i = 0; i < loops; i++)
            {
                Option<int> o = i;

                int value;
                if (o.TryGetValue(out value))
                {
                    sum += value;
                }
            }
        }

        [Test]
        public void BenchmarkTryGetCached()
        {
            int sum = 0;
            Option<int> o = 0;

            for (int i = 0; i < loops; i++)
            {
                int value;
                if (o.TryGetValue(out value))
                {
                    sum += value;
                }
            }
        }

        [Test]
        public void BenchmarkMatchT()
        {
            for (int i = 0; i < loops; i++)
            {
                Option<int> o = i;
                o.Match(None: () => { }, Some: x => { });
            }
        }

        [Test]
        public void BenchmarkMatchTCached()
        {
            Option<int> o = 0;

            for (int i = 0; i < loops; i++)
            {
                o.Match(None: () => { }, Some: x => { });
            }
        }

        [Test]
        public void BenchmarkMatchTInTOut()
        {
            for (int i = 0; i < loops; i++)
            {
                Option<int> o = i;

                var v = o.Match(
                    None: () => 0,
                    Some: x => x);
            }
        }

        [Test]
        public void BenchmarkMatchTInTOutCached()
        {
            for (int i = 0; i < loops; i++)
            {
                Option<int> o = i;

                var v = o.Match(
                    None: () => 0,
                    Some: x => x);
            }
        }

        [Test]
        public void BenchmarkOptionPatternMatcherT()
        {
            for (int i = 0; i < loops; i++)
            {
                Option<int> o = Option.None;
                o.Match()
                    .None(() => { })
                    .Some((x) => { })
                    .Result();
            }
        }

        [Test]
        public void BenchmarkOptionPatternMatcherTCached()
        {
            var matcher = Option<int>.PatternMatch()
                .None(() => { })
                .Some((x) => { });

            for (int i = 0; i < loops; i++)
            {
                Option<int> o = Option.None;
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

        [Test]
        public void BenchmarkOptionPatternMatcherTInTOutValues()
        {
            Option<int> o = Option.None;

            for (int i = 0; i < loops; i++)
            {
                var v = o.Match<int>()
                    .None(() => 0)
                    .Some(0, () => 1)
                    .Some((x) => 2)
                    .Result();
            }
        }

        [Test]
        public void BenchmarkOptionPatternMatcherTInTOutValuesCached()
        {
            Option<int> o = Option.None;
            var matcher = Option<int>.PatternMatch<int>()
                .None(() => 0)
                .Some(0, () => 1)
                .Some((x) => 2);

            for (int i = 0; i < loops; i++)
            {
                var v = matcher.Result(o);
            }
        }

        [Test]
        public void BenchmarkEnumerateOption()
        {
            int sum = 0;
            Option<int> o = 1;

            for (int i = 0; i < loops; i++)
            {
                foreach (var value in o)
                {
                    sum += value;
                }
            }
        }

        [Test]
        public void BenchmarkEnumerableForEachOption()
        {
            int sum = 0;
            Option<int> o = 1;

            for (int i = 0; i < loops; i++)
            {
                o.ForEach(x => sum += x);
            }
        }

        [Test]
        public void BenchmarkEnumerableSelectOption()
        {
            Option<int> o = 1;

            for (int i = 0; i < loops; i++)
            {
                var s = o.Select(x => true);
            }
        }

        [Test]
        public void BenchmarkEnumerableWhereOption()
        {
            Option<int> o = 1;

            for (int i = 0; i < loops; i++)
            {
                var s = o.Where(x => true);
            }
        }

        [Test]
        public void BenchmarkEnumerableSelectManyOption()
        {
            Option<int>[] o = new Option<int>[] { 1 };

            for (int i = 0; i < loops; i++)
            {
                var sm = o.SelectMany(x => x);
            }
        }

        [Test]
        public void BenchmarkEnumerableFlattenOption()
        {
            Option<int>[] o = new Option<int>[] { 1 };

            for (int i = 0; i < loops; i++)
            {
                var s = o.Flatten();
            }
        }

        [Test]
        public void BenchmarkEnumerateArray()
        {
            int sum = 0;
            int[] arr = new int[] { 1 };

            for (int i = 0; i < loops; i++)
            {
                foreach (var value in arr)
                {
                    sum += value;
                }
            }
        }

        [Test]
        public void BenchmarkEnumerableSelectArray()
        {
            int[] arr = new int[] { 1 };

            for (int i = 0; i < loops; i++)
            {
                var s = arr.Select(x => true);
            }
        }

        [Test]
        public void BenchmarkEnumerableWhereArray()
        {
            int[] arr = new int[] { 1 };

            for (int i = 0; i < loops; i++)
            {
                var s = arr.Where(x => true);
            }
        }

        [Test]
        public void BenchmarkEnumerateList()
        {

            int sum = 0;
            List<int> list = new List<int> { 1 };

            for (int i = 0; i < loops; i++)
            {

                foreach (var value in list)
                {
                    sum += value;
                }
            }
        }

        [Test]
        public void BenchmarkEnumerableForEachList()
        {
            int sum = 0;
            List<int> list = new List<int> { 1 };
            list.ToArray();

            for (int i = 0; i < loops; i++)
            {
                list.ForEach(val => sum += val);
            }
        }

        [Test]
        public void BenchmarkEnumerableSelectList()
        {
            List<int> list = new List<int> { 1 };

            for (int i = 0; i < loops; i++)
            {
                var s = list.Select(x => true);
            }
        }

        [Test]
        public void BenchmarkEnumerableWhereList()
        {
            List<int> list = new List<int> { 1 };

            for (int i = 0; i < loops; i++)
            {
                var s = list.Where(x => true);
            }
        }

    }
}
