using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wordle.BLL;
using Wordle.SAL;

namespace Wordle
{
    public class WordleSolver : IWordleSolver
    {
        private WordSearcher _searcher;

        public WordleSolver()
        {
            _searcher = new WordSearcher(new CsvReader().GetAllWords("SAL/Lexique381.csv"));
        }

        public IEnumerable<KeyValuePair<string, float>> SearchPossibleWord(int wordLenght)
        {
            _searcher.WordLength = wordLenght;

            return _searcher.Search();
        }

        public IEnumerable<KeyValuePair<string, float>> RetrieveCurrentEntropy()
        {
            return new Solver().GetEntropy(_searcher);
        }

        public void ApplyWordPattern(string word, List<Pattern> patterns)
        {
            new Rule().Filter(word, patterns, _searcher);
        }
    }
}
