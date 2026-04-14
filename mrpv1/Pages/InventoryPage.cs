using System;
using Spectre.Console;
using mrpv1.Helpers;
using Npgsql;
using mrpv1.Controllers;
using mrpv1.Models;
using Spectre.Console.Rendering;

namespace mrpv1.Pages;

public class InventoryPage() : Page

{
    readonly InventoryController inventoryController = new();
    readonly PartController partController = new();
    public async Task Display()
    {
        await DisplayInventory();
        await MainMenu();
        // await CreatePart();
    }
    public async Task DisplayInventory()
    {
        var partTable = await PartTable();
        // var panel = new Panel(partTable)
        //     .Header("Server Status")
        //     .BorderColor(Color.Blue);



        var materialTable = await MaterialTable();
        var toolTable = await ToolTable();
        var equipmentTable = await EquipmentTable();
        var machineTable = await MachineTable();
        var inventortyTable = await InventoryLocationsTable();


        var partsPanel = new Panel(partTable).Header("Parts").BorderColor(Color.Black).Expand();
        var inventoryLocationsPanel = new Panel(inventortyTable).Header("Inventory").BorderColor(Color.Black).Expand();

        var left1 = new Panel(equipmentTable).Header("Machines").BorderColor(Color.Black).Expand();
        var right1 = new Panel(materialTable).Header("Materials").BorderColor(Color.Black).Expand();
        var left2 = new Panel(machineTable).Header("Tools").BorderColor(Color.Black).Expand();
        var right2 = new Panel(toolTable).Header("Equipment").BorderColor(Color.Black).Expand();


        // AnsiConsole.Write(new Columns(partsPanel, inventoryLocationsPanel));

        AnsiConsole.Write(new Columns(left1, right1, right2));
        AnsiConsole.Write(new Columns(left2, partsPanel, inventoryLocationsPanel));
    }
    public Table InventoryItemTableBuilder(string title)
    {
        var myTable = new Table()
            .Title($"[yellow bold]{title}[/]")
            .RoundedBorder()
            .BorderColor(Color.Grey);
        myTable.AddColumn("Id");
        myTable.AddColumn("InvId");
        myTable.AddColumn("Name");
        myTable.AddColumn("Quantity");
        return myTable;
    }
    public async Task<Table> PartTable()
    {
        var myTable = InventoryItemTableBuilder("parts");
        var inventoryItems = await inventoryController.GetParts();
        foreach (Part inventoryItem in inventoryItems)
        {
            myTable.AddRow(inventoryItem.Id.ToString(), inventoryItem.InventoryId.ToString(), inventoryItem.Name, inventoryItem.Quantity.ToString());
        }
        return myTable;
    }
    public async Task<Table> MaterialTable()
    {
        var myTable = InventoryItemTableBuilder("materials");
        var inventoryItems = await inventoryController.GetMaterials();
        foreach (Material inventoryItem in inventoryItems)
        {
            myTable.AddRow(inventoryItem.Id.ToString(), inventoryItem.InventoryId.ToString(), inventoryItem.Name, inventoryItem.Quantity.ToString());
        }
        return myTable;

    }
    public async Task<Table> ToolTable()
    {
        var myTable = InventoryItemTableBuilder("tools");
        var inventoryItems = await inventoryController.GetTools();
        foreach (Tool inventoryItem in inventoryItems)
        {
            myTable.AddRow(inventoryItem.Id.ToString(), inventoryItem.InventoryId.ToString(), inventoryItem.Name, inventoryItem.Quantity.ToString());
        }
        return myTable;

    }
    public async Task<Table> EquipmentTable()
    {
        var myTable = InventoryItemTableBuilder("equipment");
        var inventoryItems = await inventoryController.GetEquipments();
        foreach (Equipment inventoryItem in inventoryItems)
        {
            myTable.AddRow(inventoryItem.Id.ToString(), inventoryItem.InventoryId.ToString(), inventoryItem.Name, inventoryItem.Quantity.ToString());
        }
        return myTable;

    }
    public async Task<Table> MachineTable()
    {
        var myTable = InventoryItemTableBuilder("machines");
        var inventoryItems = await inventoryController.GetMachines();
        foreach (Machine inventoryItem in inventoryItems)
        {
            myTable.AddRow(inventoryItem.Id.ToString(), inventoryItem.InventoryId.ToString(), inventoryItem.Name, inventoryItem.Quantity.ToString());
        }
        return myTable;

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
        int partId = await partController.CreatePart(newPart);

        return partId;
    }
}