using System;
using Spectre;
using Spectre.Console;
using mrpv1.Helpers;
using mrpv1.Queries;
using Npgsql;
using mrpv1.Controllers;

namespace mrpv1.Pages;

public class ManufacturePage() : Page

{
    public async Task Display()
    {
        Console.WriteLine("MANUFACTURE PAGE");
        var pageChoice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[green]Select a wc:[/]")
                .PageSize(10)
                .AddChoices(new List<string> { "wc1:kitchen", "wc2:notImplemented" }));
        if (pageChoice == "wc1:kitchen")
        {



            Console.WriteLine("Welcome: wc1:kitchen");
            Console.WriteLine("current work in progress: Producing = colonized tub");
            Console.WriteLine("------------------------------- WorkOrderId: 100");
            Console.WriteLine("------------------------------------- PartId: 700");
            Console.WriteLine("------------------------------------- PartInstanceId: 400");
            Console.WriteLine("WORK CENTER QUEUE");

            ExamplePrintOperationStack();


        }
        else if (pageChoice == "wc2:notImplemented")
        {
            Console.WriteLine("Welcome: wc2:notImplemented");
        }
        else
        {
            Console.WriteLine("Unknown WC...");
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

