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
        public void StressTests2()
        {
            var _solver = new WordleSolver(5);
            var result = _solver.RetrieveRecommendedWords(new List<Tuple<string, string>>())
                .OrderByDescending(t => t.Entropy).Take(1).Single();
            Assert.AreEqual(1,1);
        }

        [TestMethod]
        public void StressTests()
        {
            var _solver = new WordleSolver(7);
            
            var result = _solver.RetrieveRecommendedWords(new List<Tuple<string, string>>{new("feuille", "0001000") }).OrderByDescending(t=>t.Entropy).Take(1).Single();
            Assert.IsNotNull(result);
            Assert.IsTrue(Math.Abs(result.Entropy - 8.124536) < 0.000001);
            Assert.AreEqual("corsant", result.Name);
        }

        #endregion

        #region Rule

        [TestMethod]
        public void EachPatternFoundIsSearchableBySameRuleSet()
        {
            var targetWord = "feuille";
            var actualWord = "abaisse";

            var possibleSolution = new CsvReader().GetAllWords("SAL/Lexique381.csv")
                .Where(t => t.Key.Length == 7);

            var wordSearcher = new WordSearcher(possibleSolution) { WordLength = actualWord.Length };
            wordSearcher.Filter(actualWord, Rule.GetPattern(actualWord, targetWord));

            var result = wordSearcher.WordDictionary.Select(t => t.Key);

            Assert.IsTrue(result.Contains(targetWord));
        }

        [TestMethod]
        public void EachPatternFoundIsSearchableBySameRuleSet2()
        {
            const string targetWord = "feuille";

            var possibleSolution = new CsvReader().GetAllWords("SAL/Lexique381.csv")
                .Where(t => t.Key.Length == targetWord.Length).OrderBy(t=>t.Key);

            var parallelQuery = possibleSolution.AsParallel().Select(key =>
            {
                var searcher = new WordSearcher(possibleSolution) { WordLength = targetWord.Length };
                searcher.Filter(key.Key, Rule.GetPattern(key.Key, targetWord));
                return searcher.WordDictionary.Select(t => t.Key);
            });

            foreach (var result in parallelQuery)
            {
                Assert.IsTrue(result.Contains(targetWord));
            }
        }

        #endregion
    }
}