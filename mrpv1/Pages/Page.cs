using System;
using Spectre;
using Spectre.Console;
using Npgsql;

namespace mrpv1.Pages;

public class Page()

{
    public async Task MainMenuWithConfirm()
    {
        if (AnsiConsole.Confirm("Return to main menu?"))
        {
            AnsiConsole.MarkupLine("[gray]Returning to main menu...[/]");
            await MainMenu();
        }
    }
    public async Task MainMenu()
    {
        var pageOptions = new List<string> { "design", "manufacture", "inventory" };
        var pageChoice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[green]Select a page to view:[/]")
                .PageSize(10)
                .AddChoices(pageOptions));
        await PageRedirect(pageChoice);
    }

    public async Task PageRedirect(string pageChoice)
    {
        if (pageChoice == "design")
        {
            await new DesignPage().Display();

        }
        else if (pageChoice == "inventory")
        {
            await new PartPage().Display();
        }
        else if (pageChoice == "manufacture")
        {
            await new ManufacturePage().Display();
        }
        else
        {
            throw new ArgumentOutOfRangeException(nameof(pageChoice), $"Not expected direction value: {pageChoice}");
        }
    }

    public async Task ClearDisplay()
    {
        AnsiConsole.Clear();
    }

}


// Dictionary<string, string> pageMessages = new Dictionary<string, string>
// {
//     { "fallback", "message" }
// };