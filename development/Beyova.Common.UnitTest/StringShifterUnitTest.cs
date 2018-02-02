using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Beyova.Common.UnitTest
{
    [TestClass]
    public class StringShifterUnitTest
    {
        [TestMethod]
        public void ConvertFormatToShiftShardTest()
        {
            string format = "Its name is {name}.";
            var shiftShards = format.ConvertFormatToShiftShard();

            Assert.AreEqual(format, shiftShards.ToString());

            format = "{name}";
            shiftShards = format.ConvertFormatToShiftShard();

            Assert.AreEqual(format, shiftShards.ToString(new Dictionary<string, string> { { "name", "{name}" } }));

            format = "{name}:{id}";
            shiftShards = format.ConvertFormatToShiftShard();

            Assert.AreEqual(format, shiftShards.ToString(new Dictionary<string, string> { { "name", "{name}" }, { "id", "{id}" } }));
        }

        [TestMethod]
        public void ConvertFormatToRegexTest()
        {
            string format = "/api/{version}/{resource}/";
            var constraints = new Dictionary<string, string>();
            constraints.Add("version", "[0-9a-zA-Z]+");
            constraints.Add("resource", ".*");

            var regex = format.ConvertFormatToRegex(constraints);

            var match = regex.Match("/api/v1/stringshift/");

            Assert.IsNotNull(match);
            Assert.IsTrue(match.Success);
            Assert.AreEqual(match.Result("${version}"), "v1");
            Assert.AreEqual(match.Result("${resource}"), "stringshift");

            match = regex.Match("/api/v_1/stringshift/");
            Assert.IsNotNull(match);
            Assert.IsFalse(match.Success);
        }

        [TestMethod]
        public void StringShiftTest()
        {
            StringShifter shifter = new StringShifter("http://{feature}.test.com/{id}", "http://test.com/api/v1/{feature}/{id}/", new Dictionary<string, string> { { "id", "[0-9A-Za-z]+" } });
            string source = "http://nav.test.com/nav1234/";
            string expected = "http://test.com/api/v1/nav/nav1234/";

            var result = shifter.Shift(source);

            Assert.AreEqual(expected, result);

            shifter = new StringShifter("http://{feature}.test.com/", "http://test.com/api/v1/{feature}/?id={id}");
            expected = "http://test.com/api/v1/nav/?id=${id}";

            result = shifter.Shift(source);

            Assert.AreEqual(expected, result);
        }
    }
}