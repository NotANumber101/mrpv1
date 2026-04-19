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
            return parts;
        }
        catch (NpgsqlException e)
        {
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


