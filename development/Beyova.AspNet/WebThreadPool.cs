using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading;

namespace Beyova.Web
{
    /// <summary>
    ///
    /// </summary>
    public static class WebThreadPool
    {
        /// <summary>
        /// The thread credentials
        /// </summary>
        private static Dictionary<int, ICredential> threadCredentials = new Dictionary<int, ICredential>();

        /// <summary>
        /// Registers this instance.
        /// </summary>
        internal static void Register()
        {
            threadCredentials.Merge(Thread.CurrentThread.ManagedThreadId, ContextHelper.ApiContext.CurrentCredential);
        }

        /// <summary>
        /// Gets the credentials.
        /// </summary>
        /// <value>
        /// The credentials.
        /// </value>
        public static JToken Credentials
        {
            get
            {
                return JToken.FromObject(threadCredentials);
            }
        }
    }
}