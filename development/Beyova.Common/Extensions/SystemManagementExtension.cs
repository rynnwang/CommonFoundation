using System;
using System.Collections.Generic;
using System.IO;

namespace Beyova
{
    /// <summary>
    /// Class SystemManagementExtension.
    /// </summary>
    public static partial class SystemManagementExtension
    {
        /// <summary>
        /// Fills the health data.
        /// </summary>
        /// <param name="healthObject">The health object.</param>
        internal static void FillHealthData(this IMachineHealth healthObject)
        {
            if (healthObject != null)
            {
                healthObject.ServerName = EnvironmentCore.ServerName;
                healthObject.IpAddress = EnvironmentCore.LocalMachineIpAddress;
                healthObject.HostName = EnvironmentCore.LocalMachineHostName;
            };
        }

        /// <summary>
        /// Gets the machine health.
        /// </summary>
        /// <returns>MachineHealth.</returns>
        public static MachineHealth GetMachineHealth()
        {
            return new MachineHealth
            {
                ServerName = EnvironmentCore.ServerName
            };
        }

        /// <summary>
        /// Gets the disk usages.
        /// </summary>
        /// <returns>List&lt;DiskDriveInfo&gt;.</returns>
        public static List<DiskDriveInfo> GetDiskUsages()
        {
            List<DiskDriveInfo> result = new List<DiskDriveInfo>();

            try
            {
                foreach (DriveInfo drive in DriveInfo.GetDrives())
                {
                    result.Add(new DiskDriveInfo
                    {
                        VolumeLabel = drive.VolumeLabel,
                        IsReady = drive.IsReady,
                        Name = drive.Name,
                        TotalFreeSpace = drive.TotalFreeSpace,
                        TotalSize = drive.TotalSize
                    });
                }
            }
            catch { }

            return result;
        }

        /// <summary>
        /// Gets the gc memory.
        /// </summary>
        /// <param name="forceFullCollection">if set to <c>true</c> [force full collection].</param>
        /// <returns>System.Nullable&lt;System.Int64&gt;.</returns>
        public static long? GetGCMemory(bool forceFullCollection = false)
        {
            try
            {
                return GC.GetTotalMemory(true);
            }
            catch { }

            return null;
        }
    }
}