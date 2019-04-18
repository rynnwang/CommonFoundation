using System;

namespace Beyova
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TContextCredential">The type of the context credential.</typeparam>
    internal class WorkerJobPackage<TEntity, TContextCredential>
    {
        /// <summary>
        /// Gets or sets the target.
        /// </summary>
        /// <value>
        /// The target.
        /// </value>
        public TEntity Target { get; set; }

        /// <summary>
        /// Gets or sets the context credential.
        /// </summary>
        /// <value>
        /// The context credential.
        /// </value>
        public TContextCredential ContextCredential { get; set; }

        /// <summary>
        /// Gets or sets the processor.
        /// </summary>
        /// <value>
        /// The processor.
        /// </value>
        public Action<TEntity> Processor { get; set; }
    }
}