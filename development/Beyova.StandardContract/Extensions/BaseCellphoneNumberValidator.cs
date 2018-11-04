using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Beyova
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class BaseCellphoneNumberValidator : ICellphoneNumberValidator
    {
        /// <summary>
        /// Gets the regex.
        /// </summary>
        /// <returns></returns>
        protected abstract Regex GetRegex();

        /// <summary>
        /// Gets or sets the nation codes.
        /// </summary>
        /// <value>
        /// The nation codes.
        /// </value>
        public abstract string NationCode { get; }

        /// <summary>
        /// Validates the specified cellphone number.
        /// </summary>
        /// <param name="cellphoneNumber">The cellphone number.</param>
        /// <param name="omitNationCode">if set to <c>true</c> [omit nation code].</param>
        public void Validate(CellphoneNumber cellphoneNumber, bool omitNationCode = false)
        {
            try
            {
                cellphoneNumber.CheckNullObject(nameof(cellphoneNumber));

                if (!omitNationCode && !cellphoneNumber.NationCode.MeaningfulEquals(this.NationCode))
                {
                    throw ExceptionFactory.CreateInvalidObjectException(nameof(cellphoneNumber.NationCode), new { expected = this.NationCode, actual = cellphoneNumber.NationCode });
                }

                var regex = GetRegex();
                regex.CheckNullObject(nameof(regex));
                cellphoneNumber.Number.CheckEmptyString(nameof(cellphoneNumber.Number));

                if (!regex.IsMatch(cellphoneNumber.Number))
                {
                    throw ExceptionFactory.CreateInvalidObjectException(nameof(cellphoneNumber.Number), new { cellphoneNumber });
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { cellphoneNumber, omitNationCode });
            }
        }
    }
}
