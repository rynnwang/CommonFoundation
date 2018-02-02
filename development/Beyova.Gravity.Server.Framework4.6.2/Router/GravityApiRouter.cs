using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Routing;
using Beyova.Api.RestApi;
using Beyova.ExceptionSystem;


namespace Beyova.Gravity
{
    /// <summary>
    /// Class GravityApiRouter.
    /// </summary>
    public sealed class GravityApiRouter : IHttpHandler, IRouteHandler, IGravityServiceProtocol
    {
        #region 

        /// <summary>
        /// The parameter key_ action
        /// </summary>
        private const string parameterKey_Action = "action";

        /// <summary>
        /// The parameter key_ module
        /// </summary>
        private const string parameterKey_Module = "module";

        #endregion

        /// <summary>
        /// The service core
        /// </summary>
        private GravityServiceCore serviceCore = new GravityServiceCore();

        /// <summary>
        /// The central authentication
        /// </summary>
        private ICentralAuthenticationProtocol centralAuthentication = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="GravityApiRouter" /> class.
        /// </summary>
        /// <param name="centralAuthenticationProtocol">The central authentication protocol.</param>
        public GravityApiRouter(ICentralAuthenticationProtocol centralAuthenticationProtocol = null)
        {
            centralAuthentication = centralAuthenticationProtocol;
        }

        #region IHttpHandler

        /// <summary>
        /// Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler" /> instance.
        /// </summary>
        /// <value><c>true</c> if this instance is reusable; otherwise, <c>false</c>.</value>
        public bool IsReusable
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler" /> interface.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpContext" /> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests.</param>
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                var module = context.Request.QueryString.Get(parameterKey_Module).SafeToString().ToLowerInvariant();
                switch (module)
                {
                    case "":
                        ProcessDefaultModule(context);
                        break;
                    case "authentication":
                        ProcessCentralAuthenticationModule(context);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                ApiHandlerBase.PackageResponse(context.Response, null, ex.Handle(new
                {
                    url = context.Request.RawUrl
                }));
            }
            finally
            {
                GravityContext.Dispose();
            }
        }

        /// <summary>
        /// Processes the default module.
        /// </summary>
        /// <param name="context">The context.</param>
        private void ProcessDefaultModule(HttpContext context)
        {
            try
            {
                var action = context.Request.QueryString.Get(parameterKey_Action);
                BaseException exception = null;
                switch (action.SafeToLower())
                {
                    case "heartbeat":
                        exception = GravityExtension.ProcessSecureHttpInvoke<Heartbeat, HeartbeatEcho, ProductInfo>(context, GetToken, this.Heartbeat, GetProductInfoByToken);
                        break;
                    case "configuration":
                        exception = GravityExtension.ProcessSecureHttpInvoke<string, RemoteConfigurationObject, ProductInfo>(context, GetToken, this.RetrieveConfiguration, GetProductInfoByToken);
                        break;
                    case "command":
                        exception = GravityExtension.ProcessSecureHttpInvoke<GravityCommandResult, Guid?, ProductInfo>(context, GetToken, this.CommitCommandResult, GetProductInfoByToken);
                        break;
                    default:
                        throw ExceptionFactory.CreateInvalidObjectException(nameof(action), action);
                }
            }
            catch (Exception ex)
            {
                ApiHandlerBase.PackageResponse(context.Response, null, ex.Handle(new { url = context.Request.RawUrl }));
            }
        }

