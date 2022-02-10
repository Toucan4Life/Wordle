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
        public List<Regex> regexesNotToMatch = new();
        public List<Regex> regexesToMatch = new();
        public Dictionary<char, int> characterCount = new();
        public Dictionary<char, int> characterAtLeastCount = new();

        public void Reset()
        {
            regexesNotToMatch = new List<Regex>();
            regexesToMatch = new List<Regex>();
            characterCount = new Dictionary<char, int>();
            characterAtLeastCount = new Dictionary<char, int>();
        }

        public void AddRegexToMatch(Regex regex)
        {
            regexesToMatch.Add(regex);
        }

        public void AddRegexesNotToMatch(Regex regex)
        {
            regexesNotToMatch.Add(regex);
        }

        public Dictionary<char, int> AddCharacterCount(char character, int count)
        {
            if (characterCount.ContainsKey(character))
            {
                characterCount[character] = Math.Max(characterCount[character], count);
            } else
            {
                characterCount[character] = count;
            }


            return characterCount;
        }
    }

}
