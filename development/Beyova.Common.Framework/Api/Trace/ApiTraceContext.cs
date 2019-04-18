using System;
using System.Runtime.Remoting.Messaging;
using Beyova.Diagnostic;

namespace Beyova.Api
{
    /// <summary>
    /// Class ApiTraceContext.
    /// </summary>
    public static partial class ApiTraceContext
    {
        /// <summary>
        /// Exits the specified method message.
        /// </summary>
        /// <param name="methodMessage">The method message.</param>
        /// <param name="exitStamp">The exit stamp.</param>
        internal static void Exit(IMethodReturnMessage methodMessage, DateTime? exitStamp = null)
        {
            Exit(_current, (methodMessage.Exception as BaseException)?.Key, exitStamp);
        }
    }
}