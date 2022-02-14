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
            Dictionary<string, float> possibleSolution = new CsvReader().GetAllWords("SAL/Lexique381.csv");
            var _solver = new Solver();
            var patterns = new List<Pattern> {Pattern.Incorrect, Pattern.Incorrect, Pattern.Incorrect, Pattern.Misplaced, Pattern.Incorrect, Pattern.Incorrect, Pattern.Incorrect };
            var result =_solver.FilterWithEntropy("feuille", patterns, new WordSearcher(possibleSolution)).ToList();
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void StressTests2()
        {
            Dictionary<string, float> possibleSolution = new CsvReader().GetAllWords("SAL/Lexique381.csv")
                .Where(t => t.Key.Length == 7).ToDictionary(t => t.Key, t => t.Value);
            var _solver = new Solver();
            var result = _solver.CalculateEntropy("feuille", possibleSolution);
            Assert.IsTrue(Math.Abs(result - 6.90924931) < 0.000001);
        }

        [TestMethod]
        public void StressTests3()
        {
            Dictionary<string, float> possibleSolution = new CsvReader().GetAllWords("SAL/Lexique381.csv");
            var _solver = new Solver();
            var result = _solver.FilterWithEntropy(4, possibleSolution).ToList();
            Assert.IsNotNull(result);
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
                .Where(t => t.Key.Length == 7).ToDictionary(t => t.Key, t => t.Value);

            var result = _solver.Filter(actualWord, _solver.GetPattern2(actualWord, targetWord),
                new WordSearcher(possibleSolution) { WordLength = actualWord.Length }).Select(t => t.Key);

            Assert.IsTrue(result.Contains(targetWord));
        }

        [TestMethod]
        public void EachPatternFoundIsSearchableBySameRuleSet2()
        {
            const string targetWord = "feuille";

            var solver = new Rule();
            var possibleSolution = new CsvReader().GetAllWords("SAL/Lexique381.csv")
                .Where(t => t.Key.Length == targetWord.Length).OrderBy(t=>t.Key).ToDictionary(t => t.Key, t => t.Value);

            foreach (var result in possibleSolution.Keys.AsParallel().Select(key =>
                         solver.Filter(key, solver.GetPattern2(key, targetWord),
                             new WordSearcher(possibleSolution) {WordLength = targetWord.Length}).Select(t => t.Key)))
            {
                Assert.IsTrue(result.Contains(targetWord));
            }
        }

        #endregion



    }
}