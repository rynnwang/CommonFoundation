using System;
using System.Text.RegularExpressions;
using Beyova.ExceptionSystem;

namespace Beyova
{
    /// <summary>
    /// Class CurrencyAmount.
    /// </summary>
    public class CurrencyAmount
    {
        /// <summary>
        /// Gets or sets the currency.
        /// </summary>
        /// <value>
        /// The currency.
        /// </value>
        public string Currency { get; set; }

        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        /// <value>
        /// The amount.
        /// </value>
        public decimal Amount { get; set; }

        /// <summary>
        /// Gets or sets the symbol.
        /// </summary>
        /// <value>
        /// The symbol.
        /// </value>
        public char Symbol { get; set; }

        /// <summary>
        /// Creates the usd.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <returns></returns>
        public static CurrencyAmount CreateUSD(decimal amount)
        {
            return new CurrencyAmount { Amount = amount, Currency = "USD", Symbol = '$' };
        }

        /// <summary>
        /// Creates the cny.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <returns></returns>
        public static CurrencyAmount CreateCNY(decimal amount)
        {
            return new CurrencyAmount { Amount = amount, Currency = "CNY", Symbol = '￥' };
        }

        /// <summary>
        /// Creates the eur.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <returns></returns>
        public static CurrencyAmount CreateEUR(decimal amount)
        {
            return new CurrencyAmount { Amount = amount, Currency = "EUR", Symbol = '€' };
        }

        /// <summary>
        /// Creates the jpy.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <returns></returns>
        public static CurrencyAmount CreateJPY(decimal amount)
        {
            return new CurrencyAmount { Amount = amount, Currency = "JPY", Symbol = '￥' };
        }

        /// <summary>
        /// Creates the aud.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <returns></returns>
        public static CurrencyAmount CreateAUD(decimal amount)
        {
            return new CurrencyAmount { Amount = amount, Currency = "AUD", Symbol = '$' };
        }

        /// <summary>
        /// Creates the KRW.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <returns></returns>
        public static CurrencyAmount CreateKRW(decimal amount)
        {
            return new CurrencyAmount { Amount = amount, Currency = "KRW", Symbol = '₩' };
        }

        /// <summary>
        /// Creates the VND.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <returns></returns>
        public static CurrencyAmount CreateVND(decimal amount)
        {
            return new CurrencyAmount { Amount = amount, Currency = "VND", Symbol = '₫' };
        }

        /// <summary>
        /// Creates the cad.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <returns></returns>
        public static CurrencyAmount CreateCAD(decimal amount)
        {
            return new CurrencyAmount { Amount = amount, Currency = "CAD", Symbol = '$' };
        }

        /// <summary>
        /// Creates the SGD.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <returns></returns>
        public static CurrencyAmount CreateSGD(decimal amount)
        {
            return new CurrencyAmount { Amount = amount, Currency = "SGD", Symbol = '$' };
        }

        /// <summary>
        /// Creates the THB.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <returns></returns>
        public static CurrencyAmount CreateTHB(decimal amount)
        {
            return new CurrencyAmount { Amount = amount, Currency = "THB", Symbol = '฿' };
        }

        /// <summary>
        /// Creates the try.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <returns></returns>
        public static CurrencyAmount CreateTRY(decimal amount)
        {
            return new CurrencyAmount { Amount = amount, Currency = "TRY", Symbol = '₺' };
        }
    }
}