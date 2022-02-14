using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wordle.BLL
{
    public class Rule
    {
        public List<KeyValuePair<string, float>> Filter(string word, IEnumerable<Pattern> pattern, WordSearcher searcher)
        {
            searcher.SetWordLength(pattern.Count());

            foreach (var tuple in pattern.Select((pat, i) => new { character = word[i], index = i, pat }).GroupBy(t => t.character))
            {
                if (tuple.Any(t => t.pat == Pattern.Incorrect))
                    searcher.AddCharacterCount(tuple.Key, tuple.Count(t => t.pat != Pattern.Incorrect));

                else
                    searcher.AddAtLeastCharacterCount(tuple.Key, tuple.Count());

                foreach (var triple in tuple)
                {
                    if (triple.pat == Pattern.Correct)
                        searcher.AddCharPosToMatch(triple.character, triple.index);
                    else
                        searcher.AddCharPosToNotMatch(triple.character, triple.index);
                }
            }

            return searcher.Search().ToList();
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

            foreach (var tuple in actualWord.Select((charac, i) => new { character = charac, index = i })
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
                    patternList[tuple.ElementAt(j).index] = Pattern.Misplaced;
                }
            }

            return patternList.ToList();
        }

        private bool IsAllowedPattern(string word, IEnumerable<Pattern> pattern)
        {
            //RIER
            //MXXI Allowed
            //IXXM Not Allowed
            //=> For the same letters, the I pattern should always be last

            return !pattern.Select((pat, i) => new { character = word[i], index = i, pat }).GroupBy(t => t.character)
                .Where(tuple =>
                    tuple.Any(t => t.pat == Pattern.Misplaced) &&
                    tuple.Any(t => t.pat == Pattern.Incorrect)).Any(tuple =>
                    tuple.Where(t => t.pat == Pattern.Misplaced).Max(t => t.index) >
                    tuple.Where(t => t.pat == Pattern.Incorrect).Min(t => t.index));
        }
    }
}
