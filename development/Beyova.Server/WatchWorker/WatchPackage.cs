using System;
using System.Collections.Generic;
using System.Text;

namespace Beyova
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public class WatchPackage<TEntity> : WatchPackage<TEntity, BaseCredential>
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TContextCredential">The type of the context credential.</typeparam>
    public  class WatchPackage<TEntity, TContextCredential>
    {
        /// <summary>
        /// Gets or sets the get items to process.
        /// </summary>
        /// <value>
        /// The get items to process.
        /// </value>
        public Func<List<TEntity>> GetItemsToProcess { get; set; }

        /// <summary>
        /// Gets or sets the item processer.
        /// </summary>
        /// <value>
        /// The item processer.
        /// </value>
        public Action<TEntity> ItemProcesser { get; set; }

        /// <summary>
        /// Gets or sets the get context credential.
        /// </summary>
        /// <value>
        /// The get context credential.
        /// </value>
        public Func<TEntity, TContextCredential> GetContextCredential { get; set; }

        /// <summary>
        /// Gets or sets the idle seconds.
        /// </summary>
        /// <value>
        /// The idle seconds.
        /// </value>
        public int? IdleSeconds { get; set; }
    }
}
