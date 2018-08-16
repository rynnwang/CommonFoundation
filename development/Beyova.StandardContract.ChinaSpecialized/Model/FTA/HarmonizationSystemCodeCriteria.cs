using System;

namespace Beyova.ChinaSpecialized
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="Beyova.IIdentifier" />
    public class HarmonizationSystemCodeCriteria : BasePageIndexedCriteria, INational, IParentNode, ICodeIdentifier
    {
        /// <summary>
        /// Gets or sets the parent key.
        /// </summary>
        /// <value>
        /// The parent key.
        /// </value>
        public Guid? ParentNodeKey { get; set; }

        /// <summary>
        /// Gets or sets the nation code.
        /// </summary>
        /// <value>
        /// The nation code.
        /// </value>
        public string NationCode { get; set; }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the culture code.
        /// </summary>
        /// <value>
        /// The culture code.
        /// e.g.: zh-CN, en-US, pt-PT, etc.
        /// </value>
        public string CultureCode { get; set; }
    }
}