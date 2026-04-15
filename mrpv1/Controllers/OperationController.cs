using System;
using mrpv1.Helpers;
using mrpv1.Models;
using mrpv1.Queries;
using Npgsql;
using Spectre.Console;

namespace mrpv1.Controllers;

public class OperationController()
{
    private static readonly string multiHost = "db,localhost";
    readonly NpgsqlDataSourceBuilder dbBuilder = new DbSourceBuilder(multiHost).Builder();

    public async Task<List<Operation>> GetOperations()
    {
        AnsiConsole.MarkupLine("[gray]Fetching[/]");
        AnsiConsole.MarkupLine("    -> [gray]Get all operations[/]");
        List<Operation> operations = [];
        try
        {
            await using var dataSource = dbBuilder.BuildMultiHost();
            await using (var getOperationsCommand = dataSource.CreateCommand(OperationQueries.GetOperations()))

            await using (var reader = await getOperationsCommand.ExecuteReaderAsync())

                while (await reader.ReadAsync())
                {

                    int operationId = reader.GetInt32(0);
                    string operationInstruction = reader.GetString(1);
                    int partProduced = reader.GetInt32(2);
                    int partConsumed = reader.GetInt32(3);
                    int mPartProduced = reader.GetInt32(4);
                    int mPartConsumed = reader.GetInt32(5);
                    int material = reader.GetInt32(6);
                    int tool = reader.GetInt32(7);
                    int equipment = reader.GetInt32(8);
                    int machine = reader.GetInt32(9);
                    Operation newOp = new() {
                        Id = operationId, Instruction = operationInstruction,
                        PartProduced = partProduced, PartConsumed = partConsumed,
                        MPartProduced = mPartConsumed, MPartConsumed = mPartConsumed,
                        Material = material, Tool = tool, Equipment = equipment, Machine = machine
                        };
                    operations.Add(newOp);
                }
            AnsiConsole.MarkupLine($"        -> [green]Done.[/]");
            return operations;
        }
        catch (NpgsqlException e)
        {
            Console.WriteLine("Failed.");
            Console.WriteLine(e.Message);
        }
        return operations;
    }
    public async Task CreateOperation(Operation newOp)
    {
        AnsiConsole.MarkupLine("[gray]Inserting data...[/]");
        AnsiConsole.MarkupLine("    -> [gray]Create Operation[/]");
        try
        {
            await using var dataSource = dbBuilder.BuildMultiHost();
            await using var connection = await dataSource.OpenConnectionAsync();
            await using var transaction = await connection.BeginTransactionAsync();

            await using var createOperationCommand = new NpgsqlCommand(OperationQueries.CreateOperation(newOp), connection, transaction);
            await createOperationCommand.ExecuteNonQueryAsync();

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


