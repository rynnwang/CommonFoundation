using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Beyova.Web
{
    /// <summary>
    /// 
    /// </summary>
    public static class BootstrapViewHelper
    {
        /// <summary>
        /// Renders the icon tooltip.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="mvcHtmlHelper">The MVC HTML helper.</param>
        /// <param name="iconClass">The icon class.</param>
        /// <param name="direction">The direction.</param>
        /// <param name="content">The content.</param>
        /// <param name="iconStyleClass">The icon style class.</param>
        /// <param name="style">The style.</param>
        /// <returns></returns>
        public static IHtmlString RenderIconTooltip<TModel>(this HtmlHelper<TModel> mvcHtmlHelper, string iconClass, string direction, string content, string iconStyleClass = null, string style = null)
        {
            return new HtmlString(string.Format("<i class=\"fa fa-fw {0} {1}\" style=\"{2}\" data-toggle=\"tooltip\" data-placement=\"{3}\" data-original-title=\"{4}\"></i>",
                iconClass.SafeToString("fa-exclamation-circle"),
                iconStyleClass,
                style,
                direction.SafeToString("right"),
                content));
        }
    }
}
