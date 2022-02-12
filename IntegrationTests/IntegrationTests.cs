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

        [TestMethod]
        public void StressTests()
        {
            Dictionary<string, float> possibleSolution = new CsvReader().GetAllWords("Lexique381.csv");
            var _solver = new WordleSolver();
            var patterns = new List<Pattern> {Pattern.Incorrect, Pattern.Incorrect, Pattern.Incorrect, Pattern.Misplaced, Pattern.Incorrect, Pattern.Incorrect, Pattern.Incorrect };
            var result =_solver.FilterWithEntropy("feuille", patterns, new WordSearcher(possibleSolution));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void StressTests2()
        {
            Dictionary<string, float> possibleSolution = new CsvReader().GetAllWords("Lexique381.csv")
                .Where(t => t.Key.Length == 7).ToDictionary(t => t.Key, t => t.Value);
            var _solver = new WordleSolver();
            var result = _solver.CalculateEntropy("feuille", possibleSolution);
            Assert.IsTrue(Math.Abs(result - 6.908886) < 0.000001);
        }

        [TestMethod]
        public void StressTests3()
        {
            Dictionary<string, float> possibleSolution = new CsvReader().GetAllWords("Lexique381.csv");
            var _solver = new WordleSolver();
            var result = _solver.FilterWithEntropy(new WordSearcher(possibleSolution.Where(t=>t.Key.Length==4).ToDictionary(t=>t.Key, t=>t.Value)) );
            Assert.IsNotNull(result);
        }

        //private bool IsAllowedPattern(string word, IEnumerable<Pattern> pattern)
        //{
        //    //RIER
        //    //MXXI Allowed
        //    //IXXM Not Allowed
        //    //=> For the same letters, the I pattern should always be last
        //    var test = pattern.Select((pat, i) => new {character = word[i], index = i, pat}).GroupBy(t => t.character);

        //    bool any = test
        //        .Where(tuple =>
        //            tuple.Any(t => t.pat == Pattern.Misplaced) &&
        //            tuple.Any(t => t.pat == Pattern.Incorrect)).Any(tuple =>
        //            tuple.Where(t => t.pat == Pattern.Misplaced).Max(t => t.index) >
        //            tuple.Where(t => t.pat == Pattern.Incorrect).Min(t => t.index));

        //    return !any;
        //}

    }
}