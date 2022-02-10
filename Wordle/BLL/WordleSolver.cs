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
        Regex? regexesToMatch ;
        IEnumerable<char> characterNotPresent = new List<char>();

        public IEnumerable<char> CharacterPresent
        {
            get => _characterPresent.Concat( regexesToMatch.ToString()[1..^1].Replace(".", string.Empty));
            private set => _characterPresent = value;
        }

        List<Tuple<char, int>> characterCount = new List<Tuple<char, int>>();
        private readonly Dictionary<string, float> _wordDictionary;
        private IEnumerable<char> _characterPresent = new List<char>();

        public WordleSolver(Dictionary<string, float> wordDictionary)
        {
            _wordDictionary = wordDictionary;
        }

        public void Reset()
        {
            regexesNotToMatch = new List<Regex>();
            regexesToMatch = null;
            characterNotPresent = new List<char>();
            CharacterPresent = new List<char>();
            characterCount = new List<Tuple<char, int>>();
        }

        public Dictionary<string, float> Filter(string word, string pattern)
        {
            CheckCorrectCharRule(word,pattern);
            CheckMisPlacedCharRule(word,pattern);
            CheckNotPresentCharRule(word,pattern);

            return _wordDictionary.Where(word => Predicate(word.Key, regexesToMatch, regexesNotToMatch, characterNotPresent, CharacterPresent))
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
            if (pattern.Count(t => t == '?') == 0)
            {
                return;
            }
            foreach (var chara in pattern)
            {
                if (chara == '?')
                {
                    CharacterPresent=CharacterPresent.Append(word[i]);
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
                    if ( !CharacterPresent.Contains(chara))
                    {
                        characterNotPresent = characterNotPresent.Append(chara);
                    }
                    else
                    {
                       characterCount.Add(new Tuple<char, int>(chara, CharacterPresent.Count(t => t == chara)));
                    }
                }
            }

        }

        bool Predicate(string word, Regex? regex,
            IEnumerable<Regex> regexNotToMatch, IEnumerable<char> notInWordChar , IEnumerable<char> inwordchar)
        {
            var isRegexMatch = regex?.IsMatch(word) ?? true;
            var isRegexNotMatch = regexNotToMatch.All(reg => !reg.IsMatch(word));
            var isAllCharPresent = inwordchar.All(word.Contains);
            var isComposedOfAllowedCharacterOnly = !notInWordChar.Any(word.Contains);
            var IsCountCorrect = characterCount.All(t => word.Count(v => v == t.Item1) == t.Item2);
            return (regex?.IsMatch(word) ?? true) && regexNotToMatch.All(reg => !reg.IsMatch(word)) &&
                   !notInWordChar.Any(word.Contains) && inwordchar.All(word.Contains)&& characterCount.All(t=> word.Count(v=>v==t.Item1)==t.Item2);
        }
    }
}
