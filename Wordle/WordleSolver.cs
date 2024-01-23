using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Wordle.BLL;
using Wordle.SAL;

namespace Wordle
{
    public class WordleSolver : IWordleSolver
    {
        private WordSearcher _searcher;
        public  WordleSolver(int wordLength, char firstchar = char.MinValue)
        {
            var wordDico = new CsvReader()
                .GetAllWords(System.AppDomain.CurrentDomain.BaseDirectory + "/SAL/Lexique381.csv")
                .Where(t => t.Key.Length == wordLength);

            _searcher = new WordSearcher(firstchar == char.MinValue
                ? wordDico
                : wordDico.Where(t => t.Key[0] == firstchar)) {WordLength = wordLength};
        }

        public IEnumerable<WordleEntity> RetrieveRecommendedWords(List<Tuple<string,string>> patterns)
        {
            var allWords = _searcher.WordDictionary;
            patterns.ForEach(pattern=>
            {
                _searcher = _searcher.Filter(pattern.Item1, pattern.Item2.Select(MapPattern));
            });

            var possibleWords = _searcher.Select(t => t.Key).ToList();

            return from d in allWords.AsParallel()
                join p in possibleWords.AsParallel() on d.Key equals p into gj
                from subpet in gj.DefaultIfEmpty()
                select new WordleEntity(d.Key, d.Value, EntropyByWord(d.Key, possibleWords), subpet != null);
        }

        private float EntropyByWord(string actualWord, List<string> possibleWords)
        {
            var patterns = possibleWords.AsParallel().Select(word => Rule.GetPattern(actualWord, word)).ToList();
            var probabilities = patterns
                .GroupBy(t => t, new ListEqualityComparer<Pattern>())
                .Select(t => (float)t.Count() / patterns.Count);
            return Entropy.CalculateEntropy(probabilities);
        }

        public float CalculateUniformEntropy(int count)
        {
            return Entropy.CalculateEntropy(Enumerable.Range(0, count).Select(_ => (float)1 / count));
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

    public record WordleEntity(string Name, float Frequency, float Entropy, bool IsCandidate);

}
