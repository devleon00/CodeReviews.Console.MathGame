namespace MathGame;
using Spectre.Console;

public class UserInterface
{
    internal void Main()
    {
        var game = new Game();
        
        AnsiConsole.MarkupLine("Welcome to the [yellow]MathGame[/]!!");
        AnsiConsole.MarkupLine("Press Any Key to Continue.");
        Console.ReadKey();
        
        while (game.KeepPlaying)
        {
            Console.Clear();
            
            var selection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("What would you like to do?")
                    .AddChoices("Play", "See previous games", "Quit"));

            switch (selection)
            {
                case "Play":
                    var operation = AnsiConsole.Prompt(
                        new SelectionPrompt<Operation>()
                            .Title("What game would you like to play?")
                            .AddChoices(Enum.GetValues<Operation>()));
                    var difficulty = AnsiConsole.Prompt(
                        new SelectionPrompt<Difficulty>()
                            .Title("What difficulty would you like to play?")
                            .AddChoices(Enum.GetValues<Difficulty>())
                    );
                    
                    game.Play(operation, difficulty);
                    break;
                case "See previous games":
                    game.DisplayPreviousGames();
                    break;
                case "Quit":
                    game.EndGame();
                    break;
            }
        }
    }
}