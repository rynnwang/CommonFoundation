using System;

namespace Beyova
{
    /// <summary>
    /// Class NullableLambdaComparableComparer. This class cannot be inherited.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TComparableType">The type of the t comparable type.</typeparam>
    public sealed class NullableLambdaComparableComparer<T, TComparableType> : NullableLambdaComparer<T, TComparableType>
        where TComparableType : struct, IComparable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LambdaComparer{T, TComparableType}" /> class.
        /// </summary>
        /// <param name="comparer">The comparer.</param>
        /// <param name="isDescending">if set to <c>true</c> [is descending].</param>
        public NullableLambdaComparableComparer(Func<T, TComparableType?> comparer, bool isDescending = false)
            : base(comparer, GetDefaultComparison(isDescending))
        {
        }

        /// <summary>
        /// Gets the default comparison.
        /// </summary>
        /// <param name="isDescending">if set to <c>true</c> [is descending].</param>
        /// <returns>Func&lt;TComparableType, TComparableType, System.Int32&gt;.</returns>
        private static Func<TComparableType, TComparableType, int> GetDefaultComparison(bool isDescending = false)
        {
            return (x, y) =>
            {
                return x.CompareTo(y) * (isDescending ? -1 : 1);
            };
        }
    }
}