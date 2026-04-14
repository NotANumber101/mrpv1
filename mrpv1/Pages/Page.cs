using System;
using Spectre.Console;

namespace mrpv1.Pages;

public class Page()
{
    enum PageTitle
    {
        DesignPage,
        ManufacturePage,
        InventoryPage,
        OrderPage
    }
    public async Task MainMenu()
    {
        var pageOptions = new List<PageTitle>
        {
            PageTitle.DesignPage, PageTitle.ManufacturePage,
            PageTitle.InventoryPage, PageTitle.OrderPage
        };
        PageTitle pageChoice = AnsiConsole.Prompt(
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
            case PageTitle.InventoryPage:
                await new InventoryPage().Display();
                break;
            case PageTitle.OrderPage:
                await new OrderPage().Display();
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
