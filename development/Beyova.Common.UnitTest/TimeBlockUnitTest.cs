using System;
using System.Linq;
using Beyova.ExceptionSystem;
using Beyova.Scheduling;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Beyova.Common.UnitTest
{
    [TestClass]
    public class TimeBlockUnitTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            TimeBlockSize min5 = new TimeBlockSize(5);
            TimeBlockSize min30 = new TimeBlockSize(30);
            TimeBlockSize min1440 = new TimeBlockSize(1440);

            try
            {
                TimeBlockSize min117 = new TimeBlockSize(117);
            }
            catch (Exception ex)
            {
                Assert.AreEqual("OutOfRange", (ex as BaseException)?.Code.Minor);
            }

            try
            {
                TimeBlockSize min1441 = new TimeBlockSize(1441);
            }
            catch (Exception ex)
            {
                Assert.AreEqual("OutOfRange", (ex as BaseException)?.Code.Minor);
            }
        }

        [TestMethod]
        public void TestBlockHit_SingleDay()
        {
            TimeBlockSize min30 = new TimeBlockSize(30);
            var sample = new SchedulingResourceDimensionAllocationIndex { DimensionCode = "DemensionCode" };
            var timeRange = new TimeRange(new DateTime(1986, 1, 3, 2, 0, 0), new DateTime(1986, 1, 3, 3, 30, 0));

            var blocks = min30.DetectBlockHit(timeRange, sample);

            // Check sample
            foreach (var one in blocks)
            {
                Assert.AreEqual("DemensionCode", (one as SchedulingResourceDimensionAllocationIndex)?.DimensionCode);
                Assert.AreEqual(new Date(1986, 1, 3), one.UtcDate);
            }

            Assert.AreEqual(3, blocks.Count());

            for (var i = 5; i <= 7; i++)
            {
                Assert.IsTrue(blocks.Where(x => x.DayTimeBlockIndex == i).Any());
            }
        }

        [TestMethod]
        public void TestBlockHit_SingleDay_UnfullBlock()
        {
            TimeBlockSize min30 = new TimeBlockSize(30);
            var sample = new SchedulingResourceDimensionAllocationIndex { DimensionCode = "DemensionCode" };
            var timeRange = new TimeRange(new DateTime(1986, 1, 3, 2, 15, 0), new DateTime(1986, 1, 3, 3, 15, 0));

            var blocks = min30.DetectBlockHit(timeRange, sample);

            // Check sample
            foreach (var one in blocks)
            {
                Assert.AreEqual("DemensionCode", (one as SchedulingResourceDimensionAllocationIndex)?.DimensionCode);
                Assert.AreEqual(new Date(1986, 1, 3), one.UtcDate);
            }

            Assert.AreEqual(3, blocks.Count());

            for (var i = 5; i <= 7; i++)
            {
                Assert.IsTrue(blocks.Where(x => x.DayTimeBlockIndex == i).Any());
            }
        }

        [TestMethod]
        public void TestBlockHit_MultipleDays()
        {
            TimeBlockSize min30 = new TimeBlockSize(30);
            var sample = new SchedulingResourceDimensionAllocationIndex { DimensionCode = "DemensionCode" };
            var timeRange = new TimeRange(new DateTime(1986, 1, 3, 2, 0, 0), new DateTime(1986, 1, 5, 3, 30, 0));

            var blocks = min30.DetectBlockHit(timeRange, sample);

            // Check sample
            foreach (var one in blocks)
            {
                Assert.AreEqual("DemensionCode", (one as SchedulingResourceDimensionAllocationIndex)?.DimensionCode);
            }

            for (var i = 0; i < 44; i++)
            {
                Assert.AreEqual(new Date(1986, 1, 3), blocks[i].UtcDate);
            }

            for (var i = 44; i < 44 + 48; i++)
            {
                Assert.AreEqual(new Date(1986, 1, 4), blocks[i].UtcDate);
            }

            for (var i = 44 + 48; i < 99; i++)
            {
                Assert.AreEqual(new Date(1986, 1, 5), blocks[i].UtcDate);
            }

            Assert.AreEqual((48 - 4) + 48 + 7, blocks.Count());
        }

        [TestMethod]
        public void TestBlockHit_MultipleDays_UnfullBlock()
        {
            TimeBlockSize min30 = new TimeBlockSize(30);
            var sample = new SchedulingResourceDimensionAllocationIndex { DimensionCode = "DemensionCode" };
            var timeRange = new TimeRange(new DateTime(1986, 1, 3, 2, 15, 0), new DateTime(1986, 1, 5, 3, 15, 0));

            var blocks = min30.DetectBlockHit(timeRange, sample);

            // Check sample
            foreach (var one in blocks)
            {
                Assert.AreEqual("DemensionCode", (one as SchedulingResourceDimensionAllocationIndex)?.DimensionCode);
            }

            for (var i = 0; i < 44; i++)
            {
                Assert.AreEqual(new Date(1986, 1, 3), blocks[i].UtcDate);
            }

            for (var i = 44; i < 44 + 48; i++)
            {
                Assert.AreEqual(new Date(1986, 1, 4), blocks[i].UtcDate);
            }

            for (var i = 44 + 48; i < 99; i++)
            {
                Assert.AreEqual(new Date(1986, 1, 5), blocks[i].UtcDate);
            }

            Assert.AreEqual((48 - 4) + 48 + 7, blocks.Count());
        }
    }
}