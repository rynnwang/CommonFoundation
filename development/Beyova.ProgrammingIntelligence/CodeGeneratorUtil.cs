using System;
using System.Collections.Generic;
using System.Text;

namespace Beyova
{
    /// <summary>
    /// 
    /// </summary>
    public static class CodeGeneratorUtil
    {
        /// <summary>
        /// Generates the name of the unique type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static string GenerateUniqueTypeName(Type type)
        {
            if (type != null)
            {
                string timestamp = DateTime.UtcNow.Ticks.ToString();
                return type.IsGenericType ? string.Format("{0}_{1}_{2}_{3}", type.Name, type.GetGenericArguments().Length, timestamp, Guid.NewGuid().ToString("N")) : string.Format("{0}_{1}_{2}", type.Name, timestamp, Guid.NewGuid().ToString("N"));
            }

            return string.Empty;
        }
    }
}
