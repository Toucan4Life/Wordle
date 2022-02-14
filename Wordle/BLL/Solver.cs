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

            if (firstCharacter != char.MinValue)
                wordSearcher.AddCharPosToMatch(firstCharacter,0);
            
            return GetEntropy(wordSearcher.Search().ToDictionary(t => t.Key, t => t.Value));
        }

        public IEnumerable<KeyValuePair<string, float>> FilterWithEntropy(string word, IEnumerable<Pattern> pattern,
            WordSearcher wordSearcher)
        {
            return GetEntropy(new Rule().Filter(word, pattern, wordSearcher).ToDictionary(t => t.Key, t => t.Value));
        }

        public IEnumerable<KeyValuePair<string, float>> GetEntropy(Dictionary<string, float> wordDico)
        {
            return wordDico.AsParallel().Select(keyValuePair =>
                new KeyValuePair<string, float>(keyValuePair.Key, CalculateEntropy(keyValuePair.Key, wordDico)));
        }

        public float CalculateEntropy(string actualWord, Dictionary<string, float> wordDico)
        {
            return (float) wordDico.Select(word => new Rule().GetPattern(actualWord, word.Key))
                .GroupBy(t => t, new ListEqualityComparer<Pattern>())
                .Select(t => (float) t.Count() / wordDico.Count * Math.Log2(wordDico.Count / (float) t.Count())).Sum();
        }
    }
}
