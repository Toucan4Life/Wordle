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

        public IOrderedEnumerable<KeyValuePair<string, float>> Filter(string word, string pattern)
        {
            _searcher.SetWordLenght(pattern.Length);

            var dictionary = new Dictionary<char, List<Tuple<int, char>>>();

            for (var i = 0; i < pattern.Length; i++)
            {
                if (!dictionary.ContainsKey(word[i]))
                    dictionary.Add(word[i], new List<Tuple<int, char>> {new(i, pattern[i])});
                else
                {
                    dictionary[word[i]].Add(new Tuple<int, char>(i, pattern[i]));
                }
            }

            foreach (var tuple in dictionary)
            {
                if (tuple.Value.Select(t => t.Item2).Contains('.'))
                    _searcher.AddCharacterCount(tuple.Key, tuple.Value.Select(t => t.Item2).Count(t => t != '.'));
                else if (tuple.Value.Select(t => t.Item2).Contains('?'))
                {
                    _searcher.AddAtLeastCharacterCount(tuple.Key,
                        tuple.Value.Select(t => t.Item2).Count(t => t != '.'));

                    foreach (var triple in tuple.Value.Where(t => t.Item2 == '?'))
                    {
                        _searcher.AddCharPosToNotMatch(tuple.Key, triple.Item1);
                    }
                }
                else
                {
                    foreach (var triple in tuple.Value.Where(t => t.Item2 == '!'))
                    {
                        _searcher.AddCharPosToMatch(tuple.Key, triple.Item1);
                    }
                }

            }

            return _searcher.Search().OrderByDescending(t => t.Value);
        }
    }
}
