using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Beyova.Aliyun.UnitTest
{
    [TestClass]
    public class OSSUnitTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            byte[] contents = "hello oss".ToByteArray();

            var accessKeyId = "LTAIHRANn6Sh1FqI";
            var accessKeySecret = "mHam2C7KI7A5SsPNXSlM4xBMzDe68b";
            var bucket = "beyova";

            Beyova.Alibaba.AliyunStorageOperator storageOperator = new Alibaba.AliyunStorageOperator(new Api.ApiEndpoint
            {
                Account = accessKeyId,
                Token = accessKeySecret,
                Host = "oss-cn-shanghai.aliyuncs.com"
            });

            //  Beyova.Alibaba.AliyunStorageOperator storageOperator = new Alibaba.AliyunStorageOperator("oss-cn-shanghai.aliyuncs.com", accessKeyId, accessKeySecret);

            var hash = contents.ToMD5().ToString();
            var uploadCredential = storageOperator.CreateBlobUploadCredential(bucket, Guid.NewGuid().ToString(), 10, hash: hash, contentType: HttpConstants.ContentType.Text);
            storageOperator.UploadBinaryBytesByCredentialUri(uploadCredential.CredentialUri, contents, HttpConstants.ContentType.Text);
        }
    }
}
