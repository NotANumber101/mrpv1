using System;
using Spectre;
using Spectre.Console;
using mrpv1.Helpers;
using mrpv1.Queries;
using Npgsql;
using mrpv1.Controllers;
using mrpv1.Models;

namespace mrpv1.Pages;

public class ManufacturePage() : Page

{
    readonly WorkCenterController workCenterController = new();
    public async Task Display()
    {
        await DisplayWorkCenters();
    }
    public async Task DisplayWorkCenters()
    {
        Dictionary<string, WorkCenter> workCenterDict = [];

        var workCenters = await workCenterController.GetWorkCenters();

        List<string> wcButtons = [];
        foreach (var wc in workCenters)
        {
            wcButtons.Add(wc.Name);
            workCenterDict[wc.Name] = wc;
        }
        string pageChoice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[green]Main Menu[/]")
                .PageSize(10)
                .AddChoices(wcButtons));

        await new WorkCenterPage().Display(workCenterDict[pageChoice]);

        if (AnsiConsole.Confirm("Return to work centers?"))
        {
            AnsiConsole.Clear();
            await Display();
        }
    }
}

