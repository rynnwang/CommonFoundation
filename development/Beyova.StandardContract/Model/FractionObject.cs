﻿using Newtonsoft.Json;
using System;

namespace Beyova
{
    /// <summary>
    /// Struct FractionObject
    /// </summary>
    public struct FractionObject
    {
        /// <summary>
        /// Gets or sets the numerator.
        /// </summary>
        /// <value>The numerator.</value>
        [JsonProperty("numerator")]
        public Int64 Numerator { get; set; }

        /// <summary>
        /// Gets or sets the denominator.
        /// </summary>
        /// <value>The denominator.</value>
        [JsonProperty("denominator")]
        public Int64 Denominator { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FractionObject"/> class.
        /// </summary>
        /// <param name="numerator">The numerator.</param>
        /// <param name="denominator">The denominator.</param>
        public FractionObject(Int64 numerator, Int64 denominator)
        {
            Numerator = numerator;
            Denominator = denominator;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return ToString(null);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="delimiter">The delimiter.</param>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public string ToString(string delimiter)
        {
            if (Denominator == 0)
            {
                throw new DivideByZeroException("Denominator is zero");
            }

            return string.Format("{0}{1}{2}", Numerator, delimiter.SafeToString("/"), Denominator);
        }

        /// <summary>
        /// To the percentage.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="includesSymbol">if set to <c>true</c> [includes symbol].</param>
        /// <returns></returns>
        /// <exception cref="DivideByZeroException">Denominator is zero</exception>
        public string ToPercentage(string format = null, bool includesSymbol = false)
        {
            var value = ToDoubleTimes().ToString(format.SafeToString("0.0"));
            return includesSymbol ? (value + "%") : value;
        }

        /// <summary>
        /// To the double times.
        /// </summary>
        /// <returns></returns>
        public double ToDoubleTimes()
        {
            return (Denominator == 0) ? 0 : ((Numerator / (double)Denominator) * 100);
        }

        /// <summary>
        /// To the double.
        /// </summary>
        /// <returns>System.Double.</returns>
        public double ToDouble()
        {
            if (Denominator == 0)
            {
                throw new DivideByZeroException("Denominator is zero");
            }

            return (double)Numerator / Denominator;
        }

        /// <summary>
        /// To the decimal.
        /// </summary>
        /// <returns>Decimal.</returns>
        /// <exception cref="DivideByZeroException">Denominator is zero</exception>
        public Decimal ToDecimal()
        {
            if (Denominator == 0)
            {
                throw new DivideByZeroException("Denominator is zero");
            }

            return (decimal)Numerator / Denominator;
        }
    }
}