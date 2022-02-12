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
        private readonly Dictionary<string, float> _wordDictionary;

        public Dictionary<char, int> _characterCount = new();
        public Dictionary<char, int> _characterAtLeastCount = new();
        public Dictionary<int, char> _charPosToMatch = new();
        public List<Tuple<int, char>> _charPosToNotMatch = new();
        public int WordLength { get; set; }

        private Dictionary<char, int> _characterCountBackup = new();
        private Dictionary<char, int> _characterAtLeastCountBackup = new();
        private Dictionary<int, char> _charPosToMatchBackup = new();
        private List<Tuple<int, char>> _charPosToNotMatchBackup = new();
        private int _wordLengthBackup;

        public WordSearcher(Dictionary<string, float> wordDictionary)
        {
            _wordDictionary = wordDictionary;
        }

        public void ResetState()
        {
            _characterCount = new Dictionary<char, int>();
            _characterAtLeastCount = new Dictionary<char, int>();
            _charPosToMatch = new Dictionary<int, char>();
            _charPosToNotMatch = new List<Tuple<int, char>>();
            WordLength = 0;
        }

        public void RememberState()
        {
            _characterCountBackup = _characterCount.ToDictionary(entry => entry.Key, entry => entry.Value);
            _characterAtLeastCountBackup = _characterAtLeastCount.ToDictionary(entry => entry.Key, entry => entry.Value);
            _charPosToMatchBackup = _charPosToMatch.ToDictionary(entry => entry.Key, entry => entry.Value);
            _charPosToNotMatchBackup = new List<Tuple<int, char>>(_charPosToNotMatch);
            _wordLengthBackup = WordLength;
        }
        public void RollbackState()
        {
            _characterCount = _characterCountBackup.ToDictionary(entry => entry.Key, entry => entry.Value);
            _characterAtLeastCount = _characterAtLeastCountBackup.ToDictionary(entry => entry.Key, entry => entry.Value);
            _charPosToMatch = _charPosToMatchBackup.ToDictionary(entry => entry.Key, entry => entry.Value);
            _charPosToNotMatch = new List<Tuple<int, char>>(_charPosToNotMatchBackup);
            WordLength = _wordLengthBackup;
            //_characterCountBackup = new();
            //_characterAtLeastCountBackup = new();
            //_charPosToMatchBackup = new();
            //_charPosToNotMatchBackup = new();
            //_wordLengthBackup = 0;
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
            return _wordDictionary.Where(word=>IsWordConformToRule(word.Key));
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
