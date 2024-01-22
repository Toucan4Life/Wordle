using Wordle.BLL;

namespace Wordle;

public class Entropy
{
    public static float CalculateEntropy(IEnumerable<float> probabilities)
    {
        return (float) probabilities
            .Select(proba => proba * Math.Log2(1 / proba))
            .Sum();
    }
}