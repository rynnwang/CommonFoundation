using System;
using System.Collections.Generic;
using System.Text;

namespace Beyova
{
    /// <summary>
    /// 
    /// </summary>
    public static class RegionalOptions
    {
        /// <summary>
        /// Gets or sets the default cellphone nation code.
        /// </summary>
        /// <value>
        /// The default cellphone nation code.
        /// </value>
        public static string DefaultCellphoneNationCode { get; set; } = "86";

        /// <summary>
        /// Gets or sets the default cellphone number validator.
        /// </summary>
        /// <value>
        /// The default cellphone number validator.
        /// </value>
        public static ICellphoneNumberValidator DefaultCellphoneNumberValidator { get; set; } = new ChineseCellphoneNumberValidator();

        /// <summary>
        /// Gets or sets the default name of the time zone.
        /// </summary>
        /// <value>
        /// The default name of the time zone.
        /// </value>
        public static string DefaultTimeZoneName { get; set; } = "China Standard Time";
    }
}
