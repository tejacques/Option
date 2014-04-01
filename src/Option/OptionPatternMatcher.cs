using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Functional.Option
{
    /// <summary>
    /// A class to help with pattern matching on Option types.
    /// This should only be used if you want to match on specific
    /// values of the option, otherwise use the default Match
    /// method on the Option.
    /// </summary>
    /// <typeparam name="T">The type of the option.</typeparam>
    public class OptionPatternMatcher<T>
    {
        private Option<T> _option;
        private Dictionary<T, Action> _matchedValues;
        private Action<T> _matchedSome;
        private Action _matchedNone;
        private bool _matched;

        /// <summary>
        /// Creates a new OptionPatternMatcher.
        /// </summary>
        public OptionPatternMatcher()
        {
            this._option = Option.None;
        }

        /// <summary>
        /// Creates a new OptionPatternMatcher with the supplied option.
        /// </summary>
        /// <param name="option">The option to match on.</param>
        internal OptionPatternMatcher(Option<T> option) : this()
        {
            this._option = option;
        }

        #region Fluent API

        /// <summary>
        /// Sets the action to run when the option matches Some&lt;T&gt;.
        /// </summary>
        /// <param name="action">
        /// The action to run if the pattern matches.
        /// </param>
        /// <returns>The current OptionPatternMatcher&lt;T&gt;</returns>
        public OptionPatternMatcher<T> Some(Action<T> action)
        {
            if (null == this._matchedSome)
            {
                this._matchedSome = action;
            }
            else
            {
                throw new InvalidOperationException(
                    "Some has already been matched.");
            }

            return this;
        }

        /// <summary>
        /// Sets the action to run when the option matches the provided value.
        /// </summary>
        /// <param name="value">The value to pattern match on.</param>
        /// <param name="action">
        /// The action to run if the pattern matches.
        /// </param>
        /// <returns>The current OptionPatternMatcher&lt;T&gt;</returns>
        public OptionPatternMatcher<T> Some(T value, Action action)
        {
            if (null == this._matchedValues)
            {
                this._matchedValues = new Dictionary<T, Action>();
            }
            if (!this._matchedValues.ContainsKey(value))
            {
                this._matchedValues.Add(value, action);
            }
            else
            {
                throw new InvalidOperationException(
                    string.Format("{0} has already been matched.", value));
            }

            return this;
        }

        /// <summary>
        /// Sets the action to run when the option matches None&lt;T&gt;.
        /// </summary>
        /// <param name="action">
        /// The action to run if the pattern matches.
        /// </param>
        /// <returns>The current OptionPatternMatcher&lt;T&gt;</returns>
        public OptionPatternMatcher<T> None(Action action)
        {
            if (null == this._matchedNone)
            {
                this._matchedNone = action;
            }
            else
            {
                throw new InvalidOperationException(
                    "None has already been matched.");
            }

            return this;
        }

        /// <summary>
        /// Runs the action whose pattern matches the option.
        /// </summary>
        public void Result()
        {
            Result(this._option, false);
        }

        /// <summary>
        /// Runs the action whose pattern matches the supplied option.
        /// </summary>
        /// <param name="option">The option to match on.</param>
        public void Result(Option<T> option)
        {
            Result(option, false);
        }

        private void Result(Option<T> option, bool fromInternal)
        {
            if (fromInternal && this._matched)
            {
                // Do nothing
            }
            else if (option.HasValue)
            {
                T value = option.Value;
                Action action;
                if (null != this._matchedValues
                    && this._matchedValues.TryGetValue(value, out action))
                {
                    if (fromInternal)
                    {
                        this._matched = true;
                    }
                    action();
                }
                else if (null != this._matchedSome)
                {
                    if (fromInternal)
                    {
                        this._matched = true;
                    }
                    this._matchedSome(value);
                }
            }
            else
            {
                if (null != this._matchedNone)
                {
                    this._matchedNone();
                }
            }
        }
        #endregion

        #region Operator Overloads
        /// <summary>
        /// Functional style of passing lambdas to the pattern matcher.
        /// </summary>
        /// <param name="pm">The pattern matcher.</param>
        /// <param name="None">
        /// The action to take if the result is none.</param>
        /// <returns>The pattern matcher.</returns>
        public static OptionPatternMatcher<T> operator |(OptionPatternMatcher<T> pm, Action None)
        {
            pm = pm.None(None);
            pm.Result(pm._option, true);

            return pm;
        }

        /// <summary>
        /// Functional style of passing lambdas to the pattern matcher.
        /// </summary>
        /// <param name="pm">The pattern matcher.</param>
        /// <param name="Some">
        /// The action to take if the result is some.</param>
        /// <returns>The pattern matcher.</returns>
        public static OptionPatternMatcher<T> operator |(OptionPatternMatcher<T> pm, Action<T> Some)
        {
            pm = pm.Some(Some);
            pm.Result(pm._option, true);

            return pm;
        }
        #endregion

    }

    /// <summary>
    /// A class to help with pattern matching on Option types.
    /// </summary>
    /// <typeparam name="TIn">The type of option.</typeparam>
    /// <typeparam name="TOut">
    /// The result type of running the pattern matcher on the option.
    /// </typeparam>
    public class OptionPatternMatcher<TIn, TOut>
    {
        private readonly Option<TIn> _option;
        private Dictionary<TIn, Func<TOut>> _matchedValues;
        private Func<TIn,TOut> _matchedSome;
        private Func<TOut> _matchedNone;

        /// <summary>
        /// Creates a new OptionPatternMatcher.
        /// </summary>
        public OptionPatternMatcher()
        {
            this._option = Option.None;
        }

        internal OptionPatternMatcher(Option<TIn> option) : this()
        {
            this._option = option;
        }

        #region Fluent API

        /// <summary>
        /// Sets the func to run when the option matches Some&lt;T&gt;.
        /// </summary>
        /// <param name="func">
        /// The func to run if the pattern matches.
        /// </param>
        /// <returns>The current OptionPatternMatcher&lt;TIn,TOut&gt;</returns>
        public OptionPatternMatcher<TIn, TOut> Some(Func<TIn, TOut> func)
        {
            if (null == this._matchedSome)
            {
                this._matchedSome = func;
            }
            else
            {
                throw new InvalidOperationException(
                    "Some has already been matched.");
            }

            return this;
        }

        /// <summary>
        /// Sets the func to run when the option matches Some&lt;T&gt;.
        /// </summary>
        /// <param name="value">The value to pattern match on.</param>
        /// <param name="func">
        /// The func to run if the pattern matches.
        /// </param>
        /// <returns>The current OptionPatternMatcher&lt;TIn,TOut&gt;</returns>
        public OptionPatternMatcher<TIn, TOut> Some(
            TIn value,
            Func<TOut> func)
        {
            if (null == this._matchedValues)
            {
                this._matchedValues = new Dictionary<TIn, Func<TOut>>();
            }
            if (!this._matchedValues.ContainsKey(value))
            {
                this._matchedValues.Add(value, func);
            }
            else
            {
                throw new InvalidOperationException(
                    string.Format("{0} has already been matched.", value));
            }

            return this;
        }

        /// <summary>
        /// Sets the func to run when the option matches None&lt;T&gt;.
        /// </summary>
        /// <param name="func">
        /// The func to run if the pattern matches.
        /// </param>
        /// <returns>The current OptionPatternMatcher&lt;TIn,TOut&gt;</returns>
        public OptionPatternMatcher<TIn, TOut> None(Func<TOut> func)
        {
            if (null == this._matchedNone)
            {
                this._matchedNone = func;
            }
            else
            {
                throw new InvalidOperationException(
                    "None has already been matched.");
            }

            return this;
        }

        /// <summary>
        /// Runs the func whose pattern matches the option
        /// and returns the result or default(TOut) if there
        /// was no match.
        /// </summary>
        public TOut Result()
        {
            return Result(default(TOut));
        }

        /// <summary>
        /// Runs the func whose pattern matches the option
        /// and returns the result or the defaultValue if 
        /// there was no match.
        /// </summary>
        /// <param name="defaultValue">
        /// The default value to return if there was no match.
        /// </param>
        /// <returns>
        /// The result of running the func matching the option's value, or
        /// defaultValue if there was no match.
        /// </returns>
        public TOut Result(TOut defaultValue)
        {
            return Result(this._option, defaultValue);
        }

        /// <summary>
        /// Runs the func whose pattern matches the option
        /// and returns the result or the defaultValue if 
        /// there was no match.
        /// </summary>
        /// <param name="option">The option to match on.</param>
        /// <returns>
        /// The result of running the func matching the option's value, or
        /// defaultValue if there was no match.
        /// </returns>
        public TOut Result(Option<TIn> option)
        {
            return Result(option, default(TOut));
        }

        /// <summary>
        /// Runs the func whose pattern matches the option
        /// and returns the result or the defaultValue if 
        /// there was no match.
        /// </summary>
        /// <param name="option">The option to match on.</param>
        /// <param name="defaultValue">
        /// The default value to return if there was no match.
        /// </param>
        /// <returns>
        /// The result of running the func matching the option's value, or
        /// defaultValue if there was no match.
        /// </returns>
        public TOut Result(Option<TIn> option, TOut defaultValue)
        {
            TOut result = defaultValue;

            if (option.HasValue)
            {
                TIn value = option.Value;
                Func<TOut> func;
                if (null != this._matchedValues
                    && this._matchedValues.TryGetValue(value, out func))
                {
                    result = func();
                }
                else if (null != this._matchedSome)
                {
                    result = this._matchedSome(value);
                }
            }
            else
            {
                if (null != this._matchedNone)
                {
                    result = this._matchedNone();
                }
            }

            return result;
        }
        #endregion

        #region Operator Overloads
        /// <summary>
        /// Functional style of passing lambdas to the pattern matcher.
        /// </summary>
        /// <param name="pm">The pattern matcher.</param>
        /// <param name="None">
        /// The result to return if the result is none.</param>
        /// <returns>The pattern matcher.</returns>
        public static OptionPatternMatcher<TIn, TOut> operator |(
            OptionPatternMatcher<TIn, TOut> pm,
            Func<TOut> None)
        {
            return pm.None(None);
        }

        /// <summary>
        /// Functional style of passing lambdas to the pattern matcher.
        /// </summary>
        /// <param name="pm">The pattern matcher.</param>
        /// <param name="Some">
        /// The result to return if the result is some.</param>
        /// <returns>The pattern matcher.</returns>
        public static OptionPatternMatcher<TIn, TOut> operator |(
            OptionPatternMatcher<TIn, TOut> pm, Func<TIn, TOut> Some)
        {
            return pm.Some(Some);
        }

        /// <summary>
        /// Implicitly converts an OptionPatternMatcher to a TOut.
        /// </summary>
        /// <param name="pm">The pattern matcher.</param>
        /// <returns>
        /// The TOut obtained by running pm.Result()
        /// </returns>
        public static implicit operator TOut(
            OptionPatternMatcher<TIn, TOut> pm)
        {
            return pm.Result();
        }
        #endregion
    }
}
