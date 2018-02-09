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
    internal class AppProvisioningAccessController : ProvisioningServiceBaseDataAccessController<AppProvisioningBase>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref=" AppProvisioningBase"/> class.
        /// </summary>
        public AppProvisioningAccessController() : base()
        {
        }

        /// <summary>
        /// Converts the entity object.
        /// </summary>
        /// <param name="sqlDataReader">The SQL data reader.</param>
        /// <returns>
        /// Object instance in type {`0}.
        /// </returns>
        protected override AppProvisioningBase ConvertEntityObject(SqlDataReader sqlDataReader)
        {
            var result = new AppProvisioningBase()
            {
                Content = sqlDataReader[column_Content].ObjectToJObject(),
                Key = sqlDataReader[column_Key].ObjectToGuid(),
                Name = sqlDataReader[column_Name].ObjectToString(),
                PlatformKey = sqlDataReader[column_PlatformKey].ObjectToGuid()
            };

            return result;
        }

        /// <summary>
        /// Queries the application provisioning.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        public List<AppProvisioningBase> QueryAppProvisioning(AppProvisioningCriteria criteria)
        {
            const string spName = "sp_QueryAppProvisioning";

            try
            {
                var parameters = new List<SqlParameter>
                {
                    GenerateSqlSpParameter(column_Key,criteria.Key),
                    GenerateSqlSpParameter(column_PlatformKey,criteria.PlatformKey),
                    GenerateSqlSpParameter(column_Name,criteria.Name),
                    GenerateSqlSpParameter(column_OperatorKey,criteria.OperatorKey)
                };

                return this.ExecuteReader(spName, parameters);
            }
            catch (Exception ex)
            {
                throw ex.Handle(criteria);
            }
        }

        /// <summary>
        /// Queries the application provisioning snapshot.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        public List<AppProvisioningBase> QueryAppProvisioningSnapshot(AppProvisioningCriteria criteria)
        {
            const string spName = "sp_QueryAppProvisioningSnapshot";

            try
            {
                var parameters = new List<SqlParameter>
                {
                    GenerateSqlSpParameter(column_Key,criteria.Key),
                    GenerateSqlSpParameter(column_PlatformKey,criteria.PlatformKey),
                    GenerateSqlSpParameter(column_Name,criteria.Name),
                    GenerateSqlSpParameter(column_OperatorKey,criteria.OperatorKey)
                };

                return this.ExecuteReader(spName, parameters);
            }
            catch (Exception ex)
            {
                throw ex.Handle(criteria);
            }
        }

        /// <summary>
        /// Creates the or update application provisioning.
        /// </summary>
        /// <param name="appProvisioning">The application provisioning.</param>
        /// <param name="operatorKey">The operator key.</param>
        /// <returns></returns>
        public Guid? CreateOrUpdateAppProvisioning(AppProvisioningBase appProvisioning, Guid? operatorKey)
        {
            const string spName = "sp_CreateOrUpdateAppProvisioning";

            try
            {
                appProvisioning.CheckNullObject(nameof(appProvisioning));

                var parameters = new List<SqlParameter>
                {
                    GenerateSqlSpParameter(column_Name,appProvisioning.Name),
                    GenerateSqlSpParameter(column_PlatformKey,appProvisioning.PlatformKey),
                    GenerateSqlSpParameter(column_Content,appProvisioning.Content),
                    GenerateSqlSpParameter(column_OperatorKey,operatorKey),
                };

                return ExecuteScalar(spName, parameters).ObjectToGuid();
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { appProvisioning, operatorKey });
            }
        }

        /// <summary>
        /// Deletes the application provisioning.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="operatorKey">The operator key.</param>
        public void DeleteAppProvisioning(Guid? key, Guid? operatorKey)
        {
            const string spName = "sp_DeleteAppProvisioning";

            try
            {
                key.CheckNullObject(nameof(key));

                var parameters = new List<SqlParameter>
                {
                    GenerateSqlSpParameter(column_Key,key),
                    GenerateSqlSpParameter(column_OperatorKey,operatorKey),
                };

                ExecuteNonQuery(spName, parameters);
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { key, operatorKey });
            }
        }
    }
}