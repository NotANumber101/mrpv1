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
    public async Task ExampleGrid1()
    {
        // Create main grid
        var mainGrid = new Grid { Expand = true };
        mainGrid.AddColumn();

        // Create header
        var operationsTable = await DisplayOperationsTable();
        // var header = new Panel(table)
        var header = new Panel("[bold yellow]Catalog[/]")

            .BorderColor(Color.Yellow)
            .RoundedBorder();

        mainGrid.AddRow(header);
        mainGrid.AddEmptyRow();

        var operationsPanel = new Panel(operationsTable)
            .Header("Operations")
            .BorderColor(Color.Green);

        var partsTable = await PartTable();
        var inventoryPanel = new Panel(partsTable)
            .Header("Inventory")
            .BorderColor(Color.Black);


        List<Panel> panels = [operationsPanel, inventoryPanel];

        // Create metrics grid
        var catalogGrid = new Grid();
        catalogGrid.AddColumns(panels.Count);


        catalogGrid.AddRow(operationsPanel, inventoryPanel);

        mainGrid.AddRow(catalogGrid);

        AnsiConsole.Write(mainGrid);
    }

    public Table InventoryItemTableBuilder(string title)
    {
        var myTable = new Table()
            .Title($"[yellow bold]{title}[/]")
            .RoundedBorder()
            .BorderColor(Color.Grey);
        myTable.AddColumn("Id");
        myTable.AddColumn("Name");
        // myTable.AddColumn("Quantity");
        return myTable;
    }
    public async Task<Table> PartTable()
    {
        var myTable = InventoryItemTableBuilder("parts");
        var parts = await partController.GetParts();
        foreach (Part part in parts)
        {
            myTable.AddRow(part.Id.ToString(), part.Name ?? "");
        }
        return myTable;
    }
    public async Task Display()
    {
        await ExampleGrid1();
        // u should start with part consumed, not produced. then wrok tyour way to a solution (part produced)
        var pageOptions = new List<string> { "Create Operation", "Create Part" };
        var pageChoice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[green]Welcome,[/]")
                .PageSize(10)
                .AddChoices(pageOptions));
        if (pageChoice == "Create Operation")
        {
            await CreateOperation();
        }
        else if (pageChoice == "Create Part")
        {
            string partName = AnsiConsole.Ask<string>("Please enter a part name");
            Part newPart = new()
            {
                Name = partName
            };
            await partController.CreatePart(newPart);
        }
    }
    public async Task<Table> DisplayOperationsTable()
    {
        var table = new Table().Title("operations")
            .AddColumn("Id")
            .AddColumn("Instructions")
            .AddColumn("pConsumed")
            .AddColumn("pProduced");
        List<Operation> operations = await operationController.GetOperations();
        foreach (Operation op in operations)
        {
            table.AddRow(
                op.Id.ToString(),
                op.Instruction ?? "",
                op.PartConsumed.ToString(),
                op.PartProduced.ToString());
        }
        return table;
    }
    public async Task CreateOperation()
    {
        // todo, a function thats seeds the db with my locations enums
        AnsiConsole.MarkupLine("to cancel, type: abort");

        string operationInstruction = AnsiConsole.Ask<string>($"[green]op instructions: [/]");
        if (operationInstruction == "abort")
        {
            await Display();
        }

        int partProduced = AnsiConsole.Ask<int>($"[green]Inventory Part Produced: [/]");
        int partConsumed = AnsiConsole.Ask<int>($"[green]Inventory Part Consumed: [/]");

        Operation newOp = new()
        {
            Instruction = operationInstruction,
            PartConsumed = partConsumed,
            PartProduced = partProduced
        };
        await operationController.CreateOperation(newOp);
        AnsiConsole.Clear();
        await Display();
    }

}



//  var grid = new Grid();

//         // Configure columns
//         grid.AddColumn(new GridColumn { Width = 20, Alignment = Justify.Right });
//         grid.AddColumn(new GridColumn());

//         // Add header
//         grid.AddRow(
//             new Text("System Information", new Style(Color.Yellow, decoration: Decoration.Bold)),
//             new Text(""));

//         grid.AddEmptyRow();

//         // Add data rows
//         grid.AddRow(new Markup("OS:"), new Markup("[blue]Linux[/]"));
//         grid.AddRow(new Markup("CPU:"), new Markup("[green]8 cores @ 3.2GHz[/]"));
//         grid.AddRow(
//             new Markup("Memory:"),
//             new BreakdownChart()
//                 .Width(40)
//                 .AddItem("Used", 12, Color.Red)
//                 .AddItem("Available", 4, Color.Green));
//         grid.AddRow(
//             new Markup("Disk:"),
//             new Panel("[yellow]65% used[/]")
//                 .BorderColor(Color.Yellow));

//         AnsiConsole.Write(grid);