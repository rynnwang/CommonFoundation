//using System;
//using System.Collections.Generic;
//using System.Collections.Specialized;
//using System.Text.RegularExpressions;
//using Beyova.Api;
//using Beyova.ExceptionSystem;

//namespace Beyova.Api.RestApi
//{
//    /// <summary>
//    /// Class ApiRouteIdentifierResolver. It is to convert URL to specific <see cref="ApiRouteIdentifier" /> instance.
//    /// </summary>
//    /// <typeparam name="TRequest">The type of the request.</typeparam>
//    /// <typeparam name="TResponse">The type of the response.</typeparam>
//    public class ApiRouteResolver<TRequest, TResponse> : ApiRouteResolver
//    {
//        /// <summary>
//        /// Initializes a new instance of the <see cref="ApiRouteResolver{TRequest, TResponse}"/> class.
//        /// </summary>
//        public ApiRouteResolver() : base()
//        {
//        }

//        /// <summary>
//        /// Maps the route.
//        /// </summary>
//        /// <param name="context">The context.</param>
//        /// <returns></returns>
//        public ApiRouteIdentifier MapRoute(HttpApiContextContainer<TRequest, TResponse> context)
//        {
//            return context == null ? null : MapRoute(context.HttpMethod, context.Url);
//        }
//    }

//    /// <summary>
//    /// Class ApiRouteIdentifierResolver. It is to convert URL to specific <see cref="ApiRouteIdentifier" /> instance.
//    /// </summary>
//    public class ApiRouteResolver
//    {
//        /// <summary>
//        /// The method match
//        /// </summary>
//        private static Regex methodMatch = new Regex(@"(/(?<realm>([^\/\?]+)))?/api/(?<version>([0-9a-zA-Z\-_\.]+))/(?<resource>([^\/\?]+))?(/(?<parameter1>([^\/\?]+)))?(/(?<parameter2>([^\/\?]+)))?(/)?", RegexOptions.Compiled | RegexOptions.IgnoreCase);

//        /// <summary>
//        /// Initializes a new instance of the <see cref="ApiRouteResolver"/> class.
//        /// </summary>
//        public ApiRouteResolver() : base()
//        {
//        }

//        /// <summary>
//        /// Maps the route.
//        /// </summary>
//        /// <param name="httpMethod">The HTTP method.</param>
//        /// <param name="uri">The URI.</param>
//        /// <returns></returns>
//        public virtual ApiRouteIdentifier MapRoute(string httpMethod, Uri uri)
//        {
//            try
//            {
//                uri.CheckNullObject(nameof(uri));
//                httpMethod.CheckEmptyString(nameof(httpMethod));

//                var match = methodMatch.Match(uri.PathAndQuery);

//                if (match.Success)
//                {
//                    var parameter1 = match.Success ? match.Result("${parameter1}") : string.Empty;
//                    var parameter2 = match.Success ? match.Result("${parameter2}") : string.Empty;
//                    var result = new ApiRouteIdentifier
//                    {
//                        Resource = match.Success ? match.Result("${resource}") : string.Empty,
//                        Version = match.Success ? match.Result("${version}") : string.Empty,
//                        Realm = match.Success ? match.Result("${realm}") : string.Empty
//                    };

//                    // Built in feature does not use parameter from path
//                    if (result.Version.Equals(ApiConstants.BuiltInFeatureVersionKeyword, StringComparison.OrdinalIgnoreCase))
//                    {
//                        return result;
//                    }

//                    if (string.IsNullOrWhiteSpace(result.Resource))
//                    {
//                        throw new ResourceNotFoundException(nameof(ApiRouteIdentifier), uri.ToString());
//                    }

//                    // Now need to know about Param 1 and 2
//                    if (!string.IsNullOrWhiteSpace(parameter1) && !string.IsNullOrWhiteSpace(parameter2))
//                    {
//                        result.Action = parameter1;
//                    }
//                    else
//                    {
//                        if (result.HttpMethod.Equals(HttpConstants.HttpMethod.Delete))
//                        {

//                        }
//                    }

//                    return result;
//                }
//                else
//                {
//                    throw ExceptionFactory.CreateInvalidObjectException(nameof(uri));
//                }
//            }
//            catch (Exception ex)
//            {
//                throw ex.Handle(new { Uri = uri?.ToString(), HttpMethod = httpMethod });
//            }
//        }
//    }
//}
