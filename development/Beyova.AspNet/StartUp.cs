using System.Linq;
using System.Web.Mvc;
using Beyova.Api.RestApi;

namespace Beyova.Web
{
    /// <summary>
    ///
    /// </summary>
    public static class StartUp
    {
        /// <summary>
        /// Sets the json net as default json serializer.
        /// </summary>
        public static void SetJsonNetAsDefaultJsonSerializer()
        {
            ValueProviderFactories.Factories.Remove(ValueProviderFactories.Factories.
                OfType<JsonValueProviderFactory>().FirstOrDefault());
            ValueProviderFactories.Factories.Add(new JsonNetValueProviderFactory());

            ModelBinders.Binders.DefaultBinder = new JsonNetModelBinder();
        }

        /// <summary>
        /// Sets the default API setting event handler. It is especially needed when ASP.NET do not host API but uses <see cref="RestApiContextConsistenceAttribute"/>
        /// </summary>
        /// <param name="apiEventHandler">The API event handler.</param>
        public static void SetDefaultApiSettingEventHandler(IRestApiEventHandlers apiEventHandler)
        {
            if (apiEventHandler != null)
            {
                RestApiSettingPool.DefaultRestApiSettings.EventHandlers = apiEventHandler;
            }
        }

        /// <summary>
        /// Sets the unauthentication redirection route.
        /// </summary>
        /// <param name="getUnauthenticationRedirection">The get unauthentication redirection.</param>
        public static void SetUnauthenticationRedirectionRoute(RestApiContextConsistenceAttribute.GetUnauthenticationRedirectionDelegate getUnauthenticationRedirection)
        {
            RestApiContextConsistenceAttribute.GetUnauthenticationRedirection = getUnauthenticationRedirection;
        }
    }
}