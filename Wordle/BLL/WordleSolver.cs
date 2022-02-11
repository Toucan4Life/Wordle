using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Wordle.BLL;

namespace Wordle
{
    public class WordleSolver
    {
        private WordSearcher _searcher;

        public WordleSolver(Dictionary<string, float> wordDictionary)
        {
            _searcher = new WordSearcher(wordDictionary);
        }

        public void Reset()
        {
            _searcher.Reset();
        }

        public IEnumerable<KeyValuePair<string, float>> Filter(string word, string pattern)
        {
            _searcher.SetWordLenght(pattern.Length);

            foreach (var tuple in pattern.Select((pat, i) => new { character = word[i], index = i, pat}).GroupBy(t=> t.character))
            {
                if (tuple.Any(t => t.pat == '.'))
                    _searcher.AddCharacterCount(tuple.Key, tuple.Count(t => t.pat != '.'));

                else
                {
                    _searcher.AddAtLeastCharacterCount(tuple.Key, tuple.Count());

                    foreach (var triple in tuple)
                    {
                        if (triple.pat == '!') _searcher.AddCharPosToMatch(triple.character, triple.index);
                        else _searcher.AddCharPosToNotMatch(triple.character, triple.index);
                    }
                }
            }

            return _searcher.Search();
        }
    }
}
