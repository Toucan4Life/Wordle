﻿using System;
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

        public Dictionary<string, float> Filter(string word, string pattern)
        {
            CheckCorrectCharRule(word,pattern);
            CheckMisPlacedCharRule(word,pattern);
            CheckNotPresentCharRule(word,pattern);

            return _searcher.Search().OrderByDescending(t=>t.Value).Take(20).ToDictionary(t => t.Key, t => t.Value);
        }

        public void CheckCorrectCharRule(string word, string pattern)
        {
            var stringToMatch = new StringBuilder();
            
            for (var i = 0 ; i < pattern.Length ; i++)
            {
                stringToMatch = stringToMatch.Append(pattern[i] == '!' ? word[i] : '.');
            }

            _searcher.AddRegexToMatch(new Regex('^' + stringToMatch.ToString() + '$'));
        }

        public void CheckMisPlacedCharRule(string word, string pattern)
        {
            var stringNotToMatch = new StringBuilder();

            var i = 0;
            if (pattern.All(t => t != '?'))
            {
                return;
            }
            foreach (var chara in pattern)
            {
                if (chara == '?')
                {
                    _searcher.AddAtLeastCharacterCount(word[i], word.Where((charInWord, j) => charInWord == word[i] && pattern[j] != '.').Count());

                    stringNotToMatch = stringNotToMatch.Append( word[i]);
                }
                else
                {
                    stringNotToMatch = stringNotToMatch.Append('.');
                }

                i++;
            }

            _searcher.AddRegexesNotToMatch(new Regex(stringNotToMatch.ToString()));

        }

        public void CheckNotPresentCharRule(string word, string pattern)
        {
            var i = 0;

            foreach (var chara in pattern)
            {
                if (chara == '.')
                {
                    _searcher.AddCharacterCount(word[i], word.Where((charInWord, j) => charInWord == word[i] && pattern[j] != '.').Count());
                }

                i++;
            }

        }
    }
}
