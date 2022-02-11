using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Wordle.BLL;

namespace Wordle
{
    public class WordleSolver
    {
        public List<KeyValuePair<string, float>> Filter(string word, IEnumerable<Pattern> pattern, WordSearcher searcher)
        {
            searcher.SetWordLength(pattern.Count());

            foreach (var tuple in pattern.Select((pat, i) => new { character = word[i], index = i, pat}).GroupBy(t=> t.character))
            {
                if (tuple.Any(t => t.pat == Pattern.Incorrect))
                    searcher.AddCharacterCount(tuple.Key, tuple.Count(t => t.pat != Pattern.Incorrect));

                else
                {
                    searcher.AddAtLeastCharacterCount(tuple.Key, tuple.Count());

                    foreach (var triple in tuple)
                    {
                        if (triple.pat == Pattern.Correct) searcher.AddCharPosToMatch(triple.character, triple.index);
                        else searcher.AddCharPosToNotMatch(triple.character, triple.index);
                    }
                }
            }

            var keyValuePairs = searcher.Search().ToList();
            return keyValuePairs;
        }

        public IEnumerable<KeyValuePair<string, float>> FilterWithEntropy(string word, IEnumerable<Pattern> pattern,
            WordSearcher wordSearcher)
        {
            var result = Filter(word, pattern, wordSearcher).ToDictionary(t=>t.Key, t=>t.Value);
            
            return result.AsParallel().Select(keyValuePair => new KeyValuePair<string, float>(keyValuePair.Key, CalculateEntropy(keyValuePair.Key, result))).ToList();
        }

        public float CalculateEntropy(string word, Dictionary<string,float> wordDico)
        {

            var allPossiblePatternCombination = CartesianProduct(word.Select(_ => new List<Pattern>
                {Pattern.Correct, Pattern.Incorrect, Pattern.Misplaced}));

            return (float)allPossiblePatternCombination.Select(pattern =>
            {
                var wordSearcher = new WordSearcher(wordDico);
                wordSearcher.SetWordLength(word.Length);
                float totalWord = wordSearcher.Search().Count();
                return Filter(word, pattern, wordSearcher).Count / totalWord;
            }).Select(t => t > 0 ? t * Math.Log2(1 / t) : 0).Sum();
        }

        public IEnumerable<IEnumerable<T>> CartesianProduct<T>(
            IEnumerable<IEnumerable<T>> sequences)
        {
            IEnumerable<IEnumerable<T>> emptyProduct = new[] { Enumerable.Empty<T>() };
            return sequences.Aggregate(
                emptyProduct,
                (accumulator, sequence) =>
                    from accseq in accumulator
                    from item in sequence
                    select accseq.Concat(new[] { item }));
        }
    }
}
