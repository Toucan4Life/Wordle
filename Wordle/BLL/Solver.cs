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
        public IEnumerable<KeyValuePair<string, float>> GetEntropy(WordSearcher wordSearcher)
        {
            var wordDico = wordSearcher.Search();

            var wordSearcherWordDictionary = wordSearcher.WordDictionary.Where(t=>t.Key.Length==wordSearcher.WordLength);
            return wordSearcherWordDictionary.AsParallel().Select(keyValuePair =>
                new KeyValuePair<string, float>(keyValuePair.Key, CalculateEntropy(keyValuePair.Key, wordDico)));
        }

        public float CalculateEntropy(string actualWord, IEnumerable<KeyValuePair<string, float>> wordDico)
        {
            return (float) wordDico.Select(word => new Rule().GetPattern(actualWord, word.Key))
                .GroupBy(t => t, new ListEqualityComparer<Pattern>())
                .Select(t => (float) t.Count() / wordDico.Count() * Math.Log2(wordDico.Count() / (float) t.Count())).Sum();
        }
    }
}
