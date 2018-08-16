using System;

namespace Beyova.ChinaSpecialized
{
    /// <summary>
    /// Class HarmonizationSystemCodeEssential, defines essential field for HarmonizationSystemCode, to support data model, view model and tree model.
    /// </summary>
    /// <seealso cref="Beyova.IGlobalObjectName" />
    /// <seealso cref="Beyova.IIdentifier" />
    public class HarmonizationSystemCodeEssential : IIdentifier, INational, IGlobalObjectName, IParentNode, ICodeIdentifier
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        public Guid? Key { get; set; }

        /// <summary>
        /// Gets or sets the country code. ISO-3 code for country. If it is null, then it is not tied to country.
        /// </summary>
        /// <value>
        /// The country code.
        /// </value>
        public string NationCode { get; set; }

        /// <summary>
        /// Gets or sets the code. NOTE: Code is not always has value
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the parent node key.
        /// </summary>
        /// <value>
        /// The parent node key.
        /// </value>
        public Guid? ParentNodeKey { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

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