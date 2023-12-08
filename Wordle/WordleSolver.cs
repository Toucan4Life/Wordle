using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        public WordleSolver(int wordLength, char firstchar = char.MinValue)
        {
            var wordDico = new CsvReader().GetAllWords("SAL/Lexique381.csv")
                .Where(t => t.Key.Length == wordLength);

            _searcher = new WordSearcher(firstchar == char.MinValue
                ? wordDico
                : wordDico.Where(t => t.Key[0] == firstchar)) {WordLength = wordLength};
        }

        private IEnumerable<KeyValuePair<string, float>> RetrievePossibleWords()
        {
            return _searcher.Search();
        }

        public IEnumerable<WordleEntity> RetrieveRecommendedWords()
        {
            var possibleWords = RetrievePossibleWords();
            var retrieveRecommendedWords = from actualWord in
                    _searcher.WordDictionary.AsParallel()
                select new KeyValuePair<string, float>(actualWord.Key,
                    new Entropy().Calculate(possibleWords.Select(word => new Rule().GetPattern(actualWord.Key, word.Key))
                        .ToList()));
            return from poss in possibleWords
                   join rec in retrieveRecommendedWords on poss.Key equals rec.Key
                select new WordleEntity(poss.Key, poss.Value, rec.Value);
        }

        public void ApplyWordPattern(string word, string patterns)
        {
            new Rule().Filter(word, patterns.Select(MapPattern), _searcher);
        }

        public float CalculateUniformEntropy(int count)
        {
            return new Entropy().CalculateEntropy(Enumerable.Range(0, count).Select(_ => (float)1 / count));
        }

        private static Pattern MapPattern(char c)
        {
            return c switch
            {
                '0' => Pattern.Incorrect,
                '1' => Pattern.Misplaced,
                '2' => Pattern.Correct,
                _ => throw new ArgumentOutOfRangeException("Pattern not supported")
            };
        }
    }

    public class WordleStartParameter
    {
        [Required]
        [Range(1, 30)]
        public int WordLength { get; set; }

        [StringLength(1, ErrorMessage = "Input is too long.")]
        public string? FirstChar { get; set; }
    }

    public class WordleStepParameter
    {
        [Required]
        public string? Word { get; set; }

        [Required]
        public string? Pattern { get; set; }
    }

    public record WordleEntity(string Name, float Frequency, float Entropy);

}
