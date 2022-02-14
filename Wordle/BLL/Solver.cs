using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Wordle.BLL;

namespace Wordle
{
    public class Solver
    {
        public IEnumerable<KeyValuePair<string, float>> FilterWithEntropy(int length, Dictionary<string, float> wordDico, char firstCharacter = char.MinValue)
        {
            var wordSearcher = new WordSearcher(wordDico) {WordLength = length};

            if (firstCharacter!=char.MinValue)
                wordSearcher.AddCharPosToMatch(firstCharacter,0);

            var dictionary = wordSearcher.Search().ToDictionary(t => t.Key, t => t.Value);

            return GetEntropy(dictionary);
        }

        public IEnumerable<KeyValuePair<string, float>> FilterWithEntropy(string word, IEnumerable<Pattern> pattern,
            WordSearcher wordSearcher)
        {
            var result = new Rule().Filter(word, pattern, wordSearcher).ToDictionary(t=>t.Key, t=>t.Value);
            
            return GetEntropy(result);
        }

        public IEnumerable<KeyValuePair<string, float>> GetEntropy(Dictionary<string, float> wordDico)
        {
            return wordDico.AsParallel().Select(keyValuePair =>
                    new KeyValuePair<string, float>(keyValuePair.Key, CalculateEntropy(keyValuePair.Key, wordDico)))
                .ToList();
        }

        public float CalculateEntropy(string actualWord, Dictionary<string, float> wordDico)
        {
            var rule = new Rule();
            var patternsList = wordDico.Select(word => rule.GetPattern(actualWord, word.Key)).ToList();

            var test = patternsList.GroupBy(t => t, new ListEqualityComparer<Pattern>())
                .Select(t => (float) t.Count() / wordDico.Count).OrderByDescending(t=>t);

            var sum = (float) test.Select(t => t * Math.Log2(1 / t)).Sum();

            return sum;
        }
    }
}
