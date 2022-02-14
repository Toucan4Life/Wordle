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

        public IEnumerable<KeyValuePair<string, float>> FilterWithEntropy(WordSearcher wordSearcher)
        {
            return wordSearcher._wordDictionary.AsParallel().Select(keyValuePair =>
                new KeyValuePair<string, float>(keyValuePair.Key, CalculateEntropy(keyValuePair.Key, wordSearcher._wordDictionary))).ToList();
        }
        public IEnumerable<KeyValuePair<string, float>> FilterWithEntropy(string word, IEnumerable<Pattern> pattern,
            WordSearcher wordSearcher)
        {
            var rule = new Rule();
            var result = rule.Filter(word, pattern, wordSearcher).ToDictionary(t=>t.Key, t=>t.Value);
            
            return result.AsParallel().Select(keyValuePair => new KeyValuePair<string, float>(keyValuePair.Key, CalculateEntropy(keyValuePair.Key, result))).ToList();
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
