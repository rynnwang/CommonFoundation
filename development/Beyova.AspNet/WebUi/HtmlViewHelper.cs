using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        /// Renders the boolean value as text.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns></returns>
        public static string RenderBooleanValueAsText(bool value)
        {
            return value ? "Yes" : "No";
        }

        /// <summary>
        /// Renders the boolean value as text.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string RenderBooleanValueAsText(bool? value)
        {
            return value.HasValue ? RenderBooleanValueAsText(value.Value) : NA;
        }

        /// <summary>
        /// Renders the boolean value as symbol.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns></returns>
        public static string RenderBooleanValueAsSymbol(bool value)
        {
            return (value ? Fun.UnicodeConstants.Yes : Fun.UnicodeConstants.No).ToString();
        }

        /// <summary>
        /// Renders the boolean value as icon raw.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <param name="additionalClass">The additional class.</param>
        /// <returns></returns>
        public static string RenderBooleanValueAsIconRaw(bool value, string additionalClass = null)
        {
            return string.Format("<i class=\"{0} {1}\"></i>", value ? "fas fa-check-circle" : "fas fa-times-circle", value);
        }

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
        /// <param name="seperators">The seperators.</param>
        /// <param name="ulClass">The ul class. BS class: <c>list-group</c></param>
        /// <param name="liClass">The li class. BS class: <c>list-group-item</c></param>
        /// <returns></returns>
        public static string SplitToListRaw(string text, string[] seperators, string ulClass = null, string liClass = null)
        {
            return SplitToListRaw(text, (x) => x.Split(seperators, StringSplitOptions.RemoveEmptyEntries), ulClass, liClass);
        }

        /// <summary>
        /// Splits to list raw.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="seperator">The seperator.</param>
        /// <param name="ulClass">The ul class.</param>
        /// <param name="liClass">The li class.</param>
        /// <param name="itemTextBuilder">The item text builder.</param>
        /// <returns></returns>
        public static string SplitToListRaw(string text, Func<string, IEnumerable<string>> seperator, string ulClass = null, string liClass = null, Func<string, string> itemTextBuilder = null)
        {
            if (!string.IsNullOrWhiteSpace(text) && seperator != null)
            {
                if (itemTextBuilder == null)
                {
                    itemTextBuilder = FuncExtension.GetSelf;
                }

                var items = seperator(text);

                if (items.HasItem())
                {
                    StringBuilder builder = new StringBuilder(text.Length * 2);
                    builder.AppendFormat("<ul class=\"{0}\">", ulClass.GetValidDomAttributeName());

                    foreach (var item in items)
                    {
                        builder.AppendFormat("<li class=\"{0}\">", liClass.GetValidDomAttributeName());
                        builder.Append(itemTextBuilder(item).ToHtmlEncodedText());
                        builder.Append("</li>");
                    }

                    builder.Append("</ul>");

                    return builder.ToString();
                }
            }

            return text;
        }

        /// <summary>
        /// Gets the name of the valid DOM attribute.
        /// </summary>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <returns></returns>
        internal static string GetValidDomAttributeName(this string attributeName)
        {
            return attributeName.Remove(new char[] { '\'', '"' });
        }

        static Regex flatListContentSymbolRegex = new Regex(@"([\(\[（【]?)(\d+)[\.\-,，、。\]\)）】]", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// Tries the content of the structurize list.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        public static string[] TryStructurizeListContentToList(string content)
        {
            if (!string.IsNullOrWhiteSpace(content))
            {
                return flatListContentSymbolRegex.Split(content);
            }

            return new string[] { };
        }

        /// <summary>
        /// Tries the content of the structurize list.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="replaceSeperator">The replace seperator.</param>
        /// <returns></returns>
        public static string TryStructurizeListContent(string content, string replaceSeperator = null)
        {
            if (!string.IsNullOrWhiteSpace(content))
            {
                return flatListContentSymbolRegex.Replace(content, replaceSeperator.SafeToString(StringConstants.NewLine));
            }

            return content;
        }
    }
}