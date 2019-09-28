using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Beyova.Web
{
    /// <summary>
    ///
    /// </summary>
    public static class HtmlViewHelper
    {
        /// <summary>
        /// Splits the content to list DOM.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="mvcHtmlHelper">The MVC HTML helper.</param>
        /// <param name="text">The text.</param>
        /// <param name="seperator">The seperator.</param>
        /// <param name="ulClass">The ul class.</param>
        /// <param name="liClass">The li class.</param>
        /// <param name="itemTextBuilder">The item text builder.</param>
        /// <returns></returns>
        public static IHtmlString SplitContentToListDom<TModel>(this HtmlHelper<TModel> mvcHtmlHelper, string text, Func<string, IEnumerable<string>> seperator, string ulClass = null, string liClass = null, Func<string, string> itemTextBuilder = null)
        {
            return mvcHtmlHelper?.Raw(HtmlViewHelper.SplitToListRaw(HtmlViewHelper.TryStructurizeListContent(text), seperator, ulClass, liClass, itemTextBuilder));
        }

        /// <summary>
        /// Splits the content to list DOM.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="mvcHtmlHelper">The MVC HTML helper.</param>
        /// <param name="text">The text.</param>
        /// <param name="seperators">The seperators.</param>
        /// <param name="ulClass">The ul class.</param>
        /// <param name="liClass">The li class.</param>
        /// <returns></returns>
        public static IHtmlString SplitContentToListDom<TModel>(this HtmlHelper<TModel> mvcHtmlHelper, string text, string[] seperators, string ulClass = null, string liClass = null)
        {
            return mvcHtmlHelper?.Raw(HtmlViewHelper.SplitToListRaw(HtmlViewHelper.TryStructurizeListContent(text), seperators, ulClass, liClass));
        }

        /// <summary>
        /// Renders the read only attribute.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="mvcHtmlHelper">The MVC HTML helper.</param>
        /// <param name="isReadOnly">if set to <c>true</c> [is read only].</param>
        /// <returns></returns>
        public static IHtmlString RenderReadOnlyAttribute<TModel>(this HtmlHelper<TModel> mvcHtmlHelper, bool? isReadOnly)
        {
            return mvcHtmlHelper?.Raw((isReadOnly ?? false) ? " readonly" : string.Empty);
        }

        /// <summary>
        /// Renders the disabled attribute.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="mvcHtmlHelper">The MVC HTML helper.</param>
        /// <param name="isDisabled">The is disabled.</param>
        /// <returns></returns>
        public static IHtmlString RenderDisabledAttribute<TModel>(this HtmlHelper<TModel> mvcHtmlHelper, bool? isDisabled)
        {
            return mvcHtmlHelper?.Raw((isDisabled ?? false) ? " disabled" : string.Empty);
        }

        /// <summary>
        /// Renders the checked attribute.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="mvcHtmlHelper">The MVC HTML helper.</param>
        /// <param name="isChecked">The is checked.</param>
        /// <returns></returns>
        public static IHtmlString RenderCheckedAttribute<TModel>(this HtmlHelper<TModel> mvcHtmlHelper, bool? isChecked)
        {
            return mvcHtmlHelper?.Raw((isChecked ?? false) ? " checked" : string.Empty);
        }

        /// <summary>
        /// Renders the selected attribute.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="mvcHtmlHelper">The MVC HTML helper.</param>
        /// <param name="isSelected">The is selected.</param>
        /// <returns></returns>
        public static IHtmlString RenderSelectedAttribute<TModel>(this HtmlHelper<TModel> mvcHtmlHelper, bool? isSelected)
        {
            return mvcHtmlHelper?.Raw((isSelected ?? false) ? " selected" : string.Empty);
        }

        /// <summary>
        /// Renders the determinable icon DOM.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="mvcHtmlHelper">The MVC HTML helper.</param>
        /// <param name="needShow">The need show.</param>
        /// <param name="classNames">The class names.</param>
        /// <returns></returns>
        public static IHtmlString RenderDeterminableIconDom<TModel>(this HtmlHelper<TModel> mvcHtmlHelper, bool? needShow, string classNames)
        {
            return mvcHtmlHelper?.Raw((needShow ?? false) ? string.Format("<i class=\"{0}\"></i>", classNames) : string.Empty);
        }

        /// <summary>
        /// Renders the form input.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="mvcHtmlHelper">The MVC HTML helper.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        /// <param name="className">Name of the class.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="isRequired">if set to <c>true</c> [is required].</param>
        /// <param name="isReadonly">if set to <c>true</c> [is readonly].</param>
        /// <returns></returns>
        public static IHtmlString RenderFormTextInput<TModel, T>(this HtmlHelper<TModel> mvcHtmlHelper, T defaultValue = default(T), string name = null, string type = "text", string className = "form-control", string id = null, bool isRequired = false, bool isReadonly = false)
        {
            return new HtmlString(isReadonly ?
                string.Format("<span class=\"{0}\" id=\"{1}\" name=\"{2}\">{3}</span>", className, id, name, defaultValue.ToString().ToHtmlEncodedText())
                : string.Format("<input class=\"{0}\" id=\"{1}\" type=\"{2}\" name=\"{3}\" {5}>{4}</span>", className, id, type, name, defaultValue.ToString().ToHtmlEncodedText(), isRequired ? "required" : string.Empty));
        }

        /// <summary>
        /// To the style.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="style">The style.</param>
        /// <returns></returns>
        public static string ToStyleValue<T>(this T style)
            where T : IColorStyle
        {
            if (style != null)
            {
                var styleBuilder = new StringBuilder(512);
                if (!string.IsNullOrWhiteSpace(style.BackgroundColorHex))
                {
                    styleBuilder.Append("background-color:").Append(style.BackgroundColorHex).Append(";");
                    styleBuilder.Append("border-color:").Append(style.BackgroundColorHex).Append(";");
                }
                if (!string.IsNullOrWhiteSpace(style.FontColorHex))
                {
                    styleBuilder.Append("color:").Append(style.FontColorHex).Append(";");
                }
                return styleBuilder.ToString();
            }

            return string.Empty;
        }

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
            return value.HasValue ? RenderBooleanValueAsText(value.Value) : StringConstants.NA;
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
            return value.SafeToString(StringConstants.NA);
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

            return StringConstants.NA;
        }

        /// <summary>
        /// Renders the date month. "{Year}.{Month}"
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="nullString">The null string.</param>
        /// <returns></returns>
        public static string RenderDateMonth(Date? date, string nullString)
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
                    itemTextBuilder = FunctionFactory.GetSelf;
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

        private static Regex flatListContentSymbolRegex = new Regex(@"([\(\[（【]?)(\d+)[\.\-,，、。\]\)）】]", RegexOptions.Compiled | RegexOptions.IgnoreCase);

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