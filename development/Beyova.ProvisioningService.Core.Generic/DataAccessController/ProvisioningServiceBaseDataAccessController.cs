using System.Data.SqlClient;

namespace Beyova.FunctionService.Generic
{
    /// <summary>
    /// Class ProvisioningServiceBaseDataAccessController.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Beyova.BaseDataAccessController{T}" />
    internal abstract class ProvisioningServiceBaseDataAccessController<T> : BaseDataAccessController<T>
    {
        protected const string column_ClientProduct = "ClientProduct";

        protected const string column_ClientEnvironment = "ClientEnvironment";

        protected const string column_Path = "Path";

        protected const string column_BlackList = "BlackList";

        protected const string column_TicketKey = "TicketKey";

        protected const string column_DeviceIdentifier = "DeviceIdentifier";

        protected const string column_DeviceModel = "DeviceModel";

        protected const string column_SystemName = "SystemName";

        protected const string column_SystemVersion = "SystemVersion";

        protected const string column_AppVersion = "AppVersion";

        protected const string column_AppBuild = "AppBuild";

        protected const string column_PackageStamp = "PackageStamp";

        protected const string column_TicketId = "TicketId";

        protected const string column_TicketTitle = "TicketTitle";

        protected const string column_TicketStatus = "TicketStatus";

        protected const string column_BundleId = "BundleId";

        protected const string column_MinOSVersion = "MinOSVersion";

        protected const string column_LatestBuild = "LatestBuild";

        protected const string column_LatestVersion = "LatestVersion";

        protected const string column_MinRequiredBuild = "MinRequiredBuild";

        protected const string column_Note = "Note";

        protected const string column_AppServiceStatus = "AppServiceStatus";

        /// <summary>
        /// Fills the application platform.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="reader">The reader.</param>
        protected static void FillAppPlatform(IAppPlatform obj, SqlDataReader reader)
        {
            if (obj != null && reader != null)
            {
                obj.BundleId = reader[column_BundleId].ObjectToString();
                obj.PlatformType = reader[column_PlatformType].ObjectToEnum<PlatformType>();
                obj.Url = reader[column_Url].ObjectToString();
                obj.MinOSVersion = reader[column_MinOSVersion].ObjectToString();
                obj.Name = reader[column_Name].ObjectToString();
            }
        }

        /// <summary>
        /// Fills the application version base.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="reader">The reader.</param>
        protected static void FillAppVersionBase(AppVersionBase obj, SqlDataReader reader)
        {
            if (obj != null && reader != null)
            {
                obj.Key = reader[column_Key].ObjectToGuid();
                obj.PlatformKey = reader[column_PlatformKey].ObjectToGuid();
                obj.LatestBuild = reader[column_LatestBuild].ObjectToInt32();
                obj.LatestVersion = reader[column_LatestVersion].ObjectToString();
                obj.MinRequiredBuild = reader[column_MinRequiredBuild].ObjectToInt32();
                obj.Note = reader[column_Note].ObjectToString();
                obj.AppServiceStatus = reader[column_AppServiceStatus].ObjectToEnum<AppServiceStatus>();
            }
        }
    }
}