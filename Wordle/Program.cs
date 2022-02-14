// See https://aka.ms/new-console-template for more information

using Wordle;
using Wordle.BLL;
using Wordle.SAL;

internal class Program
{
    private static void Main()
    {

        IEnumerable<KeyValuePair<string, float>> possibleSolution = new CsvReader().GetAllWords("SAL/Lexique381.csv");


        while (true)
        {
            var enteredLine = Console.ReadLine();
            switch (enteredLine)
            {
                case "start":
                {
                    Console.WriteLine(
                        "Type a word, Add pattern with +. use \".\" for not in word, ? for misplaced and ! for correct letters.type \"newgame\" to reset game");

                    possibleSolution = new CsvReader().GetAllWords("SAL/Lexique381.csv");

                    Console.Write("Word Length : ");

                    var length = int.Parse(Console.ReadLine());

                    Console.Write("First char (can be empty) :");

                    var enteredline = Console.ReadLine();

                    var _solver = new Solver();

                    possibleSolution = _solver
                        .FilterWithEntropy(length, possibleSolution,
                            string.IsNullOrWhiteSpace(enteredline) ? char.MinValue : enteredline[0])
                        .OrderByDescending(t => t.Value);

                    Console.WriteLine($"# possible solution : {possibleSolution.Count()}");
                    foreach (var (key, value) in possibleSolution.Take(20)) Console.WriteLine($"{key} , {value}");
                    break;
                }
                default:
                {

                    var patternString = Console.ReadLine();

                    if (patternString.Length != enteredLine.Length)
                        throw new ArgumentException("Pattern and Word are not same size");

                    var searcher = new WordSearcher(possibleSolution);

                    var _solver = new Solver();

                    possibleSolution = _solver.FilterWithEntropy(enteredLine, patternString.Select(MapPattern).ToList(), searcher)
                        .OrderByDescending(t => t.Value);

                    Console.WriteLine($"# possible solution : {possibleSolution.Count()}");
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