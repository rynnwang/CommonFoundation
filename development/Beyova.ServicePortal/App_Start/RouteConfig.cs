using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Beyova.Api.RestApi;
using Beyova.CentralAuthentication.Server;
using Beyova.Gravity;

using Beyova.ServicePortal.Controllers;
using Beyova.Web;

namespace Beyova.ServicePortal
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            var centralAuthenticationServer = new CentralAuthenticationServer();
            GravityApiRouter router = new GravityApiRouter(centralAuthenticationServer);

            routes.Add(router.ToWebRoute());

            var apiRouter = new RestApiRouter(new RestApiSettings
            {
                EventHandlers = new CentralAuthenticationServerEventHandlers(),
                ApiTracking = Framework.ApiTracking,
                TokenHeaderKey = HttpConstants.HttpHeader.TOKEN,
                TrackingEvent = true
            });

            apiRouter.Add(centralAuthenticationServer, new RestApiSettings
            {
                EventHandlers = new CentralAuthenticationServerEventHandlers(),
                ApiTracking = Framework.ApiTracking,
                TokenHeaderKey = HttpConstants.HttpHeader.TOKEN,
                TrackingEvent = true
            });

            routes.Add(apiRouter.ToWebRoute());

            CodeSmith.CodeSmithController.RegisterToRoute(routes);
            ErrorController.RegisterToRoute(routes);

            #region /{ProductModule}/

            routes.MapRoute(
                name: "ProductModules",
                url: "{productKey}/{controller}/{action}/{key}",
                defaults: new { controller = "Administration", action = "Index", key = UrlParameter.Optional },
                constraints: new
                {
                    productKey = @"[a-fA-F0-9]{8}(-[a-fA-F0-9]{4}){3}-[a-fA-F0-9]{12}",
                    controller = @"((Administration))"
                }
            );

            #endregion

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{key}",
                defaults: new { controller = "Home", action = "Index", key = UrlParameter.Optional }
            );
        }
    }
}