        /// <summary>
        /// Processes the central authentication module.
        /// </summary>
        /// <param name="context">The context.</param>
        private void ProcessCentralAuthenticationModule(HttpContext context)
        {
            try
            {
                if (centralAuthentication == null)
                {
                    throw new UnimplementedException("CentralAuthentication");
                }

                var action = context.Request.QueryString.Get(parameterKey_Action);
                BaseException exception = null;
                switch (action.SafeToLower())
                {
                    case "token":
                        exception = GravityExtension.ProcessSecureHttpInvoke<string, AdminUserInfo, ProductInfo>(context, GetToken, centralAuthentication.GetUserByToken, GetProductInfoByToken);
                        break;
                    case "session":
                        exception = GravityExtension.ProcessSecureHttpInvoke<string, List<AdminSession>, ProductInfo>(context, GetToken, centralAuthentication.GetSessionList, GetProductInfoByToken);
                        break;
                    case "dispose":
                        exception = GravityExtension.ProcessSecureHttpInvoke<string, DateTime?, ProductInfo>(context, GetToken, centralAuthentication.DisposeSession, GetProductInfoByToken);
                        break;
                    case "authenticate":
                        exception = GravityExtension.ProcessSecureHttpInvoke<AuthenticationRequest, AdminAuthenticationResult, ProductInfo>(context, GetToken, centralAuthentication.Authenticate, GetProductInfoByToken);
                        break;
                    default:
                        throw ExceptionFactory.CreateInvalidObjectException(nameof(action), action);
                }
            }
            catch (Exception ex)
            {
                ApiHandlerBase.PackageResponse(context.Response, null, ex.Handle(new { url = context.Request.RawUrl }));
            }
        }

        #endregion

        #region IRouteHandler

        /// <summary>
        /// Provides the object that processes the request.
        /// </summary>
        /// <param name="requestContext">An object that encapsulates information about the request.</param>
        /// <returns>An object that processes the request.</returns>
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return this;
        }

        #endregion

        /// <summary>
        /// Gets the token.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>System.String.</returns>
        private static string GetToken(HttpRequest request)
        {
            return request?.TryGetHeader(HttpConstants.HttpHeader.TOKEN);
        }

        /// <summary>
        /// Gets the product information by token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>Beyova.Gravity.ProductInfo.</returns>
        private ProductInfo GetProductInfoByToken(string token)
        {
            try
            {
                var currentProduct = serviceCore.GetProductInfoByToken(token);
                GravityContext.ProductInfo = currentProduct;
                return currentProduct;
            }
            catch { }

            return null;
        }

        /// <summary>
        /// To the web route.
        /// </summary>
        /// <returns>Route.</returns>
        public Route ToWebRoute()
        {
            var routeValueDictionary = new RouteValueDictionary { { "apiUrl", @"(([^\/\?]+)/)?[Gg][rR][aA][vV][iI][tT][yY]/[Vv]1/(.)?" } };

            return new Route("{*apiUrl}", defaults: null, routeHandler: this, constraints: routeValueDictionary);
        }

        #region Default module actions

        /// <summary>
        /// Heartbeats the specified heartbeat.
        /// </summary>
        /// <param name="heartbeat">The heartbeat.</param>
        /// <returns>Beyova.Gravity.HeartbeatEcho.</returns>
        public HeartbeatEcho Heartbeat(Heartbeat heartbeat)
        {
            try
            {
                var clientKey = serviceCore.SaveHeartbeatInfo(GravityContext.ProductKey, heartbeat);
                clientKey.CheckNullObject(nameof(clientKey));

                return new HeartbeatEcho
                {
                    CommandRequests = serviceCore.GetPendingCommandRequest(clientKey),
                    ClientKey = clientKey
                };
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { heartbeat });
            }
        }

        /// <summary>
        /// Retrieves the configuration.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>RemoteConfigurationObject.</returns>
        public RemoteConfigurationObject RetrieveConfiguration(string name = null)
        {
            try
            {
                return serviceCore.RetrieveConfiguration(GravityContext.ProductKey, name);
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { name });
            }
        }

        /// <summary>
        /// Commits the command result.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns>System.Nullable&lt;Guid&gt;.</returns>
        public Guid? CommitCommandResult(GravityCommandResult result)
        {
            try
            {
                result.CheckNullObject(nameof(result));

                return serviceCore.CommitCommandResult(result);
            }
            catch (Exception ex)
            {
                throw ex.Handle(result);
            }
        }

        #endregion
    }
}
