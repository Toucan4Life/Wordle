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
            var _solver = new WordleSolver();
            var result =_solver.GetPattern("usurier","usagers");
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
            var _solver = new WordleSolver();
            var result = _solver.GetPattern("usagers", "usurier");
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
            var _solver = new WordleSolver();
            var result = _solver.GetPattern("abregee", "feuille");
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
            var _solver = new WordleSolver();
            var result = _solver.GetPattern("aeriens", "feuille");
            var expected = new List<Pattern>
            {
                Pattern.Incorrect, Pattern.Correct, Pattern.Incorrect, Pattern.Correct, Pattern.Misplaced,
                Pattern.Incorrect, Pattern.Incorrect
            };
            Assert.IsTrue(expected.SequenceEqual(result));
        }

        [TestMethod]
        public void EachPatternFoundIsSearchableBySameRuleSet()
        {
            var _solver = new WordleSolver();
            var targetWord = "feuille";
            var actualWord = "aeriens";

            var possibleSolution = new CsvReader().GetAllWords("Lexique381.csv")
                .Where(t => t.Key.Length == 7).ToDictionary(t => t.Key, t => t.Value);

            var pattern = _solver.GetPattern(actualWord, targetWord);

            var wordSearcher = new WordSearcher(possibleSolution) {WordLength = actualWord.Length};
            var result = _solver.Filter(actualWord, pattern,
                wordSearcher).Select(t => t.Key);

            Assert.IsTrue(result.Contains(targetWord));
        }

        [TestMethod]
        public void EachPatternFoundIsSearchableBySameRuleSet2()
        {
            var _solver = new WordleSolver();
            var targetWord = "feuille";

            var possibleSolution = new CsvReader().GetAllWords("Lexique381.csv")
                .Where(t => t.Key.Length == 7).ToDictionary(t => t.Key, t => t.Value);

            foreach (var (key, _) in possibleSolution)
            {
                var pattern = _solver.GetPattern(key, targetWord);

                var wordSearcher = new WordSearcher(possibleSolution) { WordLength = targetWord.Length };
                var result = _solver.Filter(key, pattern,
                    wordSearcher).Select(t => t.Key).ToList();

                Assert.IsTrue(result.Contains(targetWord));
            }

        }
    }
}