using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Beyova.ProgrammingIntelligence
{
    /// <summary>
    /// 
    /// </summary>
    public enum TypeKindFilter
    {
        /// <summary>
        /// Any
        /// </summary>
        Any = 0,
        /// <summary>
        /// The is class
        /// </summary>
        IsClass = 0x1,
        /// <summary>
        /// The is value type
        /// </summary>
        IsValueType = 0x2,
        /// <summary>
        /// The is interface
        /// </summary>
        IsInterface = 0x4,
        /// <summary>
        /// The is public
        /// </summary>
        IsPublic = 0x8,
        /// <summary>
        /// The is primitive
        /// </summary>
        IsPrimitive = 0x10
    }
}
