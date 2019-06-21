using Newtonsoft.Json;
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
        [JsonProperty("from")]
        public TCoordinate? From { get; set; }

        /// <summary>
        /// Gets or sets the end.
        /// </summary>
        /// <value>
        /// The end.
        /// </value>
        [JsonProperty("to")]
        public TCoordinate? To { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Range{TCoordinate}" /> class.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="fromValueReachable">if set to <c>true</c> [from value reachable].</param>
        /// <param name="toValueReachable">if set to <c>true</c> [to value reachable].</param>
        /// <param name="omitRangeValidation">if set to <c>true</c> [omit range validation].</param>
        public Range(TCoordinate? from = null, TCoordinate? to = null, bool fromValueReachable = true, bool toValueReachable = false, bool omitRangeValidation = false)
        {
            From = from;
            To = to;

            if (!omitRangeValidation && from.HasValue && to.HasValue && ((IComparable)From.Value).CompareTo(To.Value) >= 0)
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

                if (fromResult >= 0)
                {
                    return false;
                }
            }

            if (To.HasValue)
            {
                var toResult = To.Value.CompareTo(value);

                if (toResult <= 0)
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
            return string.Format("{0} - {1}", From, To);
        }
    }
}