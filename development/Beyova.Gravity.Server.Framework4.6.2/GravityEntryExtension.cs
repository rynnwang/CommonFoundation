using System;
using System.Text;
using System.Web;
using Beyova.ExceptionSystem;
using Newtonsoft.Json;

namespace Beyova.Gravity
{
    /// <summary>
    /// Class GravityEntryExtension.
    /// </summary>
    internal static partial class GravityEntryExtension
    {
        /// <summary>
        /// Creates the gravity entry file.
        /// </summary>
        /// <param name="productInfo">The product information.</param>
        /// <param name="gravityUri">The gravity URI.</param>
        /// <param name="configurationName">Name of the configuration.</param>
        /// <param name="issueTo">The issue to.</param>
        /// <returns>GravityEntryFile.</returns>
        public static GravityEntryFile CreateGravityEntryFile(this ProductInfo productInfo, string gravityUri, string configurationName, string issueTo = null)
        {
            return productInfo != null ? new GravityEntryFile
            {
                IssuedTo = issueTo,
                IssuedStamp = productInfo.CreatedStamp,
                GravityServiceUri = new Uri(gravityUri.SafeToString("http://beyova.chinacloudsites.cn/")),
                MemberIdentifiableKey = productInfo.Token,
                PublicKey = productInfo.PublicKey,
                ConfigurationName = configurationName
            } : null;
        }
    }
}
