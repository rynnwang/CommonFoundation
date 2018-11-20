using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Beyova
{
    /// <summary>
    ///
    /// </summary>
    public static class SpecificTypeDiscover
    {
        /// <summary>
        /// Gets the type with specific attribute.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="isInherit">if set to <c>true</c> [is inherit].</param>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public static List<Type> GetTypeWithSpecificAttribute<T>(bool isInherit = false, TypeKindFilter filter = TypeKindFilter.Any) where T : Attribute
        {
            HashSet<Type> result = new HashSet<Type>();

            foreach (var item in EnvironmentCore.DescendingAssemblyDependencyChain)
            {
                foreach (var one in item.GetTypes())
                {
                    if (MeetsFilter(one.IsClass, TypeKindFilter.IsClass, filter)
                        && MeetsFilter(one.IsInterface, TypeKindFilter.IsInterface, filter)
                        && MeetsFilter(one.IsPrimitive, TypeKindFilter.IsPrimitive, filter)
                        && MeetsFilter(one.IsPublic, TypeKindFilter.IsPublic, filter)
                        && MeetsFilter(one.IsValueType, TypeKindFilter.IsValueType, filter)
                        && one.GetCustomAttribute<T>(isInherit) != null)
                    {
                        result.Add(one);
                    }
                }
            }

            return result.ToList();
        }

        /// <summary>
        /// Meetses the filter.
        /// </summary>
        /// <param name="boolPropertyValue">if set to <c>true</c> [bool property value].</param>
        /// <param name="filterFactor">The filter factor.</param>
        /// <param name="filterCriteria">The filter criteria.</param>
        /// <returns></returns>
        private static bool MeetsFilter(bool boolPropertyValue, TypeKindFilter filterFactor, TypeKindFilter filterCriteria)
        {
            return filterCriteria == TypeKindFilter.Any //No need to filter
                || (filterCriteria & filterFactor) == 0 //Not hit the factor to filter
                || boolPropertyValue;
        }
    }
}