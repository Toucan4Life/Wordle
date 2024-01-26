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
            var _solver = new WordleSolver(new WordleStartParameter { WordLength = 5, FirstChar = "t" });
            var result = _solver.RetrieveRecommendedWords(new List<Tuple<string, string>>())
                .OrderByDescending(t => t.Entropy).Take(1).Single();
            Assert.AreEqual(result.Name,"tarie");
            Assert.AreEqual(result.Entropy, 5.190980369389831);
        }

        [TestMethod]
        public void StressTests()
        {
            var _solver = new WordleSolver(new WordleStartParameter{WordLength = 7});
            
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

            var patternsList = new List<KeyValuePair<string, List<Pattern>>>
                { new(actualWord, Rule.GetPattern(actualWord, targetWord)) };
            var result = possibleSolution.Where(word =>
                    patternsList.Select(wp => new Rule(wp.Key, wp.Value)).All(rule => rule.IsWordConform(word.Key)))
                .Select(w => w.Key).ToList();

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
                var patternsList = new List<KeyValuePair<string, List<Pattern>>>
                    { new(key.Key, Rule.GetPattern(key.Key, targetWord)) };
                return possibleSolution.Where(word =>
                        patternsList.Select(wp => new Rule(wp.Key, wp.Value)).All(rule => rule.IsWordConform(word.Key)))
                    .Select(w => w.Key).ToList();
            });

            foreach (var result in parallelQuery)
            {
                Assert.IsTrue(result.Contains(targetWord));
            }
        }

        #endregion
    }
}