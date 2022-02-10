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
        //[TestMethod]
        //public void SolverReturnWordWhenWholeWordIsPassed2()
        //{
        //    WordleSolver _solver = new(new Dictionary<string, float> { { "coucou", 0 } });
        //    Assert.IsFalse(_solver.Filter("", "......").ContainsKey("coucou"));
        //}

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
            WordleSolver _solver = new(new Dictionary<string, float> { { "tivrv", 0 } });
            var response = _solver.Filter("vivre", "?!!!.");
            Assert.IsTrue(response.ContainsKey("tivrv"));
        }

        [TestMethod]
        public void RedYellowGreen()
        {
            WordleSolver _solver = new(new Dictionary<string, float> { { "eaye", 0 } });
            Assert.IsTrue(_solver.Filter("eeet", "!?..").ContainsKey("eaye"));
        }

        [TestMethod]
        public void RedYellowGreen1()
        {
            WordleSolver _solver = new(new Dictionary<string, float> { { "poursuivis", 0 } });
            Assert.IsFalse(_solver.Filter("maintenant", "..???.....").ContainsKey("poursuivis"));
        }

        [TestMethod]
        public void RedYellowGreen2()
        {
            WordleSolver _solver = new(new Dictionary<string, float> { { "proportion", 0 } });
            Assert.IsTrue(_solver.Filter("maintenant", "..???.....").ContainsKey("proportion"));
        }

        //[TestMethod]
        //public void RedYellowGreen3()
        //{
        //    WordleSolver _solver = new(new Dictionary<string, float> { { "protection", 0 }, { "maintenant", 0 }, { "inquietude", 0 } });
        //    var dictionary = _solver.Filter("maintenant", "..???.....");
        //    var dictionary2 = _solver.Filter("inquietude", "??...?!...");
        //    Assert.IsTrue(dictionary2.ContainsKey("protection"));
        //}

        [TestMethod]
        public void RedYellowGreen4()
        {
            WordleSolver _solver = new(new Dictionary<string, float> { { "maintenant", 0 }, { "exactement", 0 }, { "encourager", 0 } });
            var dictionary = _solver.Filter("maintenant", ".?.?.?....");
            var dictionary2 = _solver.Filter("exactement", "!.??.?..?.");
            Assert.IsTrue(dictionary2.ContainsKey("encourager"));
        }

        [TestMethod]
        public void RedYellowGreen5()
        {
            WordleSolver _solver = new(new Dictionary<string, float> { { "mauvaise", 0 }, { "aiguille", 0 },  { "habitude", 0 }, { "restaure", 0 } });
            var dictionary = _solver.Filter("restaure", "....??.!");
            var dictionary2 = _solver.Filter("habitude", ".?.?.?.!");
            Assert.IsFalse(dictionary2.ContainsKey("mauvaise"));
        }

        [TestMethod]
        public void RedYellowGreen6()
        {
            WordleSolver _solver = new(new Dictionary<string, float> { { "abdicataire", 0 }, { "litterature", 0 }, { "ventilateur", 0 }, { "realisateur", 0 }, { "utilisateur", 0 } });
            var dictionary = _solver.Filter("abdicataire", "?..?..?.???");
            var dictionary2 = _solver.Filter("litterature", "???.??!!?..");
            var dictionary3 = _solver.Filter("ventilateur", "...?!?!!!!!");
            Assert.IsFalse(dictionary3.ContainsKey("realisateur"));
        }

        //[TestMethod]
        //public void SolverReturnWordWhenWholeWordIsPassed4()
        //{
        //    WordleSolver _solver = new(new Dictionary<string, float> { { "admission", 0 } });
        //    Assert.IsTrue(_solver.Filter("imaginais", "???.??..?").ContainsKey("admission"));
        //}
    }
}