using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wordle;
using Wordle.BLL;
using Wordle.SAL;

namespace IntegrationTests
{
    [TestClass]
    public class PatternTests
    {

        [TestMethod]
        public void StressTests()
        {
            var result = Rule.GetPattern("usurier","usagers");
            var expected = new List<Pattern>
            {
                Pattern.Correct, Pattern.Correct, Pattern.Incorrect, Pattern.Misplaced, Pattern.Incorrect,
                Pattern.Misplaced, Pattern.Incorrect
            };
            Assert.IsTrue(expected.SequenceEqual( result));
        }

        [TestMethod]
        public void StressTests3()
        {
            var result = Rule.GetPattern("usagers", "usurier");
            var expected = new List<Pattern>
            {
                Pattern.Correct, Pattern.Correct, Pattern.Incorrect, Pattern.Incorrect, Pattern.Misplaced,
                Pattern.Misplaced, Pattern.Incorrect
            };
            Assert.IsTrue(expected.SequenceEqual(result));
        }

        [TestMethod]
        public void StressTests4()
        {
            var result = Rule.GetPattern("abregee", "feuille");
            var expected = new List<Pattern>
            {
                Pattern.Incorrect, Pattern.Incorrect, Pattern.Incorrect, Pattern.Misplaced, Pattern.Incorrect,
                Pattern.Incorrect, Pattern.Correct
            };
            Assert.IsTrue(expected.SequenceEqual(result));
        }

        [TestMethod]
        public void StressTests5()
        {
            var result = Rule.GetPattern("aeriens", "feuille");
            var expected = new List<Pattern>
            {
                Pattern.Incorrect, Pattern.Correct, Pattern.Incorrect, Pattern.Correct, Pattern.Misplaced,
                Pattern.Incorrect, Pattern.Incorrect
            };
            Assert.IsTrue(expected.SequenceEqual(result));
        }

    }
}