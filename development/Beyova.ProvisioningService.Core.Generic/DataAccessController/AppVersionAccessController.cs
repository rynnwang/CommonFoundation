using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Beyova;
using Newtonsoft.Json.Linq;

namespace Beyova.ProvisioningService.DataAccessController
{
    internal class AppVersionAccessController : ProvisioningServiceBaseDataAccessController<AppVersion>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref=" AppVersion"/> class.
        /// </summary>
        public AppVersionAccessController() : base()
        {
        }

        /// <summary>
        /// Converts the entity object.
        /// </summary>
        /// <param name="sqlDataReader">The SQL data reader.</param>
        /// <returns>
        /// Object instance in type {`0}.
        /// </returns>
        protected override AppVersion ConvertEntityObject(SqlDataReader sqlDataReader)
        {
            var result = new AppVersion();
            FillAppVersionBase(result, sqlDataReader);
            FillAppPlatform(result, sqlDataReader);

            return result;
        }

        /// <summary>
        /// Creates the application version.
        /// </summary>
        /// <param name="appVersionBase">The application version base.</param>
        /// <param name="operatorKey">The operator key.</param>
        /// <returns></returns>
        public Guid? CreateAppVersion(AppVersionBase appVersionBase, Guid? operatorKey)
        {
            const string spName = "sp_CreateAppVersion";

            try
            {
                appVersionBase.CheckNullObject(nameof(appVersionBase));

                var parameters = new List<SqlParameter>
                {
                    GenerateSqlSpParameter(column_PlatformKey,appVersionBase.PlatformKey),
                    GenerateSqlSpParameter(column_LatestBuild,appVersionBase.LatestBuild),
                    GenerateSqlSpParameter(column_LatestVersion,appVersionBase.LatestVersion),
                    GenerateSqlSpParameter(column_MinRequiredBuild,appVersionBase.MinRequiredBuild),
                    GenerateSqlSpParameter(column_Note,appVersionBase.Note),
                    GenerateSqlSpParameter(column_AppServiceStatus,appVersionBase.AppServiceStatus),
                    GenerateSqlSpParameter(column_OperatorKey,operatorKey)
                };

                return this.ExecuteScalar(spName, parameters).ObjectToGuid();
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { appVersionBase, operatorKey });
            }
        }

        /// <summary>
        /// Queries the application version.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        public List<AppVersion> QueryAppVersion(AppVersionCriteria criteria)
        {
            const string spName = "sp_QueryAppVersion";

            try
            {
                var parameters = new List<SqlParameter>
                {
                        GenerateSqlSpParameter(column_Key,criteria.Key),
                        GenerateSqlSpParameter(column_PlatformKey,criteria.PlatformKey),
                        GenerateSqlSpParameter(column_Platform,criteria.Platform.EnumToInt32()),
                        GenerateSqlSpParameter(column_OperatorKey,criteria.OperatorKey)
                };

                return this.ExecuteReader(spName, parameters);
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { criteria });
            }
        }

        /// <summary>
        /// Queries the application version snapshot.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        public List<AppVersion> QueryAppVersionSnapshot(AppVersionCriteria criteria)
        {
            const string spName = "sp_QueryAppVersionSnapshot";

            try
            {
                var parameters = new List<SqlParameter>
                {
                        GenerateSqlSpParameter(column_Key,criteria.Key),
                        GenerateSqlSpParameter(column_PlatformKey,criteria.PlatformKey),
                        GenerateSqlSpParameter(column_Platform,criteria.Platform.EnumToInt32()),
                        GenerateSqlSpParameter(column_OperatorKey,criteria.OperatorKey)
                };

                return this.ExecuteReader(spName, parameters);
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { criteria });
            }
        }
    }
}