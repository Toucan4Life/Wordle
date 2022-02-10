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
        Regex? regexesToMatch;
        List<Tuple<char, int>> characterCount = new();
        List<Tuple<char, int>> characterAtLeastCount = new();
    }
}
