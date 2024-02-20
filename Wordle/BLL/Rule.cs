namespace Wordle.BLL
{
    public class Rule
    {
        private readonly Dictionary<char, int> _characterCount = new();
        private readonly Dictionary<char, int> _characterAtLeastCount = new();
        private readonly Dictionary<int, char> _charPosToMatch = new();
        private readonly List<Tuple<int, char>> _charPosToNotMatch = new();
        public int WordLength { get; set; }

        public Rule(string word, IEnumerable<Pattern> pattern)
        {
            SetWordLength(pattern.Count());

            foreach (var tuple in pattern.Select((pat, i) => new { character = word[i], index = i, pat }).GroupBy(t => t.character))
            {
                if (tuple.Any(t => t.pat == Pattern.Incorrect))
                    AddCharacterCount(tuple.Key, tuple.Count(t => t.pat != Pattern.Incorrect));

                else
                    AddAtLeastCharacterCount(tuple.Key, tuple.Count());

                foreach (var triple in tuple)
                {
                    if (triple.pat == Pattern.Correct)
                        AddCharPosToMatch(triple.character, triple.index);
                    else
                        AddCharPosToNotMatch(triple.character, triple.index);
                }
            }
        }

        private void SetWordLength(int length)
        {
            WordLength = length;
        }

        private void AddCharPosToMatch(char character, int pos)
        {
            _charPosToMatch.TryAdd(pos, character);
        }
        private void AddCharPosToNotMatch(char character, int pos)
        {
            _charPosToNotMatch.Add(new Tuple<int, char>(pos, character));
        }

        private void AddCharacterCount(char character, int count)
        {
            if (_characterCount.TryGetValue(character, out int value))
            {
                _characterCount[character] = Math.Max(value, count);
            }
            else
            {
                _characterAtLeastCount.Remove(character);
                _characterCount[character] = count;
            }
        }

        private void AddAtLeastCharacterCount(char character, int count)
        {
            if (_characterAtLeastCount.TryGetValue(character, out int value))
            {
                _characterAtLeastCount[character] = Math.Max(value, count);
            }
            else
            {
                _characterAtLeastCount[character] = count;
            }
        }

        public bool IsWordConform(string word)
        {
            return word.Length == WordLength && _charPosToMatch.All(charpos => word[charpos.Key] == charpos.Value) &&
                   _charPosToNotMatch.All(charpos => word[charpos.Item1] != charpos.Item2) &&
                   _characterCount.All(t => word.Count(v => v == t.Key) == t.Value) &&
                   _characterAtLeastCount.All(t => word.Count(v => v == t.Key) >= t.Value);
        }

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
        
        //public static List<Pattern> GetPattern(string actualWord, string targetWord)
        //{
        //   var correctIndex =  actualWord.ToCharArray().Select((c, i) => new { charac = c, index = i })
        //        .Where(t => t.charac == targetWord[t.index]).Select(t => t.index);

        //   var incorrectIndex = (from ttuple in actualWord.Select((charac, i) => new { character = charac, index = i })
        //           .Where(t => !correctIndex.Contains(t.index)).GroupBy(t => t.character)
        //       join s in targetWord.ToCharArray().Select((c, i) => new { charac = c, index = i })
        //           .Where(t => !correctIndex.Contains(t.index)).Select(t => t.charac) on ttuple.Key equals s into gj
        //       select ttuple.TakeLast(ttuple.Count() - gj.Count()).Select(t => t.index)).SelectMany(t => t);

        //   return Enumerable.Range(0, actualWord.Length).Select(index =>
        //   {
        //       if (correctIndex.Contains(index)){
        //           return Pattern.Correct;
        //       }
        //       if (incorrectIndex.Contains(index)) {  return Pattern.Incorrect; }

        //       return Pattern.Misplaced;
        //   }).ToList();
        //}

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
