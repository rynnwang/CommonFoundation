using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Beyova.ProgrammingIntelligence;

namespace Beyova
{
    /// <summary>
    /// Class ObjectMemberCloneGenerator
    /// </summary>
    internal static class ObjectMemberCloneGenerator
    {
        /// <summary>
        /// The namespace
        /// </summary>
        private const string _namespace = "Beyova.ProgrammingIntelligence.ObjectMemberClone";

        /// <summary>
        /// The cloner instances
        /// </summary>
        private static Dictionary<Type, object> clonerInstances = new Dictionary<Type, object>();

        /// <summary>
        /// The locker
        /// </summary>
        private static object locker = new object();

        /// <summary>
        /// Creates the deep equality instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        internal static IObjectMemberClone<T> CreateMemberClone<T>()
        {
            Type type = typeof(T);
            object instance;
            if (!clonerInstances.TryGetValue(type, out instance))
            {
                lock (locker)
                {
                    if (!clonerInstances.TryGetValue(type, out instance))
                    {
                        var typeName = CodeGeneratorUtil.GenerateUniqueTypeName(type);
                        var codes = CreateMemberCloneClassCode(type, typeName);

                        TempAssemblyProvider provider = new TempAssemblyProvider();
                        var tmpAssembly = provider.CreateTempAssembly(codes, TempAssemblyProvider.GetCurrentAppDomainAssemblyLocations(), type.Namespace.AsArray(), @namespace: _namespace);
                        instance = tmpAssembly.CreateInstance(string.Format("{0}.{1}", _namespace, typeName));
                        clonerInstances.Add(type, instance);
                    }
                }
            }

            return instance as IObjectMemberClone<T>;
        }

        /// <summary>
        /// Fills the and terms.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder">The builder.</param>
        /// <param name="objects">The objects.</param>
        private static void FillSetTerms<T>(StringBuilder builder, IEnumerable<T> objects)
            where T : MemberInfo
        {
            if (objects.HasItem())
            {
                foreach (var one in objects)
                {
                    builder.AppendLineWithFormat("destination.{0} = source.{0};", one.Name);
                }
            }
        }

        /// <summary>
        /// Creates the member clone class code.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="typeName">Name of the type.</param>
        /// <returns></returns>
        private static string CreateMemberCloneClassCode(Type type, string typeName)
        {
            if (type != null)
            {
                var getProperties = type.GetInstanceFullyPublicAccessiableProperties();
                var getFields = type.GetInstanceFullyPublicAccessiableFields();

                StringBuilder builder = new StringBuilder(4096 + getProperties.Length * 256 + getFields.Length * 256);

                var memberCloneType = typeof(IObjectMemberClone<>);

                builder.AppendLineWithFormat("public class {0}: {1}", typeName, memberCloneType.MakeGenericType(type).ToCodeLook());
                builder.AppendBeginBrace();

                // Constructor
                builder.AppendLineWithFormat("public {0}()", typeName);
                builder.AppendLine("{");
                builder.AppendLine("}");

                builder.AppendLineWithFormat("public void ShadowClone({0} source, {0} destination)", type.ToCodeLook());
                builder.AppendBeginBrace();
                builder.AppendLine("if(source != null && destination != null)");
                builder.AppendBeginBrace();

                FillSetTerms(builder, getProperties);
                FillSetTerms(builder, getFields);

                builder.AppendEndBrace();
                builder.AppendEndBrace();

                // End of ObjectMemberClone
                builder.AppendEndBrace();

                return builder.ToString();
            }

            return string.Empty;
        }
    }
}