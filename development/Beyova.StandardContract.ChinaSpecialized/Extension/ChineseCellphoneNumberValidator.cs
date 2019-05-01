using System.Text.RegularExpressions;

namespace Beyova
{
    /// <summary>
    ///
    /// </summary>
    public class ChineseCellphoneNumberValidator : BaseCellphoneNumberValidator
    {
        /// <summary>
        /// The nation code
        /// </summary>
        private const string nationCode = "86";

        /// <summary>
        /// The cellphone number regex
        /// </summary>
        private static Regex cellphoneNumberRegex = new Regex("^1[0-9]{10}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// Gets the nation codes.
        /// </summary>
        /// <value>
        /// The nation codes.
        /// </value>
        public override string NationCode
        {
            get { return nationCode; }
        }

        /// <summary>
        /// Gets the regex.
        /// </summary>
        /// <returns></returns>
        protected override Regex GetRegex()
        {
            return cellphoneNumberRegex;
        }
    }
}