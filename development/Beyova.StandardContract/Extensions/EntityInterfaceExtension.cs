using System;
using System.Collections.Generic;
using System.Text;

namespace Beyova
{
    /// <summary>
    /// 
    /// </summary>
    public static class EntityInterfaceExtension
    {
        /// <summary>
        /// Ases the same nation as.
        /// </summary>
        /// <param name="nationalObject1">The national object1.</param>
        /// <param name="nationalObject2">The national object2.</param>
        /// <returns></returns>
        public static bool? AsSameNationAs(INational nationalObject1, INational nationalObject2)
        {
            if (nationalObject1 == null || string.IsNullOrWhiteSpace(nationalObject1.NationCode) || nationalObject2 == null || string.IsNullOrWhiteSpace(nationalObject2.NationCode))
            {
                return null;
            }

            return nationalObject1.NationCode.Equals(nationalObject2.NationCode, StringComparison.OrdinalIgnoreCase);
        }
    }
}