using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Beyova;
using Newtonsoft.Json.Linq;

namespace Beyova.FunctionService.Generic
{
    internal class AppPlatformAccessController : ProvisioningServiceBaseDataAccessController<AppPlatform>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref=" AppPlatform"/> class.
        /// </summary>
        public AppPlatformAccessController() : base()
        {
        }

        /// <summary>
        /// Converts the entity object.
        /// </summary>
        /// <param name="sqlDataReader">The SQL data reader.</param>
        /// <returns>
        /// Object instance in type {`0}.
        /// </returns>
        protected override AppPlatform ConvertEntityObject(SqlDataReader sqlDataReader)
        {
            var result = new AppPlatform
            {
                Description = sqlDataReader[column_Description].ObjectToString(),
                Key = sqlDataReader[column_Key].ObjectToGuid(),
                Name = sqlDataReader[column_Name].ObjectToString()
            };

            FillAppPlatform(result, sqlDataReader);

            return result;
        }

        /// <summary>
        /// Creates the or update application platform.
        /// </summary>
        /// <param name="appPlatform">The application platform.</param>
        /// <param name="operatorKey">The operator key.</param>
        /// <returns></returns>
        public Guid? CreateOrUpdateAppPlatform(AppPlatform appPlatform, Guid? operatorKey)
        {
            const string spName = "sp_CreateOrUpdateAppPlatform";

            try
            {
                appPlatform.CheckNullObject(nameof(appPlatform));

                var parameters = new List<SqlParameter>
                {
                    GenerateSqlSpParameter(column_Key,appPlatform.Key),
                    GenerateSqlSpParameter(column_Name,appPlatform.Name),
                    GenerateSqlSpParameter(column_PlatformType,appPlatform.PlatformType.EnumToInt32()),
                    GenerateSqlSpParameter(column_BundleId,appPlatform.BundleId),
                    GenerateSqlSpParameter(column_Url,appPlatform.Url),
                    GenerateSqlSpParameter(column_MinOSVersion,appPlatform.MinOSVersion),
                    GenerateSqlSpParameter(column_Description,appPlatform.Description),
                    GenerateSqlSpParameter(column_OperatorKey,operatorKey)
                };

                return this.ExecuteScalar(spName, parameters).ObjectToGuid();
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { appPlatform, operatorKey });
            }
        }

        /// <summary>
        /// Queries the application platform.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="bundleId">The bundle identifier.</param>
        /// <returns></returns>
        public List<BaseObject<AppPlatform>> QueryAppPlatform(Guid? key, string bundleId)
        {
            const string spName = "sp_QueryAppPlatform";

            try
            {
                var parameters = new List<SqlParameter>
                {
                        GenerateSqlSpParameter(column_Key,key),
                        GenerateSqlSpParameter(column_Name,null),
                        GenerateSqlSpParameter(column_PlatformType,null),
                        GenerateSqlSpParameter(column_BundleId,bundleId),
                        GenerateSqlSpParameter(column_Url,null)
                };

                return this.ExecuteReaderAsBaseObject(spName, parameters);
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { key, bundleId });
            }
        }
    }
}