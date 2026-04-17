using System;
using mrpv1.Helpers;
using mrpv1.Models;
using mrpv1.Queries;
using Npgsql;
using Spectre.Console;

namespace mrpv1.Controllers;

public class InventoryController()
{
    private static readonly string multiHost = "db,localhost";
    readonly NpgsqlDataSourceBuilder dbBuilder = new DbSourceBuilder(multiHost).Builder();


    public async Task GetIlocations()
    {
        try
        {
            await using var dataSource = dbBuilder.BuildMultiHost();
            await using (var getPartsCommand = dataSource.CreateCommand("SELECT location from inventory;"))

            await using (var reader = await getPartsCommand.ExecuteReaderAsync())

                while (await reader.ReadAsync())
                {

                    var enumValue = reader.GetFieldValue<string>(0);
                    Console.WriteLine($"--ENUM VALLLLLUUUEEE: {enumValue}");
                }
        }
        catch (NpgsqlException e)
        {
            Console.WriteLine("Failed.");
            Console.WriteLine(e.Message);
        }
        // return parts;











        // // List<Inventory.Locations> iLocations = [];
        // // Reading
        // await using var dataSource = dbBuilder.BuildMultiHost();

        // await using var connection = await dataSource.OpenConnectionAsync();

        // await using (var cmd = new NpgsqlCommand("SELECT location from inventory", connection))
        // await using (var reader = cmd.ExecuteReaderAsync())
        // while (await reader.read{

        //     reader.Read();
        //     var enumValue = reader.GetFieldValue<string>(0);
        //     Console.WriteLine($"ENUM VALLLLLUUUEEE: {enumValue}");
        //     // iLocations.Add(enumValue);
        // }
        // // return iLocations;
    }
    public async Task<List<Part>> GetParts()
    {
        // AnsiConsole.MarkupLine("[gray]Fetching[/]");
        // AnsiConsole.MarkupLine("    -> [gray]Get Parts..[/]");
        List<Part> parts = [];
        try
        {
            await using var dataSource = dbBuilder.BuildMultiHost();
            await using (var getPartsCommand = dataSource.CreateCommand(PartQueries.GetParts()))

            await using (var reader = await getPartsCommand.ExecuteReaderAsync())

                while (await reader.ReadAsync())
                {
                    int partId = reader.GetInt32(0);
                    string partName = reader.GetString(1);
                    Part newPart = new() { Id = partId, Name = partName };
                    parts.Add(newPart);
                }
            // AnsiConsole.MarkupLine($"        -> [green]Done.[/]");
            return parts;
        }
        catch (NpgsqlException e)
        {
            Console.WriteLine("Failed.");
            Console.WriteLine(e.Message);
        }
        return parts;
    }
    public async Task<Part> GetPart(int id)
    {
        Part part = new() { Name = "Not found" };
        try
        {
            await using var dataSource = dbBuilder.BuildMultiHost();
            await using (var cmd = dataSource.CreateCommand($"SELECT * FROM part WHERE id={id};"))

            await using (var reader = await cmd.ExecuteReaderAsync())

                while (await reader.ReadAsync())
                {
                    part.Id = reader.GetInt32(0);
                    part.Name = reader.GetString(1);
                    // Part part = new() { Id = equipmentId, InventoryId = inventoryId, Name = equipmentName, Quantity = quantity };
                }
        }
        catch (NpgsqlException e)
        {
            Console.WriteLine("Failed.");
            Console.WriteLine(e.Message);
        }
        return part;
    }
    public async Task<List<Equipment>> GetEquipments()
    {
        // AnsiConsole.MarkupLine("[gray]Fetching[/]");
        // AnsiConsole.MarkupLine("    -> [gray]Get Equipments..[/]");
        List<Equipment> equipments = [];
        try
        {
            await using var dataSource = dbBuilder.BuildMultiHost();
            await using (var getPartsCommand = dataSource.CreateCommand("SELECT * FROM equipment;"))

            await using (var reader = await getPartsCommand.ExecuteReaderAsync())

                while (await reader.ReadAsync())
                {
                    int equipmentId = reader.GetInt32(0);
                    int inventoryId = reader.GetInt32(1);
                    string equipmentName = reader.GetString(2);
                    int quantity = reader.GetInt32(3);
                    Equipment equipment = new() { Id = equipmentId, InventoryId = inventoryId, Name = equipmentName, Quantity = quantity };
                    equipments.Add(equipment);
                }
            AnsiConsole.MarkupLine($"        -> [green]Done.[/]");
            return equipments;
        }
        catch (NpgsqlException e)
        {
            Console.WriteLine("Failed.");
            Console.WriteLine(e.Message);
        }
        return equipments;
    }
    public async Task<List<Material>> GetMaterials()
    {
        // AnsiConsole.MarkupLine("[gray]Fetching[/]");
        // AnsiConsole.MarkupLine("    -> [gray]Get Materials..[/]");
        List<Material> materials = [];
        try
        {
            await using var dataSource = dbBuilder.BuildMultiHost();
            await using (var getMaterialsCommand = dataSource.CreateCommand("SELECT * FROM material;"))

            await using (var reader = await getMaterialsCommand.ExecuteReaderAsync())

                while (await reader.ReadAsync())
                {
                    int materialId = reader.GetInt32(0);
                    int inventoryId = reader.GetInt32(1);
                    string materialName = reader.GetString(2);
                    int quantity = reader.GetInt32(3);
                    Material newMaterial = new() { Id = materialId, InventoryId = inventoryId, Name = materialName, Quantity = quantity };
                    materials.Add(newMaterial);
                }
            // AnsiConsole.MarkupLine($"        -> [green]Done.[/]");
            return materials;
        }
        catch (NpgsqlException e)
        {
            Console.WriteLine("Failed.");
            Console.WriteLine(e.Message);
        }
        return materials;
    }
    public async Task<List<Tool>> GetTools()
    {
        // AnsiConsole.MarkupLine("[gray]Fetching[/]");
        // AnsiConsole.MarkupLine("    -> [gray]Get Tools..[/]");
        List<Tool> tools = [];
        try
        {
            await using var dataSource = dbBuilder.BuildMultiHost();
            await using (var getToolsCommand = dataSource.CreateCommand("SELECT * FROM tool;"))

            await using (var reader = await getToolsCommand.ExecuteReaderAsync())

                while (await reader.ReadAsync())
                {
                    int toolId = reader.GetInt32(0);
                    int inventoryId = reader.GetInt32(1);
                    string toolName = reader.GetString(2);
                    int quantity = reader.GetInt32(3);
                    Tool newTool = new() { Id = toolId, InventoryId = inventoryId, Name = toolName, Quantity = quantity };
                    tools.Add(newTool);
                }
            // AnsiConsole.MarkupLine($"        -> [green]Done.[/]");
            return tools;
        }
        catch (NpgsqlException e)
        {
            Console.WriteLine("Failed.");
            Console.WriteLine(e.Message);
        }
        return tools;
    }
    public async Task<List<Inventory>> GetInventories()
    {
        // AnsiConsole.MarkupLine("[gray]Fetching[/]");
        // AnsiConsole.MarkupLine("    -> [gray]Get Inventories..[/]");
        List<Inventory> inventories = [];
        try
        {
            await using var dataSource = dbBuilder.BuildMultiHost();
            await using (var getInventoriesCommand = dataSource.CreateCommand("SELECT * FROM inventory;"))

            await using (var reader = await getInventoriesCommand.ExecuteReaderAsync())

                while (await reader.ReadAsync())
                {
                    int inventoryId = reader.GetInt32(0);
                    string inventoryLocation = reader.GetString(2);
                    string inventoryDescription = reader.GetString(2);
                    Inventory inventory = new() { Id = inventoryId, Location = inventoryLocation, Description = inventoryDescription };
                    inventories.Add(inventory);
                }
            // AnsiConsole.MarkupLine($"        -> [green]Done.[/]");
            return inventories;
        }
        catch (NpgsqlException e)
        {
            Console.WriteLine("Failed.");
            Console.WriteLine(e.Message);
        }
        return inventories;
    }





