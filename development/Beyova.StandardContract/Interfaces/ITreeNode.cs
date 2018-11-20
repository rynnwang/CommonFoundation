using System.Collections.Generic;

namespace Beyova
{
    /// <summary>
    /// Interface ITreeNode
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ITreeNode<T> : IIdentifier, IParentNode
    {
        /// <summary>
        /// Gets or sets the children.
        /// </summary>
        /// <value>The children.</value>
        List<T> Children { get; set; }
    }
}