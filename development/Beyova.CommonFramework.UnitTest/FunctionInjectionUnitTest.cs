using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Beyova.CommonFramework.UnitTest
{
    /// <summary>
    /// 
    /// </summary>
    [FunctionInjectionHostTypeMap(typeof(TestInjectionContainer))]
    public static class TestInjectionHost
    {
        /// <summary>
        /// Tests the injection method.
        /// </summary>
        /// <returns></returns>        
        [FunctionInjectionMap(nameof(TestInjectionContainer.TestInjectionMethodContainer))]
        public static int TestInjectionMethod()
        {
            return 12;
        }
    }

    public static class TestInjectionContainer
    {
        /// <summary>
        /// Tests the injection method.
        /// </summary>
        /// <returns></returns>        
        public static ParameterlessPrioritizedFunctionInjection<int> TestInjectionMethodContainer { get; private set; } = new ParameterlessPrioritizedFunctionInjection<int>();
    }

    [TestClass]
    public class FunctionInjectionUnitTest
    {
        [TestMethod]
        public void TestFunctionInjection()
        {
           // FunctionInjectionController.ApplyInjectionByAttribute(typeof(TestInjectionHost));

            //var method = typeof(TestInjectionHost).GetMethod(nameof(TestInjectionHost.TestInjectionMethod), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);

            //if ((method.GetParameters()?.Length ?? 0) == 0
            //    && !(method.ReturnType.IsVoid() ?? false))
            //{
            //    var delegateObject = method.CreateDelegate(typeof(ParameterlessFunctionInjection<>).MakeGenericType(method.ReturnType));

            //    var attachMethod = typeof(FunctionInjectionController).GetMethod(nameof(FunctionInjectionController.ApplyInjection), System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);

            //    var attachMethodToInvoke = attachMethod.MakeGenericMethod(method.ReturnType);
            //    attachMethodToInvoke.Invoke(null,
            //        new object[] {
            //        //Type staticType
            //        typeof(TestInjectionContainer),
            //        //string propertyName
            //        nameof(TestInjectionContainer.TestInjectionMethodContainer),
            //        //ParameterlessFunctionInjection< T > injectionCandidate
            //        delegateObject,
            //        // bool expectAsLowPriority
            //        false
            //        });

            Assert.AreEqual((int)TestInjectionContainer.TestInjectionMethodContainer, TestInjectionHost.TestInjectionMethod());
            //}

            //                if (methods.HasItem())
            //                {
            //                    foreach (var item in methods)
            //                    {
            //                        if (item != null
            //                            && (item.GetParameters()?.Length ?? 0) == 0
            //                            && !(item.ReturnType.IsVoid() ?? false))
            //                        {
            //                            item.CreateDelegate(item.ReturnType);
            //                        }
            //                    }
            //                }
        }
    }
}
