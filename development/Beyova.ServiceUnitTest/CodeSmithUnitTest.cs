using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beyova.CodeSmith;
using Beyova.ExceptionSystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace Beyova.Common.UnitTest
{
    [TestClass]
    public class CodeSmithUnitTest
    {
        [TestMethod]
        public void SandboxInvokerUnitTest()
        {
            string typeFullName = typeof(ObjectiveCCodeGenerator).GetFullName();
            object[] constructorParameters = (new ObjectiveCCodeGeneratorSetting
            {
                CodeFileIdentifier = "BA",
                ClassPrefix = "PI",
            } as object).AsArray();

            string methodName = "GenerateAsZip";
            object[] methodParameters = null;

            SandboxMarshalInvokeResult result = new SandboxMarshalInvokeResult();

            try
            {
                typeFullName.CheckEmptyString(nameof(typeFullName));
                methodName.CheckEmptyString(nameof(methodName));

                var type = ReflectionExtension.SmartGetType(typeFullName);
                type.CheckNullObject(nameof(type));

                var instance = Activator.CreateInstance(type, constructorParameters);

                var methodInfo = type.GetMethod(methodName);
                methodInfo.CheckNullObject(nameof(methodInfo));

                if (methodInfo.IsStatic)
                {
                    throw new InvalidObjectException(nameof(methodName), data: methodName, reason: "Method is static");
                }

                result.SetValue(methodInfo.Invoke(instance, methodParameters));
            }
            catch (Exception ex)
            {
                result.SetException(ex.Handle(new { typeFullName, constructorParameters, methodName, methodParameters }));
            }

            Assert.IsNotNull(result.Value);
        }

        [TestMethod]
        public void SandboxAndObjectiveC()
        {
            Dictionary<string, byte[]> assemblyBytes = new Dictionary<string, byte[]>();

            foreach (var one in new string[] {
                typeof(Framework).Assembly.Location,
                typeof(JToken).Assembly.Location,
            })
            {
                assemblyBytes.Add(Path.GetFileNameWithoutExtension(one), File.ReadAllBytes(one));
            }

            var key = WorkshopManager.InitializeWorkshop(assemblyBytes);

            Assert.IsNotNull(key);

            var workshop = WorkshopManager.GetWorkshop(key);
            Assert.IsNotNull(workshop);

            var interfaces = workshop.GetInterfaces();
            Assert.IsNotNull(interfaces);
            Assert.IsTrue(interfaces.Any());

            var zipByte = workshop.GenerateObjectiveCCodeAsZip(new ObjectiveCCodeGeneratorSetting
            {
                CodeFileIdentifier = "BA",
                ClassPrefix = "PI",
            });
            Assert.IsNotNull(zipByte);

            File.WriteAllBytes(@"D:\pi_output.zip", zipByte);
        }
    }
}
