using System;
using Spectre.Console;

namespace mrpv1.Pages;

public class Page()
{
    enum PageTitle
    {
        DesignPage,
        ManufacturePage,
        PartPage
    }
    public async Task MainMenu()
    {
        var pageOptions = new List<PageTitle>
        {
            PageTitle.DesignPage, PageTitle.ManufacturePage, PageTitle.PartPage
        };
        var pageChoice = AnsiConsole.Prompt(
            new SelectionPrompt<PageTitle>()
                .Title("[green]Main Menu[/]")
                .PageSize(10)
                .AddChoices(pageOptions));
        await Redirect(pageChoice);
    }
    private async Task Redirect(PageTitle pageTitle)
    {
        await ClearDisplay();
        switch (pageTitle)
        {
            case PageTitle.DesignPage:
                await new DesignPage().Display();
                break;
            case PageTitle.ManufacturePage:
                await new ManufacturePage().Display();
                break;
            case PageTitle.PartPage:
                await new PartPage().Display();
                break;
        }
    }
    public async Task MainMenuWithConfirm()
    {
        if (AnsiConsole.Confirm("Return to Main Menu?"))
        {
            await MainMenu();
        }
    }
    public async Task ClearDisplay()
    {
        AnsiConsole.Clear();
    }
}
