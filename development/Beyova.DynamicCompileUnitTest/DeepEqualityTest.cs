using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beyova;

namespace Beyova.DynamicCompileUnitTest._DeepEquality
{
    public class TestEntity
    {
        public string Name { get; set; }

        public string Name2 { get { return string.Format("{0}.1", this.Name); } }

        public string Birthday;

        internal string Description;

        [DeepEqualityIgnore]
        public int IgnoreProperty { get; set; }
    }

    public static class DeepEqualityTest
    {
        public static void Test()
        {
            var item1 = new TestEntity
            {
                Name = "xx2",
                Birthday = "1986",
                Description = "ddd",
                IgnoreProperty = 1
            };

            var item2 = new TestEntity
            {
                Name = "xX2",
                Birthday = "1986",
                Description = "ddd",
                IgnoreProperty = 2
            };

            Console.WriteLine(DeepEquality.DeepEquals(item1, item2, StringComparison.OrdinalIgnoreCase));

            Console.WriteLine(DeepEquality.DeepEquals(item1, item2, StringComparison.Ordinal));
        }
    }
}