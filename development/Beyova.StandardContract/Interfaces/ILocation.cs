namespace Beyova
{
    /// <summary>
    /// </summary>
    public interface ILocation : ILocationEssential, IGeographyLocationPoint, INational
    {
        /// <summary>
        /// Gets or sets the zip code.
        /// </summary>
        /// <value>
        /// The zip code.
        /// </value>
        string ZipCode { get; set; }
    }
}