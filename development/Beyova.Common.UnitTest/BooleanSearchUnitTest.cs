using System;
using Beyova.BooleanSearch;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace Beyova.Common.UnitTest
{
    [TestClass]
    public class BooleanSearchUnitTest
    {
        private JObject basicObject = JObject.FromObject(new
        {
            IntValue = 13,
            DoubleValue = 11.24,
            DateValue = new DateTime(2016, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            IntArrayValue = new int[] { 1, 2, 3, 4, 5 },
            StringArrayValue = new string[] { "a", "b", "c", "d", "e" },
            StringValue = "#Hello World!",
            ObjectValue = new
            {
                PropertyA = 1,
                PropertyB = "2"
            }
        });

        [TestMethod]
        public void TestCriteria()
        {
            var intCriteria = new CriteriaOperatorComputable
            {
                Item1 = "IntValue",
                Operator = ComputeOperator.Equals,
                Item2 = "13"
            };

            Assert.IsTrue(intCriteria.Compute(basicObject));

            intCriteria.Operator = ComputeOperator.GreatThan;
            intCriteria.Item2 = "14";
            Assert.IsFalse(intCriteria.Compute(basicObject));

            intCriteria.Item2 = "12";
            Assert.IsTrue(intCriteria.Compute(basicObject));

            //double
            var doubleCriteria = new CriteriaOperatorComputable
            {
                Item1 = "DoubleValue",
                Operator = ComputeOperator.Equals,
                Item2 = "11.24"
            };

            Assert.IsTrue(doubleCriteria.Compute(basicObject));

            doubleCriteria.Operator = ComputeOperator.GreatThan;
            doubleCriteria.Item2 = "12";
            Assert.IsFalse(doubleCriteria.Compute(basicObject));

            doubleCriteria.Item2 = "11";
            Assert.IsTrue(doubleCriteria.Compute(basicObject));

            //datetime
            var dateTimeCriteria = new CriteriaOperatorComputable
            {
                Item1 = "DateValue",
                Operator = ComputeOperator.Equals,
                Item2 = "2016-01-01 0:00:00"
            };

            Assert.IsTrue(dateTimeCriteria.Compute(basicObject));

            dateTimeCriteria.Operator = ComputeOperator.GreatThan;
            dateTimeCriteria.Item2 = "2016-1-1 1:00";
            Assert.IsFalse(dateTimeCriteria.Compute(basicObject));

            dateTimeCriteria.Item2 = "2015-12-1";
            Assert.IsTrue(dateTimeCriteria.Compute(basicObject));

            //StringArray
            var stringArrayCriteria = new CriteriaOperatorComputable
            {
                Item1 = "StringArrayValue",
                Operator = ComputeOperator.Contains,
                Item2 = "a"
            };

            Assert.IsTrue(stringArrayCriteria.Compute(basicObject));

            //int array
            var intArrayCriteria = new CriteriaOperatorComputable
            {
                Item1 = "IntArrayValue",
                Operator = ComputeOperator.Contains,
                Item2 = "2"
            };

            Assert.IsTrue(intArrayCriteria.Compute(basicObject));

            //string
            var stringCriteria = new CriteriaOperatorComputable
            {
                Item1 = "StringValue",
                Operator = ComputeOperator.Contains,
                Item2 = "llo"
            };

            Assert.IsTrue(stringCriteria.Compute(basicObject));

            stringCriteria.Operator = ComputeOperator.StartWith;
            stringCriteria.Item2 = "#";
            Assert.IsTrue(stringCriteria.Compute(basicObject));

            stringCriteria.Operator = ComputeOperator.EndWith;
            stringCriteria.Item2 = "!";
            Assert.IsTrue(stringCriteria.Compute(basicObject));

            //object
            var objectCriteria = new CriteriaOperatorComputable
            {
                Item1 = "ObjectValue",
                Operator = ComputeOperator.Exists,
                Item2 = "PropertyA"
            };

            Assert.IsTrue(objectCriteria.Compute(basicObject));

            objectCriteria.Item2 = "PropertyC";
            Assert.IsFalse(objectCriteria.Compute(basicObject));
        }

        [TestMethod]
        public void TestRelationship()
        {
        }

        [TestMethod]
        public void TestExpressionCast()
        {
            string expression = "(IntValue=13) AND (DoubleValue> '11')";

            BooleanSearchExpressionReader reader = new BooleanSearchExpressionReader();
            var expressionObject = reader.ReadAsObject(expression);

            Assert.IsNotNull(expressionObject);

            var expressionString = expressionObject.ToString();
            Assert.IsNotNull(expressionString);

            var computeResult = expressionObject.Compute(basicObject);
            Assert.IsTrue(computeResult);
        }
    }
}