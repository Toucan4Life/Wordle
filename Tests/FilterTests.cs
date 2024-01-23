using System;
using System.Collections;
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

            var wordSearcher = new WordSearcher(new Dictionary<string, float> { { "coucou", 0 } });
            Assert.IsTrue(wordSearcher.Filter("coucou", patterns).Any(t=>t.Key == "coucou"));
        }

        [TestMethod]
        public void SolverDictionaryWordWhenThereIsOnlyOneWord()
        {
            var patterns = new List<Pattern>
            {
                Pattern.Correct, Pattern.Correct, Pattern.Correct, Pattern.Correct,
                Pattern.Correct, Pattern.Correct
            };
            var wordSearcher = new WordSearcher(new Dictionary<string, float> { { "coucou", 0 } });
            Assert.IsTrue(wordSearcher.Filter("coucou", patterns).Any(t => t.Key == "coucou"));
        }

        [TestMethod]
        public void ReturnNullIfNoWordCorrespond()
        {
            var patterns = new List<Pattern>
            {
                Pattern.Correct, Pattern.Incorrect, Pattern.Incorrect, Pattern.Incorrect,
                Pattern.Incorrect, Pattern.Incorrect
            };
            var wordSearcher = new WordSearcher(new Dictionary<string, float> { { "toucan", 0 } }); ;
            Assert.AreEqual(0, wordSearcher.Filter("coucou", patterns).Count());
        }

        [TestMethod]
        public void ReturnAllOccurenceOfCorrectStringWhenAllMatch()
        {
            var patterns = new List<Pattern>
            {
                Pattern.Incorrect, Pattern.Correct, Pattern.Correct, Pattern.Incorrect,
                Pattern.Incorrect, Pattern.Incorrect
            };
            Rule _solver = new();
            var wordSearcher = new WordSearcher(new Dictionary<string, float> { { "toucan", 0 }, { "coucou", 1 } });
            var enumerable = wordSearcher.Filter("boubbb", patterns);
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
            var wordSearcher = new WordSearcher(new Dictionary<string, float> { { "toucan", 0 }, { "coucou", 1 }, { "ehbahnon", 0 }, { "couchera", 1 } });
            var enumerable = wordSearcher.Filter("boubbb", patterns);
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

            var wordSearcher = new WordSearcher(new Dictionary<string, float> { { "toucan", 0 }, { "coucou", 1 }, { "ehbahnon", 0 }, { "couchera", 1 } });
            var enumerable = wordSearcher.Filter("doucat", patterns);
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
            Rule _solver =
                new();

            var wordSearcher = new WordSearcher(new Dictionary<string, float> { { "toucan", 0 }, { "coucou", 1 }, { "ehbahnon", 0 }, { "couchera", 1 } });
            var enumerable = wordSearcher.Filter("coucot", patterns);
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

            var wordSearcher = new WordSearcher(new Dictionary<string, float> { { "toucan", 0 }, { "coucou", 1 }, { "ehbahnon", 0 }, { "couchera", 1 } });
            var enumerable = wordSearcher.Filter("coucot", patterns);
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

            var wordSearcher = new WordSearcher(new Dictionary<string, float> { { "vivre", 0 }, { "livre", 0 }, { "givre", 0 } });
            Assert.IsFalse(wordSearcher.Filter("vivre", patterns).Any(t => t.Key == "vivre"));
        }

        [TestMethod]
        public void YellowGreen()
        {
            var patterns = new List<Pattern>
            {
                Pattern.Misplaced, Pattern.Correct, Pattern.Correct,
                Pattern.Correct, Pattern.Incorrect
            };

            var wordSearcher = new WordSearcher(new Dictionary<string, float> { { "tivrv", 0 } });
            Assert.IsTrue(wordSearcher.Filter("vivre", patterns).Any(t => t.Key == "tivrv"));
        }

        [TestMethod]
        public void RedYellowGreen()
        {
            var patterns = new List<Pattern>
            {
                Pattern.Correct, Pattern.Misplaced, Pattern.Incorrect, Pattern.Incorrect
            };

            var wordSearcher = new WordSearcher(new Dictionary<string, float> { { "eaye", 0 } });
            Assert.IsTrue(wordSearcher.Filter("eeet", patterns).Any(t => t.Key == "eaye"));
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

            var wordSearcher = new WordSearcher(new Dictionary<string, float> { { "poursuivis", 0 } });
            Assert.IsFalse(wordSearcher.Filter("maintenant", patterns).Any(t => t.Key == "poursuivis"));
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

            var wordSearcher = new WordSearcher(new Dictionary<string, float> { { "proportion", 0 } });
            Assert.IsTrue(wordSearcher.Filter("maintenant", patterns).Any(t => t.Key == "proportion"));
        }

        [TestMethod]
        public void RedYellowGreen4()
        {
            var patterns2 = new List<Pattern>
            {
                Pattern.Correct, Pattern.Incorrect, Pattern.Misplaced,
                Pattern.Misplaced, Pattern.Incorrect, Pattern.Misplaced,
                Pattern.Incorrect, Pattern.Incorrect, Pattern.Misplaced,
                Pattern.Incorrect
            };
            var dictionary2 = new WordSearcher(new Dictionary<string, float>
                { { "maintenant", 0 }, { "exactement", 0 }, { "encourager", 0 } });
            Assert.IsTrue(dictionary2.Filter("exactement", patterns2).Any(t => t.Key == "encourager"));

        }

        [TestMethod]
        public void RedYellowGreen5()
        {
            var patterns2 = new List<Pattern>
            {
                Pattern.Incorrect, Pattern.Misplaced, Pattern.Incorrect,
                Pattern.Misplaced, Pattern.Incorrect, Pattern.Misplaced,
                Pattern.Incorrect, Pattern.Correct
            };
            var dictionary2 = new WordSearcher(new Dictionary<string, float> { { "coucou", 0 } });
            Assert.IsFalse(dictionary2.Filter("habitude", patterns2).Any(t => t.Key == "mauvaise"));
        }

        [TestMethod]
        public void RedYellowGreen6()
        {
            var patterns3 = new List<Pattern>
            {
                Pattern.Incorrect, Pattern.Incorrect, Pattern.Incorrect,
                Pattern.Misplaced, Pattern.Correct, Pattern.Misplaced,
                Pattern.Correct, Pattern.Correct, Pattern.Correct,
                Pattern.Correct, Pattern.Correct
            };
            var dictionary2 = new WordSearcher(new Dictionary<string, float>
            {
                {"abdicataire", 0}, {"litterature", 0}, {"ventilateur", 0}, {"realisateur", 0},
                {"utilisateur", 0}
            });
            Assert.IsFalse(dictionary2.Filter("ventilateur", patterns3).Any(t => t.Key == "realisateur"));
        }

        [TestMethod]
        public void RedYellowGreen7()
        {
            var patterns2 = new List<Pattern>
            {
                Pattern.Misplaced, Pattern.Misplaced, Pattern.Misplaced,
                Pattern.Incorrect, Pattern.Incorrect, Pattern.Correct
            };
            var dictionary2 = new WordSearcher(new Dictionary<string, float> { { "grande", 0 }, { "andain", 0 }, { "dansee", 0 } });
            Assert.IsTrue(dictionary2.Filter("dansee", patterns2).Any(t => t.Key == "grande"));
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
            var dictionary = new WordSearcher(new Dictionary<string, float> { { "usurier", 0 }, { "butoirs", 0 } });
            Assert.IsFalse(dictionary.Filter("usurier", patterns1).Any(t => t.Key == "butoirs"));
        }


        [TestMethod]
        public void StressTests6()
        {
            var patterns1 = new List<Pattern>
            {
                Pattern.Incorrect, Pattern.Incorrect, Pattern.Incorrect, Pattern.Correct, Pattern.Incorrect,
                Pattern.Incorrect, Pattern.Correct
            };
            var dictionary = new WordSearcher(new Dictionary<string, float> { { "abaisse", 0 }, { "feuille", 0 } });
            Assert.IsTrue(dictionary.Filter("abaisse", patterns1).Any(t => t.Key == "feuille"));
        }
    }
}