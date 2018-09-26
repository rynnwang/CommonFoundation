//using Microsoft.VisualStudio.TestTools.UnitTesting;

///// <summary>
/////
///// </summary>
//namespace Beyova.Common.UnitTest
//{
//    public class TestBaseClass
//    {
//        public void MethodOk()
//        {
//        }
//    }

//    public class TestClass : TestBaseClass, ITestClass
//    {
//        public void Method(int x = 2)
//        {
//            throw ExceptionFactory.CreateInvalidObjectException(nameof(x), x);
//        }
//    }

//    public interface ITestClass
//    {
//        void Method(int x = 2);
//    }

//    [TestClass]
//    public class AopProxyUnitTest
//    {
//        [TestMethod]
//        public void TestAop()
//        {
//            var proxy = AOP.AopProxyFactory.AsAopInterfaceProxy<TestClass>(new TestClass()) as ITestClass;

//            proxy.CheckNullObject(nameof(proxy));

//            proxy.Method();
//        }
//    }
//}