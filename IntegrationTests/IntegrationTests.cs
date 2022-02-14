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
            Dictionary<string, float> possibleSolution = new CsvReader().GetAllWords("Lexique381.csv");
            var _solver = new Solver();
            var patterns = new List<Pattern> {Pattern.Incorrect, Pattern.Incorrect, Pattern.Incorrect, Pattern.Misplaced, Pattern.Incorrect, Pattern.Incorrect, Pattern.Incorrect };
            var result =_solver.FilterWithEntropy("feuille", patterns, new WordSearcher(possibleSolution));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void StressTests2()
        {
            Dictionary<string, float> possibleSolution = new CsvReader().GetAllWords("Lexique381.csv")
                .Where(t => t.Key.Length == 7).ToDictionary(t => t.Key, t => t.Value);
            var _solver = new Solver();
            var result = _solver.CalculateEntropy("feuille", possibleSolution);
            Assert.IsTrue(Math.Abs(result - 6.908886) < 0.000001);
        }

        [TestMethod]
        public void StressTests3()
        {
            Dictionary<string, float> possibleSolution = new CsvReader().GetAllWords("Lexique381.csv");
            var _solver = new Solver();
            var result = _solver.FilterWithEntropy(4, possibleSolution);
            Assert.IsNotNull(result);
        }

        #endregion

        #region Rule

        [TestMethod]
        public void EachPatternFoundIsSearchableBySameRuleSet()
        {
            var targetWord = "feuille";
            var actualWord = "aeriens";

            var _solver = new Rule();
            var possibleSolution = new CsvReader().GetAllWords("Lexique381.csv")
                .Where(t => t.Key.Length == 7).ToDictionary(t => t.Key, t => t.Value);

            var result = _solver.Filter(actualWord, _solver.GetPattern(actualWord, targetWord),
                new WordSearcher(possibleSolution) { WordLength = actualWord.Length }).Select(t => t.Key);

            Assert.IsTrue(result.Contains(targetWord));
        }

        [TestMethod]
        public void EachPatternFoundIsSearchableBySameRuleSet2()
        {
            const string targetWord = "feuille";

            var solver = new Rule();
            var possibleSolution = new CsvReader().GetAllWords("Lexique381.csv")
                .Where(t => t.Key.Length == targetWord.Length).ToDictionary(t => t.Key, t => t.Value);

            foreach (var result in from string key in possibleSolution.Keys
                     select solver.Filter(key, solver.GetPattern(key, targetWord),
                         new WordSearcher(possibleSolution) { WordLength = targetWord.Length }).Select(t => t.Key))
                Assert.IsTrue(result.Contains(targetWord));
        }

        #endregion



    }
}