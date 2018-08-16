using System;

namespace Beyova.Gravity
{
    /// <summary>
    /// Class GravityEntryObject.
    /// </summary>
    public class GravityEntryObject
    {
        /// <summary>
        /// Gets or sets the member key. 
        /// </summary>
        /// <value>The member identifiable key.</value>
        public string MemberKey { get; set; }

        /// <summary>
        /// Gets or sets the secrect key. (Token assigned by gravity central server)
        /// </summary>
        /// <value>
        /// The secrect key.
        /// </value>
        public string SecrectKey { get; set; }

        /// <summary>
        /// Gets or sets the public key. This the RSA public key of graviry server.
        /// </summary>
        /// <value>The public key.</value>
        public string PublicKey { get; set; }

        /// <summary>
        /// Gets or sets the gravity service URI. E.g.: https://localhost/gravity/
        /// </summary>
        /// <value>The gravity service URI.</value>
        public Uri GravityServiceUri { get; set; }

        /// <summary>
        /// Gets or sets the name of the configuration. When gravity server side has more than one configuration set, specify name to get.
        /// </summary>
        /// <value>The name of the configuration.</value>
        public string ConfigurationName { get; set; }
    }
}