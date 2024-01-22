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
        private Entropy _entropy;
        public  WordleSolver(int wordLength, char firstchar = char.MinValue)
        {
            var wordDico = new CsvReader()
                .GetAllWords(System.AppDomain.CurrentDomain.BaseDirectory + "/SAL/Lexique381.csv")
                .Where(t => t.Key.Length == wordLength);

            _searcher = new WordSearcher(firstchar == char.MinValue
                ? wordDico
                : wordDico.Where(t => t.Key[0] == firstchar)) {WordLength = wordLength};
            _entropy = new Entropy();
        }

        public IEnumerable<WordleEntity> RetrieveRecommendedWords(List<Tuple<string,string>> patterns)
        {
            patterns.ForEach(pattern=>
            {
                Rule.Filter(pattern.Item1, pattern.Item2.Select(MapPattern), _searcher);
            });

            var possibleWords = _searcher.Search().Select(t => t.Key).ToList();

            var dictionaryWithEntropy = _searcher.WordDictionary.AsParallel().Select(word =>
                new WordleEntity(word.Key, word.Value, EntropyByWord(word.Key, possibleWords),false)).ToList();


            var t = from d in dictionaryWithEntropy
                join p in possibleWords on d.Name equals p into gj
                from subpet in gj.DefaultIfEmpty()
                select new WordleEntity(d.Name, d.Frequency, EntropyByWord(d.Name, possibleWords),subpet!=null);
            return t;
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
