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
            CheckCorrectCharRule(word,pattern);
            CheckMisPlacedCharRule(word,pattern);
            CheckNotPresentCharRule(word,pattern);

            return _searcher.Search().OrderByDescending(t=>t.Value);
        }

        public void CheckCorrectCharRule(string word, string pattern)
        {
            
            for (var i = 0 ; i < pattern.Length ; i++)
            {
                if (pattern[i] == '!')
                    _searcher.AddCharPosToMatch(word[i],i);
            }
        }

        public void CheckMisPlacedCharRule(string word, string pattern)
        {
            if (pattern.All(t => t != '?'))
            {
                return;
            }
            for (var i = 0; i < pattern.Length; i++)
            {
                if (pattern[i] == '?')
                {
                    _searcher.AddAtLeastCharacterCount(word[i], CountOccurenceOfPresentCharacterInWord(word, pattern, i));
                    _searcher.AddCharPosToNotMatch(word[i], i);
                }

                i++;
            }

        }
        public int CountOccurenceOfPresentCharacterInWord(string word, string pattern, int i)
        {
            return word.Where((charInWord, j) => charInWord == word[i] && pattern[j] != '.').Count();
        }

        public void CheckNotPresentCharRule(string word, string pattern)
        {

            for (var i = 0; i < pattern.Length; i++)
            {
                if (pattern[i] == '.')
                {
                    _searcher.AddCharacterCount(word[i], CountOccurenceOfPresentCharacterInWord(word, pattern, i));
                }
            }

        }
    }
}
