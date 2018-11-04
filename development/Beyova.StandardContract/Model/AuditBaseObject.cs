using System;

namespace Beyova
{
    /// <summary>
    /// Class AuditBaseObject
    /// </summary>
    public abstract class AuditBaseObject : IAuditBaseObject
    {
        #region Properties

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public Guid? Key
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the created stamp.
        /// </summary>
        /// <value>
        /// The created stamp.
        /// </value>
        public DateTime? CreatedStamp
        {
            get;
            set;
        }

        #endregion Properties
    }
}