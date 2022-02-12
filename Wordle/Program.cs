// See https://aka.ms/new-console-template for more information

using Wordle;
using Wordle.BLL;
using Wordle.SAL;

internal class Program
{
    private static void Main()
    {

        Dictionary<string, float> possibleSolution = new Dictionary<string, float>();

       
        while (true)
        {
            var enteredLine = Console.ReadLine();
            switch (enteredLine)
            {
                case "start":
                {
                    Console.WriteLine(
                        "Type a word, Add pattern with +. use \".\" for not in word, ? for misplaced and ! for correct letters.type \"newgame\" to reset game");

                    possibleSolution = new CsvReader().GetAllWords("Lexique381.csv");
                    Console.Write("Word Length : ");
                    var length = int.Parse(Console.ReadLine());

                    var searcher = new WordSearcher(possibleSolution){WordLength = length };

                    Console.Write("First char (can be empty) :");

                    var enteredline = Console.ReadLine();

                    if(!string.IsNullOrWhiteSpace(enteredline)) searcher.AddCharPosToMatch(enteredline[0],0);

                    var result = searcher.Search().ToDictionary(t => t.Key, t => t.Value);

                    var _solver = new WordleSolver();

                    possibleSolution =_solver.FilterWithEntropy(new WordSearcher(result)).OrderByDescending(t => t.Value).ToDictionary(t => t.Key, t => t.Value);
                    Console.WriteLine($"# possible solution : {possibleSolution.Count}");
                    foreach (var (key, value) in possibleSolution.Take(20)) Console.WriteLine($"{key} , {value}");
                    break;
                }
                default:
                {
                    var patternString = Console.ReadLine();

                    if (patternString.Length != enteredLine.Length)
                        throw new ArgumentException("Pattern and Word are not same size");

                    var searcher = new WordSearcher(possibleSolution);
                    var _solver = new WordleSolver();
                    possibleSolution = _solver.FilterWithEntropy(enteredLine, patternString.Select(MapPattern).ToList(), searcher)
                        .OrderByDescending(t => t.Value).ToDictionary(t=>t.Key, t=>t.Value);
                    Console.WriteLine($"# possible solution : {possibleSolution.Count}");
                    foreach (var (key, value) in possibleSolution.Take(20)) Console.WriteLine($"{key} , {value}");
                    break;
                }
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