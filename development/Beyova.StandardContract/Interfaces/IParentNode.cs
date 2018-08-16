using System;
using System.Collections.Generic;

namespace Beyova
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IParentNode<T>
    {
        /// <summary>
        /// Gets or sets the parent node key.
        /// </summary>
        /// <value>
        /// The parent node key.
        /// </value>
        T ParentNodeKey { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IParentNode : IParentNode<Guid?>
    {
    }
}