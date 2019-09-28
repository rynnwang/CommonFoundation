namespace Beyova.Web
{
    /// <summary>
    ///
    /// </summary>
    public class PartialViewWrapper
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the layout.
        /// </summary>
        /// <value>
        /// The layout.
        /// </value>
        public string Layout { get; set; }

        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        /// <value>
        /// The model.
        /// </value>
        public object Model { get; set; }

        /// <summary>
        /// Gets or sets the partial view.
        /// </summary>
        /// <value>
        /// The partial view.
        /// </value>
        public string PartialView { get; set; }

        /// <summary>
        /// Gets or sets the java scripts.
        /// </summary>
        /// <value>
        /// The java scripts.
        /// </value>
        public string JavaScripts { get; set; }
    }
}