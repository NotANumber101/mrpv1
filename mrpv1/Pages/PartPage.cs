using System;
using Spectre;
using Spectre.Console;
using mrpv1.Helpers;
using mrpv1.Queries;
using Npgsql;
using mrpv1.Controllers;
using mrpv1.Models;

namespace mrpv1.Pages;

public class PartPage() : Page

{
    readonly PartController partController = new();
    public async Task Display()
    {
        await DisplayPartTable();
        await CreatePart();
    }
    public async Task DisplayPartTable()
    {
        var myTable = new Table()
            .RoundedBorder()
            .BorderColor(Color.Grey);
        myTable.AddColumn("Id");
        myTable.AddColumn("InventoryId");
        myTable.AddColumn("Name");
        myTable.AddColumn("Quantity");

        var parts = await partController.GetParts();
        foreach (Part part in parts)
        {
            myTable.AddRow(part.Id.ToString(), part.InventoryId.ToString(), part.Name, part.Quantity.ToString());
        }
        AnsiConsole.Write(myTable);
    }
    public async Task CreatePart()
    {
        string partName = AnsiConsole.Ask<string>($"[green]partname:[/]");
        var partInventoryOptions = new List<Locations.InventoryLocations>
                {
                    Locations.InventoryLocations.CabinetA,
                    Locations.InventoryLocations.ClosetA,
                    Locations.InventoryLocations.Fridge,
                    Locations.InventoryLocations.Garage,
                    Locations.InventoryLocations.Kitchen
                };
        var partInventoryLocation = AnsiConsole.Prompt(
            new SelectionPrompt<Locations.InventoryLocations>()
                .Title("[green]Select inventory location for the new part[/]")
                .PageSize(10)
                .AddChoices(partInventoryOptions));

        int locationId = (int)partInventoryLocation;

        Part newPart = new()
        {
            InventoryId = locationId,
            Name = partName,
            Quantity = 0
        };

        await partController.CreatePart(newPart);

        if (AnsiConsole.Confirm("Create more Parts?"))
        {
            await CreatePart();
        }
        else
        {
            await MainMenu();
        }
    }
}