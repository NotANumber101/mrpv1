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
    OperationController operationController = new();
    public async Task Display()
    {
        Console.WriteLine("DESIGN PAGE");
        var designPageOptions = new List<string> { "manage catalog" };
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
        if (AnsiConsole.Confirm("Display Inventory?"))
        {
            await new InventoryPage().DisplayInventory();
        }
        AnsiConsole.MarkupLine("to cancel, type: abort");
        AnsiConsole.MarkupLine("to create part on the fly, type: newpart");

        string operationInstruction = AnsiConsole.Ask<string>($"[green]op instructions: [/]");
        if (operationInstruction == "abort")
        {
            await Display();
        }
        if (operationInstruction == "newpart")
        {
            int partId = await new InventoryPage().CreatePart();
            Console.WriteLine($"New part created: {partId}");
            await CreateOperation();
        }

        int partProduced = AnsiConsole.Ask<int>($"[green]Inventory Part Produced: [/]");
        // auto create part, set location to wc
        int partConsumed = AnsiConsole.Ask<int>($"[green]Inventory Part Consumed: [/]");
        int mPartProduced = AnsiConsole.Ask<int>($"[green]Manufacture Part Produced: [/]");
        int mPartConsumed = AnsiConsole.Ask<int>($"[green]Manufacture Part Consumed: [/]");
        int equipment = AnsiConsole.Ask<int>($"[green]equipment: [/]");
        int material = AnsiConsole.Ask<int>($"[green]material: [/]");
        int machine = AnsiConsole.Ask<int>($"[green]machine: [/]");
        int tool = AnsiConsole.Ask<int>($"[green]tool: [/]");
                /// INSERT OPERATION
        Operation newOp = new()
        {
            Instruction = operationInstruction,
            PartConsumed = partConsumed,
            PartProduced = partProduced,
            MPartConsumed = mPartConsumed,
            MPartProduced = mPartProduced,
            Material = material,
            Tool = tool,
            Equipment = equipment,
            Machine = machine
        };
        await operationController.CreateOperation(newOp);
        



        if (AnsiConsole.Confirm("Create more operations?"))
        {
            await CreateOperation();
        }
        else
        {
            await DisplayOperationsTable();
        }
    }
    public async Task DisplayOperationsTable()
    {
        Console.WriteLine("Operations");
        var table = new Table().Title("operations")
            .AddColumn("Id")
            .AddColumn("Instructions")
            .AddColumn("pProduced")
            .AddColumn("pConsumed")
            .AddColumn("mpProduced")
            .AddColumn("mpConsumed")
            .AddColumn("material")
            .AddColumn("tool")
            .AddColumn("machine")
            .AddColumn("equipment");


        List<Operation> operations = await operationController.GetOperations();
        foreach (Operation op in operations)
        {
            table.AddRow(op.Id.ToString(), op.Instruction,
            op.PartConsumed.ToString(),
            op.PartProduced.ToString(),
            op.MPartConsumed.ToString(),
            op.MPartProduced.ToString(),
            op.Material.ToString(), op.Tool.ToString(),
            op.Machine.ToString(), op.Equipment.ToString());
        }
        AnsiConsole.Write(table);
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