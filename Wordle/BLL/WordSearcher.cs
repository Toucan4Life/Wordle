using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Wordle.BLL
{
    public class WordSearcher(IEnumerable<KeyValuePair<string, float>> wordDictionary) : IEnumerable<KeyValuePair<string, float>>
    {
        public IEnumerable<KeyValuePair<string, float>> WordDictionary { get; } = wordDictionary;
        private readonly Dictionary<char, int> _characterCount = new();
        private readonly Dictionary<char, int> _characterAtLeastCount = new();
        private readonly Dictionary<int, char> _charPosToMatch = new();
        private readonly List<Tuple<int, char>> _charPosToNotMatch = new();

        public int WordLength { get; set; }

        public void SetWordLength(int length)
        {
            WordLength = length;
        }

        public void AddCharPosToMatch(char character, int pos)
        {
            _charPosToMatch.TryAdd(pos, character);
        }
        public void AddCharPosToNotMatch(char character, int pos)
        {
            _charPosToNotMatch.Add(new Tuple<int, char>(pos, character));
        }

        public void AddCharacterCount(char character, int count)
        {
            if (_characterCount.TryGetValue(character, out int value))
            {
                _characterCount[character] = Math.Max(value, count);
            } else
            {
                _characterAtLeastCount.Remove(character);
                _characterCount[character] = count;
            }
        }

        public void AddAtLeastCharacterCount(char character, int count)
        {
            if (_characterAtLeastCount.TryGetValue(character, out int value))
            {
                _characterAtLeastCount[character] = Math.Max(value, count);
            }
            else
            {
                _characterAtLeastCount[character] = count;
            }
        }

        public bool IsWordConformToRule(string word)
        {
            return word.Length == WordLength && _charPosToMatch.All(charpos => word[charpos.Key] == charpos.Value) &&
                   _charPosToNotMatch.All(charpos => word[charpos.Item1] != charpos.Item2) &&
                   _characterCount.All(t => word.Count(v => v == t.Key) == t.Value) &&
                   _characterAtLeastCount.All(t => word.Count(v => v == t.Key) >= t.Value);
        }

        public WordSearcher Filter(string word, IEnumerable<Pattern> pattern)
        {
            SetWordLength(pattern.Count());

            foreach (var tuple in pattern.Select((pat, i) => new { character = word[i], index = i, pat }).GroupBy(t => t.character))
            {
                if (tuple.Any(t => t.pat == Pattern.Incorrect))
                    AddCharacterCount(tuple.Key, tuple.Count(t => t.pat != Pattern.Incorrect));

                else
                    AddAtLeastCharacterCount(tuple.Key, tuple.Count());

                foreach (var triple in tuple)
                {
                    if (triple.pat == Pattern.Correct)
                        AddCharPosToMatch(triple.character, triple.index);
                    else
                        AddCharPosToNotMatch(triple.character, triple.index);
                }
            }

            return new WordSearcher(WordDictionary.Where(word => IsWordConformToRule(word.Key)));
        }

        public IEnumerator<KeyValuePair<string, float>> GetEnumerator()
        {
            return WordDictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

}
