using System;
using System.Linq;
using Beyova;
using Beyova.AOP;

namespace Beyova.DynamicCompileUnitTest._Aop
{
    public class TestBaseClass
    {
        public void MethodOk()
        {
        }
    }

    public class TestClass : TestBaseClass, ITestClass
    {
        public void Method(int x = 2)
        {
            throw ExceptionFactory.CreateOperationException(x);
        }
    }

    public interface ITestClass
    {
        void Method(int x = 2);
    }

    public static class AopTest
    {
        /// <summary>
        /// Basics the function test.
        /// </summary>
        private static void BasicFunctionTest()
        {
            try
            {
                var proxy = Beyova.AOP.AopProxyFactory.AsAopInterfaceProxy<TestClass>(new TestClass()) as ITestClass;

                proxy.CheckNullObject(nameof(proxy));

                proxy.Method();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Advanceds the function test.
        /// </summary>
        private static void AdvancedFunctionTest()
        {
            try
            {
                var proxy = Beyova.AOP.AopProxyFactory.AsAopInterfaceProxy<TestClass>(new TestClass(), new MethodInjectionDelegates
                {
                    MethodInvokingEvent = (x) =>
                    {
                        Console.WriteLine("Before invoke {0}, arguments:", x.MethodFullName);

                        foreach (var one in x.InArgs)
                        {
                            Console.WriteLine("\t{0} - Serialiazable: {1}", one.Key, x.SerializableArgNames.Contains(one.Key));
                        }

                        Console.WriteLine("Injection completed.");
                        Console.WriteLine("-------------------------------");
                    },

                    MethodInvokedEvent = (x) =>
                    {
                        Console.WriteLine("After invoke {0}", x.MethodFullName);
                        Console.WriteLine("Exception: {0}", x?.Exception.ToExceptionInfo().ToJson());
                        Console.WriteLine("Injection completed.");
                    }
                }) as ITestClass;

                proxy.CheckNullObject(nameof(proxy));

                proxy.Method();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}