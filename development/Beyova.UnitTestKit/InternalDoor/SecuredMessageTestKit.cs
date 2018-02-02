using System;
using Beyova.Gravity;

namespace Beyova.UnitTestKit.InternalDoor
{
    /// <summary>
    /// Class SecuredMessageTestKit.
    /// </summary>
    public static class SecuredMessageTestKit
    {
        /// <summary>
        /// Secures the HTTP invoke.
        /// </summary>
        /// <typeparam name="TInput">The type of the input.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <param name="uri">The URI.</param>
        /// <param name="data">The data.</param>
        /// <param name="rsaPublicKey">The RSA public key.</param>
        /// <param name="token">The token.</param>
        /// <returns>
        /// System.String.
        /// </returns>
        public static SecuredMessageObject<TOutput> SecureHttpInvoke<TInput, TOutput>(this Uri uri, TInput data, string rsaPublicKey, string token)
        {
            return GravityExtension.SecureHttpInvoke<TInput, TOutput>(uri, data, rsaPublicKey, token);
        }

        /// <summary>
        /// As secured message object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="httpRequestBodyBytes">The HTTP request body bytes.</param>
        /// <param name="encryptionKey">The encryption key.</param>
        /// <returns>SecuredMessageObject&lt;T&gt;.</returns>
        public static SecuredMessageObject<T> ConvertToSecuredMessageObject<T>(this byte[] httpRequestBodyBytes, byte[] encryptionKey)
        {
            return GravityExtension.ConvertToSecuredMessageObject<T>(httpRequestBodyBytes, encryptionKey);
        }

        /// <summary>
        /// Converts to secured message package.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="messageObject">The message object.</param>
        /// <param name="publicKey">The public key.</param>
        /// <returns>System.Byte[].</returns>
        public static byte[] ConvertToSecuredMessagePackage<T>(this SecuredMessageRequest<T> messageObject, string publicKey)
        {
            return GravityExtension.ConvertToSecuredMessagePackage<T>(messageObject, publicKey)?.ToBytes();
        }

        /// <summary>
        /// Converts to secured message request.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="package">The package.</param>
        /// <param name="privateKey">The private key.</param>
        /// <returns>Beyova.Gravity.SecuredMessageRequest&lt;T&gt;.</returns>
        public static SecuredMessageRequest<T> ConvertToSecuredMessageRequest<T>(this SecuredMessagePackage package, string privateKey)
        {
            return GravityExtension.ConvertToSecuredMessageRequest<T>(package, privateKey);
        }

        /// <summary>
        /// To the bytes.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="messageObject">The message object.</param>
        /// <param name="encryptionKey">The encryption key.</param>
        /// <returns>System.Byte[].</returns>
        public static byte[] ToBytes<T>(SecuredMessageObject<T> messageObject, byte[] encryptionKey)
        {
            return GravityExtension.ToBytes<T>(messageObject, encryptionKey);
        }
    }
}
