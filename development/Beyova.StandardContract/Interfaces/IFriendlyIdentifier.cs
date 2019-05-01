using System;

namespace Beyova
{
    /// <summary>
    /// Interface for object IIdentifier.
    /// </summary>
    public interface IFriendlyIdentifier: IIdentifier
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        string Name { get; set; }
    }
}