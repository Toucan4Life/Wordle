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
        private List<KeyValuePair<string, float>> _wordDictionary;
        private List<KeyValuePair<string, float>> _allWords;

        public WordleSolver(WordleStartParameter? startParam = null)
        {
            _wordDictionary = new CsvReader()
                .GetAllWords(System.AppDomain.CurrentDomain.BaseDirectory + "/SAL/Lexique381.csv").ToList();
            if (startParam != null)
            {
                _wordDictionary = !string.IsNullOrWhiteSpace(startParam.FirstChar)
                    ? _wordDictionary.Where(w => w.Key[0] == startParam.FirstChar[0]).ToList()
                    : _wordDictionary;

                _wordDictionary = startParam.WordLength != 0
                    ? _wordDictionary.Where(w => w.Key.Length == startParam.WordLength).ToList()
                    : _wordDictionary;
            }
            _allWords=_wordDictionary;
        }

        public IEnumerable<WordleEntity> RetrieveRecommendedWords(List<Tuple<string,string>> patterns)
        {
            _wordDictionary = _wordDictionary
                .Where(word => patterns.Select(wp => new Rule(wp.Item1, wp.Item2.Select(MapPattern)))
                    .All(rule => rule.IsWordConform(word.Key))).ToList();
            var possibleWord = _wordDictionary.Select(t => t.Key).ToList();
            return from d in _allWords.AsParallel()
                join p in possibleWord.AsParallel() on d.Key equals p into gj
                from subpet in gj.DefaultIfEmpty()
                select new WordleEntity(d.Key, d.Value, EntropyByWord(d.Key, possibleWord), subpet != null);
        }

        private static double EntropyByWord(string actualWord, IEnumerable<string> possibleWords)
        {
            var patterns = possibleWords.AsParallel().Select(word => Rule.GetPattern(actualWord, word)).ToList();
            return patterns
                .GroupBy(t => t, new ListEqualityComparer<Pattern>())
                .Select(t =>
                {
                    var proba = (float)t.Count() / patterns.Count;
                    return proba * Math.Log2(1 / proba);
                }).Sum();
        }

        public double CalculateUniformEntropy(int count)
        {
            return Enumerable.Range(0, count).Select(_ =>
            {
                var proba = (float)1 / count;
                return proba * Math.Log2(1 / proba);
            }).Sum();
        }

        private static Pattern MapPattern(char pattern)
        {
            return pattern switch
            {
                '0' => Pattern.Incorrect,
                '1' => Pattern.Misplaced,
                '2' => Pattern.Correct,
                _ => throw new ArgumentOutOfRangeException(nameof(pattern))
            };
        }
    }

    public class WordleStartParameter
    {
        [Required]
        [Range(1, 30)]
        public int WordLength { get; set; }

        [StringLength(1, ErrorMessage = "Input is too long.")]
        public string FirstChar { get; set; }
    }

    public class WordleStepParameter
    {
        [Required]
        public string? Word { get; set; }

        [Required]
        public string? Pattern { get; set; }
    }

    public record WordleEntity(string Name, double Frequency, double Entropy, bool IsCandidate);

}
