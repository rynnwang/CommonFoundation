using System;

namespace Beyova.Document
{
    /// <summary>
    ///
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Interface)]
    public abstract class DocumentDescriptionAttribute : Attribute
    {
        /// <summary>
        /// Gets the document HTML.
        /// </summary>
        /// <returns></returns>
        public abstract string GetDocumentHtml();
    }
}