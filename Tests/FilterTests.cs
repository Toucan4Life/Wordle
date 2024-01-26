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

            Assert.IsTrue(new Rule("coucou", patterns).IsWordConform("coucou"));
        }

        [TestMethod]
        public void ReturnNullIfNoWordCorrespond()
        {
            var patterns = new List<Pattern>
            {
                Pattern.Correct, Pattern.Incorrect, Pattern.Incorrect, Pattern.Incorrect,
                Pattern.Incorrect, Pattern.Incorrect
            };

            Assert.IsFalse(new Rule("coucou", patterns).IsWordConform("toucan"));
        }

        [TestMethod]
        public void ReturnAllOccurenceOfCorrectStringWhenAllMatch()
        {
            var patterns = new List<Pattern>
            {
                Pattern.Incorrect, Pattern.Correct, Pattern.Correct, Pattern.Incorrect,
                Pattern.Incorrect, Pattern.Incorrect
            };
            var rule = new Rule("boubbb", patterns);
            Assert.IsTrue(rule.IsWordConform("coucou"));
            Assert.IsTrue(rule.IsWordConform("toucan"));
        }

        [TestMethod]
        public void ReturnAllOccurenceOfCorrectStringWhenNotAllMatch()
        {
            var patterns = new List<Pattern>
            {
                Pattern.Incorrect, Pattern.Correct, Pattern.Correct, Pattern.Incorrect,
                Pattern.Incorrect, Pattern.Incorrect
            };

            var rule = new Rule("boubbb", patterns);
            Assert.IsTrue(rule.IsWordConform("coucou"));
            Assert.IsTrue(rule.IsWordConform("toucan"));
            Assert.IsFalse(rule.IsWordConform("ehbahnon"));
            Assert.IsFalse(rule.IsWordConform("couchera"));
        }

        [TestMethod]
        public void SplitWorks()
        {
            var patterns = new List<Pattern>
            {
                Pattern.Incorrect, Pattern.Correct, Pattern.Correct, Pattern.Correct,
                Pattern.Correct, Pattern.Misplaced
            };

            var rule = new Rule("doucat", patterns);
            Assert.IsTrue(rule.IsWordConform("toucan"));
            Assert.IsFalse(rule.IsWordConform("coucou"));
            Assert.IsFalse(rule.IsWordConform("ehbahnon"));
            Assert.IsFalse(rule.IsWordConform("couchera"));
        }

        [TestMethod]
        public void SplitWorks2()
        {
            var patterns = new List<Pattern>
            {
                Pattern.Incorrect, Pattern.Correct, Pattern.Correct, Pattern.Correct,
                Pattern.Incorrect, Pattern.Misplaced
            };

            var rule = new Rule("coucot", patterns);
            Assert.IsTrue(rule.IsWordConform("toucan"));
            Assert.IsFalse(rule.IsWordConform("coucou"));
            Assert.IsFalse(rule.IsWordConform("ehbahnon"));
            Assert.IsFalse(rule.IsWordConform("couchera"));
        }

        [TestMethod]
        public void MinusWork()
        {
            var patterns = new List<Pattern>
            {
                Pattern.Correct, Pattern.Correct, Pattern.Correct, Pattern.Correct,
                Pattern.Correct, Pattern.Incorrect
            };

            var rule = new Rule("coucot", patterns);
            Assert.IsTrue(rule.IsWordConform("coucou"));
            Assert.IsFalse(rule.IsWordConform("toucan"));
            Assert.IsFalse(rule.IsWordConform("ehbahnon"));
            Assert.IsFalse(rule.IsWordConform("couchera"));
        }

        [TestMethod]
        public void RedGreen()
        {
            var patterns = new List<Pattern>
            {
                Pattern.Incorrect, Pattern.Correct, Pattern.Correct, Pattern.Correct,
                Pattern.Correct
            };

            var rule = new Rule("vivre", patterns);
            Assert.IsTrue(rule.IsWordConform("givre"));
            Assert.IsTrue(rule.IsWordConform("livre"));
            Assert.IsFalse(rule.IsWordConform("vivre"));
        }

        [TestMethod]
        public void YellowGreen()
        {
            var patterns = new List<Pattern>
            {
                Pattern.Misplaced, Pattern.Correct, Pattern.Correct,
                Pattern.Correct, Pattern.Incorrect
            };

            var rule = new Rule("vivre", patterns);
            Assert.IsTrue(rule.IsWordConform("tivrv"));
        }

        [TestMethod]
        public void RedYellowGreen()
        {
            var patterns = new List<Pattern>
            {
                Pattern.Correct, Pattern.Misplaced, Pattern.Incorrect, Pattern.Incorrect
            };

            var rule = new Rule("eeet", patterns);
            Assert.IsTrue(rule.IsWordConform("eaye"));
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

            var rule = new Rule("poursuivis", patterns);
            Assert.IsFalse(rule.IsWordConform("poursuivis"));
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
            var rule = new Rule("maintenant", patterns);
            Assert.IsTrue(rule.IsWordConform("proportion"));
        }

        [TestMethod]
        public void RedYellowGreen4()
        {
            var patterns = new List<Pattern>
            {
                Pattern.Correct, Pattern.Incorrect, Pattern.Misplaced,
                Pattern.Misplaced, Pattern.Incorrect, Pattern.Misplaced,
                Pattern.Incorrect, Pattern.Incorrect, Pattern.Misplaced,
                Pattern.Incorrect
            };
            var rule = new Rule("exactement", patterns);
            Assert.IsTrue(rule.IsWordConform("encourager"));
            Assert.IsFalse(rule.IsWordConform("maintenant"));
            Assert.IsFalse(rule.IsWordConform("exactement"));

        }

        [TestMethod]
        public void RedYellowGreen5()
        {
            var patterns = new List<Pattern>
            {
                Pattern.Incorrect, Pattern.Misplaced, Pattern.Incorrect,
                Pattern.Misplaced, Pattern.Incorrect, Pattern.Misplaced,
                Pattern.Incorrect, Pattern.Correct
            };
            var rule = new Rule("habitude", patterns);
            Assert.IsFalse(rule.IsWordConform("mauvaise"));
        }

        [TestMethod]
        public void RedYellowGreen6()
        {
            var patterns = new List<Pattern>
            {
                Pattern.Incorrect, Pattern.Incorrect, Pattern.Incorrect,
                Pattern.Misplaced, Pattern.Correct, Pattern.Misplaced,
                Pattern.Correct, Pattern.Correct, Pattern.Correct,
                Pattern.Correct, Pattern.Correct
            };
            var rule = new Rule("ventilateur", patterns);
            Assert.IsFalse(rule.IsWordConform("realisateur"));
        }

        [TestMethod]
        public void RedYellowGreen7()
        {
            var patterns = new List<Pattern>
            {
                Pattern.Misplaced, Pattern.Misplaced, Pattern.Misplaced,
                Pattern.Incorrect, Pattern.Incorrect, Pattern.Correct
            };
            var rule = new Rule("dansee", patterns);
            Assert.IsTrue(rule.IsWordConform("grande"));
        }

        [TestMethod]
        public void RedYellowGreen8()
        {
            var patterns = new List<Pattern>
            {
                Pattern.Correct, Pattern.Misplaced, Pattern.Incorrect,
                Pattern.Correct, Pattern.Correct, Pattern.Incorrect, Pattern.Correct
            };
            var rule = new Rule("usurier", patterns);
            Assert.IsFalse(rule.IsWordConform("butoirs"));
        }


        [TestMethod]
        public void StressTests6()
        {
            var patterns = new List<Pattern>
            {
                Pattern.Incorrect, Pattern.Incorrect, Pattern.Incorrect, Pattern.Correct, Pattern.Incorrect,
                Pattern.Incorrect, Pattern.Correct
            };
            var rule = new Rule("abaisse", patterns);
            Assert.IsTrue(rule.IsWordConform("feuille"));
        }
    }
}