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
        List<Regex> regexesNotToMatch = new();
        public List<Regex> regexesToMatch = new();
        Dictionary<char, int> characterCount = new();
        Dictionary<char, int> characterAtLeastCount = new();

        public void Reset()
        {
            regexesNotToMatch = new List<Regex>();
            regexesToMatch = new List<Regex>();
            characterCount = new Dictionary<char, int>();
            characterAtLeastCount = new Dictionary<char, int>();
        }

        public List<Regex> AddRegexToMatch(Regex regex)
        {

            regexesToMatch.Add(regex);

            return regexesToMatch;
        }
    }

}
