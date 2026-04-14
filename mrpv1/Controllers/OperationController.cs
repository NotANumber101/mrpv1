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
                    Operation newOp = new() { Id = operationId, Instruction = operationInstruction };
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


