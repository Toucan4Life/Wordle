// See https://aka.ms/new-console-template for more information

using Wordle;
using Wordle.SAL;

var words = new CsvReader().GetAllWords("Lexique381.csv");

var _solver = new WordleSolver(words);

Console.WriteLine("Type a word, Add pattern with +. use \".\" for not in word, ? for misplaced and ! for correct letters.type \"newgame\" to reset game");
while (true)
{
    var enteredLine = Console.ReadLine();
    if (enteredLine == "newgame")
    {
        _solver.Reset();
        Console.WriteLine("Game has been reset !");
    }
    else
    {
        var split = enteredLine.Split('+');
        var pattern = "";
        if (split.Length != 1) pattern = split[1];
        var solution = _solver.Filter(split[0], pattern).Take(20);

        foreach (var (key, value) in solution) Console.WriteLine($"{key} , {value}");
    }
}