using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wordle.BLL
{
    public class Rule
    {




        public static List<Pattern> GetPattern(string actualWord, string targetWord)
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
                         .GroupBy(t => t.character).ToList())
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
