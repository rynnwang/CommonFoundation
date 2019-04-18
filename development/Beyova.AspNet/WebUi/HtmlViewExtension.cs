using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace Beyova.Web
{
    /// <summary>
    ///
    /// </summary>
    public static class HtmlViewExtension
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
    }
}