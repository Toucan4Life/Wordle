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
            var patterns = new List<Pattern> {Pattern.Incorrect, Pattern.Incorrect, Pattern.Misplaced, Pattern.Misplaced, Pattern.Incorrect, Pattern.Incorrect, Pattern.Incorrect };
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
            Assert.IsTrue(result< Math.Log2(Math.Pow(3, 7)));
        }
        [TestMethod]
        public void CalculateEntropy()
        {
            var word = "usagers";
            var wordDico = new CsvReader().GetAllWords("Lexique381.csv").Where(t => t.Key.Length == 7)
                .ToDictionary(t => t.Key, t => t.Value);

            var _solver = new WordleSolver();

            var allPossiblePatternCombination = CartesianProduct(word.Select(_ => new List<Pattern>
                {Pattern.Correct, Pattern.Incorrect, Pattern.Misplaced})).Where(pat=>IsAllowedPattern(word,pat));

            var testDictionarry = new Dictionary<string, float>();
            float totalWord = wordDico.Count(t => t.Key.Length==word.Length);

            var enumerable = allPossiblePatternCombination.Select(pattern => {
                var wordSearcher = new WordSearcher(wordDico) { WordLength = word.Length };
                
                var keyValuePairs = _solver.Filter(word, pattern, wordSearcher);
                keyValuePairs.ForEach(t => testDictionarry.Add(t.Key, t.Value));
                return keyValuePairs.Count / totalWord;
            }).OrderByDescending(t=>t);

            Assert.IsTrue(Math.Abs(enumerable.Sum() - 1f) < 0.0000001);
        }

        private bool IsAllowedPattern(string word, IEnumerable<Pattern> pattern)
        {
            //RIER
            //MXXI Allowed
            //IXXM Not Allowed
            //=> For the same letters, the I pattern should always be last
            var test = pattern.Select((pat, i) => new {character = word[i], index = i, pat}).GroupBy(t => t.character);

            bool any = test
                .Where(tuple =>
                    tuple.Any(t => t.pat == Pattern.Misplaced) &&
                    tuple.Any(t => t.pat == Pattern.Incorrect)).Any(tuple =>
                    tuple.Where(t => t.pat == Pattern.Misplaced).Max(t => t.index) >
                    tuple.Where(t => t.pat == Pattern.Incorrect).Min(t => t.index));

            return !any;
        }

        public IEnumerable<IEnumerable<T>> CartesianProduct<T>(
            IEnumerable<IEnumerable<T>> sequences)
        {
            IEnumerable<IEnumerable<T>> emptyProduct = new[] { Enumerable.Empty<T>() };
            return sequences.Aggregate(
                emptyProduct,
                (accumulator, sequence) =>
                    from accseq in accumulator
                    from item in sequence
                    select accseq.Concat(new[] { item }));
        }

    }
}