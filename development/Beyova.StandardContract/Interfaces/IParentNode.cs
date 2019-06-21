using System;

namespace Beyova
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public interface IParentNode<TKey>
    {
        /// <summary>
        /// Gets or sets the parent node key.
        /// </summary>
        /// <value>
        /// The parent node key.
        /// </value>
        TKey ParentNodeKey { get; set; }
    }

    /// <summary>
    ///
    /// </summary>
    public interface IParentNode : IParentNode<Guid?>
    {
    }
}