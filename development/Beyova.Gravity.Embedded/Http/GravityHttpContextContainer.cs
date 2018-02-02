using System;
using System.Collections.Generic;
using System.Text;
using Beyova.Http;

namespace Beyova.Gravity
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    public abstract class GravityHttpContextContainer<TRequest, TResponse> : HttpContextContainer<TRequest, TResponse>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GravityHttpContextContainer{TRequest, TResponse}" /> class.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="response">The response.</param>
        /// <param name="options">The options.</param>
        protected GravityHttpContextContainer(TRequest request, TResponse response, HttpContextOptions<TRequest> options) : base(request, response, options)
        {
        }

        #region Server Usages

        /// <summary>
        /// To bytes.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="messageObject">The message object.</param>
        /// <param name="encryptionKey">The encryption key.</param>
        /// <returns>System.Byte[].</returns>
        internal static byte[] ToBytes<T>(SecuredMessageObject<T> messageObject, byte[] encryptionKey)
        {
            if (messageObject != null && encryptionKey.HasItem())
            {
                return Encoding.UTF8.GetBytes(messageObject.ToJson(false)).EncryptTripleDES(encryptionKey);
            }

            return null;
        }

        /// <summary>
        /// Converts to secured message request.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="package">The package.</param>
        /// <param name="privateKey">The private key.</param>
        /// <returns>SecuredMessageRequest&lt;T&gt;.</returns>
        internal static SecuredMessageRequest<T> ConvertToSecuredMessageRequest<T>(SecuredMessagePackage package, string privateKey)
        {
            try
            {
                package.CheckNullObject(nameof(package));
                privateKey.CheckNullObject(nameof(privateKey));

                package.Security.CheckNullOrEmptyCollection(nameof(package.Security));
                package.Data.CheckNullOrEmptyCollection(nameof(package.Data));

                var encryptionKey = package.Security.RsaDecrypt(privateKey);
                encryptionKey.CheckNullOrEmptyCollection(nameof(encryptionKey));

                return new SecuredMessageRequest<T>
                {
                    EncryptionKey = encryptionKey,
                    Message = Encoding.UTF8.GetString(package.Data.DecryptTripleDES(encryptionKey)).TryConvertJsonToObject<SecuredMessageObject<T>>()
                };
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { package, privateKey });
            }
        }

        /// <summary>
        /// Gets the secured message request.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="httpContext">The HTTP context.</param>
        /// <param name="privateKey">The private key.</param>
        /// <returns>
        /// SecuredMessageRequest&lt;T&gt;.
        /// </returns>
        internal SecuredMessageRequest<T> GetSecuredMessageRequest<T>(HttpContextContainer<TRequest, TResponse> httpContext, string privateKey)
        {
            try
            {
                httpContext.CheckNullObject(nameof(httpContext));
                httpContext.Request.CheckNullObject(nameof(httpContext.Request));
                privateKey.CheckEmptyString(nameof(privateKey));

                var requestData = httpContext.RequestBodyStream.ToBytes();
                var package = SecuredMessagePackage.FromBytes(requestData);
                return ConvertToSecuredMessageRequest<T>(package, privateKey);
            }
            catch (Exception ex)
            {
                throw ex.Handle(data: new { privateKey });
            }
        }

        /// <summary>
        /// Responses the secure communication package.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context">The context.</param>
        /// <param name="responseObject">The response object.</param>
        /// <param name="encryptionKey">The encryption key.</param>
        internal static void ResponseSecureCommunicationPackage<T>(HttpContextContainer<TRequest, TResponse> context, T responseObject, byte[] encryptionKey)
        {
            try
            {
                context.CheckNullObject(nameof(context));
                responseObject.CheckNullObject(nameof(responseObject));
                encryptionKey.CheckNullOrEmptyCollection(nameof(encryptionKey));

                var responseBodyBytes = ToBytes(new SecuredMessageObject<T> { Data = responseObject }, encryptionKey);
                context.WriteResponseBody(responseBodyBytes, HttpConstants.ContentType.Json);
            }
            catch (Exception ex)
            {
                throw ex.Handle(data: new { responseObject, encryptionKey });
            }
        }

        /// <summary>
        /// Processes the secure HTTP invoke.
        /// </summary>
        /// <typeparam name="TInput">The type of the t input.</typeparam>
        /// <typeparam name="TOutput">The type of the t output.</typeparam>
        /// <typeparam name="TGravityClientObject">The type of the t gravity client object.</typeparam>
        /// <param name="httpContext">The HTTP context.</param>
        /// <param name="getToken">The get token.</param>
        /// <param name="processFunc">The process function.</param>
        /// <param name="getClientObjectByToken">The get client object by token.</param>
        /// <param name="omitStampValidation">The omit stamp validation.</param>
        /// <returns>Beyova.ExceptionSystem.BaseException.</returns>
        internal Beyova.ExceptionSystem.BaseException ProcessSecureHttpInvoke<TInput, TOutput, TGravityClientObject>(HttpContextContainer<TRequest, TResponse> httpContext,
            Func<HttpContextContainer<TRequest, TResponse>, string> getToken,
            Func<TInput, TOutput> processFunc,
            Func<string, TGravityClientObject> getClientObjectByToken,
            bool omitStampValidation = false)
            where TGravityClientObject : class, IRsaKeys
        {
            if (httpContext != null)
            {
                if (getToken == null)
                {
                    getToken = (r) => { return r?.TryGetRequestHeader(HttpConstants.HttpHeader.TOKEN); };
                }

                string token = null;

                try
                {
                    token = getToken(httpContext);

                    TGravityClientObject clientObject = null;
                    if (!string.IsNullOrWhiteSpace(token) && getClientObjectByToken != null)
                    {
                        clientObject = getClientObjectByToken(token);
                    }

                    clientObject.CheckNullObject(nameof(clientObject));
                    clientObject.PrivateKey.CheckNullObject(nameof(clientObject.PrivateKey));

                    var messageRequest = GetSecuredMessageRequest<TInput>(httpContext, clientObject.PrivateKey);
                    var inputMessageObject = messageRequest.Message;

                    inputMessageObject.CheckNullObject(nameof(inputMessageObject));
                    inputMessageObject.Data.CheckNullObject("inputMessageObject.Data");

                    if (!omitStampValidation && !inputMessageObject.ValidateStamp())
                    {
                        throw ExceptionFactory.CreateInvalidObjectException(nameof(messageRequest), reason: "Stamp Invalid.");
                    }

                    var ouput = processFunc(inputMessageObject.Data);
                    ResponseSecureCommunicationPackage<TOutput>(httpContext, ouput, messageRequest.EncryptionKey);
                }
                catch (Exception ex)
                {
                    return ex.Handle(new { token, url = httpContext?.RawUrl });
                }
            }

            return null;
        }

        #endregion Server Usages
    }
}
