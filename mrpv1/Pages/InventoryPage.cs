using Spectre.Console;
using mrpv1.Controllers;
using mrpv1.Helpers;
using mrpv1.Models;

namespace mrpv1.Pages;

public class InventoryPage() : Page

{
    readonly InventoryController inventoryController = new();
    readonly PartController partController = new();
    public async Task Display()
    {
        Console.WriteLine("nothing here...");
    }
    
    public async Task<Table> InventoryLocationsTable()
    {
        var myTable = new Table()
    .Title($"[yellow bold]Inventory Locations[/]")
    .RoundedBorder()
    .BorderColor(Color.Grey);
        myTable.AddColumn("Id");
        myTable.AddColumn("Name");
        myTable.AddColumn("Description");
        var inventories = await inventoryController.GetInventories();
        foreach (Inventory inventory in inventories)
        {
            myTable.AddRow(inventory.Id.ToString(), inventory.Location, inventory.Description);
        }
        return myTable;
    }
    public async Task<int> CreatePart()
    {
        string partName = AnsiConsole.Ask<string>($"[green]partname:[/]");
        var partInventoryOptions = new List<Locations.InventoryLocations>
                {
                    Locations.InventoryLocations.CabinetA,
                    Locations.InventoryLocations.ClosetA,
                    Locations.InventoryLocations.Fridge
                    // Locations.InventoryLocations.Garage,
                    // Locations.InventoryLocations.Kitchen
                };
        var partInventoryLocation = AnsiConsole.Prompt(
            new SelectionPrompt<Locations.InventoryLocations>()
                .Title("[green]Select inventory location for the new part[/]")
                .PageSize(10)
                .AddChoices(partInventoryOptions));

        int locationId = (int)partInventoryLocation;

        Part newPart = new()
        {
            Name = partName
        };
        int partId = await partController.CreatePart(newPart);

        return partId;
    }
}