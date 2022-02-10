using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Wordle
{
    public class WordleSolver
    {
        List<Regex> regexesNotToMatch = new();
        Regex? regexesToMatch;
        Dictionary<char, int> characterCount = new();
        Dictionary<char, int> characterAtLeastCount = new();
        private readonly Dictionary<string, float> _wordDictionary;

        public WordleSolver(Dictionary<string, float> wordDictionary)
        {
            _wordDictionary = wordDictionary;
        }

        public IEnumerable<char> CharacterInRegexToMatch()
        {
            return regexesToMatch.ToString().Replace(".", string.Empty).Replace("$", string.Empty).Replace("^", string.Empty);
        }

        public void Reset()
        {
            regexesNotToMatch = new List<Regex>();
            regexesToMatch = null;
            characterCount = new Dictionary<char, int>();
            characterAtLeastCount = new Dictionary<char, int>();
        }

        public Dictionary<string, float> Filter(string word, string pattern)
        {
            CheckCorrectCharRule(word,pattern);
            CheckMisPlacedCharRule(word,pattern);
            CheckNotPresentCharRule(word,pattern);

            return _wordDictionary.Where(word => Predicate(word.Key, regexesToMatch, regexesNotToMatch))
                .OrderByDescending(t => t.Value).Take(20).ToDictionary(t => t.Key, t => t.Value);
        }

        public void CheckCorrectCharRule(string word, string pattern)
        {
            var stringToMatch = new StringBuilder();
            string regexAlreadyKnow;
            if (regexesToMatch != null)
                regexAlreadyKnow = regexesToMatch.ToString()[1..^1];
            else
                regexAlreadyKnow = "".PadRight(word.Length, '.');

            var i = 0;
            foreach (var chara in regexAlreadyKnow)
            {
                stringToMatch=stringToMatch.Append(chara == '!' ? regexAlreadyKnow[i] : (pattern[i] == '!' ? word[i] : '.'));
                i++;
            }

            regexesToMatch = new Regex('^' + stringToMatch.ToString() + '$');
        }

        public void CheckMisPlacedCharRule(string word, string pattern)
        {
            var stringNotToMatch = new StringBuilder();

            int i = 0;
            if (pattern.All(t => t != '?'))
            {
                return;
            }
            foreach (var chara in pattern)
            {
                if (chara == '?')
                {
                    if(! characterAtLeastCount.ContainsKey(word[i])) characterAtLeastCount.Add(word[i], CharacterInRegexToMatch().Count(t => t == word[i]) + 1);
                    stringNotToMatch = stringNotToMatch.Append( word[i]);
                }
                else
                {
                    stringNotToMatch = stringNotToMatch.Append('.');
                }

                i++;
            }

            regexesNotToMatch.Add(new Regex(stringNotToMatch.ToString()));

        }

        public void CheckNotPresentCharRule(string word, string pattern)
        {
            int i = 0;

            foreach (var chara in pattern)
            {
                if (chara == '.')
                {
                    //Green + not yellow => on a le compte juste
                    if (CharacterInRegexToMatch().Any(t=>t== word[i]) && characterAtLeastCount.Select(t => t.Key).All(p => p != word[i]))
                    {
                        characterCount.Add(word[i], CharacterInRegexToMatch().Count(t => t == word[i]));
                    }
                    //Green + yellow => on un compte pas juste
                    else if (CharacterInRegexToMatch().Any(t => t == word[i]) && characterAtLeastCount.Select(t => t.Key).Any(p => p == word[i]))
                    {
                        if (characterAtLeastCount.ContainsKey(word[i]))
                            characterAtLeastCount[word[i]] = (Math.Max(characterAtLeastCount[word[i]],
                                CharacterInRegexToMatch().Count(t => t == word[i])));
                        else
                            characterAtLeastCount.Add(word[i], CharacterInRegexToMatch().Count(t => t == word[i]));
                    }
                    //yellow + RED => on un compte juste
                    else if (characterAtLeastCount.Select(t => t.Key).Any(p => p == word[i]))
                    {
                        if (!characterCount.ContainsKey(word[i]))  characterCount.Add(word[i], characterAtLeastCount.Single(t => t.Key == word[i]).Value);

                    }
                    //RED=>LettersNotPresent => count = 0
                    else
                    {
                        if(!characterCount.ContainsKey(word[i])) characterCount.Add(word[i], 0);
                    }
                }

                i++;
            }

        }

        bool Predicate(string word, Regex? regex,
            IEnumerable<Regex> regexNotToMatch)
        {
            var isRegexMatch = regex?.IsMatch(word) ?? true;
            var isRegexNotMatch = regexNotToMatch.All(reg => !reg.IsMatch(word));
            var IsCountCorrect = characterCount.All(t => word.Count(v => v == t.Key) == t.Value);
            var IsAtLeastCountCorrect = characterAtLeastCount.All(t => word.Count(v => v == t.Key) >= t.Value);
            return (regex?.IsMatch(word) ?? true) && regexNotToMatch.All(reg => !reg.IsMatch(word))
                  && characterCount.All(t=> word.Count(v=>v==t.Key) ==t.Value) && characterAtLeastCount.All(t => word.Count(v => v == t.Key) >= t.Value);
        }
    }
}
