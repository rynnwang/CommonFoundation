using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Beyova.Web
{
    /// <summary>
    /// 
    /// </summary>
    public static class HtmlViewHelper
    {
        /// <summary>
        /// The na
        /// </summary>
        public const string NA = "(N/A)";

        /// <summary>
        /// Renders the information value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string RenderTextValue(string value)
        {
            return value.SafeToString(NA);
        }

        /// <summary>
        /// Renders the information value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predict">The predict.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string RenderTextValue<T>(Func<T, string> predict, T value)
        {
            return RenderTextValue(predict?.Invoke(value));
        }

        /// <summary>
        /// Renders the link raw.
        /// </summary>
        /// <param name="link">The link.</param>
        /// <param name="openNew">if set to <c>true</c> [open new].</param>
        /// <param name="prefix">The prefix.</param>
        /// <returns></returns>
        public static string RenderLinkRaw(string link, bool openNew = false, string prefix = null)
        {
            if (!string.IsNullOrWhiteSpace(link))
            {
                return string.Format(openNew ? "<a href=\"{2}{0}\" target=\"_blank\">{1}</a>" : "<a href=\"{2}{0}\">{1}</a>",
                    link.ToUrlPathEncodedText(),
                    link.ToHtmlEncodedText(),
                    prefix);
            }

            return NA;
        }

        /// <summary></summary>
        /// <param name="date"></param>
        /// <param name="nullString"></param>
        public static string RenderDate(Date? date, string nullString)
        {
            return date.HasValue ? string.Format("{0}.{1}", date.Value.Year, date.Value.Month) : nullString;
        }

        /// <summary>
        /// Renders the flag enum.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="flagEnum">The flag enum.</param>
        /// <returns></returns>
        public static string RenderFlagEnum<T>(T flagEnum)
            where T : struct, IConvertible, IComparable, IFormattable
        {
            var bitWiseValues = flagEnum.GetEnumFlagValues();
            if (bitWiseValues.Count < 1)
            {
                return string.Empty;
            }
            else if (bitWiseValues.Count == 1)
            {
                return string.Format("{0} ({1})", flagEnum.ToString(), flagEnum.ToInt32(null));
            }
            else
            {
                StringBuilder builder = new StringBuilder();

                foreach (var item in bitWiseValues)
                {
                    builder.AppendFormat("{0} ({1}), ", item.ToString(), item.ToInt32(null));
                }
                builder.TrimEnd();
                builder.RemoveLastIfMatch(',');

                return builder.ToString();
            }
        }

        /// <summary>
        /// Renders the flag enum.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="flagEnum">The flag enum.</param>
        /// <returns></returns>
        public static string RenderFlagEnum<T>(T? flagEnum)
            where T : struct, IConvertible, IComparable, IFormattable
        {
            return flagEnum.HasValue ? RenderFlagEnum(flagEnum.Value) : string.Empty;
        }

        /// <summary>
        /// Splits to list raw.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="ulClass">The ul class.</param>
        /// <param name="liClass">The li class.</param>
        /// <param name="liFormat">The li format.</param>
        /// <param name="seperators">The seperators.</param>
        /// <returns></returns>
        public static string SplitToListRaw(string text, string ulClass, string liClass, string liFormat, params string[] seperators)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                var items = text.Split(seperators, StringSplitOptions.RemoveEmptyEntries);
                StringBuilder builder = new StringBuilder(text.Length * 2);
                builder.AppendFormat("<ul class=\"{0}\">", ulClass.Remove(new char[] { '\'', '"' }));

                foreach (var item in items)
                {
                    builder.AppendFormat("<li class=\"{0}\">", liClass.Remove(new char[] { '\'', '"' }));
                    builder.Append((string.IsNullOrWhiteSpace(liFormat) ? item : string.Format(liFormat, item)).ToHtmlEncodedText());
                    builder.Append("</li>");
                }

                builder.Append("</ul>");

                return builder.ToString();
            }

            return text;
        }

        /// <summary>
        /// Splits to list raw.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="ulClass">The ul class.</param>
        /// <param name="liClass">The li class.</param>
        /// <param name="seperators">The seperators.</param>
        /// <returns></returns>
        public static string SplitToListRaw(string text, string ulClass, string liClass, params string[] seperators)
        {
            return SplitToListRaw(text, ulClass, liClass, null, seperators);
        }

        /// <summary>
        /// Splits to list raw.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="seperators">The seperators.</param>
        /// <returns></returns>
        public static string SplitToListRaw(string text, params string[] seperators)
        {
            return SplitToListRaw(text, null, null, null, seperators);
        }
    }
}