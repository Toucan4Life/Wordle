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

        [TestMethod]
        public void SolverReturnWordWhenWholeWordIsPassed()
        {
            var patterns = new List<Pattern>
            {
                Pattern.Correct, Pattern.Correct, Pattern.Correct, Pattern.Correct,
                Pattern.Correct, Pattern.Correct
            };

            WordleSolver _solver = new();
            Assert.IsTrue(  _solver.Filter("coucou", patterns, new WordSearcher(new Dictionary<string, float> { { "coucou", 0 } })).Any(t=>t.Key == "coucou"));
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
            WordleSolver _solver = new();
            Assert.IsTrue(_solver.Filter("coucou", patterns, new WordSearcher(new Dictionary<string, float> { { "coucou", 0 } })).Any(t => t.Key == "coucou"));
        }

        [TestMethod]
        public void ReturnNullIfNoWordCorrespond()
        {
            var patterns = new List<Pattern>
            {
                Pattern.Correct, Pattern.Incorrect, Pattern.Incorrect, Pattern.Incorrect,
                Pattern.Incorrect, Pattern.Incorrect
            };
            WordleSolver _solver = new();
            Assert.AreEqual(0, _solver.Filter("coucou", patterns, new WordSearcher(new Dictionary<string, float> { { "toucan", 0 } })).Count());
        }

        [TestMethod]
        public void ReturnAllOccurenceOfCorrectStringWhenAllMatch()
        {
            var patterns = new List<Pattern>
            {
                Pattern.Incorrect, Pattern.Correct, Pattern.Correct, Pattern.Incorrect,
                Pattern.Incorrect, Pattern.Incorrect
            };
            WordleSolver _solver = new();
            var enumerable = _solver.Filter("boubbb", patterns, new WordSearcher(new Dictionary<string, float> { { "toucan", 0 }, { "coucou", 1 } }));
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
                new();

            var enumerable = _solver.Filter("boubbb", patterns, new WordSearcher(new Dictionary<string, float> { { "toucan", 0 }, { "coucou", 1 }, { "ehbahnon", 0 }, { "couchera", 1 } }));
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
                new();

            var enumerable = _solver.Filter("doucat", patterns, new WordSearcher(new Dictionary<string, float> { { "toucan", 0 }, { "coucou", 1 }, { "ehbahnon", 0 }, { "couchera", 1 } }));
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
                new();

            var enumerable = _solver.Filter("coucot", patterns, new WordSearcher(new Dictionary<string, float> { { "toucan", 0 }, { "coucou", 1 }, { "ehbahnon", 0 }, { "couchera", 1 } }));
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
                new();

            var enumerable = _solver.Filter("coucot", patterns, new WordSearcher(new Dictionary<string, float> { { "toucan", 0 }, { "coucou", 1 }, { "ehbahnon", 0 }, { "couchera", 1 } }));
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
            WordleSolver _solver = new();
            Assert.IsFalse(_solver.Filter("vivre", patterns, new WordSearcher(new Dictionary<string, float> { { "vivre", 0 }, { "livre", 0 }, { "givre", 0 } })).Any(t => t.Key == "vivre"));
        }

        [TestMethod]
        public void YellowGreen()
        {
            var patterns = new List<Pattern>
            {
                Pattern.Misplaced, Pattern.Correct, Pattern.Correct,
                Pattern.Correct, Pattern.Incorrect
            };
            WordleSolver _solver = new();
            var response = _solver.Filter("vivre", patterns, new WordSearcher(new Dictionary<string, float> { { "tivrv", 0 } }));
            Assert.IsTrue(response.Any(t => t.Key == "tivrv"));
        }

        [TestMethod]
        public void RedYellowGreen()
        {
            var patterns = new List<Pattern>
            {
                Pattern.Correct, Pattern.Misplaced, Pattern.Incorrect, Pattern.Incorrect
            };
            WordleSolver _solver = new();
            Assert.IsTrue(_solver.Filter("eeet", patterns, new WordSearcher(new Dictionary<string, float> { { "eaye", 0 } })).Any(t => t.Key == "eaye"));
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
            WordleSolver _solver = new();
            Assert.IsFalse(_solver.Filter("maintenant", patterns, new WordSearcher(new Dictionary<string, float> { { "poursuivis", 0 } })).Any(t => t.Key == "poursuivis"));
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
            WordleSolver _solver = new();
            Assert.IsTrue(_solver.Filter("maintenant", patterns, new WordSearcher(new Dictionary<string, float> { { "proportion", 0 } })).Any(t => t.Key == "proportion"));
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
            WordleSolver _solver = new();
            var dictionary = _solver
                .Filter("maintenant", patterns1,
                    new WordSearcher(new Dictionary<string, float>
                        {{"maintenant", 0}, {"exactement", 0}, {"encourager", 0}}))
                .ToDictionary(t => t.Key, t => t.Value);
            var dictionary2 = _solver.Filter("exactement", patterns2, new WordSearcher(dictionary ));
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
            WordleSolver _solver = new();
            var dictionary = _solver.Filter("restaure", patterns1, new WordSearcher(new Dictionary<string, float> { { "mauvaise", 0 }, { "aiguille", 0 }, { "habitude", 0 }, { "restaure", 0 } }));
            var dictionary2 = _solver.Filter("habitude", patterns2, new WordSearcher(new Dictionary<string, float> { { "coucou", 0 } }));
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
            WordleSolver _solver = new();
            var dictionary = _solver
                .Filter("abdicataire", patterns1,
                    new WordSearcher(new Dictionary<string, float>
                    {
                        {"abdicataire", 0}, {"litterature", 0}, {"ventilateur", 0}, {"realisateur", 0},
                        {"utilisateur", 0}
                    })).ToDictionary(t => t.Key, t => t.Value);
            var dictionary2 = _solver.Filter("litterature", patterns2, new WordSearcher(dictionary)).ToDictionary(t => t.Key, t => t.Value);
            var dictionary3 = _solver.Filter("ventilateur", patterns3, new WordSearcher(dictionary2));
            Assert.IsFalse(dictionary3.Any(t => t.Key == "realisateur"));
        }

        [TestMethod]
        public void RedYellowGreen7()
        {
            var patterns1 = new List<Pattern>
            {
                Pattern.Misplaced, Pattern.Misplaced, Pattern.Misplaced,
                Pattern.Incorrect, Pattern.Incorrect, Pattern.Incorrect
            };
            var patterns2 = new List<Pattern>
            {
                Pattern.Misplaced, Pattern.Misplaced, Pattern.Misplaced,
                Pattern.Incorrect, Pattern.Incorrect, Pattern.Correct
            };
            var wordDictionary = new Dictionary<string, float> { { "grande", 0 }, { "andain", 0 }, { "dansee", 0 } };
            WordleSolver _solver = new();
            var dictionary = _solver.Filter("andain", patterns1, new WordSearcher(wordDictionary)).ToDictionary(t=>t.Key, t=>t.Value);
            var dictionary2 = _solver.Filter("dansee", patterns2, new WordSearcher(dictionary));
            Assert.IsTrue(dictionary2.Any(t => t.Key == "grande"));
        }

        [TestMethod]
        public void RedYellowGreen8()
        {
            var patterns1 = new List<Pattern>
            {
                Pattern.Correct, Pattern.Misplaced, Pattern.Incorrect,
                Pattern.Correct, Pattern.Correct, Pattern.Incorrect
                ,Pattern.Correct
            };
            var wordDictionary = new Dictionary<string, float> { { "usurier", 0 }, { "butoirs", 0 }};
            WordleSolver _solver = new();
            var dictionary = _solver.Filter("usurier", patterns1, new WordSearcher(wordDictionary)).ToDictionary(t => t.Key, t => t.Value);
            Assert.IsFalse(dictionary.Any(t => t.Key == "butoirs"));
        }

        //[TestMethod]
        //public void RedYellowGreen9()
        //{
        //    //CCIIIMM
        //    //CCIMIMI
        //    var patterns1 = new List<Pattern>
        //    {
        //        Pattern.Correct, Pattern.Correct, Pattern.Incorrect,
        //        Pattern.Misplaced, Pattern.Incorrect, Pattern.Misplaced
        //        ,Pattern.Incorrect
        //    };
        //    var wordDictionary = new Dictionary<string, float> { { "usurier", 0 }, { "usagers", 0 } };
        //    WordleSolver _solver = new();
        //    var dictionary = _solver.Filter("usurier", patterns1, new WordSearcher(wordDictionary)).ToDictionary(t => t.Key, t => t.Value);
        //    Assert.IsFalse(dictionary.Any(t => t.Key == "usagers"));
        //}
    }
}