using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using Beyova.ExceptionSystem;

namespace Beyova
{
    partial class ExceptionExtension
    {
        #region Http To Exception Scene

        /// <summary>
        /// To the exception scene.
        /// </summary>
        /// <param name="httpRequest">The HTTP request.</param>
        /// <param name="controllerOrServiceName">Name of the controller or service.</param>
        /// <returns>Beyova.ExceptionSystem.ExceptionScene.</returns>
        public static ExceptionScene ToExceptionScene(this HttpRequest httpRequest, string controllerOrServiceName = null)
        {
            return httpRequest == null ? null : new ExceptionScene
            {
                MethodName = string.Format("{0}: {1}", httpRequest.HttpMethod, httpRequest.RawUrl),
                FilePath = controllerOrServiceName
            };
        }

        /// <summary>
        /// To the exception scene.
        /// </summary>
        /// <param name="httpRequest">The HTTP request.</param>
        /// <param name="controllerOrServiceName">Name of the controller or service.</param>
        /// <returns>Beyova.ExceptionSystem.ExceptionScene.</returns>
        public static ExceptionScene ToExceptionScene(this HttpRequestBase httpRequest, string controllerOrServiceName = null)
        {
            return httpRequest == null ? null : new ExceptionScene
            {
                MethodName = string.Format("{0}: {1}", httpRequest.HttpMethod, httpRequest.RawUrl),
                FilePath = controllerOrServiceName
            };
        }

        #endregion Http To Exception Scene
    }
}
