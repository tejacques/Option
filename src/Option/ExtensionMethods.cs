using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Functional.Option
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
        /// <typeparam name="T">The type of the elements.</typeparam>
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

        /// <summary>
        /// Flattens a sequence of Option&lt;T&gt; into one sequence of T
        /// elements where the value of the Option&lt;T&gt; was Some&lt;T&gt;
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="source">A sequence of Option&lt;T&gt;.</param>
        /// <returns>
        /// A sequence of T elements where the value of the Option&lt;T&gt;
        /// was Some&lt;T&gt;.
        /// </returns>
        public static IEnumerable<T> Flatten<T>(
            this IEnumerable<Option<T>> source)
        {
            foreach (var option in source)
            {
                if(option.HasValue)
                {
                    yield return option.Value;
                }
            }
        }
    }
}
