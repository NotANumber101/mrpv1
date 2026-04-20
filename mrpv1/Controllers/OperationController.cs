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


    public async Task<List<Operation>> GetOperationById(int id)
    {
        List<Operation> res = [];
        try
        {
            await using var dataSource = dbBuilder.BuildMultiHost();
            await using (var cmd = dataSource.CreateCommand($"SELECT * FROM operation WHERE id={id};"))

            await using (var reader = await cmd.ExecuteReaderAsync())

                while (await reader.ReadAsync())
                {
                    Operation foundOp = new Operation()
                    {
                        Id = reader.GetInt32(0),
                        Instruction = reader.GetString(1),
                        PartProduced = reader.GetInt32(2),
                        PartConsumed = reader.GetInt32(3)
                    };
                    res.Add(foundOp);
                }
        }
        catch (NpgsqlException e)
        {
            Console.WriteLine("Failed.");
            Console.WriteLine(e.Message);
        }
        return res;
    }
    public async Task<List<Operation>> GetOperations()
    {
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
                    Operation newOp = new()
                    {
                        Id = operationId,
                        Instruction = operationInstruction,
                        PartProduced = partProduced,
                        PartConsumed = partConsumed
                    };
                    operations.Add(newOp);
                }
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
        List<Part> partsConsumed = new();
        List<Part> partsProduced = new();
        try
        {
            await using var dataSource = dbBuilder.BuildMultiHost();
            await using var connection = await dataSource.OpenConnectionAsync();
            await using var transaction = await connection.BeginTransactionAsync();

            await using var cmd1 = new NpgsqlCommand($"SELECT * FROM part WHERE id={newOp.PartProduced}", connection, transaction);
            await using (var reader = await cmd1.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    Part partProduced = new Part()
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1)
                    };
                    partsProduced.Add(partProduced);
                }

            await using var cmd2 = new NpgsqlCommand($"SELECT * FROM part WHERE id={newOp.PartConsumed}", connection, transaction);
            await using (var reader = await cmd2.ExecuteReaderAsync())

                while (await reader.ReadAsync())
                {

                    Part partConsumed = new Part()
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1)
                    };
                    partsConsumed.Add(partConsumed);
                }

            if (partsConsumed.Any() && partsProduced.Any())
            {
                await using var createOperationCommand = new NpgsqlCommand(OperationQueries.CreateOperation(newOp), connection, transaction);
                await createOperationCommand.ExecuteNonQueryAsync();

                await transaction.CommitAsync();
            }


        }
        catch (NpgsqlException e)
        {
            Console.WriteLine("Failed.");
            Console.WriteLine(e.Message);
        }
    }
    public async Task<List<OpExecution>> GetWorkOrderOpExecutions(int workOrderQueueId)
    {
        {
            List<OpExecution> opExecutions = [];
            try
            {
                await using var dataSource = dbBuilder.BuildMultiHost();
                await using (var cmd = dataSource.CreateCommand($"SELECT * FROM op_execution WHERE workOrderQueueId={workOrderQueueId};"))

                await using (var reader = await cmd.ExecuteReaderAsync())

                    while (await reader.ReadAsync())
                    {
                        OpExecution opExecution = new()
                        {
                            Id = reader.GetInt32(0),
                            OperationId = reader.GetInt32(1),
                            WorkOrderQueueId = reader.GetInt32(2),
                            ExecutionLog = reader.GetString(3),
                            TimeStart = reader.GetDateTime(4),
                            TimeStop = reader.GetDateTime(5),

                        };
                        opExecutions.Add(opExecution);
                    }
            }
            catch (NpgsqlException e)
            {
                Console.WriteLine("Failed.");
                Console.WriteLine(e.Message);
            }
            return opExecutions;
        }
    }
}


