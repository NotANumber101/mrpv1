using Spectre.Console;
using mrpv1.Controllers;
using mrpv1.Models;

namespace mrpv1.Pages;

public class DesignPage() : Page

{
    static PartController partController = new();
    OperationController operationController = new();

    public Table PartsTable(List<Part> parts)
    {
        var partsTable = new Table()
    .Title($"[yellow bold]Parts[/]")
    .RoundedBorder()
    .BorderColor(Color.Grey);
        partsTable.AddColumn("Id");
        partsTable.AddColumn("Name");
        // var parts = await partController.GetParts();
        foreach (Part part in parts)
        {
            partsTable.AddRow(part.Id.ToString(), part.Name ?? "");
        }
        return partsTable;
    }
    public async Task Display()

    {
        List<Operation> operations = await operationController.GetOperations();
        List<Part> parts = await partController.GetParts();

        DesignPageLayout(parts, operations);

        var pageOptions = new List<string> { "Create Operation", "Create Part" };
        var pageChoice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[green]Welcome,[/]")
                .PageSize(3)
                .AddChoices(pageOptions));
        if (pageChoice == "Create Operation")
        {
            await CreateOperation();
        }
        else if (pageChoice == "Create Part")
        {
            string partName = AnsiConsole.Ask<string>("Please enter a part name:");
            Part newPart = new()
            {
                Name = partName
            };
            await partController.CreatePart(newPart);
            AnsiConsole.Clear();
            await Display();
        }
    }
    public void DesignPageLayout(List<Part> parts, List<Operation> operations)
    {
        // Create main grid
        var mainGrid = new Grid { Expand = true };
        mainGrid.AddColumn();

        // Create header
        var operationsTable = DisplayOperationsTable(operations);
        // var header = new Panel(table)
        var header = new Panel("[bold yellow]Catalog[/]")

            .BorderColor(Color.Yellow)
            .RoundedBorder();

        mainGrid.AddRow(header);
        mainGrid.AddEmptyRow();

        var operationsPanel = new Panel(operationsTable)
            .Header("Operations")
            .BorderColor(Color.Green);

        var partsTable = PartsTable(parts);
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
    public Table DisplayOperationsTable(List<Operation> operations)
    {
        var table = new Table().Title("operations")
            .AddColumn("Id")
            .AddColumn("Instructions")
            .AddColumn("pConsumed")
            .AddColumn("pProduced");
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

