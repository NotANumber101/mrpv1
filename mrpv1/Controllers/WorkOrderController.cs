using System;
using mrpv1.Helpers;
using mrpv1.Models;
using mrpv1.Queries;
using Npgsql;
using Spectre.Console;

namespace mrpv1.Controllers;

public class WorkOrderController()
{
    private static readonly string multiHost = "db,localhost";
    readonly NpgsqlDataSourceBuilder dbBuilder = new DbSourceBuilder(multiHost).Builder();

    public async Task<List<WorkOrderQueue>> GetWorkOrderQueues(int workCenterQueueId)
    {
        {
            List<WorkOrderQueue> workOrderQueues = [];
            try
            {
                await using var dataSource = dbBuilder.BuildMultiHost();
                await using (var cmd = dataSource.CreateCommand($"SELECT * FROM work_order_queue WHERE workCenterQueueId={workCenterQueueId};"))

                await using (var reader = await cmd.ExecuteReaderAsync())

                    while (await reader.ReadAsync())
                    {
                        WorkOrderQueue workOrderQueue = new()
                        {
                            Id = reader.GetInt32(0),
                            PartProducedSerialNumber = reader.GetInt32(1),
                            WorkCenterQueueId = reader.GetInt32(2)
                        };
                        workOrderQueues.Add(workOrderQueue);
                    }
            }
            catch (NpgsqlException e)
            {
                Console.WriteLine("Failed.");
                Console.WriteLine(e.Message);
            }
            return workOrderQueues;
        }
    }
    public async Task CreateWorkOrder(int partProducedId)
    {
        AnsiConsole.WriteLine($"[red]MOCK SCENARIO -- CREATING WORK ORDER -- [/]");
        int wc6QueueId = 86;
        // int wcQueueId = 86;

        // TODO: turn this into a cool tree print out
        try
        {
            await using var dataSource = dbBuilder.Build();
            await using var connection = await dataSource.OpenConnectionAsync();
            await using var transaction = await connection.BeginTransactionAsync();
            //for now, manual input wc. In the future this will be part of the buisness logic.
            // for example. wcs  will have machines and equipment that are required by ops

            // work_center (id, location, name) VALUES (99006, 'cabinet a', 'wc6: inoc chamber');
            // work_center_queue (id, workCenterId) VALUES (86, 99006);

            // WorkCenter inocChamber = new() { Id = 99006, Location = "cabinet a", Name = "wc6: inoc chamber" };
            // int inocChamberWcQueueId = 86;
            // pp 111002
            AnsiConsole.MarkupLine($"[red]Createing new workorder...[/]");
            await using var createWorkOrderCommand = new NpgsqlCommand($"INSERT into work_order (partProduced) VALUES ({partProducedId}) RETURNING id;", connection, transaction);
            int workOrderId = (int)createWorkOrderCommand.ExecuteScalar()!;
            AnsiConsole.MarkupLine($"[blue]Work Order: {workOrderId} created\n[/]");


            // create new part instance
            AnsiConsole.MarkupLine($"[red]Createing new part instance...[/]");
            await using var createPartInstance = new NpgsqlCommand($"INSERT into part_instance (partId) VALUES ({partProducedId}) RETURNING id;", connection, transaction);
            int newSerialNumber = (int)createPartInstance.ExecuteScalar()!;
            AnsiConsole.MarkupLine($"[red]part_instance row created for s/n: {newSerialNumber}\n --->created using part produced id = {partProducedId}[/]");



            AnsiConsole.MarkupLine($"[red]Determining the best operations and wcs...[/]");
            // find operations to put into a work order queue
            // Mock = operation (id, instruction, partProduced, partConsumed) VALUES (55750, 'incubate', 111002, 111001);
            // List<Operation> requiredOps = [];
            List<int> opids = [];

            await using var cmd = new NpgsqlCommand($"SELECT * FROM operation WHERE partProduced={partProducedId};", connection, transaction);
            await using (var reader = await cmd.ExecuteReaderAsync())

                while (await reader.ReadAsync())
                {
                    Operation ppOperation = new()
                    {
                        Id = reader.GetInt32(0),
                        Instruction = reader.GetString(1),
                        PartProduced = reader.GetInt32(2)
                    };
                    Console.WriteLine($"----------pp op id-----------{ppOperation.Id}");
                    opids.Add(ppOperation.Id);
                }
            foreach (int id in opids)
            {
                AnsiConsole.MarkupLine($"[red]Operation Success: found op(s) required: op Id={id}[/]");
            }
            AnsiConsole.MarkupLine($"[red]Work Center Success: wc required: 99006[/]");
            



//////////////////////////////////////
/// 
/// 


            AnsiConsole.MarkupLine($"[red]Createing new work order queue...[/]");
            await using var createWorkOrderQueueCommand = new NpgsqlCommand(
                $"INSERT into work_order_queue (partProducedSerialNumber, workCenterQueueId) " +
                $"VALUES ({newSerialNumber}, {wc6QueueId}) RETURNING id;", connection, transaction);
            int workOrderQueueId = (int)createWorkOrderQueueCommand.ExecuteScalar()!;
            AnsiConsole.MarkupLine($"[blue]Work Order Queue{workOrderQueueId} created using work center queue id = {wc6QueueId}[/]");




///////////////////////////////////////////////////

            // create op executions to attach to work order queue
            // attach work order queue to qworkcenter queue
            await using var cmd2 = new NpgsqlCommand($"INSERT into op_execution (operationId, workOrderQueueId, executionLog) "
            + $"VALUES (55750, {workOrderQueueId}, 'test log') RETURNING id;", connection, transaction);
            int opexid = (int)cmd2.ExecuteScalar()!;
            AnsiConsole.MarkupLine($"[red]op ex success: opexid{opexid}[/]");

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