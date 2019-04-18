using System;

namespace Beyova
{
    /// <summary>
    /// </summary>
    public static class UiExtension
    {
        /// <summary>
        /// To the UI text.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="anyType">Any type.</param>
        /// <param name="toString">To string.</param>
        /// <param name="nullText">The null text.</param>
        /// <param name="prefix">The prefix.</param>
        /// <param name="suffix">The suffix.</param>
        /// <returns></returns>
        public static string ToUiText<T>(this T anyType, Func<T, string> toString, string nullText = null, string prefix = null, string suffix = null)
        {
            var stringValue = anyType == null ? null : (toString != null ? toString(anyType) : anyType.ToString());
            return ToUiText(anyType == null ? null : stringValue, nullText, prefix, suffix);
        }

        /// <summary>
        /// To the UI text.
        /// </summary>
        /// <param name="stringObject">The string object.</param>
        /// <param name="nullText">The null text.</param>
        /// <param name="prefix">The prefix.</param>
        /// <param name="suffix">The suffix.</param>
        /// <returns></returns>
        public static string ToUiText(this string stringObject, string nullText, string prefix = null, string suffix = null)
        {
            return string.IsNullOrEmpty(stringObject) ? nullText.SafeToString(StringConstants.NA) : string.Format("{0}{1}{2}", prefix, stringObject, suffix);
        }

        /// <summary>
        /// To the friendly date time display.
        /// </summary>
        /// <param name="minutes">The minutes.</param>
        /// <param name="minuteUnit">The minute unit.</param>
        /// <param name="hourUnit">The hour unit.</param>
        /// <param name="dayUnit">The day unit.</param>
        /// <param name="monthUnit">The month unit.</param>
        /// <returns></returns>
        public static string ToFriendlyDateTimeDisplay(this int minutes, string minuteUnit, string hourUnit, string dayUnit, string monthUnit)
        {
            const string format = "{0} {1}";
            if (minutes < 60)
            {
                return string.Format(format, minutes, minuteUnit.SafeToString("min"));
            }
            else if (minutes < 1440)
            {
                return string.Format(format, (int)((double)minutes / 60), hourUnit.SafeToString("hr"));
            }
            else if (minutes < 43200)
            {
                return string.Format(format, (int)((double)minutes / 1440), dayUnit.SafeToString("day"));
            }
            else
            {
                return string.Format(format, (int)((double)minutes / 43200), monthUnit.SafeToString("month"));
            }
        }
    }
}