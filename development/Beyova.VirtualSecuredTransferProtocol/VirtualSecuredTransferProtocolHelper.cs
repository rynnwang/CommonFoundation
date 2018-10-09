using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Beyova.ExceptionSystem;

namespace Beyova.VirtualSecuredTransferProtocol
{
    /// <summary>
    /// class VirtualSecuredTransferProtocolHelper
    /// </summary>
    public static class VirtualSecuredTransferProtocolHelper
    {
        /// <summary>
        /// The security key indication byte length
        /// </summary>
        const int _securityKeyIndicationByteLength = sizeof(UInt16);

        /// <summary>
        /// The stamp indication byte length
        /// </summary>
        const int _stampIndicationByteLength = sizeof(UInt64);

        /// <summary>
        /// The schema version indication byte length
        /// </summary>
        const int _schemaVersionIndicationByteLength = 1;

        /// <summary>
        /// The allowed stamp deviation
        /// </summary>
        const int allowedStampDeviation = 10 * 60; //10 mins

        /// <summary>
        /// The schema version
        /// </summary>
        const int _schemaVersion = 1;

        /// <summary>
        /// The pack initial capacity
        /// </summary>
        const int _packInitialCapacity = 2048;

        /// <summary>
        /// The dw key size
        /// </summary>
        const int dwKeySize = 2048;

        /// <summary>
        /// The zero stamp
        /// </summary>
        static readonly DateTime _zeroStamp = new DateTime(2016, 1, 1);

        /// <summary>
        /// The allowed stamp second deviation
        /// </summary>
        static int _allowedStampSecondDeviation = 5 * 60;// 5 min

        /// <summary>
        /// Gets the schema version.
        /// </summary>
        /// <value>
        /// The schema version.
        /// </value>
        public static int SchemaVersion { get { return _schemaVersion; } }

        /// <summary>
        /// Gets the size of the dw key.
        /// </summary>
        /// <value>
        /// The size of the dw key.
        /// </value>
        public static int DwKeySize { get { return dwKeySize; } }

        /// <summary>
        /// Gets or sets the allowed stamp second deviation.
        /// </summary>
        /// <value>
        /// The allowed stamp second deviation.
        /// </value>
        public static int AllowedStampSecondDeviation
        {
            get { return _allowedStampSecondDeviation; }
            set
            {
                if (value > 0)
                {
                    _allowedStampSecondDeviation = value;
                }
            }
        }

        /// <summary>
        /// Initializes the <see cref="VirtualSecuredTransferProtocolHelper"/> class.
        /// </summary>
        static VirtualSecuredTransferProtocolHelper()
        {
        }

        /// <summary>
        /// Gets the stamp bytes.
        /// </summary>
        /// <param name="utcStamp">The UTC stamp.</param>
        /// <returns></returns>
        static byte[] GetStampBytes(DateTime utcStamp)
        {
            var offset = (long)((utcStamp - _zeroStamp).TotalSeconds);
            return BitConverter.GetBytes(offset);
        }

        /// <summary>
        /// Gets the UTC stamp from offset bytes.
        /// </summary>
        /// <param name="stampOffsetBytes">The stamp offset bytes.</param>
        /// <returns></returns>
        static DateTime GetUtcStampFromOffsetBytes(byte[] stampOffsetBytes)
        {
            var offset = BitConverter.ToUInt64(stampOffsetBytes, 0);
            return _zeroStamp.AddSeconds(offset);
        }

        #region VirtualSecuredRequestRawMessage <-> Bytes

        /// <summary>
        /// Packs to bytes.
        /// </summary>
        /// <param name="requestMessage">The request message.</param>
        /// <param name="rsaPublicKey">The RSA public key.</param>
        /// <param name="aesProvider">The aes provider.</param>
        /// <returns></returns>
        public static byte[] PackToBytes(this VirtualSecuredRequestRawMessage requestMessage, CryptoKey rsaPublicKey, RijndaelProvider aesProvider)
        {
            // Byte[] composition: [Schema Version]{1}[UTC Stamp]{4}[Encrypted Security Key Length Indication]{2+2}[Encrypted Security Key]{M+N}[Encrypted Body]{L}.

            try
            {
                rsaPublicKey.CheckNullObject(nameof(rsaPublicKey));
                aesProvider.CheckNullObject(nameof(aesProvider));

                requestMessage.CheckNullObject(nameof(requestMessage));
                requestMessage.SymmetricPrimaryKey.CheckNullObject(nameof(requestMessage.SymmetricPrimaryKey));
                requestMessage.Stamp.CheckNullObject(nameof(requestMessage.Stamp));

                List<byte> result = new List<byte>(_packInitialCapacity);

                // index1: version.
                result.Add(Convert.ToByte(requestMessage.SchemaVersion));

                // index2: Stamp.
                result.AddRange(GetStampBytes(requestMessage.Stamp ?? DateTime.UtcNow));

                // index3: Encrypted Security Key Length Indication.
                var encryptedSymmetricPrimaryKey = EncodingOrSecurityExtension.RsaEncrypt(requestMessage.SymmetricPrimaryKey, rsaPublicKey);
                var encryptedSymmetricSecondaryKey = (requestMessage.SymmetricSecondaryKey.ByteValue == null) ? null : EncodingOrSecurityExtension.RsaEncrypt(requestMessage.SymmetricSecondaryKey, rsaPublicKey);
                result.AddRange(BitConverter.GetBytes((UInt16)encryptedSymmetricPrimaryKey.Length));
                result.AddRange(BitConverter.GetBytes((UInt16)encryptedSymmetricSecondaryKey.Length));

                // index4: Encrypted Security Key
                result.AddRange(encryptedSymmetricPrimaryKey);
                if (encryptedSymmetricSecondaryKey.HasItem())
                {
                    result.AddRange(encryptedSymmetricSecondaryKey);
                }

                // index5: Encrypted content
                result.AddRange(aesProvider.EncryptAes(requestMessage.Data));

                return result.ToArray();
            }
            catch (Exception ex)
            {
                throw ex.Handle();
            }
        }

