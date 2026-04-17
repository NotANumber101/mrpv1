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
            var tree = new Tree($"{wc.Name} | location:{wc.Location} ")
                .Guide(TreeGuide.BoldLine)
                .Style(Style.Parse("dim"));
            var wcPanel = new Panel(tree).BorderColor(Color.Green);
            int wcqid = await workCenterController.GetWorkCenterQueue(wc.Id);
            var wcNodePanel = new Panel($"[red]work center queue id: {wcqid}[/]");
            var wcNode = tree.AddNode(wcNodePanel);

            List<WorkOrderQueue> workOrderQueues = await workOrderController.GetWorkOrderQueues(wcqid);
            if (workOrderQueues.Count < 1)
            {
                wcNode.AddNode("no work orders");
            }
            foreach (WorkOrderQueue workOrderQueue in workOrderQueues)
            {
                var workorderqueuenodePanel = new Panel("WorkOrderQueue: " + workOrderQueue.Id.ToString());
                var workorderqueuenode = wcNode.AddNode(workorderqueuenodePanel);
                List<OpExecution> opExecutions = await operationController.GetWorkOrderOpExecutions(workOrderQueue.Id);
                foreach (OpExecution opExecution in opExecutions)
                {
                    var opNodePanel = new Panel($"op_execution:{opExecution.Id}\nExecution Log: {opExecution.ExecutionLog}");
                    var opNode = workorderqueuenode.AddNode(opNodePanel);


                    List<Operation> ops = await operationController.GetOperationById(opExecution.OperationId);
                    var opTemplate = opNode.AddNode($"operation:({ops[0].Id}){ops[0].Instruction}");
                    var opConsumptionNode = opTemplate.AddNode($"consumes: {ops[0].PartConsumed}\nproduces:{ops[0].PartProduced}");

                }
            }
            AnsiConsole.Write(wcPanel);


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