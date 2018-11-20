using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Beyova.ExceptionSystem;
using Newtonsoft.Json.Linq;

namespace Beyova
{
    /// <summary>
    /// Class ReflectionExtension.
    /// </summary>
    public static partial class ReflectionExtension
    {
        #region Convert Object

        /// <summary>
        /// Converts the objects by <see cref="Type" /> instance.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="value">The value.</param>
        /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
        /// <returns>System.Object.</returns>
        public static object ConvertToObjectByType(Type type, string value, bool throwException = true)
        {
            return type == typeof(string) ? value : ConvertStringToObject(value, type, throwException);
        }

        /// <summary>
        /// Converts to object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
        /// <returns>``0.</returns>
        /// <exception cref="InvalidObjectException"></exception>
        public static T ConvertStringToObject<T>(this string value, bool throwException = true)
        {
            try
            {
                if (typeof(T) == typeof(string))
                {
                    return (T)((object)value);
                }

                var converter = TypeDescriptor.GetConverter(typeof(T));
                if (converter != null)
                {
                    //Cast ConvertFromString(string text) : object to (T)
                    return (T)converter.ConvertFromString(value);
                }
            }
            catch (Exception ex)
            {
                if (throwException)
                {
                    throw ex.Handle(new { value, type = typeof(T)?.Name });
                }
            }

            return default(T);
        }

        /// <summary>
        /// Converts the string to object.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="type">The type.</param>
        /// <param name="throwException">The throw exception.</param>
        /// <returns>System.Object.</returns>
        private static object ConvertStringToObject(this string value, Type type, bool throwException = true)
        {
            try
            {
                var converter = TypeDescriptor.GetConverter(type);
                return converter?.ConvertFromString(value);
            }
            catch (Exception ex)
            {
                if (throwException)
                {
                    throw ex.Handle(new { value, type = type?.Name });
                }
            }

            return null;
        }

        #endregion Convert Object

        #region Invoke methods

        /// <summary>
        /// Invokes the method.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="instance">The instance.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="invokeParameters">The invoke parameters.</param>
        /// <param name="genericTypes">The generic types.</param>
        /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
        /// <returns>System.Object.</returns>
        /// <exception cref="System.InvalidCastException">Neither objectType or instance is valid.</exception>
        public static object InvokeMethod(Type objectType, object instance, string methodName, object[] invokeParameters, Type[] genericTypes = null, bool throwException = false)
        {
            try
            {
                if (objectType == null && instance == null)
                {
                    throw new InvalidObjectException("objectType/instance");
                }
                else
                {
                    if (objectType == null)
                    {
                        objectType = instance.GetType();
                    }

                    MethodInfo methodInfo = objectType.GetMethod(methodName);
                    methodInfo.CheckNullObject(nameof(methodInfo));

                    if (!methodInfo.IsStatic)
                    {
                        instance.CheckNullObject(nameof(instance));
                    }
                    else
                    {
                        instance = null;
                    }

                    if (methodInfo.IsGenericMethod)
                    {
                        genericTypes.CheckNullObject(nameof(genericTypes));
                        methodInfo = methodInfo.MakeGenericMethod(genericTypes);
                    }

                    return methodInfo.Invoke(instance, invokeParameters);
                }
            }
            catch (Exception ex)
            {
                if (throwException)
                {
                    throw ex.Handle(new { methodName, objectType = objectType == null ? "null" : objectType.FullName });
                }
            }

            return null;
        }

        /// <summary>
        /// Invokes the static generic method.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="invokeParameters">The invoke parameters.</param>
        /// <param name="genericTypes">The generic types.</param>
        /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
        /// <returns>System.Object.</returns>
        public static object InvokeStaticGenericMethod(Type objectType, string methodName, object[] invokeParameters, Type[] genericTypes = null, bool throwException = false)
        {
            return InvokeMethod(objectType, null, methodName, invokeParameters, genericTypes, throwException);
        }

        #endregion Invoke methods

        #region CreateSampleObject

