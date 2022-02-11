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

        public IEnumerable<KeyValuePair<string, float>> Filter(string word, List<Pattern> pattern)
        {
            _searcher.SetWordLength(pattern.Count);

            foreach (var tuple in pattern.Select((pat, i) => new { character = word[i], index = i, pat}).GroupBy(t=> t.character))
            {
                if (tuple.Any(t => t.pat == Pattern.Incorrect))
                    _searcher.AddCharacterCount(tuple.Key, tuple.Count(t => t.pat != Pattern.Incorrect));

                else
                {
                    _searcher.AddAtLeastCharacterCount(tuple.Key, tuple.Count());

                    foreach (var triple in tuple)
                    {
                        if (triple.pat == Pattern.Correct) _searcher.AddCharPosToMatch(triple.character, triple.index);
                        else _searcher.AddCharPosToNotMatch(triple.character, triple.index);
                    }
                }
            }

            return _searcher.Search();
        }
    }
}
