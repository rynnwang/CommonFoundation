using System.Security.Cryptography;
using Newtonsoft.Json.Linq;

namespace Beyova
{
    /// <summary>
    /// Class DefaultDataSecurityProvider
    /// </summary>
    public sealed class DefaultDataSecurityProvider : IDataSecurityProvider
    {
        /// <summary>
        /// The intance
        /// </summary>
        static DefaultDataSecurityProvider _intance = new DefaultDataSecurityProvider(new AesKeys
        {
            Key = "FXDh3AkXYSPECIZGmZbnMJED/dTFcYsv+IuJ6JSz/KQ=",
            InitializationVector = "4jmXeWg/7iBGv5srl2uj8g==",
            KeySize = 256
        });

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static DefaultDataSecurityProvider Instance { get { return _intance; } }

        /// <summary>
        /// The provider
        /// </summary>
        private RijndaelProvider _provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultDataSecurityProvider"/> class.
        /// </summary>
        /// <param name="aesKeys">The aes keys.</param>
        private DefaultDataSecurityProvider(AesKeys aesKeys)
        {
            _provider = aesKeys.CreateAesProvider();
        }

        /// <summary>
        /// Decrypts the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public byte[] Decrypt(byte[] value)
        {
            return _provider?.DecryptAes(value);
        }

        /// <summary>
        /// Encrypts the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public byte[] Encrypt(byte[] value)
        {
            return _provider?.EncryptAes(value);
        }
    }
}