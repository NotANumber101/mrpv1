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
    readonly InventoryController inventoryController = new();
    readonly OperationController operationController = new();
    public async Task Display()
    {
        Console.WriteLine("MANUFACTURE PAGE");
        await new WorkCenterPage().Display();
        await DisplayOperations();
    }
    public async Task DisplayOperations()
    {
        Console.WriteLine("Operations");

        List<Operation> operations = await operationController.GetOperations();
        foreach (Operation op in operations)
        {
            await DisplayOperationPanel(op);
        }
    }
    public async Task DisplayOperationPanel(Operation operation)
    {
        Part partConsumed = await inventoryController.GetPart(operation.PartConsumed);
        Part partProduced = await inventoryController.GetPart(operation.PartProduced);
        Console.WriteLine("Operation Details");
        AnsiConsole.Write(new Panel($"{operation.Instruction}, produces: {partProduced.Name}({partProduced.Id})\nconsumes: {partConsumed.Name}({partConsumed.Id})"));
    }
    public async Task CreateOperation()
    {
        string opInstructionInput = AnsiConsole.Ask<string>($"[green]Instructions:[/]");

        Operation newOp = new()
        {
            Instruction = opInstructionInput
        };
        await operationController.CreateOperation(newOp);
        if (AnsiConsole.Confirm("Return?"))
        {
            await MainMenu();
        }
    }














    public void ExamplePrintOperationStack()
    {
        Console.WriteLine("WORK CENTER QUEUE");
        Console.WriteLine("------------------------");

        Console.WriteLine("                                                               WorkOrderId: 100");
        Console.WriteLine("        PartId: 700 <------op70------ MpartId:1005 <------op40----- PartId: 200");
        Console.WriteLine("PartInstanceId: 400 <---------------------------------------PartInstanceId: 400");


        // TODO: TREE VERSION
    }
}

