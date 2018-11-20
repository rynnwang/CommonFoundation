namespace Beyova.Scheduling
{
    /// <summary>
    /// class SchedulingResourceContainerConstraints. It defines constraints of container.
    /// </summary>
    public class SchedulingResourceContainerConstraints : SchedulingResourceContainerConstraintsEssential, IIdentifier
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }
    }
}