using System;

namespace Beyova
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TCoordinate">The type of the coordinate.</typeparam>
    public abstract class Range<TCoordinate>
        where TCoordinate : struct, IComparable
    {
        /// <summary>
        /// Gets or sets from.
        /// </summary>
        /// <value>
        /// From.
        /// </value>
        public TCoordinate? From { get; set; }

        /// <summary>
        /// Gets or sets the end.
        /// </summary>
        /// <value>
        /// The end.
        /// </value>
        public TCoordinate? To { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [from value reachable].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [from value reachable]; otherwise, <c>false</c>.
        /// </value>
        public bool FromValueReachable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [to value reachable].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [to value reachable]; otherwise, <c>false</c>.
        /// </value>
        public bool ToValueReachable { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Range{TCoordinate}" /> class.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="fromValueReachable">if set to <c>true</c> [from value reachable].</param>
        /// <param name="toValueReachable">if set to <c>true</c> [to value reachable].</param>
        /// <param name="checkRangeWhenConstruct">if set to <c>true</c> [check range when construct].</param>
        public Range(TCoordinate? from = null, TCoordinate? to = null, bool fromValueReachable = true, bool toValueReachable = false, bool checkRangeWhenConstruct = true)
        {
            From = from;
            To = to;
            FromValueReachable = fromValueReachable;
            ToValueReachable = toValueReachable;

            if (checkRangeWhenConstruct && from.HasValue && to.HasValue && ((IComparable)From.Value).CompareTo(To.Value) > 0)
            {
                throw ExceptionFactory.CreateInvalidObjectException(nameof(to), new { from, to }, "InvalidRange");
            }
        }

        /// <summary>
        /// Determines whether [contains] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///   <c>true</c> if [contains] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool Contains(TCoordinate value)
        {
            if (From.HasValue)
            {
                var fromResult = From.Value.CompareTo(value);

                if (FromValueReachable ? fromResult > 0 : fromResult >= 0)
                {
                    return false;
                }
            }

            if (To.HasValue)
            {
                var toResult = To.Value.CompareTo(value);

                if (ToValueReachable ? toResult < 0 : toResult <= 0)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Determines whether [contains] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///   <c>true</c> if [contains] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(TCoordinate? value)
        {
            return value.HasValue && Contains(value.Value);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}{1},{2}{3}",
                FromValueReachable ? "[" : "(",
                From,
                To,
                ToValueReachable ? "]" : ")");
        }
    }
}