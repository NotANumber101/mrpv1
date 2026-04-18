using Spectre.Console;
using mrpv1.Controllers;
using mrpv1.Models;

namespace mrpv1.Pages;

public class WorkCenterPage() : Page

{
    readonly WorkCenterController workCenterController = new();
    readonly WorkOrderController workOrderController = new();
    readonly OperationController operationController = new();
    
    public async Task Display()
    {


        AnsiConsole.MarkupLine("wc page");
        var workCenters = await workCenterController.GetWorkCenters();
        await DisplayWorkCenter(workCenters[0]);
        await DisplayWorkCenter(workCenters[^1]);
        
        
    }
    public async Task DisplayWorkCenter(WorkCenter wc)
    {
        var tree = new Tree($"{wc.Name} | location:{wc.Location} ")
                .Guide(TreeGuide.BoldLine)
                .Style(Style.Parse("dim"));
            var wcPanel = new Panel(tree).BorderColor(Color.Green);

            int wcqid = await workCenterController.GetWorkCenterQueue(wc.Id);

            var wcQueueNodePanel = new Panel($"[red]work center queue id: {wcqid}[/]");
            var wcNode = tree.AddNode(wcQueueNodePanel);

            List<WorkOrderQueue> workOrderQueues = await workOrderController.GetWorkOrderQueues(wcqid);
            if (workOrderQueues.Count < 1)
            {
                wcNode.AddNode("no work orders");
            }
            else
            {
                foreach (WorkOrderQueue workOrderQueue in workOrderQueues)
                {
                    var workorderqueuenodePanel = new Tree("WorkOrderQueue: " + workOrderQueue.Id.ToString());
                    // var workorderqueuenodePanel = new Panel("WorkOrderQueue: " + workOrderQueue.Id.ToString());
                    var workorderqueuenode = wcNode.AddNode(workorderqueuenodePanel);
                    List<OpExecution> opExecutions = await operationController.GetWorkOrderOpExecutions(workOrderQueue.Id);
                    foreach (OpExecution opExecution in opExecutions)
                    {
                        var opNodePanel = new Panel(
                            $"op_execution for:{opExecution.Id}\nExecution Log: {opExecution.ExecutionLog}\nstart time: {opExecution.TimeStart}"
                        );
                        var opNode = workorderqueuenode.AddNode(opNodePanel);

                        // List<Operation> ops = await operationController.GetOperationById(opExecution.OperationId);

                        // var opTemplate = opNode.AddNode($"operation:({ops[0].Id}){ops[0].Instruction}");
                        // var opConsumptionNode = opTemplate.AddNode($"{"test"}");
                        // var opConsumptionNode = opTemplate.AddNode($"consumes: {ops[0].PartConsumed}\nproduces:{ops[0].PartProduced}");
                    }
                }
            }

            AnsiConsole.Write(wcPanel);
        
    }
}

