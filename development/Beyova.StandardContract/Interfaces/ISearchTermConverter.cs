using System;
using System.Collections.Generic;
using System.Text;

namespace Beyova
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISearchTermConverter
    {
        /// <summary>
        /// Gets the search term.
        /// </summary>
        /// <param name="orignalValue">The orignal value.</param>
        /// <returns></returns>
        string GetSearchTerm(string orignalValue);
    }
}
