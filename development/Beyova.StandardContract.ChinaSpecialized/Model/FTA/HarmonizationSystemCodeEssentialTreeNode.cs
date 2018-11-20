using System.Collections.Generic;

namespace Beyova.ChinaSpecialized
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="Beyova.IGlobalObjectName" />
    /// <seealso cref="Beyova.IIdentifier" />
    public class HarmonizationSystemCodeEssentialTreeNode : HarmonizationSystemCodeEssential, ITreeNode<HarmonizationSystemCodeEssentialTreeNode>, IIdentifier
    {
        /// <summary>
        /// Gets or sets the children.
        /// </summary>
        /// <value>
        /// The children.
        /// </value>
        public List<HarmonizationSystemCodeEssentialTreeNode> Children { get; set; }
    }
}