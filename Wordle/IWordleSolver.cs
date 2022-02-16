using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wordle.BLL;

namespace Wordle
{
    public interface IWordleSolver
    {
        public IEnumerable<KeyValuePair<string, float>> RetrievePossibleWords();
        public IEnumerable<KeyValuePair<string, float>> RetrieveRecommendedWords();
        public void ApplyWordPattern(string word, List<Pattern> patterns);
        public float CalculateEntropy(List<float> probabilities);

    }
}
