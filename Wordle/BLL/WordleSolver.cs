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

        public Dictionary<string, float> Filter(string word, string pattern)
        {
            CheckCorrectCharRule(word,pattern);
            CheckMisPlacedCharRule(word,pattern);
            CheckNotPresentCharRule(word,pattern);

            return _searcher.Search().Take(20).ToDictionary(t => t.Key, t => t.Value);
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
                    _searcher.AddAtLeastCharacterCount(word[i], _searcher.CharacterInRegexToMatch().Count(t => t == word[i]) + 1);
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
            int i = 0;

            foreach (var chara in pattern)
            {
                if (chara == '.')
                {
                    //Green + RED => on a le compte juste
                    if (_searcher.CharacterInRegexToMatch().Any(t=>t== word[i]) && _searcher.characterAtLeastCount.Select(t => t.Key).All(p => p != word[i]))
                    {
                        _searcher.AddCharacterCount(word[i], _searcher.CharacterInRegexToMatch().Count(t => t == word[i]));
                    }
                    //Green + yellow => on un compte pas juste
                    else if (_searcher.CharacterInRegexToMatch().Any(t => t == word[i]) && _searcher.characterAtLeastCount.Select(t => t.Key).Any(p => p == word[i]))
                    {
                       _searcher.AddAtLeastCharacterCount(word[i], _searcher.CharacterInRegexToMatch().Count(t => t == word[i]));
                    }
                    //yellow + RED => on un compte juste
                    //todo:wtf
                    else if (_searcher.characterAtLeastCount.Select(t => t.Key).Any(p => p == word[i]))
                    {
                        _searcher.AddCharacterCount(word[i], _searcher.characterAtLeastCount.Single(t => t.Key == word[i]).Value);
                    }
                    //RED=>LettersNotPresent => count = 0
                    else
                    {
                        _searcher.AddCharacterCount(word[i], 0);
                    }
                }

                i++;
            }

        }
    }
}
