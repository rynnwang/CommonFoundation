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
                ItemLeft = "IntValue",
                Operator = ComputeOperator.Equals,
                ItemRight = "13"
            };

            Assert.IsTrue(intCriteria.Compute(basicObject));

            intCriteria.Operator = ComputeOperator.GreatThan;
            intCriteria.ItemRight = "14";
            Assert.IsFalse(intCriteria.Compute(basicObject));

            intCriteria.ItemRight = "12";
            Assert.IsTrue(intCriteria.Compute(basicObject));

            //double
            var doubleCriteria = new CriteriaOperatorComputable
            {
                ItemLeft = "DoubleValue",
                Operator = ComputeOperator.Equals,
                ItemRight = "11.24"
            };

            Assert.IsTrue(doubleCriteria.Compute(basicObject));

            doubleCriteria.Operator = ComputeOperator.GreatThan;
            doubleCriteria.ItemRight = "12";
            Assert.IsFalse(doubleCriteria.Compute(basicObject));

            doubleCriteria.ItemRight = "11";
            Assert.IsTrue(doubleCriteria.Compute(basicObject));

            //datetime
            var dateTimeCriteria = new CriteriaOperatorComputable
            {
                ItemLeft = "DateValue",
                Operator = ComputeOperator.Equals,
                ItemRight = "2016-01-01 0:00:00"
            };

            Assert.IsTrue(dateTimeCriteria.Compute(basicObject));

            dateTimeCriteria.Operator = ComputeOperator.GreatThan;
            dateTimeCriteria.ItemRight = "2016-1-1 1:00";
            Assert.IsFalse(dateTimeCriteria.Compute(basicObject));

            dateTimeCriteria.ItemRight = "2015-12-1";
            Assert.IsTrue(dateTimeCriteria.Compute(basicObject));

            //StringArray
            var stringArrayCriteria = new CriteriaOperatorComputable
            {
                ItemLeft = "StringArrayValue",
                Operator = ComputeOperator.Contains,
                ItemRight = "a"
            };

            Assert.IsTrue(stringArrayCriteria.Compute(basicObject));

            //int array
            var intArrayCriteria = new CriteriaOperatorComputable
            {
                ItemLeft = "IntArrayValue",
                Operator = ComputeOperator.Contains,
                ItemRight = "2"
            };

            Assert.IsTrue(intArrayCriteria.Compute(basicObject));

            //string
            var stringCriteria = new CriteriaOperatorComputable
            {
                ItemLeft = "StringValue",
                Operator = ComputeOperator.Contains,
                ItemRight = "llo"
            };

            Assert.IsTrue(stringCriteria.Compute(basicObject));

            stringCriteria.Operator = ComputeOperator.StartWith;
            stringCriteria.ItemRight = "#";
            Assert.IsTrue(stringCriteria.Compute(basicObject));

            stringCriteria.Operator = ComputeOperator.EndWith;
            stringCriteria.ItemRight = "!";
            Assert.IsTrue(stringCriteria.Compute(basicObject));

            //object
            var objectCriteria = new CriteriaOperatorComputable
            {
                ItemLeft = "ObjectValue",
                Operator = ComputeOperator.Exists,
                ItemRight = "PropertyA"
            };

            Assert.IsTrue(objectCriteria.Compute(basicObject));

            objectCriteria.ItemRight = "PropertyC";
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