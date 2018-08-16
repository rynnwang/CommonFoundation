using System;
using Beyova.Api.RestApi;

namespace Beyova.Gravity
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class GravityApiOperationAttribute : ApiOperationAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GravityApiOperationAttribute" /> class.
        /// </summary>
        /// <param name="resourceName">Name of the resource.</param>
        /// <param name="action">The action.</param>
        public GravityApiOperationAttribute(string resourceName, string action)
            : base(resourceName, HttpConstants.HttpMethod.Post, action, GravityConstants.ContentType, true)
        {
        }
    }
}