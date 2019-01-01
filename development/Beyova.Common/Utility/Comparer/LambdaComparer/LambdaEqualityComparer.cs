using System;
using System.Collections.Generic;

namespace Beyova
{
    /// <summary>
    /// Class LambdaEqualityComparer. This class cannot be inherited.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TComparableType">The type of the comparable type.</typeparam>
    public sealed class LambdaEqualityComparer<T, TComparableType> : IEqualityComparer<T>
    {
        /// <summary>
        /// Gets or sets the comparer.
        /// </summary>
        /// <value>
        /// The comparer.
        /// </value>
        public IEqualityComparer<TComparableType> Comparer { get; set; }

        /// <summary>
        /// Gets or sets the hash code getter.
        /// </summary>
        /// <value>The hash code getter.</value>
        public Func<T, TComparableType> ComparableSelector { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LambdaEqualityComparer{T, TComparableType}" /> class.
        /// </summary>
        /// <param name="comparableSelector">The comparable selector.</param>
        /// <param name="comparer">The comparer.</param>
        public LambdaEqualityComparer(Func<T, TComparableType> comparableSelector, IEqualityComparer<TComparableType> comparer = null)
        {
            this.ComparableSelector = comparableSelector == null ? (x) => { return default(TComparableType); } : comparableSelector;
            this.Comparer = comparer ?? EqualityComparer<TComparableType>.Default;
        }

        /// <summary>
        /// Equalses the specified x.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public bool Equals(T x, TComparableType y)
        {
            //https://referencesource.microsoft.com/#mscorlib/system/nullable.cs,c9505a785f9fd8c5
            var left = x == null ? default(TComparableType) : this.ComparableSelector(x);

            if (left == null)
            {
                return y == null;
            }

            if (y == null)
            {
                return false;
            }
            else
            {
                return Comparer.Equals(left, y);
            }
        }

        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        /// <param name="x">The first object of type T to compare.</param>
        /// <param name="y">The second object of type T to compare.</param>
        /// <returns>
        /// true if the specified objects are equal; otherwise, false.
        /// </returns>
        public bool Equals(T x, T y)
        {
            //https://referencesource.microsoft.com/#mscorlib/system/nullable.cs,c9505a785f9fd8c5
            var left = x == null ? default(TComparableType) : this.ComparableSelector(x);
            var right = y == null ? default(TComparableType) : this.ComparableSelector(y);

            if (left == null)
            {
                return right == null;
            }

            if (right == null)
            {
                return false;
            }
            else
            {
                return Comparer.Equals(left, right);
            }
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object" /> for which a hash code is to be returned.</param>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public int GetHashCode(T obj)
        {
            return Comparer.GetHashCode(ComparableSelector(obj));
        }
    }
}