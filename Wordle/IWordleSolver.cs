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
        public IEnumerable<WordleEntity> RetrieveRecommendedWords();
        public void ApplyWordPattern(string word, string patterns);
        public float CalculateUniformEntropy(int count);

    }
}
