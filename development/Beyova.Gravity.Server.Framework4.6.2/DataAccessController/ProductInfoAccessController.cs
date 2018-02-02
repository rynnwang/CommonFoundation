using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Beyova.Gravity.DataAccessController
{
    /// <summary>
    /// Class ProductInfoAccessController.
    /// </summary>
    public class ProductInfoAccessController : GravityAccessController<ProductInfo>
    {
        /// <summary>
        /// Converts the entity object.
        /// </summary>
        /// <param name="sqlDataReader">The SQL data reader.</param>
        /// <returns>Object instance in type {`0}.</returns>
        protected override ProductInfo ConvertEntityObject(SqlDataReader sqlDataReader)
        {
            var result = new ProductInfo
            {
                Name = sqlDataReader[column_Name].ObjectToString(),
                PrivateKey = sqlDataReader[column_PrivateKey].ObjectToString(),
                PublicKey = sqlDataReader[column_PublicKey].ObjectToString(),
                Token = sqlDataReader[column_Token].ObjectToString(),
                ExpiredStamp = sqlDataReader[column_ExpiredStamp].ObjectToDateTime()
            };

            FillSimpleBaseObjectFields(result, sqlDataReader);

            return result;
        }

        /// <summary>
        /// Creates the or update product.
        /// </summary>
        /// <param name="productInfo">The product information.</param>
        /// <returns>System.Nullable&lt;Guid&gt;.</returns>
        public Guid? CreateOrUpdateProduct(ProductInfo productInfo)
        {
            const string spName = "sp_CreateOrUpdateProduct";

            try
            {
                productInfo.CheckNullObject(nameof(productInfo));

                var parameters = new List<SqlParameter>
                {
                    GenerateSqlSpParameter(column_Key, productInfo.Key),
                    GenerateSqlSpParameter(column_Name, productInfo.Name),
                    GenerateSqlSpParameter(column_Token, productInfo.Token),
                    GenerateSqlSpParameter(column_PublicKey, productInfo.PublicKey),
                    GenerateSqlSpParameter(column_PrivateKey, productInfo.PrivateKey),
                    GenerateSqlSpParameter(column_ExpiredStamp, productInfo.ExpiredStamp)
                };

                return this.ExecuteScalar(spName, parameters).ObjectToGuid();
            }
            catch (Exception ex)
            {
                throw ex.Handle(productInfo);
            }
        }

        /// <summary>
        /// Gets the product information by token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>ProductInfo.</returns>
        public ProductInfo GetProductInfoByToken(string token)
        {
            const string spName = "sp_GetProductInfoByToken";

            try
            {
                token.CheckEmptyString(nameof(token));

                var parameters = new List<SqlParameter>
                {
                    GenerateSqlSpParameter(column_Token, token)
                };

                return this.ExecuteReader(spName, parameters).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex.Handle(token);
            }
        }

        /// <summary>
        /// Queries the product information.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>
        /// List&lt;ProductInfo&gt;.
        /// </returns>
        public List<ProductInfo> QueryProductInfo(ProductCriteria criteria)
        {
            const string spName = "sp_QueryProductInfo";

            try
            {
                criteria.CheckNullObject(nameof(criteria));

                var parameters = new List<SqlParameter>
                {
                    GenerateSqlSpParameter(column_Key, criteria.Key)
                };

                return this.ExecuteReader(spName, parameters);
            }
            catch (Exception ex)
            {
                throw ex.Handle(criteria);
            }
        }

        /// <summary>
        /// Deletes the product information.
        /// </summary>
        /// <param name="key">The key.</param>
        public void DeleteProductInfo(Guid? key)
        {
            const string spName = "sp_DeleteProductInfo";

            try
            {
                key.CheckNullObject(nameof(key));

                var parameters = new List<SqlParameter>
                {
                    GenerateSqlSpParameter(column_Key, key)
                };

                this.ExecuteNonQuery(spName, parameters);
            }
            catch (Exception ex)
            {
                throw ex.Handle(key);
            }
        }
    }
}
