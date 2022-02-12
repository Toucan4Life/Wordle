using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;
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
                    searcher.AddAtLeastCharacterCount(tuple.Key, tuple.Count());

                foreach (var triple in tuple)
                {
                    switch (triple.pat)
                    {
                        case Pattern.Correct:
                            searcher.AddCharPosToMatch(triple.character, triple.index);
                            break;
                        case Pattern.Misplaced:
                            searcher.AddCharPosToNotMatch(triple.character, triple.index);
                            break;
                        case Pattern.Incorrect:
                            searcher.AddCharPosToNotMatch(triple.character, triple.index);
                            break;
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
                {Pattern.Correct, Pattern.Incorrect, Pattern.Misplaced})).Where(pat => IsAllowedPattern(word, pat)); ;
            int i=0;
            var testDictionarry = new Dictionary<string, float>();
            float totalWord = wordDico.Count(t => t.Key.Length == word.Length);
            var enumerable = allPossiblePatternCombination.Select(pattern =>
            {
                
                i++;
                var wordSearcher = new WordSearcher(wordDico);
                wordSearcher.SetWordLength(word.Length);
                var keyValuePairs = Filter(word, pattern, wordSearcher);
                keyValuePairs.ForEach(t=> testDictionarry.Add(t.Key,t.Value));
                var count = keyValuePairs.Count;
                return count / totalWord;
            }).OrderByDescending(t=>t).ToList();

            if (Math.Abs(enumerable.Sum() - 1) > 0.00001)
            {
                throw new Exception();
            }

            var sum = (float)enumerable.Select(t => t > 0 ? t * Math.Log2(1 / t) : 0).Sum();
   

            return sum;
        }
        private bool IsAllowedPattern(string word, IEnumerable<Pattern> pattern)
        {
            //RIER
            //MXXI Allowed
            //IXXM Not Allowed
            //=> For the same letters, the I pattern should always be last
            var test = pattern.Select((pat, i) => new { character = word[i], index = i, pat }).GroupBy(t => t.character);

            bool any = test
                .Where(tuple =>
                    tuple.Any(t => t.pat == Pattern.Misplaced) &&
                    tuple.Any(t => t.pat == Pattern.Incorrect)).Any(tuple =>
                    tuple.Where(t => t.pat == Pattern.Misplaced).Max(t => t.index) >
                    tuple.Where(t => t.pat == Pattern.Incorrect).Min(t => t.index));

            return !any;
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