        /// <summary>
        /// Unpacks from bytes.
        /// </summary>
        /// <param name="requestBytes">The request bytes.</param>
        /// <param name="rsaProvider">The RSA provider.</param>
        /// <param name="aesProvider">The aes provider.</param>
        /// <returns></returns>
        public static VirtualSecuredRequestRawMessage UnpackRequestFromBytes(this byte[] requestBytes, RSACryptoServiceProvider rsaProvider, out RijndaelProvider aesProvider)
        {
            // Byte[] composition: [Schema Version]{1}[UTC Stamp]{4}[Encrypted Security Key Length Indication]{2+2}[Encrypted Security Key]{M+N}[Encrypted Body]{L}.
            aesProvider = null;

            try
            {
                rsaProvider.CheckNullObject(nameof(rsaProvider));
                requestBytes.CheckNullOrEmptyCollection(nameof(requestBytes));

                var result = new VirtualSecuredRequestRawMessage
                {
                    // Index1
                    SchemaVersion = Convert.ToInt32(requestBytes[0])
                };

                var currentIndex = 1;
                // Index2
                var stampBytes = requestBytes.Read(_stampIndicationByteLength, ref currentIndex);
                result.Stamp = GetUtcStampFromOffsetBytes(stampBytes);

                ValidateStamp(result.Stamp);

                // Index3
                var primaryKeyLengthBytes = requestBytes.Read(_securityKeyIndicationByteLength, ref currentIndex);
                var primaryKeyLength = BitConverter.ToUInt16(primaryKeyLengthBytes, 0);
                var secondaryKeyLengthBytes = requestBytes.Read(_securityKeyIndicationByteLength, ref currentIndex);
                var secondaryKeyLength = BitConverter.ToUInt16(secondaryKeyLengthBytes, 0);

                // Index4
                var primaryKeyBytes = requestBytes.Read(primaryKeyLength, ref currentIndex);
                result.SymmetricPrimaryKey = rsaProvider.Decrypt(primaryKeyBytes, true);

                var secondaryKeyBytes = requestBytes.Read(secondaryKeyLength, ref currentIndex);
                result.SymmetricSecondaryKey = secondaryKeyBytes == null ? null : rsaProvider.Decrypt(secondaryKeyBytes, true);

                aesProvider = new AesKeys { KeySize = dwKeySize, InitializationVector = result.SymmetricSecondaryKey, Key = result.SymmetricPrimaryKey }.CreateAesProvider();

                // Index5
                var dataBytes = requestBytes.SubArray(currentIndex);
                result.Data = aesProvider.DecryptAes(dataBytes);

                return result;
            }
            catch (Exception ex)
            {
                throw ex.Handle();
            }
        }

        #endregion

        #region VirtualSecuredResponseRawMessage <-> Bytes

        /// <summary>
        /// Packs to bytes.
        /// </summary>
        /// <param name="responseMessage">The response message.</param>
        /// <param name="rsaProvider">The RSA provider.</param>
        /// <param name="aesProvider">The aes provider.</param>
        /// <returns></returns>
        public static byte[] PackToBytes(this VirtualSecuredResponseRawMessage responseMessage, RSACryptoServiceProvider rsaProvider, RijndaelProvider aesProvider)
        {
            // Byte[] composition: [Schema Version]{1}[UTC Stamp]{4}[Encrypted Body]{N}.

            try
            {
                rsaProvider.CheckNullObject(nameof(rsaProvider));
                aesProvider.CheckNullObject(nameof(aesProvider));

                responseMessage.CheckNullObject(nameof(responseMessage));

                responseMessage.SchemaVersion = _schemaVersion;

                List<byte> result = new List<byte>(_packInitialCapacity);

                // index1: version.
                result.Add(Convert.ToByte(responseMessage.SchemaVersion));

                // index2: Stamp.
                result.AddRange(GetStampBytes(responseMessage.Stamp ?? DateTime.UtcNow));

                // index3: Encrypted content
                result.AddRange(aesProvider.EncryptAes(responseMessage.Data));

                return result.ToArray();
            }
            catch (Exception ex)
            {
                throw ex.Handle();
            }
        }

