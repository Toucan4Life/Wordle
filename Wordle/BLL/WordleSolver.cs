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

        public float CalculateEntropy(string actualWord, Dictionary<string, float> wordDico)
        { 
            var patternsList = wordDico.Select(word => GetPattern(actualWord, word.Key)).ToList();

            var test = patternsList.GroupBy(t => t, new ListEqualityComparer<Pattern>())
                .Select(t => (float) t.Count() / wordDico.Count).OrderByDescending(t=>t);

            var sum = (float) test.Select(t => t * Math.Log2(1 / t)).Sum();

            return sum;
        }

        public List<Pattern> GetPattern(string actualWord, string targetWord)
        {
            var patternList = new Pattern[actualWord.Length].ToList();

            for (var k = 0; k < actualWord.Length; k++)
            {
                if (targetWord[k] == actualWord[k])
                {
                    patternList[k] = Pattern.Correct;
                }
            }

            foreach (var tuple in actualWord.Select((charac, i) => new {character = charac, index = i})
                         .GroupBy(t => t.character))
            {
                var count = Math.Min(targetWord.Count(t => tuple.Key == t), tuple.Count()) -
                            tuple.Count(t => patternList[t.index] == Pattern.Correct);

                for (int j = 0; j < count; j++)
                {
                    if (patternList[tuple.ElementAt(j).index] == Pattern.Correct)
                    {
                        count++;
                        continue;
                    }
                    patternList[tuple.ElementAt(j).index] =  Pattern.Misplaced;
                }
            }

            return patternList.ToList();
        }
    }
}
