using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Wordle.BLL
{
    public class WordSearcher
    {
        private List<Regex> _regexesNotToMatch = new();
        private List<Regex> _regexesToMatch = new();
        private Dictionary<char, int> _characterCount = new();
        private Dictionary<char, int> _characterAtLeastCount = new();
        private readonly Dictionary<string, float> _wordDictionary;

        public WordSearcher(Dictionary<string, float> wordDictionary)
        {
            _wordDictionary = wordDictionary;
        }

        public void Reset()
        {
            _regexesNotToMatch = new List<Regex>();
            _regexesToMatch = new List<Regex>();
            _characterCount = new Dictionary<char, int>();
            _characterAtLeastCount = new Dictionary<char, int>();
        }

        public void AddRegexToMatch(Regex regex)
        {
            _regexesToMatch.Add(regex);
        }

        public void AddRegexesNotToMatch(Regex regex)
        {
            _regexesNotToMatch.Add(regex);
        }

        public void AddCharacterCount(char character, int count)
        {
            if (_characterCount.ContainsKey(character))
            {
                _characterCount[character] = Math.Max(_characterCount[character], count);
            } else
            {
                _characterCount[character] = count;
            }
        }

        public void AddAtLeastCharacterCount(char character, int count)
        {
            if (_characterAtLeastCount.ContainsKey(character))
            {
                _characterAtLeastCount[character] = Math.Max(_characterAtLeastCount[character], count);
            }
            else
            {
                _characterAtLeastCount[character] = count;
            }
        }

        public IEnumerable<KeyValuePair<string, float>> Search()
        {
            return _wordDictionary.Where(word=>IsWordConformToRule(word.Key));
        }

        public bool IsWordConformToRule(string word)
        {
            return _regexesToMatch.All(reg => reg.IsMatch(word)) && _regexesNotToMatch.All(reg => !reg.IsMatch(word)) &&
                   _characterCount.All(t => word.Count(v => v == t.Key) == t.Value) &&
                   _characterAtLeastCount.All(t => word.Count(v => v == t.Key) >= t.Value);
        }
    }

}
