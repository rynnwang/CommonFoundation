using System;
using System.Collections.Generic;
using System.Text;
using Beyova.ExceptionSystem;
using Beyova.UnitTestKit.InternalDoor;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Beyova.Gravity.UnitTest
{
    [TestClass]
    public class SecuredMessageUnitTest : UnitTestKit.BaseUnitTest
    {
        /// <summary>
        /// Mains the test.
        /// </summary>
        [TestMethod]
        public void MainTest()
        {
            GravityManagementServiceCore service = new GravityManagementServiceCore();
            var testProduct = service.QueryProductInfo(new ProductCriteria { }).SafeFirstOrDefault();

            /// If null reference check is failed, try <see cref="ManagementUnitTest.TestCreateProduct"/> first.
            testProduct.CheckNullObject(nameof(testProduct));

            var exception = Test(testProduct);

            Assert.IsNull(exception);
        }

        /// <summary>
        /// Tests the specified product information.
        /// </summary>
        /// <typeparam name="TInput">The type of the t input.</typeparam>
        /// <typeparam name="TOutput">The type of the t output.</typeparam>
        /// <param name="productInfo">The product information.</param>
        /// <param name="processFunc">The process function.</param>
        /// <returns>Beyova.ExceptionSystem.BaseException.</returns>
        private static BaseException Test(ProductInfo productInfo)
        {
            try
            {
                var rsaKeys = EncodingOrSecurityExtension.CreateRsaKeys();
                var tripleDesKey = EncodingOrSecurityExtension.GenerateTripleDESKey();

                var messageBytes = TestSecuredMessageClientSide(productInfo.PublicKey, rsaKeys, tripleDesKey);
                var output = TestSecuredMessageServerSide(productInfo?.Token, messageBytes, (t) =>
               {
                   return productInfo;
               }, false);

                var commands = SecuredMessageTestKit.ConvertToSecuredMessageObject<List<GravityCommandRequest>>(output, tripleDesKey);
                Assert.IsNotNull(commands);
            }
            catch (Exception ex)
            {
                return ex.Handle();
            }

            return null;
        }

        /// <summary>
        /// Tests the secured message client side.
        /// </summary>
        /// <param name="publicKey">The public key.</param>
        /// <param name="rsaKeys">The RSA keys.</param>
        /// <returns>System.Byte[].</returns>
        private static byte[] TestSecuredMessageClientSide(string publicKey, IRsaKeys rsaKeys, byte[] tripleDesKey)
        {
            publicKey.CheckEmptyString(nameof(publicKey));
            tripleDesKey.CheckNullOrEmptyCollection(nameof(tripleDesKey));

            var heartbeat = GravityKit.GetHeartbeat();
            return SecuredMessageTestKit.ConvertToSecuredMessagePackage(new SecuredMessageRequest<Heartbeat>
            {
                Message = new SecuredMessageObject<Heartbeat>
                {
                    Data = heartbeat
                },
                EncryptionKey = tripleDesKey
            }, publicKey);
        }

        /// <summary>
        /// Tests the secured message server side.
        /// </summary>
        /// <typeparam name="TInput">The type of the t input.</typeparam>
        /// <typeparam name="TOutput">The type of the t output.</typeparam>
        /// <param name="token">The token.</param>
        /// <param name="bodyBytes">The body bytes.</param>
        /// <param name="processFunc">The process function.</param>
        /// <param name="getClientObjectByToken">The get client object by token.</param>
        /// <param name="omitStampValidation">The omit stamp validation.</param>
        /// <returns>TOutput.</returns>
        private static byte[] TestSecuredMessageServerSide(string token, byte[] bodyBytes, Func<string, ProductInfo> getClientObjectByToken, bool omitStampValidation = false)
        {
            ProductInfo productObject = null;
            if (!string.IsNullOrWhiteSpace(token) && getClientObjectByToken != null)
            {
                productObject = getClientObjectByToken(token);
            }

            productObject.CheckNullObject(nameof(productObject));
            productObject.PrivateKey.CheckNullObject(nameof(productObject.PrivateKey));

            bodyBytes.CheckNullObject(nameof(bodyBytes));
            var request = SecuredMessageTestKit.ConvertToSecuredMessageRequest<Heartbeat>(SecuredMessagePackage.FromBytes(bodyBytes), productObject.PrivateKey);
            var inputMessage = request.Message;

            if (!omitStampValidation && !inputMessage.ValidateStamp())
            {
                throw ExceptionFactory.CreateInvalidObjectException(nameof(inputMessage));
            }

            var service = new GravityServiceCore();
            var clientKey = service.SaveHeartbeatInfo(productObject.Key, inputMessage.Data);
            var commands = service.GetPendingCommandRequest(clientKey);

            return SecuredMessageTestKit.ToBytes<List<GravityCommandRequest>>(new SecuredMessageObject<List<GravityCommandRequest>>
            {
                Data = commands,
                Stamp = DateTime.UtcNow
            }, request.EncryptionKey);
        }
    }
}
