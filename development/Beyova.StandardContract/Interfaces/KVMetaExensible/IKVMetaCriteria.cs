using System.Collections.Generic;

namespace Beyova
{
    /// <summary>
    /// </summary>
    public interface IKVMetaCriteria
    {
        /// <summary>
        /// Gets or sets the kv meta criteria.
        /// </summary>
        /// <value>
        /// The kv meta criteria.
        /// </value>
        List<KVMetaCriteriaExpression> KVMetaCriteria { get; set; }
    }
}