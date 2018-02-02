using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Beyova;

namespace Beyova
{
    /// <summary>
    /// Class BinaryOffsiteDistributionAccessController.
    /// </summary>
    internal class BinaryOffsiteDistributionAccessController : BaseBinaryOffsiteDistributionAccessController<BinaryOffsiteDistribution>
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryOffsiteDistributionAccessController" /> class.
        /// </summary>
        public BinaryOffsiteDistributionAccessController(string sqlConnection)
            : base(sqlConnection)
        {
        }

        #endregion Constructor

        /// <summary>
        /// Converts the entity object.
        /// </summary>
        /// <param name="sqlDataReader">The SQL data reader.</param>
        /// <returns>Object instance in type {`0}.</returns>
        protected override BinaryOffsiteDistribution ConvertEntityObject(SqlDataReader sqlDataReader)
        {
            return new BinaryOffsiteDistribution
            {
                Container = sqlDataReader[column_Container].ObjectToString(),
                Identifier = sqlDataReader[column_Identifier].ObjectToString(),
                HostRegion = sqlDataReader[column_HostRegion].ObjectToString(),
                State = (BinaryStorageState)sqlDataReader[column_State].ObjectToInt32(),
                UploadExpiredStamp = sqlDataReader[column_UploadCredentialExpiredStamp].ObjectToDateTime(),
            };
        }

        /// <summary>
        /// Creates the or update binary offsite distribution. If already existed + credential is NOT expired. Return null.
        /// </summary>
        /// <param name="binaryOffsiteDistribution">The binary offsite distribution.</param>
        /// <param name="credentialExpiredStamp">The credential expired stamp.</param>
        /// <returns>BinaryOffsiteDistribution.</returns>
        public BinaryOffsiteDistribution CreateOrUpdateBinaryOffsiteDistribution(BinaryOffsiteDistribution binaryOffsiteDistribution, DateTime? credentialExpiredStamp)
        {
            const string spName = "sp_CreateOrUpdateBinaryOffsiteDistribution";

            try
            {
                binaryOffsiteDistribution.CheckNullObject(nameof(binaryOffsiteDistribution));
                binaryOffsiteDistribution.Identifier.CheckNullObject("binaryOffsiteDistribution.Identifier");
                binaryOffsiteDistribution.Container.CheckNullObject("binaryOffsiteDistribution.Container");
                binaryOffsiteDistribution.HostRegion.CheckNullObject("binaryOffsiteDistribution.HostRegion");

                var parameters = new List<SqlParameter>
                {
                    GenerateSqlSpParameter(column_Identifier, binaryOffsiteDistribution.Identifier),
                    GenerateSqlSpParameter(column_Container, binaryOffsiteDistribution.Container),
                    GenerateSqlSpParameter(column_HostRegion, binaryOffsiteDistribution.HostRegion),
                    GenerateSqlSpParameter(column_ExpiredStamp, credentialExpiredStamp)
                };

                return this.ExecuteReader(spName, parameters).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { binaryOffsiteDistribution, credentialExpiredStamp });
            }
        }

        /// <summary>
        /// Gets the binary offsite distribution by country.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="country">The country.</param>
        /// <returns>BinaryOffsiteDistribution.</returns>
        public BinaryOffsiteDistribution GetBinaryOffsiteDistributionByCountry(BinaryStorageIdentifier identifier, string country)
        {
            const string spName = "sp_GetBinaryOffsiteDistributionByCountry";

            try
            {
                identifier.CheckNullObject(nameof(identifier));
                identifier.Identifier.CheckNullObject("identifier.Identifier");
                country.CheckEmptyString(nameof(country));

                var parameters = new List<SqlParameter>
                {
                    GenerateSqlSpParameter(column_Identifier, identifier.Identifier),
                    GenerateSqlSpParameter(column_Container, identifier.Container),
                    GenerateSqlSpParameter(column_Country, country)
                };

                return this.ExecuteReader(spName, parameters).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { identifier, country });
            }
        }

        /// <summary>
        /// Gets the binary offsite distribution by keys.
        /// </summary>
        /// <param name="keys">The keys.</param>
        /// <param name="container">The container.</param>
        /// <returns>BinaryOffsiteDistribution.</returns>
        public List<BinaryOffsiteDistribution> GetBinaryOffsiteDistributionByIdentifiers(List<Guid> keys, string container = null)
        {
            const string spName = "sp_GetBinaryOffsiteDistributionByIdentifiers";

            try
            {
                keys.CheckNullObject(nameof(keys));

                var parameters = new List<SqlParameter>
                {
                    GenerateSqlSpParameter(column_Xml, keys.ListToXml()),
                    GenerateSqlSpParameter(column_Container, container)
                };

                return this.ExecuteReader(spName, parameters);
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { keys, container });
            }
        }

        /// <summary>
        /// Gets the binary offsite distribution by key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>BinaryOffsiteDistribution.</returns>
        internal BinaryOffsiteDistribution GetBinaryOffsiteDistributionByKey(Guid? key)
        {
            const string spName = "sp_GetBinaryOffsiteDistributionByKey";

            try
            {
                key.CheckNullObject(nameof(key));

                var parameters = new List<SqlParameter>
                {
                    GenerateSqlSpParameter(column_Key, key)
                };

                return this.ExecuteReader(spName, parameters).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { key });
            }
        }

        /// <summary>
        /// Commits the binary offsite distribution.
        /// </summary>
        /// <param name="binaryOffsiteDistribution">The binary offsite distribution.</param>
        /// <returns>BinaryOffsiteDistribution.</returns>
        public BinaryOffsiteDistribution CommitBinaryOffsiteDistribution(BinaryOffsiteDistribution binaryOffsiteDistribution)
        {
            const string spName = "sp_CommitBinaryOffsiteDistribution";

            try
            {
                binaryOffsiteDistribution.CheckNullObject(nameof(binaryOffsiteDistribution));

                var parameters = new List<SqlParameter>
                {
                    GenerateSqlSpParameter(column_Identifier, binaryOffsiteDistribution.Identifier),
                    GenerateSqlSpParameter(column_Container, binaryOffsiteDistribution.Container),
                    GenerateSqlSpParameter(column_HostRegion, binaryOffsiteDistribution.HostRegion)
                };

                return this.ExecuteReader(spName, parameters).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex.Handle(binaryOffsiteDistribution);
            }
        }

        /// <summary>
        /// Deletes the binary offsite distribution.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>BinaryOffsiteDistribution.</returns>
        public void DeleteBinaryOffsiteDistribution(BinaryStorageIdentifier identifier)
        {
            const string spName = "sp_DeleteBinaryOffsiteDistribution";

            try
            {
                identifier.CheckNullObject(nameof(identifier));

                var parameters = new List<SqlParameter>
                {
                    GenerateSqlSpParameter(column_Identifier, identifier.Identifier),
                    GenerateSqlSpParameter(column_Container, identifier.Container)
                };

                this.ExecuteNonQuery(spName, parameters);
            }
            catch (Exception ex)
            {
                throw ex.Handle(identifier);
            }
        }
    }
}