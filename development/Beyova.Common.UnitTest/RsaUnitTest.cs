using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Beyova.Cache;
using System.Security.Cryptography;
using System.Text;

namespace Beyova.Common.UnitTest
{
    [TestClass]
    public class RsaUnitTest
    {
        const string testData = "Basically, this code is exporting the public/private key pair as an encrypted blob.";

        [TestMethod]
        public void TestRsa()
        {
            var dwKeySize = 2048;

            using (var originRsaProvider = new RSACryptoServiceProvider(dwKeySize))
            {
                var rsaKey = new RsaKeys
                {
                    PrivateKey = originRsaProvider.ExportCspBlob(true),
                    PublicKey = originRsaProvider.ExportCspBlob(false),
                    DoubleWordKeySize = originRsaProvider.KeySize
                };

                var publicProvider = rsaKey.CreateRsaPublicProvider();
                var privateProvider = rsaKey.CreateRsaProvider();

                var originalEncyptedText = originRsaProvider.Encrypt(Encoding.UTF8.GetBytes(testData), true).EncodeBase64();
                var publicEncyptedText = publicProvider.Encrypt(Encoding.UTF8.GetBytes(testData), true).EncodeBase64();
                var privateEncyptedText = privateProvider.Encrypt(Encoding.UTF8.GetBytes(testData), true).EncodeBase64();

                //self
                var originalDecyptedText1 = Encoding.UTF8.GetString(originRsaProvider.Decrypt(originalEncyptedText.DecodeBase64ToByteArray(), true));
                var privateDecyptedText1 = Encoding.UTF8.GetString(privateProvider.Decrypt(privateEncyptedText.DecodeBase64ToByteArray(), true));

                Assert.AreEqual(testData, originalDecyptedText1);
                Assert.AreEqual(testData, privateDecyptedText1);

                // from private
                var originalDecyptedText2 = Encoding.UTF8.GetString(originRsaProvider.Decrypt(privateEncyptedText.DecodeBase64ToByteArray(), true));
                var privateDecyptedText2 = Encoding.UTF8.GetString(privateProvider.Decrypt(originalEncyptedText.DecodeBase64ToByteArray(), true));
                Assert.AreEqual(testData, originalDecyptedText2);
                Assert.AreEqual(testData, privateDecyptedText2);

                // from public
                var originalDecyptedText3 = Encoding.UTF8.GetString(originRsaProvider.Decrypt(publicEncyptedText.DecodeBase64ToByteArray(), true));
                var privateDecyptedText3 = Encoding.UTF8.GetString(privateProvider.Decrypt(publicEncyptedText.DecodeBase64ToByteArray(), true));
                Assert.AreEqual(testData, originalDecyptedText3);
                Assert.AreEqual(testData, privateDecyptedText3);
            }
        }
    }
}