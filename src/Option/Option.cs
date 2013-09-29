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
    public struct Option<T> : IEquatable<Option<T>>
    {
        private readonly T _value;
        private readonly bool _hasValue;

        private static Option<T> _none = new Option<T>();

        /// <summary>
        /// The Option indication there is no value.
        /// </summary>
        public static Option<T> None
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
        [DebuggerDisplay("_value")]
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

        internal Option(T value)
        {
            _hasValue = true;
            _value = value;
        }

        #region Operators

        /// <summary>
        /// Implicitely converts an Option to and Option<T>.
        /// </summary>
        /// <param name="option">The option to convert.</param>
        /// <returns>Option<T>.None</returns>
        public static implicit operator Option<T>(Option option)
        {
            return Option<T>.None;
        }

        /// <summary>
        /// Implicitely converts a value to an Option<T>.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>
        /// Option<T>.None if value is null, otherwise and Option<T> whose 
        /// value is set to <paramref name="value"/>.
        /// </returns>
        public static implicit operator Option<T>(T value)
        {
            return null == value
                ? Option.None
                : new Option<T>(value);
        }

        /// <summary>
        /// Implicitely converts an Option<T> to a T.
        /// </summary>
        /// <param name="option">The option to convert.</param>
        /// <returns>
        /// Option<T>.Value, which will throw InvalidOperationException
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
        /// or both options are Option<T>.None,
        /// and false if the options' values are not
        /// equal or only one option is Option<T>.None
        /// </returns>
        public static bool operator ==(Option<T> lhs, Option<T> rhs)
        {
            return lhs.Equals(rhs);
        }

        /// <summary>
        /// Comapares two options for inequality.
        /// </summary>
        /// <param name="lhs">The option on the left hand side.</param>
        /// <param name="rhs">The option on the right hand side.</param>
        /// <returns>
        /// true if the options' values are not
        /// equal or only one option is Option<T>.None,
        /// and false if the options' values are equal
        /// or both options are Option<T>.None.
        /// </returns>
        public static bool operator !=(Option<T> lhs, Option<T> rhs)
        {
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
        /// or both options are Option<T>.None,
        /// and false if the options' values are not
        /// equal or only one option is Option<T>.None
        /// </returns>
        public bool Equals(Option<T> other)
        {
            if (this.HasValue != other.HasValue)
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
        /// true if the object is an Option<T> and
        /// the options' values are equal
        /// or both options are Option<T>.None,
        /// and false if the object is not an Option<T> or
        /// the options' values are not equal
        /// or only one option is Option<T>.None
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj is Option<T>)
            {
                return Equals((Option<T>)obj);
            }

            return false;
        }

        /// <summary>
        /// Gets the HashCode for the Option<T>.
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
        /// A new Option<T> whose value is set to <paramref name="value"/>.
        /// </returns>
        public static Option<T> From<T>(T value)
        {
            return new Option<T>(value);
        }

        /// <summary>
        /// The default Option type specifying there is no value.
        /// </summary>
        public static Option None
        {
            get { return _none; }
        }
    }

    /// <summary>
    /// Excention methods for working with Options.
    /// </summary>
    public static class OptionExtensions
    {
        /// <summary>
        /// Creates a new option from a value.
        /// </summary>
        /// <typeparam name="T">The type to create an option for.</typeparam>
        /// <param name="value">The value to create an option for.</param>
        /// <returns>
        /// A new Option<T> whose value is set to <paramref name="value"/>.
        /// </returns>
        public static Option<T> ToOption<T>(this T value)
        {
            return Option.From(value);
        }
    }
}
