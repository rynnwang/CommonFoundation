//using System;
//using Beyova.Utility;
//using Microsoft.VisualStudio.TestTools.UnitTesting;

//namespace Beyova.Common.UnitTest
//{
//    [TestClass]
//    public class SentenceAnalyzerUnitTest
//    {
//        [TestMethod]
//        public void FixWrapWordUnitTest()
//        {
//            string text = @"This is a pen-  
//cil.";

//            var result = SentenceAnalyzer.FixWrapWord(text);
//            Assert.AreEqual(result, "This is a pencil.");
//        }

//        [TestMethod]
//        public void SentenceAnalyzerTokenUnitTest()
//        {
//            string text = "John said, \"he had been there before, a few years ago\", but now he looks forgetful, painful, etc..";

//            var analyzer = new SentenceAnalyzer();
//            var tokens = analyzer.ConvertToTokens(text);
//            Assert.IsNotNull(tokens);
//        }

//    }
//}
