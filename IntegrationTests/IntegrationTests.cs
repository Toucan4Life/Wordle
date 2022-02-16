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
    public class IntegrationTests
    {
        #region Solver



        [TestMethod]
        public void StressTests()
        {
            var _solver = new WordleSolver(7);
            var patterns = new List<Pattern> { Pattern.Incorrect, Pattern.Incorrect, Pattern.Incorrect, Pattern.Misplaced, Pattern.Incorrect, Pattern.Incorrect, Pattern.Incorrect };
            _solver.ApplyWordPattern("feuille", patterns);
            var result = _solver.RetrieveRecommendedWords().OrderByDescending(t=>t.Value).Take(1).Single();
            Assert.IsNotNull(result);
            Assert.IsTrue(Math.Abs(result.Value - 8.124536) < 0.000001);
            Assert.AreEqual("corsant", result.Key);
        }

        #endregion

        #region Rule

        [TestMethod]
        public void EachPatternFoundIsSearchableBySameRuleSet()
        {
            var targetWord = "feuille";
            var actualWord = "abaisse";

            var _solver = new Rule();
            var possibleSolution = new CsvReader().GetAllWords("SAL/Lexique381.csv")
                .Where(t => t.Key.Length == 7);

            var result = _solver.Filter(actualWord, _solver.GetPattern(actualWord, targetWord),
                new WordSearcher(possibleSolution) { WordLength = actualWord.Length }).Search().Select(t => t.Key);

            Assert.IsTrue(result.Contains(targetWord));
        }

        [TestMethod]
        public void EachPatternFoundIsSearchableBySameRuleSet2()
        {
            const string targetWord = "feuille";

            var solver = new Rule();
            var possibleSolution = new CsvReader().GetAllWords("SAL/Lexique381.csv")
                .Where(t => t.Key.Length == targetWord.Length).OrderBy(t=>t.Key);

            foreach (var result in possibleSolution.AsParallel().Select(key =>
                         solver.Filter(key.Key, solver.GetPattern(key.Key, targetWord),
                             new WordSearcher(possibleSolution) {WordLength = targetWord.Length}).Search().Select(t => t.Key)))
            {
                Assert.IsTrue(result.Contains(targetWord));
            }
        }

        #endregion



    }
}