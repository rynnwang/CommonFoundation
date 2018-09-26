using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Beyova
{
    /// <summary>
    /// 
    /// </summary>
    public static class FunctionInjectionController
    {
        /// <summary>
        /// The is discovered
        /// </summary>
        static bool isDiscovered = false;

        /// <summary>
        /// Automatics the discover function injection. No worries, it would only be run once.
        /// </summary>
        public static void AutoDiscoverFunctionInjection()
        {
            if (!isDiscovered)
            {
                foreach (var ass in EnvironmentCore.AscendingAssemblyDependencyChain)
                {
                    foreach (var item in ass.GetTypes())
                    {
                        // Funny, .NET use IsAbstract + IsSealed to indicates IsStatic
                        if (item.IsClass && item.IsAbstract && item.IsSealed)
                        {
                            ApplyInjectionByAttribute(item);
                        }
                    }
                }

                isDiscovered = true;
            }
        }

        /// <summary>
        /// Initializes the <see cref="FunctionInjectionController"/> class.
        /// </summary>
        static FunctionInjectionController()
        {
            AutoDiscoverFunctionInjection();
        }

        /// <summary>
        /// Applies the injection by attribute.
        /// </summary>
        /// <param name="sourceHostType">Type of the source host.</param>
        private static void ApplyInjectionByAttribute(Type sourceHostType)
        {
            if (sourceHostType != null)
            {
                var hostMapInfo = sourceHostType.GetCustomAttribute<FunctionInjectionHostTypeMapAttribute>();

                if (hostMapInfo != null)
                {
                    var targetContainerType = hostMapInfo.TargetStaticType;
                    var methods = sourceHostType.GetMethodInfoWithinAttribute<FunctionInjectionMapAttribute>(bindingFlags: System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);

                    if (methods.HasItem())
                    {
                        foreach (var item in methods)
                        {
                            if (item != null
                                && (item.GetParameters()?.Length ?? 0) == 0
                                && !(item.ReturnType.IsVoid() ?? false))
                            {
                                var methodMapInfo = item.GetCustomAttribute<FunctionInjectionMapAttribute>();
                                var targetProperty = string.IsNullOrWhiteSpace(methodMapInfo.TargetName) ? null : targetContainerType.GetProperty(methodMapInfo.TargetName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

                                if (targetProperty != null)
                                {
                                    // Do attach
                                    var delegateObject = item.CreateDelegate(typeof(ParameterlessFunctionInjection<>).MakeGenericType(item.ReturnType));

                                    var attachMethod = typeof(FunctionInjectionController).GetMethod(nameof(FunctionInjectionController.ApplyInjection), System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);

                                    var attachMethodToInvoke = attachMethod.MakeGenericMethod(item.ReturnType);

                                    //// See <see cref="FunctionInjectionController.ApplyInjection{T}(Type, string, ParameterlessFunctionInjection{T}, bool)"/>
                                    attachMethodToInvoke.Invoke(null,
                                        new object[] {
                                        //Type staticType
                                        targetContainerType,
                                        //string propertyName
                                        targetProperty.Name,
                                        //ParameterlessFunctionInjection< T > injectionCandidate
                                        delegateObject,
                                        // bool expectAsLowPriority
                                        methodMapInfo.ExpectAsLowPriority
                                    });
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Applies the injection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="staticType">Type of the static.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="injectionCandidate">The injection candidate.</param>
        /// <param name="expectAsLowPriority">if set to <c>true</c> [expect as low priority].</param>
        public static void ApplyInjection<T>(Type staticType, string propertyName, ParameterlessFunctionInjection<T> injectionCandidate, bool expectAsLowPriority = false)
        {
            if (staticType != null && !string.IsNullOrWhiteSpace(propertyName) && injectionCandidate != null)
            {
                var hitProperty = staticType.GetProperties(BindingFlags.Static | BindingFlags.Public | BindingFlags.GetProperty).FirstOrDefault(x => x.Name == propertyName);
                if (hitProperty != null)
                {
                    if (typeof(ParameterlessPrioritizedFunctionInjection<T>).IsAssignableFrom(hitProperty.PropertyType))
                    {
                        ParameterlessPrioritizedFunctionInjection<T> existed = hitProperty.GetValue(null) as ParameterlessPrioritizedFunctionInjection<T>;
                        if (existed == null)
                        {
                            if (hitProperty.SetMethod != null)
                            {
                                existed = new ParameterlessPrioritizedFunctionInjection<T>();
                                hitProperty.SetValue(null, existed);
                            }
                            else
                            {
                                throw ExceptionFactory.CreateInvalidObjectException(hitProperty?.Name, new { type = staticType?.FullName, property = hitProperty?.Name });
                            }
                        }

                        if (expectAsLowPriority)
                        {
                            existed.Append(injectionCandidate);
                        }
                        else
                        {
                            existed.Prepend(injectionCandidate);
                        }
                    }
                    else if (hitProperty.PropertyType == typeof(ParameterlessFunctionInjection<>).MakeGenericType(typeof(T)))
                    {
                        hitProperty.SetValue(null, injectionCandidate);
                    }
                }
            }
        }
    }
}
