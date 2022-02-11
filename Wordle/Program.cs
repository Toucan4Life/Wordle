// See https://aka.ms/new-console-template for more information

using Wordle;
using Wordle.BLL;
using Wordle.SAL;

internal class Program
{
    private static void Main()
    {
        var words = new CsvReader().GetAllWords("Lexique381.csv");
        var searcher = new WordSearcher(words);
        var _solver = new WordleSolver(words);

        Console.WriteLine(
            "Type a word, Add pattern with +. use \".\" for not in word, ? for misplaced and ! for correct letters.type \"newgame\" to reset game");
        while (true)
        {
            var enteredLine = Console.ReadLine();
            if (enteredLine == "newgame")
            {
                searcher = new WordSearcher(words);
                Console.WriteLine("Game has been reset !");
            }
            else
            {
                var patternString = Console.ReadLine();

                if (patternString.Length != enteredLine.Length)
                    throw new ArgumentException("Pattern and Word are not same size");

                var solution = _solver.FilterWithEntropy(enteredLine, patternString.Select(MapPattern).ToList(), searcher)
                    .OrderByDescending(t => t.Value).Take(20);

                foreach (var (key, value) in solution) Console.WriteLine($"{key} , {value}");
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