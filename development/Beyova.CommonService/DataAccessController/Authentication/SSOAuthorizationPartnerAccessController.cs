using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Beyova.CommonService.DataAccessController
{
    /// <summary>
    /// Class SSOAuthorizationPartnerAccessController.
    /// </summary>
    public class SSOAuthorizationPartnerAccessController : SSOAuthorizationPartnerAccessController<SSOAuthorizationPartner, SSOAuthorizationPartnerCriteria>
    {
    }

    /// <summary>
    /// Class SSOAuthorizationPartnerAccessController.
    /// </summary>
    /// <typeparam name="TSSOAuthorizationPartner">The type of SSO authorization Partner.</typeparam>
    /// <typeparam name="TSSOAuthorizationPartnerCriteria">The type of SSO authorization Partner criteria.</typeparam>
    public class SSOAuthorizationPartnerAccessController<TSSOAuthorizationPartner, TSSOAuthorizationPartnerCriteria> : BaseAuthenticationController<TSSOAuthorizationPartner, TSSOAuthorizationPartnerCriteria>
        where TSSOAuthorizationPartner : SSOAuthorizationPartner, new()
        where TSSOAuthorizationPartnerCriteria : SSOAuthorizationPartnerCriteria
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SSOAuthorizationPartnerAccessController" /> class.
        /// </summary>
        public SSOAuthorizationPartnerAccessController()
            : base()
        {
        }

        #endregion

        /// <summary>
        /// Converts the entity object.
        /// </summary>
        /// <param name="sqlDataReader">The SQL data reader.</param>
        /// <returns>AccessCredentialInfo.</returns>
        protected override TSSOAuthorizationPartner ConvertEntityObject(SqlDataReader sqlDataReader)
        {
            var result = new TSSOAuthorizationPartner
            {
                OwnerKey = sqlDataReader[column_OwnerKey].ObjectToGuid(),
                Name = sqlDataReader[column_Name].ObjectToString(),
                Token = sqlDataReader[column_Token].ObjectToString(),
                CallbackUrl = sqlDataReader[column_CallbackUrl].ObjectToString(),
                Realm = sqlDataReader[column_Realm].ObjectToString()
            };

            FillSimpleBaseObjectFields(result, sqlDataReader);
            this.FillAdditionalFieldValue(result, sqlDataReader);

            return result;
        }

        /// <summary>
        /// Creates the or update sso authorization Partner.
        /// </summary>
        /// <param name="partner">The Partner.</param>
        /// <param name="operatorKey">The operator key.</param>
        /// <returns>System.Nullable&lt;Guid&gt;.</returns>
        public Guid? CreateOrUpdateSSOAuthorizationPartner(TSSOAuthorizationPartner partner, Guid? operatorKey)
        {
            const string spName = "sp_CreateOrUpdateSSOAuthorizationPartner";

            try
            {
                partner.CheckNullObject(nameof(partner));
                operatorKey.CheckNullObject(nameof(operatorKey));

                if (!partner.Key.HasValue && string.IsNullOrWhiteSpace(partner.Token))
                {
                    partner.Token = GenerateToken();
                }

                var parameters = new List<SqlParameter>
                {
                    this.GenerateSqlSpParameter(column_Key, partner.Key),
                    this.GenerateSqlSpParameter(column_OwnerKey, partner.OwnerKey),
                    this.GenerateSqlSpParameter(column_Name, partner.Name),
                    this.GenerateSqlSpParameter(column_Token, partner.Token),
                    this.GenerateSqlSpParameter(column_CallbackUrl, partner.CallbackUrl),
                    this.GenerateSqlSpParameter(column_Realm, partner.Realm.SafeToString()),
                    this.GenerateSqlSpParameter(column_TokenExpiration, partner.TokenExpiration ),
                    this.GenerateSqlSpParameter(column_OperatorKey, operatorKey)
                };

                FillAdditionalFieldValue(parameters, partner);

                return this.ExecuteScalar(spName, parameters).ObjectToGuid();
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { partner, operatorKey });
            }
        }

        /// <summary>
        /// Deletes the sso authorization Partner.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="operatorKey">The operator key.</param>
        public void DeleteSSOAuthorizationPartner(Guid? key, Guid? operatorKey)
        {
            const string spName = "sp_DeleteSSOAuthorizationPartner";

            try
            {
                key.CheckNullObject(nameof(key));
                operatorKey.CheckNullObject(nameof(operatorKey));

                var parameters = new List<SqlParameter>
                {
                    this.GenerateSqlSpParameter(column_Key, key),
                    this.GenerateSqlSpParameter(column_OperatorKey, operatorKey)
                };

                this.ExecuteNonQuery(spName, parameters);
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { key, operatorKey });
            }
        }

        /// <summary>
        /// Queries the sso authorization Partner.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List&lt;TSSOAuthorizationPartner&gt;.</returns>
        public List<TSSOAuthorizationPartner> QuerySSOAuthorizationPartner(TSSOAuthorizationPartnerCriteria criteria)
        {
            const string spName = "sp_QuerySSOAuthorizationPartner";

            try
            {
                criteria.CheckNullObject(nameof(criteria));

                var parameters = new List<SqlParameter>
                {
                    this.GenerateSqlSpParameter(column_Key, criteria.Key),
                    this.GenerateSqlSpParameter(column_OwnerKey, criteria.OwnerKey),
                    this.GenerateSqlSpParameter(column_Name, criteria.Name),
                    this.GenerateSqlSpParameter(column_Token, criteria.Token),
                    this.GenerateSqlSpParameter(column_CallbackUrl, criteria.CallbackUrl),
                    this.GenerateSqlSpParameter(column_Realm, criteria.Realm)
                };
                FillAdditionalFieldValue(parameters, criteria);

                return this.ExecuteReader(spName, parameters);
            }
            catch (Exception ex)
            {
                throw ex.Handle(criteria);
            }
        }

        /// <summary>
        /// Generates the token.
        /// </summary>
        /// <returns>System.String.</returns>
        protected string GenerateToken()
        {
            return this.CreateRandomString(32);
        }
    }
}