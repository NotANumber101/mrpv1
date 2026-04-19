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
                    Part newPart = new() { Id = partId, Name = partName};
                    parts.Add(newPart);
                }
            return parts;
        }
        catch (NpgsqlException e)
        {
            Console.WriteLine("Failed.");
            Console.WriteLine(e.Message);
        }
        return parts;
    }
    public async Task<Part> GetPartV1(int id)
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
    public async Task<Part?> GetPartV2(int id)
    {
        try
        {
            await using var dataSource = dbBuilder.BuildMultiHost();
            await using var command = dataSource.CreateCommand($"SELECT * FROM part WHERE id={id}");
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {   if (reader[0] != null)
                {
                    Part foundPart = new Part() { 
                    Id= reader.GetInt32(0),
                    Name = reader.GetString(0)
                };
                return foundPart;
                }
            }
        }
        catch (NpgsqlException e)
        {
            Console.WriteLine(e.Message);
        }
        return null;
    }
    public async Task<int> CreatePart(Part part)
    {
        int newPartId = 0;
        try
        {
            await using var dataSource = dbBuilder.BuildMultiHost();
            await using var connection = await dataSource.OpenConnectionAsync();
            await using var transaction = await connection.BeginTransactionAsync();

            await using var createPartCommand = new NpgsqlCommand(PartQueries.CreatePart(part), connection, transaction);
            newPartId = await createPartCommand.ExecuteNonQueryAsync();

            await transaction.CommitAsync();
        }
        catch (NpgsqlException e)
        {
            Console.WriteLine(e.Message);
        }
        return newPartId;
    }

}


