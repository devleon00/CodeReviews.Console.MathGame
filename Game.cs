using System.Diagnostics;
using Spectre.Console;

namespace MathGame;

internal class Game
{
    private int _score;
    private List<(Operation operation, int score, Difficulty difficulty, TimeSpan time)> _previousGames = new();
    public bool KeepPlaying { get; private set; } = true;

    internal void DisplayPreviousGames()
    {
        var table = new Table();
        
        table.AddColumn(new TableColumn("Score") );
        table.AddColumn(new TableColumn("Previous Games"));
        table.AddColumn(new TableColumn("Difficulty"));
        table.AddColumn(new TableColumn("Time"));
        
        foreach (var game in _previousGames)
        {
            // Add row
            table.AddRow(game.score.ToString(), game.operation.ToString(), game.difficulty.ToString(), game.time.ToString("mm':'ss"));
        }
        AnsiConsole.Write(table);
        AnsiConsole.MarkupLine("Press Any Key to Continue.");
        Console.ReadKey();
    }
    internal void EndGame()
    {
        KeepPlaying = false;
        AnsiConsole.MarkupLine("[yellow]Ending game[/]");
        AnsiConsole.MarkupLine($"Total Score: [yellow]{_score}[/]");
        DisplayPreviousGames();
    }

    internal void Play(Operation operation, Difficulty difficulty)
    { 
        var currentScore = 0;
        var random = new Random();
        var isCorrect = true;
        var stopWatch = new Stopwatch();
        stopWatch.Start();
        var randomizeOperation = operation == Operation.Random;
        
        while (isCorrect)
        {
            Console.Clear();
            AnsiConsole.MarkupLine($"Score: [yellow]{currentScore}[/]");
            
            var num1 = random.Next(1, (int) difficulty);
            var num2 = random.Next(1, (int) difficulty);

            if (randomizeOperation)
            {
                operation = (Operation)random.Next(3);
            }

            switch (operation)
            {
                case Operation.Divide:
                    while (num1 % num2 != 0)
                    {
                        num1 = random.Next(1, (int) difficulty);
                        num2 = random.Next(1, (int) difficulty);
                    }

                    if (Divide(num1, num2))
                        currentScore++;
                    else
                        isCorrect = false;
                    break;
                case Operation.Multiply:
                    if (Multiply(num1, num2))
                        currentScore++;
                    else
                        isCorrect = false;
                    break;
                case Operation.Sum:
                    if (Sum(num1, num2))
                        currentScore++;
                    else
                        isCorrect = false;
                    break;
                case Operation.Rest:
                    if (Substract(num1, num2))
                        currentScore++;
                    else
                        isCorrect = false;
                    break;
            }
                
        }

        if (randomizeOperation)
        {
            operation =  Operation.Random;
        }

        stopWatch.Stop();
        var timeElapsed =  stopWatch.Elapsed;
        
        _previousGames.Add((operation, currentScore, difficulty, timeElapsed));
        _score += currentScore;
        
        AnsiConsole.MarkupLine("Press Any Key to Continue.");
        Console.ReadKey();
    }

    private bool Evaluation(int result, int userInput)
    {
        if (result == userInput)
        {
            AnsiConsole.MarkupLine($"[green]You got it! The Result was {result}[/]");
            return true;
        } 
        AnsiConsole.MarkupLine($"[red]That's wrong!! The result was {result}[/]");
        return false;
    }

    private bool Divide(int num1, int num2)
    {
        var answer = AnsiConsole.Ask<int>($"Divide [yellow]{num1}/{num2}[/] = ");
        return Evaluation(num1 / num2, answer);
    }

    private bool Multiply(int num1, int num2)
    {
        var answer = AnsiConsole.Ask<int>($"Multiply [yellow]{num1}*{num2}[/] = ");
        return Evaluation(num1 * num2, answer);
    }

    private bool Sum(int num1, int num2)
    {
        var answer = AnsiConsole.Ask<int>($"Sum [yellow]{num1}+{num2}[/] = ");
        return Evaluation(num1 + num2, answer);
    }

    private bool Substract(int num1, int num2)
    {
        var answer = AnsiConsole.Ask<int>($"Subtract [yellow]{num1}-{num2}[/] =");
        return Evaluation(num1 - num2, answer);
    }
}