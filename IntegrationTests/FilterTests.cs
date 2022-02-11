using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wordle;
using Wordle.BLL;

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
            var patterns = new List<Pattern>
            {
                Pattern.Correct, Pattern.Correct, Pattern.Correct, Pattern.Correct,
                Pattern.Correct, Pattern.Correct
            };

            WordleSolver _solver = new(new Dictionary<string, float> {{"coucou", 0}});
            Assert.IsTrue(  _solver.Filter("coucou", patterns).Any(t=>t.Key == "coucou"));
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
            var patterns = new List<Pattern>
            {
                Pattern.Correct, Pattern.Correct, Pattern.Correct, Pattern.Correct,
                Pattern.Correct, Pattern.Correct
            };
            WordleSolver _solver = new(new Dictionary<string, float> {{"coucou", 0}});
            Assert.IsTrue(_solver.Filter("coucou", patterns).Any(t => t.Key == "coucou"));
        }

        [TestMethod]
        public void ReturnNullIfNoWordCorrespond()
        {
            var patterns = new List<Pattern>
            {
                Pattern.Correct, Pattern.Incorrect, Pattern.Incorrect, Pattern.Incorrect,
                Pattern.Incorrect, Pattern.Incorrect
            };
            WordleSolver _solver = new(new Dictionary<string, float> {{"toucan", 0}});
            Assert.AreEqual(0, _solver.Filter("coucou", patterns).Count());
        }

        [TestMethod]
        public void ReturnAllOccurenceOfCorrectStringWhenAllMatch()
        {
            var patterns = new List<Pattern>
            {
                Pattern.Incorrect, Pattern.Correct, Pattern.Correct, Pattern.Incorrect,
                Pattern.Incorrect, Pattern.Incorrect
            };
            WordleSolver _solver = new(new Dictionary<string, float> {{"toucan", 0}, {"coucou", 1}});
            var enumerable = _solver.Filter("boubbb", patterns);
            Assert.IsTrue(enumerable.Any(t => t.Key == "coucou"));
            Assert.IsTrue(enumerable.Any(t => t.Key == "toucan"));
        }

        [TestMethod]
        public void ReturnAllOccurenceOfCorrectStringWhenNotAllMatch()
        {
            var patterns = new List<Pattern>
            {
                Pattern.Incorrect, Pattern.Correct, Pattern.Correct, Pattern.Incorrect,
                Pattern.Incorrect, Pattern.Incorrect
            };
            WordleSolver _solver =
                new(new Dictionary<string, float> { {"toucan", 0}, {"coucou", 1}, {"ehbahnon", 0}, {"couchera", 1}});

            var enumerable = _solver.Filter("boubbb", patterns);
            Assert.IsTrue(enumerable.Any(t => t.Key == "coucou"));
            Assert.IsTrue(enumerable.Any(t => t.Key == "toucan"));
            Assert.AreEqual(2, enumerable.Count());
        }

        [TestMethod]
        public void SplitWorks()
        {
            var patterns = new List<Pattern>
            {
                Pattern.Incorrect, Pattern.Correct, Pattern.Correct, Pattern.Correct,
                Pattern.Correct, Pattern.Misplaced
            };
            WordleSolver _solver =
                new(new Dictionary<string, float> { { "toucan", 0 }, { "coucou", 1 }, { "ehbahnon", 0 }, { "couchera", 1 } });

            var enumerable = _solver.Filter("doucat", patterns);
            Assert.IsTrue(enumerable.Any(t => t.Key == "toucan"));
            Assert.AreEqual(1, enumerable.Count());
        }

        [TestMethod]
        public void SplitWorks2()
        {
            var patterns = new List<Pattern>
            {
                Pattern.Incorrect, Pattern.Correct, Pattern.Correct, Pattern.Correct,
                Pattern.Incorrect, Pattern.Misplaced
            };
            WordleSolver _solver =
                new(new Dictionary<string, float> { { "toucan", 0 }, { "coucou", 1 }, { "ehbahnon", 0 }, { "couchera", 1 } });

            var enumerable = _solver.Filter("coucot", patterns);
            Assert.IsTrue(enumerable.Any(t => t.Key == "toucan"));
            Assert.AreEqual(1, enumerable.Count());
        }

        [TestMethod]
        public void MinusWork()
        {
            var patterns = new List<Pattern>
            {
                Pattern.Correct, Pattern.Correct, Pattern.Correct, Pattern.Correct,
                Pattern.Correct, Pattern.Incorrect
            };
            WordleSolver _solver =
                new(new Dictionary<string, float> { { "toucan", 0 }, { "coucou", 1 }, { "ehbahnon", 0 }, { "couchera", 1 } });

            var enumerable = _solver.Filter("coucot", patterns);
            Assert.IsTrue(enumerable.Any(t => t.Key == "coucou"));
            Assert.AreEqual(1, enumerable.Count());
        }

        [TestMethod]
        public void RedGreen()
        {
            var patterns = new List<Pattern>
            {
                Pattern.Incorrect, Pattern.Correct, Pattern.Correct, Pattern.Correct,
                Pattern.Correct
            };
            WordleSolver _solver = new(new Dictionary<string, float> { { "vivre", 0 }, { "livre", 0 } , { "givre", 0 } });
            Assert.IsFalse(_solver.Filter("vivre", patterns).Any(t => t.Key == "vivre"));
        }

        [TestMethod]
        public void YellowGreen()
        {
            var patterns = new List<Pattern>
            {
                Pattern.Misplaced, Pattern.Correct, Pattern.Correct,
                Pattern.Correct, Pattern.Incorrect
            };
            WordleSolver _solver = new(new Dictionary<string, float> { { "tivrv", 0 } });
            var response = _solver.Filter("vivre", patterns);
            Assert.IsTrue(response.Any(t => t.Key == "tivrv"));
        }

        [TestMethod]
        public void RedYellowGreen()
        {
            var patterns = new List<Pattern>
            {
                Pattern.Correct, Pattern.Misplaced, Pattern.Incorrect, Pattern.Incorrect
            };
            WordleSolver _solver = new(new Dictionary<string, float> { { "eaye", 0 } });
            Assert.IsTrue(_solver.Filter("eeet", patterns).Any(t => t.Key == "eaye"));
        }

        [TestMethod]
        public void RedYellowGreen1()
        {
            var patterns = new List<Pattern>
            {
                Pattern.Incorrect, Pattern.Incorrect, Pattern.Misplaced,
                Pattern.Misplaced, Pattern.Misplaced, Pattern.Incorrect,
                Pattern.Incorrect, Pattern.Incorrect, Pattern.Incorrect,
                Pattern.Incorrect
            };
            WordleSolver _solver = new(new Dictionary<string, float> { { "poursuivis", 0 } });
            Assert.IsFalse(_solver.Filter("maintenant", patterns).Any(t => t.Key == "poursuivis"));
        }

        [TestMethod]
        public void RedYellowGreen2()
        {
            var patterns = new List<Pattern>
            {
                Pattern.Incorrect, Pattern.Incorrect, Pattern.Misplaced,
                Pattern.Misplaced, Pattern.Misplaced, Pattern.Incorrect,
                Pattern.Incorrect, Pattern.Incorrect, Pattern.Incorrect,
                Pattern.Incorrect
            };
            WordleSolver _solver = new(new Dictionary<string, float> { { "proportion", 0 } });
            Assert.IsTrue(_solver.Filter("maintenant", patterns).Any(t => t.Key == "proportion"));
        }

        [TestMethod]
        public void RedYellowGreen4()
        {
            var patterns1 = new List<Pattern>
            {
                Pattern.Incorrect, Pattern.Misplaced, Pattern.Incorrect,
                Pattern.Misplaced, Pattern.Incorrect, Pattern.Misplaced,
                Pattern.Incorrect, Pattern.Incorrect, Pattern.Incorrect,
                Pattern.Incorrect
            };
            var patterns2 = new List<Pattern>
            {
                Pattern.Correct, Pattern.Incorrect, Pattern.Misplaced,
                Pattern.Misplaced, Pattern.Incorrect, Pattern.Misplaced,
                Pattern.Incorrect, Pattern.Incorrect, Pattern.Misplaced,
                Pattern.Incorrect
            };
            WordleSolver _solver = new(new Dictionary<string, float> { { "maintenant", 0 }, { "exactement", 0 }, { "encourager", 0 } });
            var dictionary = _solver.Filter("maintenant", patterns1);
            var dictionary2 = _solver.Filter("exactement", patterns2);
            Assert.IsTrue(dictionary2.Any(t => t.Key == "encourager"));
        }

        [TestMethod]
        public void RedYellowGreen5()
        {
            var patterns1 = new List<Pattern>
            {
                Pattern.Incorrect, Pattern.Incorrect, Pattern.Incorrect,
                Pattern.Incorrect, Pattern.Misplaced, Pattern.Misplaced,
                Pattern.Incorrect, Pattern.Correct
            };
            var patterns2 = new List<Pattern>
            {
                Pattern.Incorrect, Pattern.Misplaced, Pattern.Incorrect,
                Pattern.Misplaced, Pattern.Incorrect, Pattern.Misplaced,
                Pattern.Incorrect, Pattern.Correct
            };
            WordleSolver _solver = new(new Dictionary<string, float> { { "mauvaise", 0 }, { "aiguille", 0 },  { "habitude", 0 }, { "restaure", 0 } });
            var dictionary = _solver.Filter("restaure", patterns1);
            var dictionary2 = _solver.Filter("habitude", patterns2);
            Assert.IsFalse(dictionary2.Any(t => t.Key == "mauvaise"));
        }

        [TestMethod]
        public void RedYellowGreen6()
        {
            var patterns1 = new List<Pattern>
            {
                Pattern.Misplaced, Pattern.Incorrect, Pattern.Incorrect,
                Pattern.Misplaced, Pattern.Incorrect, Pattern.Incorrect,
                Pattern.Misplaced, Pattern.Incorrect, Pattern.Misplaced,
                Pattern.Misplaced, Pattern.Misplaced
            };
            var patterns2 = new List<Pattern>
            {
                Pattern.Misplaced, Pattern.Misplaced, Pattern.Misplaced,
                Pattern.Incorrect, Pattern.Misplaced, Pattern.Misplaced,
                Pattern.Correct, Pattern.Correct, Pattern.Misplaced,
                Pattern.Incorrect, Pattern.Incorrect
            };
            var patterns3 = new List<Pattern>
            {
                Pattern.Incorrect, Pattern.Incorrect, Pattern.Incorrect,
                Pattern.Misplaced, Pattern.Correct, Pattern.Misplaced,
                Pattern.Correct, Pattern.Correct, Pattern.Correct,
                Pattern.Correct, Pattern.Correct
            };
            WordleSolver _solver = new(new Dictionary<string, float> { { "abdicataire", 0 }, { "litterature", 0 }, { "ventilateur", 0 }, { "realisateur", 0 }, { "utilisateur", 0 } });
            var dictionary = _solver.Filter("abdicataire", patterns1);
            var dictionary2 = _solver.Filter("litterature", patterns2);
            var dictionary3 = _solver.Filter("ventilateur", patterns3);
            Assert.IsFalse(dictionary3.Any(t => t.Key == "realisateur"));
        }
    }
}