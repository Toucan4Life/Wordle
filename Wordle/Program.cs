// See https://aka.ms/new-console-template for more information

using Wordle;
using Wordle.BLL;
using Wordle.SAL;

internal class Program
{
    private static void Main()
    {

        Dictionary<string, float> possibleSolution = new CsvReader().GetAllWords("Lexique381.csv");

        Console.WriteLine(
            "Type a word, Add pattern with +. use \".\" for not in word, ? for misplaced and ! for correct letters.type \"newgame\" to reset game");
        while (true)
        {
            var enteredLine = Console.ReadLine();
            if (enteredLine == "newgame")
            {
                possibleSolution = new CsvReader().GetAllWords("Lexique381.csv");
                Console.WriteLine("Game has been reset !");
            }
            else
            {
                var patternString = Console.ReadLine();

                if (patternString.Length != enteredLine.Length)
                    throw new ArgumentException("Pattern and Word are not same size");
                var searcher = new WordSearcher(possibleSolution);
                var _solver = new WordleSolver();
                possibleSolution = _solver.FilterWithEntropy(enteredLine, patternString.Select(MapPattern).ToList(), searcher)
                    .OrderByDescending(t => t.Value).Take(20).ToDictionary(t=>t.Key, t=>t.Value);

                foreach (var (key, value) in possibleSolution) Console.WriteLine($"{key} , {value}");
            }
        }
    }

    private static Pattern MapPattern(char c)
    {
        return c switch
        {
            '0' => Pattern.Incorrect,
            '1' => Pattern.Misplaced,
            '2' => Pattern.Correct,
            _ => throw new ArgumentOutOfRangeException("Pattern not supported")
        };
    }
}