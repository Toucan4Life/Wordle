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
        Console.WriteLine("Recommend next words and associated entropy :");
        foreach (var (key, value) in possibleSolution.OrderByDescending(t => t.Value).Take(10)) Console.WriteLine($"{key} , {value}");


        while (true)
        {
            Console.WriteLine("Entered Word (or type \"start\" to start a new game) :");
            var enteredLine = Console.ReadLine();
            switch (enteredLine)
            {
                case "start":
                {

                    wordSearcher = new WordSearcher(new CsvReader().GetAllWords("SAL/Lexique381.csv")); 
                    
                        Console.Write("Word Length : ");

                    var length2 = int.Parse(Console.ReadLine());

                    wordSearcher.WordLength = length2;

                    Console.Write("First char (can be empty) :");

                    var enteredline2 = Console.ReadLine();
                        
                    if (!string.IsNullOrWhiteSpace(enteredline2))
                        wordSearcher.AddCharPosToMatch(enteredline2[0], 0);

                    possibleSolution = new Solver().GetEntropy(wordSearcher);

                    Console.WriteLine($"# possible solution : {wordSearcher.Search().Count()}");
                    Console.WriteLine("Recommend next words and associated entropy :");
                        foreach (var (key, value) in possibleSolution.OrderByDescending(t => t.Value).Take(10)) Console.WriteLine($"{key} , {value}");
                    break;
                }
                default:
                {
                    Console.WriteLine("Received pattern (Incorrect = 0, Misplaced = 1, Correct = 2) :");
                    var patternString = Console.ReadLine();

                    if (patternString.Length != enteredLine.Length)
                        throw new ArgumentException("Pattern and Word are not same size");

                    var possibleSolsolution=new Rule().Filter(enteredLine, patternString.Select(MapPattern).ToList(), wordSearcher).Search();

                    var numberOfSolution = possibleSolsolution.Count();
                    Console.WriteLine($"# possible solution and associated frequency in language : {numberOfSolution}");
                    foreach (var (key, value) in possibleSolsolution.OrderByDescending(t => t.Value).Take(10)) Console.WriteLine($"{key} , {value}");

                    if (numberOfSolution == 1) { break; }

                    Console.WriteLine("Recommend next words and associated entropy :");
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