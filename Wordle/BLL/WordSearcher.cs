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
        private Dictionary<char, int> _characterCount = new();
        private Dictionary<char, int> _characterAtLeastCount = new();
        private Dictionary<int, char> _charPosToMatch = new();
        private List<Tuple<int, char>> _charPosToNotMatch = new();
        private int _wordLength = 0;
        private readonly Dictionary<string, float> _wordDictionary;

        public WordSearcher(Dictionary<string, float> wordDictionary)
        {
            _wordDictionary = wordDictionary;
        }

        public void Reset()
        {
            _characterCount = new Dictionary<char, int>();
            _characterAtLeastCount = new Dictionary<char, int>();
            _charPosToMatch = new Dictionary<int, char>();
            _charPosToNotMatch = new List<Tuple<int, char>>();
            _wordLength = 0;
        }

        public void SetWordLenght(int lenght)
        {
            _wordLength = lenght;
        }

        public void AddCharPosToMatch(char character, int pos)
        {
            if (!_charPosToMatch.ContainsKey(character))
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
            return word.Length == _wordLength &&
                   _characterCount.All(t => word.Count(v => v == t.Key) == t.Value) &&
                   _characterAtLeastCount.All(t => word.Count(v => v == t.Key) >= t.Value) &&
                   _charPosToMatch.All(charpos => word[charpos.Key] == charpos.Value) &&
                   _charPosToNotMatch.All(charpos => word[charpos.Item1] != charpos.Item2);
        }
    }

}
