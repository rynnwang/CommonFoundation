using Beyova.ProgrammingIntelligence;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Beyova.Common.UnitTest
{
#if DEBUG
    [TestClass]
    public class ProgrammingIntelligenceUnitTest
    {
        public class TestClass
        {
            public int NormalMethod(int? x, int y)
            {
                return x.HasValue ? (x.Value + y) : 0;
            }

            public T GenericMethod<T>(int? x, int y)
                where T : class
            {
                return default(T);
            }
        }

        public class TestGenericClass<T>
            where T : class
        {
            public int NormalMethod(int? x, int y)
            {
                return x.HasValue ? (x.Value + y) : 0;
            }

            public T GenericMethod(int? x, int y)
            {
                return default(T);
            }
        }

        [TestMethod]
        public void CodeLookUnitTest()
        {
            TestDeclarationCodeLook();
            TestInvokeCodeLook();
        }

        private void TestDeclarationCodeLook()
        {
            Assert.AreEqual<string>(typeof(TestClass).GetMethod("NormalMethod").ToDeclarationCodeLook(), "System.Int32 NormalMethod(System.Nullable<System.Int32> x,System.Int32 y)");
            Assert.AreEqual<string>(typeof(TestClass).GetMethod("GenericMethod").ToDeclarationCodeLook(), "T GenericMethod<T>(System.Nullable<System.Int32> x,System.Int32 y)");

            Assert.AreEqual<string>(typeof(TestGenericClass<TestClass>).GetMethod("NormalMethod").ToDeclarationCodeLook(), "System.Int32 NormalMethod(System.Nullable<System.Int32> x,System.Int32 y)");
            Assert.AreEqual<string>(typeof(TestGenericClass<TestClass>).GetMethod("GenericMethod").ToDeclarationCodeLook(), typeof(TestClass).GetFullName() + " GenericMethod(System.Nullable<System.Int32> x,System.Int32 y)");
        }

        private void TestInvokeCodeLook()
        {
            Assert.AreEqual<string>(typeof(TestClass).GetMethod("NormalMethod").ToInvokeCodeLook(null, new string[2] { "x", "y" }), "NormalMethod(x,y)");
            Assert.AreEqual<string>(typeof(TestClass).GetMethod("GenericMethod").ToInvokeCodeLook(typeof(TestClass).AsArray(), new string[2] { "x", "y" }), "GenericMethod<"+ typeof(TestClass).GetFullName() + ">(x,y)");
        }
    }
#endif
}