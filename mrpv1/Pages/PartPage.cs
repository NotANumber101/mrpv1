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
    PartController partController = new();
    public async Task Display()
    {
        await CreatePart();
    }
    public async Task CreatePart()
    {
        var parts = await partController.GetParts();
        foreach (Part part in parts)
        {
            Console.WriteLine($"PartId: {part.Id}, PartName: {part.Name}");
            Console.WriteLine($"InventoryId: {part.InventoryId}, Quantity: {part.Quantity}");
        }
        string partName = AnsiConsole.Ask<string>($"[green]partname:[/]");
        Console.WriteLine("Select Inventory location for the new part");
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
                .Title("[green]Select a page to view:[/]")
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
            await new PartPage().Display();
        }
    }
}