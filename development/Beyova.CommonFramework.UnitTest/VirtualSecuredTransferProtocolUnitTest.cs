using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Beyova.VirtualSecuredTransferProtocol;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Beyova.Common.UnitTest
{
    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    public class VirtualSecuredTransferProtocolUnitTest
    {
        static int schemaVersion = VirtualSecuredTransferProtocolHelper.SchemaVersion;

        static RsaKeys rsaKeys = null;

        static RSACryptoServiceProvider serverSideRsaProvider = null;

        static RSACryptoServiceProvider clientSideRsaProvider = null;

        static string testToken = Guid.NewGuid().ToString();

        static string requestTestData = Guid.NewGuid().ToString();

        static string responseTestData = Guid.NewGuid().ToString();

        static AesKeys aesKeys = null;

        /// <summary>
        /// Prepares this instance.
        /// </summary>
        [TestInitialize]
        public void Prepare()
        {
            rsaKeys = RsaKeys.Create(VirtualSecuredTransferProtocolHelper.DwKeySize);
            serverSideRsaProvider = rsaKeys.CreateRsaProvider();
            clientSideRsaProvider = new RsaKeys { DoubleWordKeySize = rsaKeys.DoubleWordKeySize, PublicKey = rsaKeys.PublicKey }.CreateRsaPublicProvider();
        }

        /// <summary>
        /// Cleans up.
        /// </summary>
        [TestCleanup]
        public void CleanUp()
        {
            serverSideRsaProvider?.Dispose();
            clientSideRsaProvider?.Dispose();
        }

        [TestMethod]
        public void Test()
        {
            if (serverSideRsaProvider != null)
            {
                // Simulate client side for sending request

                var utcNow = DateTime.UtcNow;
                aesKeys = AesKeys.Create();
                var aesProvider = aesKeys.CreateAesProvider();


                VirtualSecuredRequestRawMessage testRequest = new VirtualSecuredRequestRawMessage()
                {
                    SchemaVersion = schemaVersion,
                    Stamp = utcNow,
                    SymmetricPrimaryKey = aesKeys.Key,
                    SymmetricSecondaryKey = aesKeys.InitializationVector,
                    Data = requestTestData.ToByteArray(Encoding.UTF8)
                };

                var requestBytes = VirtualSecuredTransferProtocolHelper.PackToBytes(testRequest, rsaKeys.PublicKey, aesProvider);

                Assert.IsNotNull(requestBytes);


                // Simulate server side for getting request
                RijndaelProvider rijndaelProviderByRequest;
                var requestRawMessage = VirtualSecuredTransferProtocolHelper.UnpackRequestFromBytes(requestBytes, serverSideRsaProvider, out rijndaelProviderByRequest);
                Assert.IsNotNull(requestRawMessage);
                Assert.IsNotNull(rijndaelProviderByRequest);

                Assert.AreEqual(testRequest.SchemaVersion, requestRawMessage.SchemaVersion);
                Assert.IsTrue(Math.Abs((testRequest.Stamp.Value - requestRawMessage.Stamp.Value).TotalSeconds) < 1);
                Assert.AreEqual(testRequest.SymmetricPrimaryKey, requestRawMessage.SymmetricPrimaryKey);
                Assert.AreEqual(testRequest.SymmetricSecondaryKey, requestRawMessage.SymmetricSecondaryKey);
                Assert.AreEqual(requestTestData, requestRawMessage.Data.ToUtf8String());

                // Simulate server side create AES provider
                using (var serverAesProvider = new AesKeys
                {
                    KeySize = VirtualSecuredTransferProtocolHelper.DwKeySize,
                    InitializationVector = requestRawMessage.SymmetricSecondaryKey,
                    Key = requestRawMessage.SymmetricPrimaryKey
                }.CreateAesProvider())
                {
                    // Simulate server side for responding
                    utcNow = DateTime.UtcNow;
                    var testResponse = new VirtualSecuredResponseRawMessage
                    {
                        SchemaVersion = schemaVersion,
                        Stamp = utcNow,
                        Data = responseTestData.ToByteArray(Encoding.UTF8)
                    };
                    var responseBytes = VirtualSecuredTransferProtocolHelper.PackToBytes(testResponse, serverSideRsaProvider, serverAesProvider);
                    Assert.IsNotNull(responseBytes);


                    // Simulate client side for getting respond
                    var responseRawMessage = VirtualSecuredTransferProtocolHelper.UnpackResponseFromBytes(responseBytes, clientSideRsaProvider, aesProvider);
                    Assert.IsNotNull(responseRawMessage);
                    Assert.AreEqual(testResponse.SchemaVersion, responseRawMessage.SchemaVersion);
                    Assert.IsTrue(Math.Abs((testResponse.Stamp.Value - responseRawMessage.Stamp.Value).TotalSeconds) < 1);
                    Assert.AreEqual(responseTestData, responseRawMessage.Data.ToUtf8String());
                }
            }
        }
    }
}