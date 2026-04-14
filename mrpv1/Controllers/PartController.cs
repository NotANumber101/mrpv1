using System;
using mrpv1.Helpers;
using mrpv1.Models;
using mrpv1.Queries;
using Npgsql;
using Spectre.Console;

namespace mrpv1.Controllers;

public class PartController()
{
    private static readonly string multiHost = "db,localhost";
    readonly NpgsqlDataSourceBuilder dbBuilder = new DbSourceBuilder(multiHost).Builder();

    public async Task<List<Part>> GetParts()
    {
        AnsiConsole.MarkupLine("[gray]Fetching[/]");
        AnsiConsole.MarkupLine("    -> [gray]GetParts..[/]");
        List<Part> parts = [];
        try
        {
            await using var dataSource = dbBuilder.BuildMultiHost();
            await using (var getPartsCommand = dataSource.CreateCommand(PartQueries.GetParts()))

            await using (var reader = await getPartsCommand.ExecuteReaderAsync())

                while (await reader.ReadAsync())
                {
                    int partId = reader.GetInt32(0);
                    int inventoryId = reader.GetInt32(1);
                    string partName = reader.GetString(2);
                    int quantity = reader.GetInt32(3);
                    Part newPart = new() { Id = partId, InventoryId = inventoryId, Name = partName, Quantity = quantity };
                    parts.Add(newPart);
                }
            AnsiConsole.MarkupLine($"        -> [green]Done.[/]");
            return parts;
        }
        catch (NpgsqlException e)
        {
            Console.WriteLine("Failed.");
            Console.WriteLine(e.Message);
        }
        return parts;
    }
    public async Task<int> CreatePart(Part part)
    {
        int newPartId = 0;
        AnsiConsole.MarkupLine("[gray]Inserting data...[/]");
        AnsiConsole.MarkupLine("    -> [gray]Create Part[/]");
        try
        {
            await using var dataSource = dbBuilder.BuildMultiHost();
            await using var connection = await dataSource.OpenConnectionAsync();
            await using var transaction = await connection.BeginTransactionAsync();

            await using var createPartCommand = new NpgsqlCommand(PartQueries.CreatePart(part), connection, transaction);
            newPartId = await createPartCommand.ExecuteNonQueryAsync();

            await transaction.CommitAsync();
            AnsiConsole.MarkupLine($"        -> [green]Done.[/]");
        }
        catch (NpgsqlException e)
        {
            Console.WriteLine("Failed.");
            Console.WriteLine(e.Message);
        }
        return newPartId;
    }

}


