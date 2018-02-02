using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beyova.ApiTracking
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class OmitApiTrackingAttribute : ApiTrackingOptionAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OmitApiTrackingAttribute"/> class.
        /// </summary>
        /// <param name="omitType">Type of the omit.</param>
        public OmitApiTrackingAttribute(ApiTrackingType omitType) : base(omitType)
        {
        }
    }
}
