using Wordle.BLL;

namespace Wordle;

public class Entropy
{
    public float Calculate(List<List<Pattern>> patterns)
    {
        return CalculateEntropy(patterns
            .GroupBy(t => t, new ListEqualityComparer<Pattern>())
            .Select(t => (float) t.Count() / patterns.Count));
    }

    public float CalculateEntropy(IEnumerable<float> probabilities)
    {
        return (float) probabilities
            .Select(proba => proba * Math.Log2(1 / proba))
            .Sum();
    }
}