        /// <summary>
        /// Creates the sample object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>``0.</returns>
        public static T CreateSampleObject<T>() where T : new()
        {
            T result = new T();

            var objectType = result.GetType();
            var publicProperties = objectType.GetProperties(BindingFlags.Public | BindingFlags.SetProperty);

            foreach (var one in publicProperties)
            {
                one.SetValue(result, CreateSampleObjectPropertyValue(one));
            }

            return result;
        }

        /// <summary>
        /// Creates the property value.
        /// </summary>
        /// <param name="propertyInfo">The property information.</param>
        /// <returns>System.Object.</returns>
        public static object CreateSampleObjectPropertyValue(this PropertyInfo propertyInfo)
        {
            object result = null;

            if (propertyInfo != null)
            {
                var type = propertyInfo.PropertyType;

                if (type.IsNullable())
                {
                    result = CreateSampleObject(type.GenericTypeArguments[0]);
                }
                else if (type.IsEnum)
                {
                    var enumValues = Enum.GetValues(type);
                    result = (enumValues != null && enumValues.Length > 0) ? enumValues.GetValue(0) : null;
                }
                else
                {
                    switch (type.FullName)
                    {
                        case "System.Int16":
                        case "System.Int32":
                        case "System.Int64":
                            result = GetRandomNumber(1000, 1);
                            break;

                        case "System.Single":
                        case "System.Double":
                            result = Single.Parse(GetRandomNumber(10, 0) + "." + GetRandomNumber(100, 0));
                            break;

                        case "System.String":
                            result = "String Value";
                            break;

                        case "System.Guid":
                            result = Guid.NewGuid();
                            break;

                        case "System.DateTime":
                            result = DateTime.Now;
                            break;

                        case "System.TimeSpan":
                            result = new TimeSpan(GetRandomNumber(23, 0), GetRandomNumber(60, 0), GetRandomNumber(60, 0));
                            break;

                        default:
                            break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Creates the sample object.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>System.Object.</returns>
        public static object CreateSampleObject(this Type type)
        {
            object result = null;

            if (type != null)
            {
                if (type.IsNullable())
                {
                    result = CreateSampleObject(type.GenericTypeArguments[0]);
                }
                else if (type.IsEnum)
                {
                    var enumValues = Enum.GetValues(type);
                    result = (enumValues != null && enumValues.Length > 0) ? enumValues.GetValue(0) : null;
                }
                else
                {
                    switch (type.FullName)
                    {
                        case "System.Int16":
                        case "System.Int32":
                        case "System.Int64":
                            result = GetRandomNumber(1000, 1);
                            break;

                        case "System.Single":
                        case "System.Double":
                            result = Single.Parse(GetRandomNumber(10, 0) + "." + GetRandomNumber(100, 0));
                            break;

                        case "System.String":
                            result = "String Value";
                            break;

                        case "System.Guid":
                            result = Guid.NewGuid();
                            break;

                        case "System.DateTime":
                            result = DateTime.Now;
                            break;

                        case "System.TimeSpan":
                            result = new TimeSpan(GetRandomNumber(23, 0), GetRandomNumber(60, 0), GetRandomNumber(60, 0));
                            break;

                        default:
                            result = Activator.CreateInstance(type);
                            var publicProperties = type.GetProperties(BindingFlags.Public | BindingFlags.SetProperty);

                            foreach (var one in publicProperties)
                            {
                                one.SetValue(result, CreateSampleObjectPropertyValue(one));
                            }
                            break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the random number.
        /// </summary>
        /// <param name="max">The maximum.</param>
        /// <param name="min">The minimum.</param>
        /// <returns>System.Int32.</returns>
        private static int GetRandomNumber(int max, int min = 0)
        {
            var random = new Random();
            return random.Next(min, max);
        }

        #endregion CreateSampleObject

        /// <summary>
        /// Gets the default value.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static object GetDefaultValue(this Type type)
        {
            return (type != null && type.IsValueType) ? Activator.CreateInstance(type) : null;
        }

        /// <summary>
        /// Gets the parameterless constructor.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static ConstructorInfo GetParameterlessConstructor(this Type type)
        {
            if (type != null)
            {
                var constructors = type.GetConstructors();

                if (constructors.HasItem())
                {
                    foreach (var one in constructors)
                    {
                        var constructorParameters = one.GetParameters();
                        if (!constructorParameters.HasItem())
                        {
                            return one;
                        }

                        bool hasNonDefault = false;
                        foreach (var p in constructorParameters)
                        {
                            if (!p.HasDefaultValue)
                            {
                                hasNonDefault = true;
                                break;
                            }
                        }

                        if (!hasNonDefault)
                        {
                            return one;
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the single parameter constructor.
        /// </summary>
        /// <typeparam name="TParameter">The type of the parameter.</typeparam>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static ConstructorInfo GetSingleParameterConstructor<TParameter>(this Type type)
        {
            if (type != null)
            {
                var constructors = type.GetConstructors();

                if (constructors.HasItem())
                {
                    foreach (var one in constructors)
                    {
                        var parameters = one.GetParameters();
                        if ((parameters?.Length ?? 0) == 1 && parameters[0].ParameterType.IsAssignableFrom(typeof(TParameter)))
                        {
                            return one;
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Tries the create instance via non parameter constructor.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static object TryCreateInstanceViaParameterlessConstructor(this Type type)
        {
            return type.GetParameterlessConstructor().CreateInstance();
        }

        /// <summary>
        /// Tries the create instance via single parameter constructor.
        /// </summary>
        /// <typeparam name="TParameter">The type of the parameter.</typeparam>
        /// <param name="type">The type.</param>
        /// <param name="paramter">The paramter.</param>
        /// <returns></returns>
        public static object TryCreateInstanceViaSingleParameterConstructor<TParameter>(this Type type, TParameter paramter)
        {
            return type.GetSingleParameterConstructor<TParameter>() != null ? CreateInstance(type, (paramter as object).AsArray()) : null;
        }

        /// <summary>
        /// Tries the create instance.
        /// </summary>
        /// <typeparam name="TParameter">The type of the parameter.</typeparam>
        /// <param name="constructure">The constructure.</param>
        /// <returns></returns>
        public static object TryCreateInstance<TParameter>(this SingleParameterInstanceConstructure<TParameter> constructure)
        {
            return constructure != null ? TryCreateInstanceViaSingleParameterConstructor(constructure.Type, constructure.Parameter) : null;
        }

        /// <summary>
        /// Creates the instance.
        /// </summary>
        /// <param name="constructor">The constructor.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public static object CreateInstance(this ConstructorInfo constructor, Dictionary<string, object> parameters = null)
        {
            if (constructor != null)
            {
                var constructorParameters = constructor.GetParameters();
                var parameterValues = constructorParameters.Length > 0 ? new object[constructorParameters.Length] : null;

                int i = 0;
                foreach (var one in constructorParameters)
                {
                    if (parameters?.ContainsKey(one.Name) ?? false)
                    {
                        parameterValues[i] = parameters[one.Name];
                    }
                    else if (one.HasDefaultValue)
                    {
                        parameterValues[i] = one.DefaultValue;
                    }
                    else
                    {
                        throw ExceptionFactory.CreateInvalidObjectException(nameof(one.Name));
                    }

                    i++;
                }

                return constructor.Invoke(parameterValues);
            }

            return null;
        }

        #region Method Invoke

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TParameters">The type of the parameters.</typeparam>
        /// <param name="methodParameterInfo">The method parameter information.</param>
        /// <param name="seq">The seq.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        private delegate object GetParameterValueDelegate<TParameters>(ParameterInfo methodParameterInfo, int seq, TParameters parameters);

        /// <summary>
        /// Gets the parameter value by parameter array.
        /// </summary>
        /// <param name="methodParameterInfo">The method parameter information.</param>
        /// <param name="sequence">The sequence.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        private static object GetParameterValueByParameterArray(ParameterInfo methodParameterInfo, int sequence, object[] parameters)
        {
            object result = null;

            if (methodParameterInfo?.IsIn ?? false)
            {
                result = (parameters.Length > sequence && parameters[sequence] != null) ?
                           parameters[sequence] :
                           ((methodParameterInfo.IsOptional && methodParameterInfo.HasDefaultValue) ?
                               methodParameterInfo.DefaultValue :
                               methodParameterInfo.ParameterType.GetDefaultValue());
            }

            return result;
        }

        /// <summary>
        /// Gets the parameter value by json.
        /// </summary>
        /// <param name="methodParameterInfo">The method parameter information.</param>
        /// <param name="sequence">The sequence.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        private static object GetParameterValueByJson(ParameterInfo methodParameterInfo, int sequence, JObject parameters)
        {
            object result = null;

            if (methodParameterInfo?.IsIn ?? false)
            {
                result = parameters.SafeGetValue<object>(methodParameterInfo.Name, false);

                if (result == null)
                {
                    result = (methodParameterInfo.IsOptional && methodParameterInfo.HasDefaultValue) ? methodParameterInfo.DefaultValue : methodParameterInfo.ParameterType.GetDefaultValue();
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the parameter requirements.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public static List<MethodInvokeParameter> GetInvokeParameters(this MethodInfo method, params object[] parameters)
        {
            return GetInvokeParameters(method, parameters, GetParameterValueByParameterArray);
        }

        /// <summary>
        /// Gets the invoke parameters.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public static List<MethodInvokeParameter> GetInvokeParameters(this MethodInfo method, JObject parameters)
        {
            return GetInvokeParameters(method, parameters, GetParameterValueByJson);
        }

        /// <summary>
        /// Gets the invoke parameters.
        /// </summary>
        /// <typeparam name="TParameters">The type of the parameters.</typeparam>
        /// <param name="method">The method.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="getParameterValue">The get parameter value.</param>
        /// <returns></returns>
        private static List<MethodInvokeParameter> GetInvokeParameters<TParameters>(this MethodInfo method, TParameters parameters, GetParameterValueDelegate<TParameters> getParameterValue)
        {
            List<MethodInvokeParameter> result = new List<MethodInvokeParameter>();

            if (method != null)
            {
                var methodParameters = method.GetParameters();
                int sequence = 0;
                foreach (var one in methodParameters)
                {
                    result.Add(new MethodInvokeParameter
                    {
                        IsOptional = one.IsOptional,
                        Name = one.Name,
                        Type = one.ParameterType,
                        Sequence = sequence,
                        IsOut = one.IsOut,
                        IsIn = one.IsIn,
                        Value = getParameterValue(one, sequence, parameters)
                    });

                    sequence++;
                }
            }

            return result;
        }

        /// <summary>
        /// Invokes the static method.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public static MethodInvokeResult InvokeStaticMethod(this MethodInfo method, List<MethodInvokeParameter> parameters)
        {
            return InvokeMethod(method, null, parameters);
        }

        /// <summary>
        /// Invokes the method.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="instance">The instance.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public static MethodInvokeResult InvokeMethod(this MethodInfo method, object instance, List<MethodInvokeParameter> parameters)
        {
            MethodInvokeResult result = null;

            if (method != null)
            {
                result = new MethodInvokeResult();
                var methodParameters = parameters?.OrderBy(x => x.Sequence)?.ToArray() ?? new object[] { };

                try
                {
                    result.ReturnObject = method.Invoke(instance, methodParameters);

                    result.OutObjects = new Dictionary<string, object>();
                    foreach (var one in parameters.Where(x => x.IsOut))
                    {
                        result.OutObjects.Add(one.Name, methodParameters[one.Sequence]);
                    }
                }
                catch (Exception ex)
                {
                    result.Exception = ex.Handle(new { method = method?.GetFullName(), parameters });
                }
            }

            return result;
        }

        /// <summary>
        /// Invokes the method.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="instance">The instance.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public static MethodInvokeResult InvokeMethod(this MethodInfo method, object instance, params object[] parameters)
        {
            var invokeParameters = GetInvokeParameters(method, parameters);
            return InvokeMethod(method, instance, invokeParameters);
        }

        /// <summary>
        /// Invokes the method.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="instance">The instance.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public static MethodInvokeResult InvokeMethod(this MethodInfo method, object instance, JObject parameters)
        {
            var invokeParameters = GetInvokeParameters(method, parameters);
            return InvokeMethod(method, instance, invokeParameters);
        }

        #endregion Method Invoke
    }
}