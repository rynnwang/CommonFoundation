namespace Beyova.Web
{
    /// <summary>
    ///
    /// </summary>
    public interface IColorStyle
    {
        /// <summary>
        /// Gets or sets Background Color Hex.
        /// </summary>
        /// <value>
        /// The background color hexadecimal.
        /// </value>
        System.String BackgroundColorHex { get; set; }

        /// <summary>
        /// Gets or sets Font Color Color Hex.
        /// </summary>
        /// <value>
        /// The font color hexadecimal.
        /// </value>
        System.String FontColorHex { get; set; }
    }
}