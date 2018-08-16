using System;
using System.Collections.Generic;
using System.Text;

namespace Beyova
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICellphoneNumberValidator
    {
        /// <summary>
        /// Gets the nation code.
        /// </summary>
        /// <value>
        /// The nation code.
        /// </value>
        string NationCode { get; }

        /// <summary>
        /// Validates the specified cellphone number.
        /// </summary>
        /// <param name="cellphoneNumber">The cellphone number.</param>
        /// <param name="omitNationCode">if set to <c>true</c> [omit nation code].</param>
        void Validate(CellphoneNumber cellphoneNumber, bool omitNationCode = false);
    }
}
