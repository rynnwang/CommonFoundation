using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Beyova
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class RijndaelProvider : IDisposable
    {
        /// <summary>
        /// The rij alg
        /// </summary>
        private Rijndael _rijAlg = Rijndael.Create();

        /// <summary>
        /// The aes keys
        /// </summary>
        private IAesKeys _aesKeys = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="RijndaelProvider"/> class.
        /// </summary>
        /// <param name="aesKeys">The aes keys.</param>
        private RijndaelProvider(IAesKeys aesKeys)
        {
            _aesKeys = aesKeys;
        }

        /// <summary>
        /// Encrypts the aes.
        /// </summary>
        /// <param name="orignal">The orignal.</param>
        /// <returns></returns>
        public byte[] EncryptAes(byte[] orignal)
        {
            try
            {
                orignal.CheckNullOrEmptyCollection(nameof(orignal));

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, _rijAlg.CreateEncryptor(_aesKeys.Key, _aesKeys.InitializationVector), CryptoStreamMode.Write))
                    {
                        csEncrypt.Write(orignal, 0, orignal.Length);
                        csEncrypt.FlushFinalBlock();
                        return msEncrypt.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { orignal });
            }
        }

        /// <summary>
        /// Decrypts the aes.
        /// </summary>
        /// <param name="bytesToDecrypt">The bytes to decrypt.</param>
        /// <returns></returns>
        public byte[] DecryptAes(byte[] bytesToDecrypt)
        {
            try
            {
                bytesToDecrypt.CheckNullOrEmptyCollection(nameof(bytesToDecrypt));

                using (MemoryStream msDecrypt = new MemoryStream(bytesToDecrypt))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, _rijAlg.CreateDecryptor(_aesKeys.Key, _aesKeys.InitializationVector), CryptoStreamMode.Read))
                    {
                        //csDecrypt.Write(bytesToDecrypt, 0, bytesToDecrypt.Length);
                        //csDecrypt.FlushFinalBlock();
                        //return msDecrypt.ToArray();

                        return csDecrypt.ReadStreamToBytes();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { bytesToDecrypt });
            }
        }

        /// <summary>
        /// Creates the provider.
        /// </summary>
        /// <param name="aesKeys">The aes keys.</param>
        /// <returns></returns>
        public static RijndaelProvider CreateProvider(IAesKeys aesKeys)
        {
            try
            {
                aesKeys.CheckNullObject(nameof(aesKeys));

                var key = aesKeys.Key;
                var iv = aesKeys.InitializationVector;
                key.CheckNullOrEmpty(nameof(key));
                iv.CheckNullOrEmpty(nameof(iv));

                return new RijndaelProvider(aesKeys);
            }
            catch (Exception ex)
            {
                throw ex.Handle(aesKeys);
            }
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
        {
            _rijAlg.Dispose();
        }
    }
}
