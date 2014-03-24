using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Option
{
    internal static class EmptyArray<TResult>
    {
        public static readonly TResult[] Array = new TResult[0];
    }
}
