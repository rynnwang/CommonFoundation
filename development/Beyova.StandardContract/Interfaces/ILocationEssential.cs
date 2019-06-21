namespace Beyova
{
    /// <summary>
    /// </summary>
    public interface ILocationEssential
    {
        /// <summary>
        /// Gets or sets the state or province.
        /// </summary>
        /// <value>
        /// The state or province.
        /// </value>
        string StateProvince { get; set; }

        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        /// <value>
        /// The city.
        /// </value>
        string City { get; set; }

        /// <summary>
        /// Gets or sets the street.
        /// </summary>
        /// <value>
        /// The street.
        /// </value>
        string Street { get; set; }
    }
}