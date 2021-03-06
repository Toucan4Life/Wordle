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
        public IEnumerable<KeyValuePair<string, float>> WordDictionary { get; }
        private readonly Dictionary<char, int> _characterCount = new();
        private readonly Dictionary<char, int> _characterAtLeastCount = new();
        private readonly Dictionary<int, char> _charPosToMatch = new();
        private readonly List<Tuple<int, char>> _charPosToNotMatch = new();

        public int WordLength { get; set; }

        public WordSearcher(IEnumerable<KeyValuePair<string, float>> wordDictionary)
        {
            WordDictionary = wordDictionary;
        }

        public void SetWordLength(int length)
        {
            WordLength = length;
        }

        public void AddCharPosToMatch(char character, int pos)
        {
            if (!_charPosToMatch.ContainsKey(pos))
                _charPosToMatch.Add(pos, character);
        }
        public void AddCharPosToNotMatch(char character, int pos)
        {
            _charPosToNotMatch.Add(new Tuple<int, char>(pos, character));
        }

        public void AddCharacterCount(char character, int count)
        {
            if (_characterCount.ContainsKey(character))
            {
                _characterCount[character] = Math.Max(_characterCount[character], count);
            } else
            {
                if (_characterAtLeastCount.ContainsKey(character))
                {
                    _characterAtLeastCount.Remove(character);

                }
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
            return WordDictionary.Where(word=>IsWordConformToRule(word.Key)).ToList();
        }

        public bool IsWordConformToRule(string word)
        {
            return word.Length == WordLength && _charPosToMatch.All(charpos => word[charpos.Key] == charpos.Value) &&
                   _charPosToNotMatch.All(charpos => word[charpos.Item1] != charpos.Item2) &&
                   _characterCount.All(t => word.Count(v => v == t.Key) == t.Value) &&
                   _characterAtLeastCount.All(t => word.Count(v => v == t.Key) >= t.Value);
        }
    }

}
