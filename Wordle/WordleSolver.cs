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

        public WordleSolver(int wordLength, char firstchar = char.MinValue)
        {
            var wordDico = new CsvReader().GetAllWords("SAL/Lexique381.csv")
                .Where(t => t.Key.Length == wordLength);

            _searcher = new WordSearcher(firstchar == char.MinValue
                ? wordDico
                : wordDico.Where(t => t.Key[0] == firstchar)) {WordLength = wordLength};
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
                    new Entropy().Calculate(_searcher.Search().Select(word => new Rule().GetPattern(actualWord.Key, word.Key))
                        .ToList()));
        }

        public void ApplyWordPattern(string word, List<Pattern> patterns)
        {
            new Rule().Filter(word, patterns, _searcher);
        }

        public float CalculateEntropy(List<float> probabilities)
        {
            return new Entropy().CalculateEntropy(probabilities);
        }
    }
}
