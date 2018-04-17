//using System;
//using System.Collections.Generic;
//using System.Text;
//using Beyova.Http;
//using Beyova.VirtualSecuredTransferProtocol;

//namespace Beyova.Gravity
//{
//    /// <summary>
//    /// 
//    /// </summary>
//    /// <typeparam name="TRequest">The type of the request.</typeparam>
//    /// <typeparam name="TResponse">The type of the response.</typeparam>
//    public abstract class GravityHttpContextContainer<TRequest, TResponse> : HttpContextContainer<TRequest, TResponse>
//    {
//        /// <summary>
//        /// Initializes a new instance of the <see cref="GravityHttpContextContainer{TRequest, TResponse}" /> class.
//        /// </summary>
//        /// <param name="request">The request.</param>
//        /// <param name="response">The response.</param>
//        /// <param name="options">The options.</param>
//        protected GravityHttpContextContainer(TRequest request, TResponse response, HttpContextOptions<TRequest> options) : base(request, response, options)
//        {
//        }

//        #region Server Usages

//        /// <summary>
//        /// Processes the secure HTTP invoke.
//        /// </summary>
//        /// <typeparam name="TInput">The type of the t input.</typeparam>
//        /// <typeparam name="TOutput">The type of the t output.</typeparam>
//        /// <typeparam name="TGravityClientObject">The type of the t gravity client object.</typeparam>
//        /// <param name="httpContext">The HTTP context.</param>
//        /// <param name="getToken">The get token.</param>
//        /// <param name="processFunc">The process function.</param>
//        /// <param name="getClientObjectByToken">The get client object by token.</param>
//        /// <param name="omitStampValidation">The omit stamp validation.</param>
//        /// <returns>Beyova.ExceptionSystem.BaseException.</returns>
//        internal Beyova.ExceptionSystem.BaseException ProcessSecureHttpInvoke<TInput, TOutput, TGravityClientObject>(HttpContextContainer<TRequest, TResponse> httpContext,
//            Func<HttpContextContainer<TRequest, TResponse>, string> getToken,
//            Func<TInput, TOutput> processFunc,
//            Func<string, TGravityClientObject> getClientObjectByToken,
//            bool omitStampValidation = false)
//            where TGravityClientObject : class, IRsaKeys
//        {
//            if (httpContext != null)
//            {
//                if (getToken == null)
//                {
//                    getToken = (r) => { return r?.TryGetRequestHeader(HttpConstants.HttpHeader.TOKEN); };
//                }

//                string token = null;

//                try
//                {
//                    token = getToken(httpContext);

//                    TGravityClientObject clientObject = null;
//                    if (!string.IsNullOrWhiteSpace(token) && getClientObjectByToken != null)
//                    {
//                        clientObject = getClientObjectByToken(token);
//                    }

//                    clientObject.CheckNullObject(nameof(clientObject));
//                    clientObject.PrivateKey.CheckNullObject(nameof(clientObject.PrivateKey));

//                    var messageRequest = GetSecuredMessageRequest<TInput>(httpContext, clientObject.PrivateKey);
//                    var inputMessageObject = messageRequest.Message;

//                    inputMessageObject.CheckNullObject(nameof(inputMessageObject));
//                    inputMessageObject.Data.CheckNullObject("inputMessageObject.Data");

//                    if (!omitStampValidation && !inputMessageObject.ValidateStamp())
//                    {
//                        throw ExceptionFactory.CreateInvalidObjectException(nameof(messageRequest), reason: "Stamp Invalid.");
//                    }

//                    var ouput = processFunc(inputMessageObject.Data);
//                    ResponseSecureCommunicationPackage<TOutput>(httpContext, ouput, messageRequest.EncryptionKey);
//                }
//                catch (Exception ex)
//                {
//                    return ex.Handle(new { token, url = httpContext?.RawUrl });
//                }
//            }

//            return null;
//        }

//        #endregion Server Usages
//    }
//}