    public async Task<List<Machine>> GetMachines()
    {
        // AnsiConsole.MarkupLine("[gray]Fetching[/]");
        // AnsiConsole.MarkupLine("    -> [gray]GetMachine..[/]");
        List<Machine> machines = [];
        try
        {
            await using var dataSource = dbBuilder.BuildMultiHost();
            await using (var getMachineCommand = dataSource.CreateCommand("SELECT * FROM machine;"))

            await using (var reader = await getMachineCommand.ExecuteReaderAsync())

                while (await reader.ReadAsync())
                {
                    int machineId = reader.GetInt32(0);
                    int inventoryId = reader.GetInt32(1);
                    string machineName = reader.GetString(2);
                    int quantity = reader.GetInt32(3);
                    Machine newMachine = new() { Id = machineId, InventoryId = inventoryId, Name = machineName, Quantity = quantity };
                    machines.Add(newMachine);
                }
            // AnsiConsole.MarkupLine($"        -> [green]Done.[/]");
            return machines;
        }
        catch (NpgsqlException e)
        {
            Console.WriteLine("Failed.");
            Console.WriteLine(e.Message);
        }
        return machines;
    }
    public async Task CreatePart(Part part)
    {
        AnsiConsole.MarkupLine("[gray]Inserting data...[/]");
        AnsiConsole.MarkupLine("    -> [gray]Create Part[/]");
        try
        {
            await using var dataSource = dbBuilder.BuildMultiHost();
            await using var connection = await dataSource.OpenConnectionAsync();
            await using var transaction = await connection.BeginTransactionAsync();

            await using var createPartCommand = new NpgsqlCommand(PartQueries.CreatePart(part), connection, transaction);
            await createPartCommand.ExecuteNonQueryAsync();

            await transaction.CommitAsync();
            AnsiConsole.MarkupLine($"        -> [green]Done.[/]");
        }
        catch (NpgsqlException e)
        {
            Console.WriteLine("Failed.");
            Console.WriteLine(e.Message);
        }
    }

}


