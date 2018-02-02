using System;
using System.Security.Cryptography;
using System.Text;

namespace Beyova.SaasPlatform
{
    /// <summary>
    /// Class SessionInfo.
    /// </summary>
    public class SecuredSessionCache : SessionInfo, IDisposable
    {
        /// <summary>
        /// Gets or sets the message decryptor.
        /// </summary>
        /// <value>
        /// The message decryptor.
        /// </value>
        public RSACryptoServiceProvider MessageDecryptor { get; protected set; }

        /// <summary>
        /// Gets or sets the message encryptor.
        /// </summary>
        /// <value>
        /// The message encryptor.
        /// </value>
        public RSACryptoServiceProvider MessageEncryptor { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecuredSessionCache"/> class.
        /// </summary>
        /// <param name="securedSessionInfo">The secured session information.</param>
        /// <param name="dwKeySize">Size of the dw key.</param>
        public SecuredSessionCache(SecuredSessionInfo securedSessionInfo, int dwKeySize = DefaultSecuritySettings.DoubleWordKeySize) : base(securedSessionInfo)
        {
            if (!string.IsNullOrWhiteSpace(securedSessionInfo.ServerPrivateKey))
            {
                MessageDecryptor = new RSACryptoServiceProvider(dwKeySize);
                MessageDecryptor.ImportCspBlob(Convert.FromBase64String(securedSessionInfo.ServerPrivateKey));
            }

            if (!string.IsNullOrWhiteSpace(securedSessionInfo.ClientPublicKey))
            {
                MessageEncryptor = new RSACryptoServiceProvider(dwKeySize);
                MessageEncryptor.ImportCspBlob(Convert.FromBase64String(securedSessionInfo.ClientPublicKey));
            }
        }

        /// <summary>
        /// Encrypts the message.
        /// </summary>
        /// <param name="responseBody">The response body.</param>
        /// <returns></returns>
        public byte[] EncryptMessage(object responseBody)
        {
            return MessageEncryptor?.Encrypt(Encoding.UTF8.GetBytes(responseBody.ToJson(false)), true);
        }

        /// <summary>
        /// Decrypts the message.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="requestBody">The request body.</param>
        /// <returns></returns>
        public TEntity DecryptMessage<TEntity>(byte[] requestBody)
        {
            return (MessageDecryptor?.Decrypt(requestBody, true)).DecodeToUtf8String().TryConvertJsonToObject<TEntity>();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            MessageDecryptor?.Dispose();
            MessageEncryptor?.Dispose();
        }
    }
}