        /// <summary>
        /// Unpacks the response from bytes.
        /// </summary>
        /// <param name="requestBytes">The request bytes.</param>
        /// <param name="rsaProvider">The RSA provider.</param>
        /// <param name="aesProvider">The aes provider.</param>
        /// <returns></returns>
        public static VirtualSecuredResponseRawMessage UnpackResponseFromBytes(this byte[] requestBytes, RSACryptoServiceProvider rsaProvider, RijndaelProvider aesProvider)
        {
            // Byte[] composition: [Schema Version]{1}[UTC Stamp]{4}[Encrypted Body]{N}.

            try
            {
                rsaProvider.CheckNullObject(nameof(rsaProvider));
                aesProvider.CheckNullObject(nameof(aesProvider));

                requestBytes.CheckNullOrEmptyCollection(nameof(requestBytes));

                var result = new VirtualSecuredResponseRawMessage
                {
                    // Index1
                    SchemaVersion = Convert.ToInt32(requestBytes[0])
                };

                var currentIndex = 1;
                // Index2
                var stampBytes = requestBytes.Read(_stampIndicationByteLength, ref currentIndex);
                result.Stamp = GetUtcStampFromOffsetBytes(stampBytes);

                ValidateStamp(result.Stamp);

                // Index5
                var dataBytes = requestBytes.SubArray(currentIndex);
                result.Data = aesProvider.DecryptAes(dataBytes);

                return result;
            }
            catch (Exception ex)
            {
                throw ex.Handle();
            }
        }

        #endregion        

        /// <summary>
        /// Validates the stamp.
        /// </summary>
        /// <returns><c>true</c> if Validation passed, <c>false</c> otherwise.</returns>
        private static void ValidateStamp(DateTime? stamp)
        {
            if (!stamp.HasValue || (Math.Abs((stamp.Value - DateTime.UtcNow).TotalSeconds) > _allowedStampSecondDeviation))
            {
                throw ExceptionFactory.CreateInvalidObjectException(nameof(stamp), stamp, "OverDeviation");
            }
        }

        /// <summary>
        /// Invokes the specified data.
        /// </summary>
        /// <typeparam name="TInput">The type of the input.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <param name="uri">The URI.</param>
        /// <param name="data">The data.</param>
        /// <param name="rsaProvider">The RSA provider.</param>
        /// <param name="clientId">The client identifier.</param>
        /// <returns></returns>
        internal static TOutput Invoke<TInput, TOutput>(this Uri uri, ClassicVirtualSecuredRequestMessagePackage<TInput> data, RSACryptoServiceProvider rsaProvider, string clientId = null)
        {
            if (uri != null && data != null && rsaProvider != null)
            {
                try
                {
                    var httpRequest = uri.CreateHttpWebRequest(HttpConstants.HttpMethod.Post);

                    if (!string.IsNullOrWhiteSpace(clientId))
                    {
                        httpRequest.SafeSetHttpHeader(VirtualSecuredTransferProtocolConstants.headerKey_ClientId, clientId);
                    }

                    var aesKeys = AesKeys.Create();
                    var aesProvider = aesKeys.CreateAesProvider();

                    var requestRawMessage = new VirtualSecuredRequestRawMessage
                    {
                        Data = Encoding.UTF8.GetBytes(data.ToJson(false)),
                        SchemaVersion = _schemaVersion,
                        SymmetricPrimaryKey = aesKeys.Key,
                        SymmetricSecondaryKey = aesKeys.InitializationVector
                    };

                    httpRequest.FillData(HttpConstants.HttpMethod.Post, PackToBytes(requestRawMessage, rsaProvider.GetRsaKeys().PublicKey, aesProvider));

                    var httpResult = httpRequest.ReadResponseAsBytes();
                    var responseBytes = httpResult.Body;

                    var responseRawMessage = UnpackResponseFromBytes(responseBytes, rsaProvider, aesProvider);

                    if (httpResult.HttpStatusCode.IsOK())
                    {
                        return Encoding.UTF8.GetString(responseRawMessage.Data).TryConvertJsonToObject<TOutput>();
                    }
                    else
                    {
                        var exception = Encoding.UTF8.GetString(responseRawMessage.Data).TryConvertJsonToObject<ExceptionInfo>();

                        if (exception != null)
                        {
                            throw exception.ToException().Handle(new { uri, data });
                        }
                        else
                        {
                            throw ExceptionFactory.CreateOperationException(new { uri, data });
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex.Handle(new { uri, data });
                }
            }

            return default(TOutput);
        }
    }
}
