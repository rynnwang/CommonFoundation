using Beyova.Gravity;

namespace Beyova.UnitTestKit.InternalDoor
{
    /// <summary>
    /// Class GravityKit.
    /// </summary>
    public static class GravityKit
    {
        /// <summary>
        /// Gets the heartbeat.
        /// </summary>
        /// <returns>Heartbeat.</returns>
        public static Heartbeat GetHeartbeat()
        {
            var machineHealth = new Heartbeat
            {
                ConfigurationName = "Any"
            };
            machineHealth.FillHealthData();

            return machineHealth;
        }
    }
}