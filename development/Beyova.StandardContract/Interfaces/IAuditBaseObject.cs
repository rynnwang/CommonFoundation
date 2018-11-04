using System;

namespace Beyova
{
    /// <summary>
    /// Interface IAuditBaseObject
    /// </summary>
    public interface IAuditBaseObject : IIdentifier
    {
        #region Properties

        /// <summary>
        /// Gets or sets the created stamp.
        /// </summary>
        /// <value>The created stamp.</value>
        DateTime? CreatedStamp
        {
            get;
            set;
        }

        #endregion Properties
    }
}