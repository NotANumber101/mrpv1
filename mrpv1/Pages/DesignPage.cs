using System;
using Spectre;
using Spectre.Console;
using mrpv1.Helpers;
using mrpv1.Queries;
using Npgsql;
using mrpv1.Controllers;
using mrpv1.Models;

namespace mrpv1.Pages;

public class DesignPage() : Page

{
        PartController partController = new();


enum PageTitle
{
    DesignPage,
    ManufacturePage,
    PartPage
};

    //TODO CASE ANALYSIS
    // all cases
    // op has pc and pp
    // op has pc and mp
    // etc
    public async Task Display()
    {
        Console.WriteLine("DESIGN PAGE");
        var designPageOptions = new List<string> { "manage catalog"};
        var designPageChoice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[green]Select a page to view:[/]")
                .PageSize(10)
                .AddChoices(designPageOptions));
        // await PageRedirect(pageChoice);
        if (designPageChoice == "manage catalog")
        {
            // display parts m parts to help pick correct id sswhile developing

            // first search for part produced in inventory, if it doesnt exist it must be made

            // 1. ask user for pp
            // 2. search db for pp name
            // if no create part return id
            // if yes return id
            // 3. ask user, does the operatio
            // wait a minute...... im doing this backwards.

            // u should start with part consumed, not produced. then wrok tyour way to a solution (part produced)
            var pageOptions = new List<string> { "Create Operation" };
            var pageChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[green]Welcome,[/]")
                    .PageSize(10)
                    .AddChoices(pageOptions));
            if (pageChoice == "Create Operation")
            {
                await CreateOperation();
            }


            // sooo, 1. ask user for pc
            // 2. search db for pc
            // 3. it must exist or be added to db NOW at time of operation creating
            // 4. user must decide if the operation creates a part or an mpart
            // 5. if part, no more operations should be added now
            // 6. if mpart, more operations are requried to be added now
            // if mpart. does it exist?
            // user cannot end create operataion without pc

            // Console.WriteLine("Search Parts Feature");

            /// BUILD OPERATION M STACK
            /// Autocomplete?

        }
    }
    public async Task CreateOperation()
    {
        // todo, a function thats seeds the db with my locations enums


        int partProduced = AnsiConsole.Ask<int>($"[green]Inventory Part Produced: [/]");
        int partConsumed = AnsiConsole.Ask<int>($"[green]Inventory Part Consumed: [/]");
        int mPartProduced = AnsiConsole.Ask<int>($"[green]Manufacture Part Produced: [/]");
        int mPartConsumed = AnsiConsole.Ask<int>($"[green]Manufacture Part Consumed: [/]");
        string operationInstruction = AnsiConsole.Ask<string>($"[green]op instructions: [/]");



        /// INSERT OPERATION
        Operation newOp = new()
        {
            Instruction = operationInstruction,
            PartConsumed = partConsumed,
            PartProduced = partProduced,
            MPartConsumed = mPartConsumed,
            MPartProduced = mPartProduced
        };
        await new OperationController().CreateOperation(newOp);
        if (AnsiConsole.Confirm("Create more operations?"))
        {
            await CreateOperation();
        } else
        {
            await new DesignPage().Display();
        }


    }
    public async Task CreateMockOperations()
    {
        Console.WriteLine("Creating mock operations");
        int i = 0;
        while (i <= 10)
        {
            string instruction = $"test-op-{i}";
            await new OperationController().CreateOperation(new Operation() { Instruction = instruction });
            i++;
        }
    }
}