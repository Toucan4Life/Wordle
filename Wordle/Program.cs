// See https://aka.ms/new-console-template for more information

using Wordle;
using Wordle.BLL;
using Wordle.SAL;

internal class Program
{
    private static void Main()
    {

        IEnumerable<KeyValuePair<string, float>> possibleSolution = new CsvReader().GetAllWords("SAL/Lexique381.csv");
        WordSearcher wordSearcher = new WordSearcher(possibleSolution);


        while (true)
        {
            var enteredLine = Console.ReadLine();
            switch (enteredLine)
            {
                case "start":
                {
                    Console.WriteLine(
                        "Type a word, Add pattern with +. use \".\" for not in word, ? for misplaced and ! for correct letters.type \"newgame\" to reset game");

                    wordSearcher = new WordSearcher(new CsvReader().GetAllWords("SAL/Lexique381.csv")); 
                    
                        Console.Write("Word Length : ");

                    var length = int.Parse(Console.ReadLine());

                    wordSearcher.WordLength = length;

                    Console.Write("First char (can be empty) :");

                    var enteredline = Console.ReadLine();
                        
                    if (!string.IsNullOrWhiteSpace(enteredline))
                        wordSearcher.AddCharPosToMatch(enteredline[0], 0);

                    possibleSolution = new Solver().GetEntropy(wordSearcher);

                    Console.WriteLine($"# possible solution : {wordSearcher.Search().Count()}");
                    foreach (var (key, value) in possibleSolution.OrderByDescending(t => t.Value).Take(10)) Console.WriteLine($"{key} , {value}");
                    break;
                }
                default:
                {

                    var patternString = Console.ReadLine();

                    if (patternString.Length != enteredLine.Length)
                        throw new ArgumentException("Pattern and Word are not same size");

                    var possibleSolsolution=new Rule().Filter(enteredLine, patternString.Select(MapPattern).ToList(), wordSearcher).Search();

                    var numberOfSolution = possibleSolsolution.Count();
                    Console.WriteLine($"# possible solution : {numberOfSolution}");
                    foreach (var (key, value) in possibleSolsolution.OrderByDescending(t => t.Value).Take(10)) Console.WriteLine($"{key} , {value}");

                    if (numberOfSolution == 1) { break; }

                    Console.WriteLine($"# recommend next word : ");
                    foreach (var (key, value) in new Solver().GetEntropy(wordSearcher).OrderByDescending(t => t.Value).Take(10)) Console.WriteLine($"{key} , {value}");
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