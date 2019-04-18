using System.Web.Mvc;

namespace Beyova.Web
{
    /// <summary>
    /// Class MVCAction.
    /// </summary>
    public class MVCAction
    {
        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        /// <value>The action.</value>
        public string Action { get; set; }

        /// <summary>
        /// Gets or sets the controller.
        /// </summary>
        /// <value>The controller.</value>
        public string Controller { get; set; }

        /// <summary>
        /// Gets or sets the area.
        /// </summary>
        /// <value>The area.</value>
        public string Area { get; set; }

        /// <summary>
        /// Automatics the URL.
        /// </summary>
        /// <param name="urlHelper">The URL helper.</param>
        /// <returns>System.String.</returns>
        public string ToUrl(UrlHelper urlHelper)
        {
            if (urlHelper != null)
            {
                return urlHelper.Action(
                    Action.SafeToString("Index"),
                    Controller.SafeToString("Home"),
                    new { area = string.IsNullOrWhiteSpace(Area) ? string.Empty : Area });
            }

            return string.Empty;
        }
    }
}