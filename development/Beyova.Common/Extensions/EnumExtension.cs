using System;

namespace Beyova
{
    /// <summary>
    /// Class EnumExtension.
    /// </summary>
    public static class EnumExtension
    {
        /// <summary>
        /// Determines whether [is valid integer value].
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <param name="intValue">The int value.</param>
        /// <returns>
        ///   <c>true</c> if [is valid integer value] [the specified int value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsValidIntegerValue<TEnum>(this int intValue)
            where TEnum : struct, IConvertible
        {
            return typeof(TEnum).IsEnum && Enum.IsDefined(typeof(TEnum), intValue);
        }

        /// <summary>
        /// Gets the enum default value.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static object GetEnumDefaultValue(Type type)
        {
            object defaultItem = null;

            if (type != null && type.IsEnum)
            {
                var enumValues = Enum.GetValues(type);

                foreach (var one in enumValues)
                {
                    if ((int)one == 0)
                    {
                        defaultItem = one;
                        break;
                    }
                }
            }

            return defaultItem;
        }

        /// <summary>
        /// Tries the parse enum.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="text">The text.</param>
        /// <param name="enumValue">The enum value.</param>
        /// <param name="ignoreCase">if set to <c>true</c> [ignore case].</param>
        /// <returns></returns>
        public static bool TryParseEnum(Type type, string text, out object enumValue, bool ignoreCase = false)
        {
            enumValue = null;
            return (type != null && type.IsEnum && !string.IsNullOrWhiteSpace(text)) ? InternalTryParseEnum(Enum.GetValues(type), text, out enumValue, ignoreCase) : false;
        }

        /// <summary>
        /// Internals the try parse enum.
        /// </summary>
        /// <param name="enumValues">The enum values.</param>
        /// <param name="text">The text.</param>
        /// <param name="enumValue">The enum value.</param>
        /// <param name="ignoreCase">if set to <c>true</c> [ignore case].</param>
        /// <returns></returns>
        internal static bool InternalTryParseEnum(Array enumValues, string text, out object enumValue, bool ignoreCase)
        {
            enumValue = null;

            if (enumValues != null && !string.IsNullOrWhiteSpace(text))
            {
                foreach (var one in enumValues)
                {
                    if (string.Compare(one.ToString(), text, ignoreCase) == 0)
                    {
                        enumValue = one;
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Determines whether the specified enum value2 has flag.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumValue1">The enum value1.</param>
        /// <param name="enumValue2">The enum value2.</param>
        /// <returns>
        ///   <c>true</c> if the specified enum value2 has flag; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasFlag<T>(this T? enumValue1, T enumValue2)
            where T : struct, IConvertible
        {
            return enumValue1.HasValue && InternalHasFlag(enumValue1.Value, enumValue2);
        }

        /// <summary>
        /// Determines whether the specified value2 has flag.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value1">The value1.</param>
        /// <param name="value2">The value2.</param>
        /// <returns>
        ///   <c>true</c> if the specified value2 has flag; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasFlag<T>(this T value1, T value2)
            where T : struct, IConvertible
        {
            return InternalHasFlag(value1, value2);
        }

        /// <summary>
        /// Internals the has flag.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value1">The value1.</param>
        /// <param name="value2">The value2.</param>
        /// <returns></returns>
        private static bool InternalHasFlag<T>(this T value1, T value2)
                 where T : struct, IConvertible
        {
            var masterValue = ((IConvertible)value1).ToInt64(null);
            var seconaryValue = ((IConvertible)value2).ToInt64(null);
            return masterValue >= seconaryValue && (masterValue & seconaryValue) == seconaryValue;
        }
    }
}