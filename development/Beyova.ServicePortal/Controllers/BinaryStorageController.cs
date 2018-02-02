//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using Beyova;
//using Beyova.AwsExtension;
//using Beyova.CommonAdminService;
//using Beyova.CommonServiceInterface;
//using EF.E1Technology.Developer.Core;

//namespace EF.E1Technology.Developer.Portal.Controllers
//{
//    public class BinaryStorageController : BinaryStorageBaseController<BinaryStorageMetaData, BinaryStorageMetaDataCriteria>
//    {
//        /// <summary>
//        /// Initializes a new instance of the <see cref="BinaryStorageController"/> class.
//        /// </summary>
//        public BinaryStorageController() : base()
//        {

//        }

//        /// <summary>
//        /// Gets the client.
//        /// </summary>
//        /// <param name="endpoint">The endpoint.</param>
//        /// <returns>IBinaryStorageService&lt;BinaryStorageMetaData, BinaryStorageMetaDataCriteria&gt;.</returns>
//        protected override IBinaryStorageService<BinaryStorageMetaData, BinaryStorageMetaDataCriteria> GetClient(EnvironmentEndpoint endpoint)
//        {
//            endpoint.CheckNullObject("endpoint");

//            return new BinaryStorageServiceClient<BinaryStorageMetaData, BinaryStorageMetaDataCriteria>(endpoint);
//        }

//        /// <summary>
//        /// Gets the BLOB helper.
//        /// </summary>
//        /// <returns></returns>
//        protected override ICloudBinaryStorageOperator  GetCloudBinaryStorageOperator()
//        {
//            return AwsStorageOperator.CreateCredentialIrrelavantStorageOperator();
//        }
//        /// <summary>
//        /// Gets the environment endpoint.
//        /// </summary>
//        /// <param name="environment">The environment.</param>
//        /// <returns>EnvironmentEndpoint.</returns>
//        protected override EnvironmentEndpoint GetEnvironmentEndpoint(string environment)
//        {
//            return ServiceConfigurationUtility.GetEndpoint(this._moduleCode, environment);
//        }
//    }
//}