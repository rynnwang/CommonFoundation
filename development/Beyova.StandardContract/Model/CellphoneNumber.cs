using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Beyova
{
    /// <summary>
    /// 
    /// </summary>
    public class CellphoneNumber : INational
    {
        /// <summary>
        /// Gets or sets the nation code.
        /// </summary>
        /// <value>
        /// The nation code.
        /// </value>
        public string NationCode { get; set; }

        /// <summary>
        /// Gets or sets the number.
        /// </summary>
        /// <value>
        /// The number.
        /// </value>
        public string Number { get; set; }

        /// <summary>
        /// Validates the specified validator.
        /// </summary>
        /// <param name="validator">The validator.</param>
        public void Validate(ICellphoneNumberValidator validator)
        {
            (validator ?? RegionalOptions.DefaultCellphoneNumberValidator)?.Validate(this);
        }

        /// <summary>
        /// To the full cellphone number.
        /// </summary>
        /// <returns></returns>
        protected string ToFullCellphoneNumber()
        {
            return string.IsNullOrWhiteSpace(this.Number) ? StringConstants.NA : string.Format("+{0} {1}", this.NationCode.SafeToString(RegionalOptions.DefaultCellphoneNationCode), this.Number);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.ToFullCellphoneNumber();
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return (obj as CellphoneNumber)?.ToFullCellphoneNumber()?.Equals(this.ToFullCellphoneNumber()) ?? false;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return this.ToFullCellphoneNumber().GetHashCode();
        }

        #region static

        static Regex regex = new Regex(@"^(\+(?<NationCode>([0-9]+))([\s\t\-]))?(?<Number>([0-9\-\s]+))$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// Froms the string.
        /// </summary>
        /// <param name="fullCellphoneNumber">The full cellphone number.</param>
        /// <param name="defaultNationCode">The default nation code.</param>
        /// <returns></returns>
        public static CellphoneNumber FromString(string fullCellphoneNumber, string defaultNationCode = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fullCellphoneNumber))
                {
                    return null;
                }

                var match = regex.Match(fullCellphoneNumber);
                if (match.Success)
                {
                    return new CellphoneNumber
                    {
                        NationCode = match.Result("${NationCode}").SafeToString(defaultNationCode).Trim(),
                        Number = match.Result("${Number}").Replace(new char[] { '-', ' ', '\t' }, StringConstants.EmptyChar)
                    };
                }

                return null;
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { fullCellphoneNumber });
            }
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.String"/> to <see cref="CellphoneNumber"/>.
        /// </summary>
        /// <param name="cellphoneNumber">The cellphone number.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator CellphoneNumber(string cellphoneNumber)
        {
            return FromString(cellphoneNumber);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="CellphoneNumber"/> to <see cref="System.String"/>.
        /// </summary>
        /// <param name="cellphoneNumber">The cellphone number.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator string(CellphoneNumber cellphoneNumber)
        {
            return cellphoneNumber.ToFullCellphoneNumber();
        }

        #endregion
    }
}
