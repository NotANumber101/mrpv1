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


var partsPanel = new Panel(partTable).Header("Parts").BorderColor(Color.Black);
var left1 = new Panel(equipmentTable).Header("Machines").BorderColor(Color.Black);
var right1 = new Panel(materialTable).Header("Materials").BorderColor(Color.Black);
var left2 = new Panel(machineTable).Header("Tools").BorderColor(Color.Black);
var right2 = new Panel(toolTable).Header("Equipment").BorderColor(Color.Black);


        AnsiConsole.Write(partsPanel);
        AnsiConsole.Write(new Columns(left1, right1));
        AnsiConsole.Write(new Columns(left2, right2));
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