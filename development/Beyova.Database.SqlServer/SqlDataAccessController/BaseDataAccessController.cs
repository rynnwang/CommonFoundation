﻿using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Beyova
{
    /// <summary>
    /// Abstract class for SQL data access controller, which refers an entity in {T} type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseDataAccessController<T> : SqlDataAccessController<T>
    {
        #region Constants

        /// <summary>
        /// The column operated by
        /// </summary>
        protected const string column_OperatedBy = "OperatedBy";

        /// <summary>
        /// The column comments
        /// </summary>
        protected const string column_Comments = "Comments";

        /// <summary>
        /// The column name of Items
        /// </summary>
        protected const string column_Items = "Items";

        /// <summary>
        /// The column name of Alias
        /// </summary>
        protected const string column_Alias = "Alias";

        /// <summary>
        /// The column name of Amount
        /// </summary>
        protected const string column_Amount = "Amount";

        /// <summary>
        /// The column json
        /// </summary>
        protected const string column_Json = "Json";

        /// <summary>
        /// The column binary key
        /// </summary>
        protected const string column_BinaryKey = "BinaryKey";

        /// <summary>
        /// The column organization key
        /// </summary>
        protected const string column_OrganizationKey = "OrganizationKey";

        /// <summary>
        /// The column short search term
        /// </summary>
        protected const string column_ShortSearchTerm = "ShortSearchTerm";

        /// <summary>
        /// The column search term
        /// </summary>
        protected const string column_SearchTerm = "SearchTerm";

        /// <summary>
        /// The column keys
        /// </summary>
        protected const string column_Keys = "Keys";

        /// <summary>
        /// The column permission
        /// </summary>
        protected const string column_Permission = "Permission";

        /// <summary>
        /// The column user name
        /// </summary>
        protected const string column_UserName = "UserName";

        /// <summary>
        /// The column gender
        /// </summary>
        protected const string column_Gender = "Gender";

        /// <summary>
        /// The column avatar URL
        /// </summary>
        protected const string column_AvatarUrl = "AvatarUrl";

        /// <summary>
        /// The column container
        /// </summary>
        protected const string column_Container = "Container";

        /// <summary>
        /// The column state
        /// </summary>
        protected const string column_State = "State";

        /// <summary>
        /// The column key
        /// </summary>
        protected const string column_Key = "Key";

        /// <summary>
        /// The column keyword
        /// </summary>
        protected const string column_Keyword = "Keyword";

        /// <summary>
        /// The column user key
        /// </summary>
        protected const string column_UserKey = "UserKey";

        /// <summary>
        /// The column channel
        /// </summary>
        protected const string column_Channel = "Channel";

        /// <summary>
        /// The column owner key
        /// </summary>
        protected const string column_OwnerKey = "OwnerKey";

        /// <summary>
        /// The column operator key
        /// </summary>
        protected const string column_OperatorKey = "OperatorKey";

        /// <summary>
        /// The column created stamp
        /// </summary>
        protected const string column_CreatedStamp = "CreatedStamp";

        /// <summary>
        /// The column last updated stamp
        /// </summary>
        protected const string column_LastUpdatedStamp = "LastUpdatedStamp";

        /// <summary>
        /// The column count
        /// </summary>
        protected const string column_Count = "Count";

        /// <summary>
        /// The column data cursor
        /// </summary>
        protected const string column_DataCursor = "DataCursor";

        /// <summary>
        /// The column identifier
        /// </summary>
        protected const string column_Identifier = "Identifier";

        /// <summary>
        /// The column culture code
        /// </summary>
        protected const string column_CultureCode = "CultureCode";

        /// <summary>
        /// The column hash
        /// </summary>
        protected const string column_Hash = "Hash";

        /// <summary>
        /// The column from stamp
        /// </summary>
        protected const string column_FromStamp = "FromStamp";

        /// <summary>
        /// The column to stamp
        /// </summary>
        protected const string column_ToStamp = "ToStamp";

        /// <summary>
        /// The column from date
        /// </summary>
        protected const string column_FromDate = "FromDate";

        /// <summary>
        /// The column to date
        /// </summary>
        protected const string column_ToDate = "ToDate";

        /// <summary>
        /// The column type
        /// </summary>
        protected const string column_Type = "Type";

        /// <summary>
        /// The column code
        /// </summary>
        protected const string column_Code = "Code";

        /// <summary>
        /// The column order descending
        /// </summary>
        protected const string column_OrderDescending = "OrderDescending";

        /// <summary>
        /// The column time forwarding
        /// </summary>
        protected const string column_TimeForwarding = "TimeForwarding";

        /// <summary>
        /// The column identifiers
        /// </summary>
        protected const string column_Identifiers = "Identifiers";

        /// <summary>
        /// The column created by
        /// </summary>
        protected const string column_CreatedBy = "CreatedBy";

        /// <summary>
        /// The column last updated by
        /// </summary>
        protected const string column_LastUpdatedBy = "LastUpdatedBy";

        /// <summary>
        /// The column name
        /// </summary>
        protected const string column_Name = "Name";

        /// <summary>
        /// The column token
        /// </summary>
        protected const string column_Token = "Token";

        /// <summary>
        /// The column realm
        /// </summary>
        protected const string column_Realm = "Realm";

        /// <summary>
        /// The column tenant
        /// </summary>
        protected const string column_Tenant = "Tenant";

        /// <summary>
        /// The column ip address
        /// </summary>
        protected const string column_IpAddress = "IpAddress";

        /// <summary>
        /// The column user agent
        /// </summary>
        protected const string column_UserAgent = "UserAgent";

        /// <summary>
        /// The column email
        /// </summary>
        protected const string column_Email = "Email";

        /// <summary>
        /// The column domain
        /// </summary>
        protected const string column_Domain = "Domain";

        /// <summary>
        /// The column description
        /// </summary>
        protected const string column_Description = "Description";

        /// <summary>
        /// The column start index
        /// </summary>
        protected const string column_StartIndex = "StartIndex";

        /// <summary>
        /// The column expired stamp
        /// </summary>
        protected const string column_ExpiredStamp = "ExpiredStamp";

        /// <summary>
        /// The column platform
        /// </summary>
        protected const string column_Platform = "Platform";

        /// <summary>
        /// The column platform type
        /// </summary>
        protected const string column_PlatformType = "PlatformType";

        /// <summary>
        /// The column device type
        /// </summary>
        protected const string column_DeviceType = "DeviceType";

        /// <summary>
        /// The column expiration
        /// </summary>
        protected const string column_Expiration = "Expiration";

        /// <summary>
        /// The column time zone
        /// </summary>
        protected const string column_TimeZone = "TimeZone";

        /// <summary>
        /// The column time zone standard name
        /// </summary>
        protected const string column_TimeZoneStandardName = "TimeZoneStandardName";

        /// <summary>
        /// The column language
        /// </summary>
        protected const string column_Language = "Language";

        /// <summary>
        /// The column token expired stamp
        /// </summary>
        protected const string column_TokenExpiredStamp = "TokenExpiredStamp";

        /// <summary>
        /// The column member key
        /// </summary>
        protected const string column_MemberKey = "MemberKey";

        /// <summary>
        /// The column functional role
        /// </summary>
        protected const string column_FunctionalRole = "FunctionalRole";

        /// <summary>
        /// The column third party identifier
        /// </summary>
        protected const string column_ThirdPartyId = "ThirdPartyId";

        /// <summary>
        /// The column parent key
        /// </summary>
        protected const string column_ParentKey = "ParentKey";

        /// <summary>
        /// The column avatar key
        /// </summary>
        protected const string column_AvatarKey = "AvatarKey";

        /// <summary>
        /// The column is expired
        /// </summary>
        protected const string column_IsExpired = "IsExpired";

        /// <summary>
        /// The column sequence
        /// </summary>
        protected const string column_Sequence = "Sequence";

        /// <summary>
        /// The column stamp
        /// </summary>
        protected const string column_Stamp = "Stamp";

        /// <summary>
        /// The column content
        /// </summary>
        protected const string column_Content = "Content";

        /// <summary>
        /// The column content type
        /// </summary>
        protected const string column_ContentType = "ContentType";

        /// <summary>
        /// The column tags
        /// </summary>
        protected const string column_Tags = "Tags";

        /// <summary>
        /// The column tag
        /// </summary>
        protected const string column_Tag = "Tag";

        /// <summary>
        /// The column tenant key
        /// </summary>
        protected const string column_TenantKey = "TenantKey";

        /// <summary>
        /// The column binary keys
        /// </summary>
        protected const string column_BinaryKeys = "BinaryKeys";

        /// <summary>
        /// The column is read only
        /// </summary>
        protected const string column_IsReadOnly = "IsReadOnly";

        /// <summary>
        /// The column public key
        /// </summary>
        protected const string column_PublicKey = "PublicKey";

        /// <summary>
        /// The column private key
        /// </summary>
        protected const string column_PrivateKey = "PrivateKey";

        /// <summary>
        /// The column snapshot key
        /// </summary>
        protected const string column_SnapshotKey = "SnapshotKey";

        /// <summary>
        /// The column product key
        /// </summary>
        protected const string column_ProductKey = "ProductKey";

        /// <summary>
        /// The column platform key
        /// </summary>
        protected const string column_PlatformKey = "PlatformKey";

        /// <summary>
        /// The column name of Quantity
        /// </summary>
        protected const string column_Quantity = "Quantity";

        /// <summary>
        /// The column name of Width
        /// </summary>
        protected const string column_Width = "Width";

        /// <summary>
        /// The column name of Height
        /// </summary>
        protected const string column_Height = "Height";

        /// <summary>
        /// The column name of Length
        /// </summary>
        protected const string column_Length = "Length";

        /// <summary>
        /// The column date
        /// </summary>
        protected const string column_Date = "Date";

        /// <summary>
        /// The column kv meta
        /// </summary>
        protected const string column_KVMeta = "KVMeta";

        /// <summary>
        /// The column kv meta criteria
        /// </summary>
        protected const string column_KVMetaCriteria = "KVMetaCriteria";

        /// <summary>
        /// The column URL
        /// </summary>
        protected const string column_Url = "Url";

        /// <summary>
        /// The column value
        /// </summary>
        protected const string column_Value = "Value";

        #endregion Constants

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseDataAccessController{T}"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        protected BaseDataAccessController(SqlDataAccessOptions options) : base(options)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseDataAccessController{T}"/> class.
        /// </summary>
        protected BaseDataAccessController()
            : this(Framework.PrimarySqlConnection)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseDataAccessController{T}" /> class.
        /// </summary>
        /// <param name="primarySqlConnectionString">The primary SQL connection string.</param>
        /// <param name="readOnlySqlConnectionString">The read only SQL connection string.</param>
        protected BaseDataAccessController(string primarySqlConnectionString, string readOnlySqlConnectionString = null)
            : base(primarySqlConnectionString, readOnlySqlConnectionString)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Beyova.SqlDataAccessController`1" /> class.
        /// </summary>
        /// <param name="primarySqlConnection">The SQL connection.</param>
        /// <param name="readOnlySqlConnection">The read only SQL connection.</param>
        protected BaseDataAccessController(SqlConnection primarySqlConnection, SqlConnection readOnlySqlConnection = null)
            : base(primarySqlConnection, readOnlySqlConnection)
        {
        }

        #endregion Constructor

        #region Fill interface based object.

        /// <summary>
        /// Converts the friendly identifier.
        /// </summary>
        /// <param name="sqlDataReader">The SQL data reader.</param>
        /// <returns></returns>
        protected FriendlyIdentifier ConvertFriendlyIdentifier(SqlDataReader sqlDataReader)
        {
            // DO NOT use default parameters so that inheried classes can use it directly to SELECT out FriendlyIdentifier objects.
            return ConvertFriendlyIdentifier(sqlDataReader, nameof(IIdentifier.Key), nameof(INameObject.Name));
        }

        /// <summary>
        /// Converts the friendly identifier.
        /// </summary>
        /// <param name="sqlDataReader">The SQL data reader.</param>
        /// <param name="keyColumnName">Name of the key column.</param>
        /// <param name="nameColumnName">Name of the name column.</param>
        /// <returns></returns>
        protected FriendlyIdentifier ConvertFriendlyIdentifier(SqlDataReader sqlDataReader, string keyColumnName, string nameColumnName)
        {
            return new FriendlyIdentifier
            {
                Name = sqlDataReader[column_Name].ObjectToString(),
                Key = sqlDataReader[column_Key].ObjectToGuid()
            };
        }

        /// <summary>
        /// Fills the kv extensible.
        /// </summary>
        /// <param name="baseObj">The base object.</param>
        /// <param name="sqlDataReader">The SQL data reader.</param>
        protected static void FillKVExtensible(IKVMetaExtensible baseObj, SqlDataReader sqlDataReader)
        {
            if (baseObj != null && sqlDataReader != null)
            {
                baseObj.KVMeta = sqlDataReader[column_KVMeta].ObjectToJsonObject<KVMetaDictionary>();
            }
        }

        /// <summary>
        /// Fills the base object fields.
        /// </summary>
        /// <param name="baseObj">The base obj.</param>
        /// <param name="sqlDataReader">The SQL data reader.</param>
        /// <param name="ignoreOperator">if set to <c>true</c> [ignore operator].</param>
        protected static void FillBaseObjectFields(IBaseObject baseObj, SqlDataReader sqlDataReader, bool ignoreOperator = false)
        {
            if (baseObj != null)
            {
                FillSimpleBaseObjectFields(baseObj, sqlDataReader);
                if (!ignoreOperator)
                {
                    baseObj.CreatedBy = sqlDataReader[column_CreatedBy].ObjectToString();
                    baseObj.LastUpdatedBy = sqlDataReader[column_LastUpdatedBy].ObjectToString();
                }
            }
        }

        /// <summary>
        /// Fills the base simple object fields.
        /// </summary>
        /// <param name="baseObj">The base object.</param>
        /// <param name="sqlDataReader">The SQL data reader.</param>
        protected static void FillSimpleBaseObjectFields(ISimpleBaseObject baseObj, SqlDataReader sqlDataReader)
        {
            if (baseObj != null)
            {
                baseObj.LastUpdatedStamp = sqlDataReader[column_LastUpdatedStamp].ObjectToDateTime();
                baseObj.State = (ObjectState)(sqlDataReader[column_State].ObjectToInt32());
                baseObj.Key = sqlDataReader[column_Key].ObjectToGuid();
                baseObj.CreatedStamp = sqlDataReader[column_CreatedStamp].ObjectToDateTime();
            }
        }

        /// <summary>
        /// Fills the audit base object fields.
        /// </summary>
        /// <param name="baseObj">The base object.</param>
        /// <param name="sqlDataReader">The SQL data reader.</param>
        protected static void FillAuditBaseObjectFields(IAuditBaseObject baseObj, SqlDataReader sqlDataReader)
        {
            if (baseObj != null)
            {
            }
        }

        #endregion Fill interface based object.

        /// <summary>
        /// Executes the reader as base object.
        /// </summary>
        /// <param name="spName">Name of the sp.</param>
        /// <param name="sqlParameters">The SQL parameters.</param>
        /// <param name="preferReadOnlyOperator">if set to <c>true</c> [prefer read only operator].</param>
        /// <param name="ignoreOperator">if set to <c>true</c> [ignore operator].</param>
        /// <returns>List&lt;BaseObject&lt;T&gt;&gt;.</returns>
        protected List<BaseObject<T>> ExecuteReaderAsBaseObject(string spName, List<SqlParameter> sqlParameters = null, bool preferReadOnlyOperator = false, bool ignoreOperator = false)
        {
            return ExecuteReader<BaseObject<T>>(spName, sqlParameters, (reader) =>
            {
                var result = new BaseObject<T>(ConvertEntityObject(reader));
                FillBaseObjectFields(result, reader, ignoreOperator);
                return result;
            }, preferReadOnlyOperator);
        }

        /// <summary>
        /// Executes the reader as simple base object.
        /// </summary>
        /// <param name="spName">Name of the sp.</param>
        /// <param name="sqlParameters">The SQL parameters.</param>
        /// <param name="preferReadOnlyOperator">if set to <c>true</c> [prefer read only operator].</param>
        /// <returns>List&lt;SimpleBaseObject&lt;T&gt;&gt;.</returns>
        protected List<SimpleBaseObject<T>> ExecuteReaderAsSimpleBaseObject(string spName, List<SqlParameter> sqlParameters = null, bool preferReadOnlyOperator = false)
        {
            return ExecuteReader<SimpleBaseObject<T>>(spName, sqlParameters, (reader) =>
            {
                var result = new SimpleBaseObject<T>(ConvertEntityObject(reader));
                FillSimpleBaseObjectFields(result, reader);
                return result;
            }, preferReadOnlyOperator);
        }

        //protected object ReadColumn(SqlDataReader reader, string columnName)
        //{
        //    reader[
        //}
    }
}