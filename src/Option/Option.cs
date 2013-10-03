using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace System.Option
{
    /// <summary>
    /// A generic Option type that allows for an explicit difference
    /// between an intentionally set value, and a default value of None.
    /// </summary>
    /// <typeparam name="T">The type to create an option for.</typeparam>
    [DebuggerDisplay("HasValue = {_hasValue}, Value = {_value}")]
    public class Option<T> : IEquatable<Option<T>>
    {
        /// <summary>
        /// The value of the option.
        /// </summary>
        protected T _value;

        /// <summary>
        /// The bool indicating whether the option has a value.
        /// </summary>
        protected readonly bool _hasValue;

        private static None<T> _none = new None<T>();

        /// <summary>
        /// Creates a new option from a specified value.
        /// </summary>
        /// <typeparam name="T">The type to create an option for.</typeparam>
        /// <param name="value">The value to create an option for.</param>
        /// <returns>
        /// A new Option&lt;T&gt; whose value is
        /// set to <paramref name="value"/>.
        /// </returns>
        public static Some<T> Some(T value)
        {
            return new Some<T>(value);
        }

        /// <summary>
        /// The Option indication there is no value.
        /// </summary>
        public static None<T> None
        {
            get { return _none; }
        }

        /// <summary>
        /// True if the option has a value, false otherwise.
        /// </summary>
        public bool HasValue
        {
            get { return _hasValue; }
        }

        /// <summary>
        /// Gets the value of the option
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the option does not have a value.
        /// </exception>
        [DebuggerDisplay("{_value}")]
        public T Value
        {
            get
            {
                if (!HasValue)
                {
                    throw new InvalidOperationException(
                        "The option does not have a value");
                }

                return _value;
            }
        }

        /// <summary>
        /// Gets the value of the option if present,
        /// and the default value otherwise.
        /// </summary>
        public T ValueOrDefault
        {
            get { return _hasValue ? _value : default(T); }
        }

        /// <summary>
        /// Tries to get the value of an option and place
        /// it in the referenced result.
        /// </summary>
        /// <param name="result">
        /// The location to store the option's value.
        /// </param>
        /// <returns>true if the option has a value, false otherwise.</returns>
        public bool TryGetValue(out T result)
        {
            result = this.ValueOrDefault;

            return this.HasValue;
        }

        internal Option()
        {
            _hasValue = false;
        }

        internal Option(T value)
        {
            _hasValue = true;
            _value = value;
        }

        #region Pattern Matching

        /// <summary>
        /// Creates and returns an OptionPatternMatcher&lt;T&gt;
        /// made from the current option.
        /// </summary>
        /// <returns>
        /// An OptionPatternMatcher&lt;T&gt; made from the current option.
        /// </returns>
        public OptionPatternMatcher<T> Match()
        {
            return new OptionPatternMatcher<T>(this);
        }

        /// <summary>
        /// Creates and returns an OptionPatternMatcher&lt;T&gt;.
        /// </summary>
        /// <returns>
        /// An OptionPatternMatcher&lt;T&gt;.
        /// </returns>
        public static OptionPatternMatcher<T> PatternMatch()
        {
            return new OptionPatternMatcher<T>();
        }

        /// <summary>
        /// Creates and returns an OptionPatternMatcher&lt;T,TOut&gt;
        /// made from the current option.
        /// </summary>
        /// <typeparam name="TOut">
        /// The type which the OptionPatternMatcher&lt;T,TOut&gt; will return.
        /// </typeparam>
        /// <returns>
        /// An OptionPatternMatcher&lt;T,TOut&gt; made from the current option.
        /// </returns>
        public OptionPatternMatcher<T, TOut> Match<TOut>()
        {
            return new OptionPatternMatcher<T, TOut>(this);
        }

        /// <summary>
        /// Creates and returns an OptionPatternMatcher&lt;T,TOut&gt;.
        /// </summary>
        /// <typeparam name="TOut">
        /// The type which the OptionPatternMatcher&lt;T,TOut&gt; will return.
        /// </typeparam>
        /// <returns>
        /// An OptionPatternMatcher&lt;T,TOut&gt;.
        /// </returns>
        public static OptionPatternMatcher<T, TOut> PatternMatch<TOut>()
        {
            return new OptionPatternMatcher<T, TOut>();
        }

        #endregion


        #region Operators

        /// <summary>
        /// Implicitly converts an Option to an Option&lt;T&gt;.
        /// </summary>
        /// <param name="option">The option to convert.</param>
        /// <returns>Option&lt;T&gt;.None</returns>
        public static implicit operator Option<T>(Option option)
        {
            return Option<T>.None;
        }

        /// <summary>
        /// Implicitly converts a value to an Option&lt;T&gt;.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>
        /// Option&lt;T&gt;.None if value is null, otherwise an
        /// Option&lt;T&gt; whose value is set to <paramref name="value"/>.
        /// </returns>
        public static implicit operator Option<T>(T value)
        {
            return null == value
                ? (Option<T>)Option<T>.None
                : new Some<T>(value);
        }

        /// <summary>
        /// Implicitly converts an Option&lt;T&gt; to a T.
        /// </summary>
        /// <param name="option">The option to convert.</param>
        /// <returns>
        /// Option&lt;T&gt;.Value, which will throw InvalidOperationException
        /// if the option does not have a value.
        /// </returns>
        public static implicit operator T(Option<T> option)
        {
            return option.Value;
        }

        /// <summary>
        /// Comapares two options for equality.
        /// </summary>
        /// <param name="lhs">The option on the left hand side.</param>
        /// <param name="rhs">The option on the right hand side.</param>
        /// <returns>
        /// true if the options' values are equal
        /// or both options are Option&lt;T&gt;.None,
        /// and false if the options' values are not
        /// equal or only one option is Option&lt;T&gt;.None
        /// </returns>
        public static bool operator ==(Option<T> lhs, Option<T> rhs)
        {
            if (null == (object)lhs)
            {
                if (null == (object)rhs)
                {
                    return true;
                }
                return false;
            }
            return lhs.Equals(rhs);
        }

        /// <summary>
        /// Comapares two options for inequality.
        /// </summary>
        /// <param name="lhs">The option on the left hand side.</param>
        /// <param name="rhs">The option on the right hand side.</param>
        /// <returns>
        /// true if the options' values are not
        /// equal or only one option is Option&lt;T&gt;.None,
        /// and false if the options' values are equal
        /// or both options are Option&lt;T&gt;.None.
        /// </returns>
        public static bool operator !=(Option<T> lhs, Option<T> rhs)
        {
            if (null == (object)lhs)
            {
                if (null == (object)rhs)
                {
                    return false;
                }
                return true;
            }
            return !lhs.Equals(rhs);
        }

        #endregion

        #region IEquatable<T> Members

        /// <summary>
        /// Compares the option to another option for equality.
        /// </summary>
        /// <param name="other">The option to compare to.</param>
        /// <returns>
        /// true if the options' values are equal
        /// or both options are Option&lt;T&gt;.None,
        /// and false if the options' values are not
        /// equal or only one option is Option&lt;T&gt;.None
        /// </returns>
        public bool Equals(Option<T> other)
        {
            if (null == (object)other
                || this.HasValue != other.HasValue)
            {
                return false;
            }

            // Both are Option<T>.None
            if (!this.HasValue)
            {
                return true;
            }

            return EqualityComparer<T>.Default.Equals(_value, other.Value);
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Compares the option to another object for equality.
        /// </summary>
        /// <param name="other">The object to compare to.</param>
        /// <returns>
        /// true if the object is an Option&lt;T&gt; and
        /// the options' values are equal
        /// or both options are Option&lt;T&gt;.None,
        /// and false if the object is not an Option&lt;T&gt; or
        /// the options' values are not equal
        /// or only one option is Option&lt;T&gt;.None
        /// </returns>
        public override bool Equals(object other)
        {
            if (other is Option<T>)
            {
                return Equals((Option<T>)other);
            }

            return false;
        }

        /// <summary>
        /// Gets the HashCode for the Option&lt;T&gt;.
        /// </summary>
        /// <returns>
        /// 0 if the Option is Option.None, otherwise
        /// returns the hash code of the value.
        /// </returns>
        public override int GetHashCode()
        {
            if (!HasValue)
            {
                return 0;
            }

            return EqualityComparer<T>.Default.GetHashCode(_value);
        }

        #endregion
    }

    /// <summary>
    /// A subclass of Option indicating that there is be a value.
    /// </summary>
    /// <typeparam name="T">The type of option.</typeparam>
    [DebuggerDisplay("Value = {Value}")]
    public class Some<T> : Option<T>
    {
        internal Some(T value) : base(value) { }

        /// <summary>
        /// Gets the value of the option
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the option does not have a value.
        /// </exception>
        [DebuggerDisplay("{_value}")]
        public new T Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }
    }

    /// <summary>
    /// A subclass of Option indicating that there is no value.
    /// </summary>
    /// <typeparam name="T">The type of option.</typeparam>
    [DebuggerDisplay("None")]
    public class None<T> : Option<T>
    {
        internal None() : base() { }
    }

    /// <summary>
    /// An Option type that allows the use of Option.None
    /// as well as the creation of Options.
    /// </summary>
    public sealed class Option
    {
        private static Option _none = new Option();

        private Option()
        {
        }

        /// <summary>
        /// Creates a new option from a specified value.
        /// </summary>
        /// <typeparam name="T">The type to create an option for.</typeparam>
        /// <param name="value">The value to create an option for.</param>
        /// <returns>
        /// A new Option&lt;T&gt; whose value is
        /// set to <paramref name="value"/>.
        /// </returns>
        public static Some<T> Some<T>(T value)
        {
            return Option<T>.Some(value);
        }

        /// <summary>
        /// The default Option type specifying there is no value.
        /// </summary>
        public static Option None
        {
            get { return _none; }
        }
    }
}
