namespace Beyova
{
    /// <summary>
    ///
    /// </summary>
    public static class BeyovaVersion
    {
        /// <summary>
        /// Gets the common vesion.
        /// </summary>
        /// <value>
        /// The common vesion.
        /// </value>
        public static string CommonVesion { get; private set; }

        /// <summary>
        /// Initializes the <see cref="BeyovaVersion"/> class.
        /// </summary>
        static BeyovaVersion()
        {
            CommonVesion = typeof(EnvironmentCore).Assembly.GetCustomAttribute<BeyovaComponentAttribute>()?.UnderlyingObject?.Version;
        }
    }
}