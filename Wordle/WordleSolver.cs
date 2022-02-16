using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wordle.BLL;
using Wordle.SAL;

namespace Wordle
{
    public class WordleSolver : IWordleSolver
    {
        private WordSearcher _searcher;

        public WordleSolver(int wordLength)
        {
            _searcher = new WordSearcher(new CsvReader().GetAllWords("SAL/Lexique381.csv")
                .Where(t => t.Key.Length == wordLength));
        }

        public IEnumerable<KeyValuePair<string, float>> RetrievePossibleWords()
        {
            return _searcher.Search();
        }

        public IEnumerable<KeyValuePair<string, float>> RetrieveRecommendedWords()
        {
            return from actualWord in
                    _searcher.WordDictionary.AsParallel()
                select new KeyValuePair<string, float>(actualWord.Key,
                    CalculateEntropy(_searcher.Search().Select(word => new Rule().GetPattern(actualWord.Key, word.Key))
                        .ToList()));
        }

        public void ApplyWordPattern(string word, List<Pattern> patterns)
        {
            new Rule().Filter(word, patterns, _searcher);
        }

        private float CalculateEntropy(List<List<Pattern>> patterns)
        {
            return (float) patterns
                .GroupBy(t => t, new ListEqualityComparer<Pattern>())
                .Select(t => (float) t.Count() / patterns.Count * Math.Log2(patterns.Count / (float) t.Count()))
                .Sum();
        }
    }
}
