using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wordle;

namespace IntegrationTests
{
    [TestClass]
    public class FilterTests
    {
        [TestMethod]
        public void SolverReturnWordWhenWholeWordIsPassed2()
        {
            WordleSolver _solver = new(new Dictionary<string, float> { { "coucou", 0 } });
            Assert.IsFalse(_solver.Filter("", "......").ContainsKey("coucou"));
        }

        [TestMethod]
        public void SolverReturnWordWhenWholeWordIsPassed()
        {
            WordleSolver _solver = new(new Dictionary<string, float> {{"coucou", 0}});
            Assert.IsTrue(  _solver.Filter("coucou", "!!!!!!").ContainsKey("coucou"));
        }

        //[TestMethod]
        //public void SolverReturnErrorWhenStringIsEmpty()
        //{
        //    WordleSolver _solver = new(new Dictionary<string, float> { });
        //    Assert.ThrowsException<ArgumentNullException>(() => _solver.Filter("",""));
        //}

        [TestMethod]
        public void SolverDictionaryWordWhenThereIsOnlyOneWord()
        {
            WordleSolver _solver = new(new Dictionary<string, float> {{"coucou", 0}});
            Assert.IsTrue(_solver.Filter("coucou","!!!!!!").ContainsKey("coucou"));
        }

        [TestMethod]
        public void ReturnNullIfNoWordCorrespond()
        {
            WordleSolver _solver = new(new Dictionary<string, float> {{"toucan", 0}});
            Assert.AreEqual(0, _solver.Filter("coucou", "!.....").Count());
        }

        [TestMethod]
        public void ReturnAllOccurenceOfCorrectStringWhenAllMatch()
        {
            WordleSolver _solver = new(new Dictionary<string, float> {{"toucan", 0}, {"coucou", 1}});
            var enumerable = _solver.Filter("boubbb", ".!!...");
            Assert.IsTrue(enumerable.ContainsKey("coucou"));
            Assert.IsTrue(enumerable.ContainsKey("toucan"));
        }

        [TestMethod]
        public void ReturnAllOccurenceOfCorrectStringWhenNotAllMatch()
        {
            WordleSolver _solver =
                new(new Dictionary<string, float> { {"toucan", 0}, {"coucou", 1}, {"ehbahnon", 0}, {"couchera", 1}});

            var enumerable = _solver.Filter("boubbb", ".!!...");
            Assert.IsTrue(enumerable.ContainsKey("coucou"));
            Assert.IsTrue(enumerable.ContainsKey("toucan"));
            Assert.AreEqual(2, enumerable.Count);
        }

        [TestMethod]
        public void SplitWorks()
        {
            WordleSolver _solver =
                new(new Dictionary<string, float> { { "toucan", 0 }, { "coucou", 1 }, { "ehbahnon", 0 }, { "couchera", 1 } });

            var enumerable = _solver.Filter("doucat", ".!!!!?");
            Assert.IsTrue(enumerable.ContainsKey("toucan"));
            Assert.AreEqual(1, enumerable.Count);
        }

        [TestMethod]
        public void SplitWorks2()
        {
            WordleSolver _solver =
                new(new Dictionary<string, float> { { "toucan", 0 }, { "coucou", 1 }, { "ehbahnon", 0 }, { "couchera", 1 } });

            var enumerable = _solver.Filter("coucot", ".!!!.?");
            Assert.IsTrue(enumerable.ContainsKey("toucan"));
            Assert.AreEqual(1, enumerable.Count);
        }

        [TestMethod]
        public void MinusWork()
        {
            WordleSolver _solver =
                new(new Dictionary<string, float> { { "toucan", 0 }, { "coucou", 1 }, { "ehbahnon", 0 }, { "couchera", 1 } });

            var enumerable = _solver.Filter("coucot", "!!!!!.");
            Assert.IsTrue(enumerable.ContainsKey("coucou"));
            Assert.AreEqual(1, enumerable.Count);
        }

        [TestMethod]
        public void RedGreen()
        {
            WordleSolver _solver = new(new Dictionary<string, float> { { "vivre", 0 }, { "livre", 0 } , { "givre", 0 } });
            Assert.IsFalse(_solver.Filter("vivre", ".!!!!").ContainsKey("vivre"));
        }

        [TestMethod]
        public void YellowGreen()
        {
            WordleSolver _solver = new(new Dictionary<string, float> { { "tivrv", 0 }, { "livre", 0 }, { "givre", 0 } });
            Assert.IsTrue(_solver.Filter("vivre", "?!!!!").ContainsKey("tivrv"));
        }

        [TestMethod]
        public void RedYellowGreen()
        {
            WordleSolver _solver = new(new Dictionary<string, float> { { "eayieu", 0 }, { "livre", 0 }, { "givre", 0 } });
            Assert.IsTrue(_solver.Filter("eetete", "!?!!.").ContainsKey("eayieu"));
        }

        //[TestMethod]
        //public void SolverReturnWordWhenWholeWordIsPassed4()
        //{
        //    WordleSolver _solver = new(new Dictionary<string, float> { { "admission", 0 } });
        //    Assert.IsTrue(_solver.Filter("imaginais", "???.??..?").ContainsKey("admission"));
        //}
    }
}