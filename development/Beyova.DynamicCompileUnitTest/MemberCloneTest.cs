using System;

namespace Beyova.DynamicCompileUnitTest._MemberClone
{
    public class TestEntity
    {
        public int age;

        public DateTime Birthday { get; set; }

        public string Name { get; set; }

        public long Id { get; protected internal set; }

        public Gender Gender { get; set; }

        internal bool IsEngineer { get; set; }
    }

    public static class MemberCloneTest
    {
        public static void Test()
        {
            var item1 = new TestEntity
            {
                Name = "xx2",
                Birthday = new DateTime(1986, 1, 1),
                Gender = Gender.Male,
                age = 29,
                Id = 12233,
                IsEngineer = true
            };
            const string format = "{0} item1: {1}, item2: {2}";
            var item2 = new TestEntity();
            ObjectMemberClone.MemberShadowClone(item1, item2);
            Console.WriteLine(format, "Birthday", item1.Birthday, item2.Birthday);
            Console.WriteLine(format, "Name", item1.Name, item2.Name);
            Console.WriteLine(format, "Gender", item1.Gender, item2.Gender);
            Console.WriteLine(format, "Id", item1.Id, item2.Id);
            Console.WriteLine(format, "age", item1.age, item2.age);
            Console.WriteLine(format, "IsEngineer", item1.IsEngineer, item2.IsEngineer);
        }
    }
}