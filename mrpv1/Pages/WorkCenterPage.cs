using mrpv1.Controllers;
using mrpv1.Helpers;
using mrpv1.Models;
using Spectre.Console;

namespace mrpv1.Pages;

public class WorkCenterPage() : Page

{
    readonly WorkCenterController workCenterController = new();
    readonly WorkOrderController workOrderController = new();
    readonly OperationController operationController = new();
    readonly InventoryController inventoryController = new();
    public async Task Display()
    {


        AnsiConsole.MarkupLine("wc page");
        var res = await workCenterController.GetWorkCenters();
        // await inventoryController.GetIlocations();
        foreach (var wc in res)
        {

            var tree = new Tree("Work center queue");
            AnsiConsole.MarkupLine("--------------------------------------------");
            // AnsiConsole.MarkupLine($"[blue]wc found! {wc.Id}[/]");
            int wcqid = await workCenterController.GetWorkCenterQueue(wc.Id);
            var wcNode = tree.AddNode($"[red]{wc.Name}(location:{wc.Location}[/]");

      
            // AnsiConsole.MarkupLine($"----> {wc.Name}");
            // AnsiConsole.MarkupLine($"    ------> {wc.Location}");
            // AnsiConsole.MarkupLine($"    ------> work center queue id: {wcqid}");

            List<WorkOrderQueue> workOrderQueues = await workOrderController.GetWorkOrderQueues(wcqid);
            if (workOrderQueues.Count < 1)
            {
                // AnsiConsole.MarkupLine($"         ------> work center not assigned any work orders");
                wcNode.AddNode("no work orders");
                // add mock work order

            }
            foreach (WorkOrderQueue workOrderQueue in workOrderQueues)
            {
                var workorderqueuenode = wcNode.AddNode(workOrderQueue.Id.ToString());
                // AnsiConsole.MarkupLine($"         ------> work ORDER queue:{workOrderQueue.Id}");
                // AnsiConsole.MarkupLine($"               ------> workcenterqueue id {workOrderQueue.WorkCenterQueueId}");
                // AnsiConsole.MarkupLine($"               ------> part produced serial {workOrderQueue.PartProducedSerialNumber}");
                List<OpExecution> opExecutions = await operationController.GetWorkOrderOpExecutions(workOrderQueue.Id);
                foreach (OpExecution op in opExecutions)
                {
                    var opNode = workorderqueuenode.AddNode($"opEx for operation:{op.OperationId}");

                    // AnsiConsole.MarkupLine($"        ---------------> woq({workOrderQueue.Id})opExecutionId: {op.Id}, op template: {op.OperationId}");
                    // AnsiConsole.MarkupLine($"        ---------------------> ");
                    // get op template

                }
            }
            AnsiConsole.Write(tree);


            // AnsiConsole.MarkupLine("--------------------------------------------");
            // await DisplayTree();
        }
                    if (AnsiConsole.Confirm("CREATE TEST WORKORDER?"))
            {
                await workOrderController.CreateWorkOrder(111002);
            }

    }
    public async Task DisplayTree()
    {
        var fileSystem = new Dictionary<string, string[]>
        {
            ["Work Order 1"] = new[] { "appsettings.json", "secrets.json" },
            ["work order 2"] = new[] { "Main.cs", "Helper.cs", "Utilities.cs" },
            ["work order 3"] = new[] { "README.md", "CHANGELOG.md" },
        };
        fileSystem["2222"] = new[] { "D" };

        var tree = new Tree("Work center queue");

        foreach (var (folder, files) in fileSystem)
        {
            var folderNode = tree.AddNode($"[yellow]{folder}[/]");
            foreach (var file in files)
            {
                folderNode.AddNode($"[grey]{file}[/]");
            }
        }

        AnsiConsole.Write(tree);
    }
}