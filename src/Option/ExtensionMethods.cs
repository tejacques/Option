using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Option
{
    /// <summary>
    /// Extension methods for turning values into options
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Converts a value to an Option&lt;T&gt;.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>
        /// Option&lt;T&gt;.None if value is null, otherwise an
        /// Option&lt;T&gt; whose value is set to <paramref name="value"/>.
        /// </returns>
        public static Option<T> ToOption<T>(this T value)
        {
            return Option.Create(value);
        }

        /// <summary>
        /// Converts a value to an Option&lt;T&gt;.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>
        /// Option&lt;T&gt;.None if value is null, otherwise an
        /// Option&lt;T&gt; whose value is set to <paramref name="value"/>.
        /// </returns>
        public static Option<T> ToOption<T>(this Nullable<T> value)
            where T: struct
        {
            return Option.Create(value);
        }
    }
}
