using Spectre.Console;
using mrpv1.Controllers;
using mrpv1.Models;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace mrpv1.Pages;

public class WorkCenterPage() : Page

{
    readonly WorkCenterController workCenterController = new();
    readonly WorkOrderController workOrderController = new();
    readonly OperationController operationController = new();

    public async Task Display()
    {


        DisplayWorkCenterQueue();

        AnsiConsole.MarkupLine("wc page");
        var workCenters = await workCenterController.GetWorkCenters();
        // await DisplayWorkCenter(workCenters[3]);
        // await DisplayWorkCenter(workCenters[^1]);
        // foreach(var wc in workCenters)
        // {
        //     await DisplayWorkCenter(wc);
        // }   
        // DisplayWorkCenterv2();
    }
    public void DisplayWorkCenterv2()
    {


        var grid = new Grid();

        // Configure columns
        grid.AddColumn(new GridColumn { Width = 20, Alignment = Justify.Right });
        grid.AddColumn(new GridColumn());

        // Add header
        grid.AddRow(
            new Text("Work Order Information", new Style(Color.Yellow, decoration: Decoration.Bold)),
            new Text(""));

        grid.AddEmptyRow();

        // Add data rows
        // grid.AddRow(new Markup("OS:"), new Markup("[blue]Linux[/]"));
        // grid.AddRow(new Markup("CPU:"), new Markup("[green]8 cores @ 3.2GHz[/]"));
        grid.AddRow(
            new Markup("Memory:"),
            new BreakdownChart()
                .Width(40)
                .AddItem("Used", 1, Color.Green)
                .AddItem("Days Left", 14, Color.Gray));
        grid.AddRow(
            new Markup("Status"),
            new Panel("[red]Operation 1 of 1[/]")
                .BorderColor(Color.Yellow));

        AnsiConsole.Write(grid);
    }

    public void DisplayWorkCenterv3()
    {

        // Grid myGrid = new Grid();
        //         myGrid.AddColumn(new GridColumn { Width = 100, Alignment = Justify.Right });
        //         myGrid.AddColumn(new GridColumn());



        //         var tree = new Tree($"{wc.Name} | location:{wc.Location} ")
        //                 .Guide(TreeGuide.BoldLine)
        //                 .Style(Style.Parse("dim"));


        //         var wcPanel = new Panel(tree).BorderColor(Color.Green);

        //         int wcqid = await workCenterController.GetWorkCenterQueue(wc.Id);

        //         var wcQueueNodePanel = new Panel($"[red]work center queue id: {wcqid}[/]");
        //         var wcNode = tree.AddNode(wcQueueNodePanel);

        //         List<WorkOrderQueue> workOrderQueues = await workOrderController.GetWorkOrderQueues(wcqid);
        //         if (workOrderQueues.Count < 1)
        //         {
        //             wcNode.AddNode("no work orders");
        //         }
        //         else
        //         {
        //             foreach (WorkOrderQueue workOrderQueue in workOrderQueues)
        //             {


        //                 var workorderqueuenodePanel = new Tree("WorkOrderQueue: " + workOrderQueue.Id.ToString());
        //                 // var workorderqueuenodePanel = new Panel("WorkOrderQueue: " + workOrderQueue.Id.ToString());

        //                 var workorderqueuenode = wcNode.AddNode(workorderqueuenodePanel);
        //                 List<OpExecution> opExecutions = await operationController.GetWorkOrderOpExecutions(workOrderQueue.Id);
        //                 foreach (OpExecution opExecution in opExecutions)
        //                 {
        //                     var opNodePanel = new Panel(
        //                         $"op_execution for:{opExecution.Id}\nExecution Log: {opExecution.ExecutionLog}\nstart time: {opExecution.TimeStart}"
        //                     );
        //                     var opNode = workorderqueuenode.AddNode(opNodePanel);

        //                     List<Operation> ops = await operationController.GetOperationById(opExecution.OperationId);

        //                     var opTemplate = opNode.AddNode($"operation:({ops[0].Id}){ops[0].Instruction}");
        //                     // var opConsumptionNode = opTemplate.AddNode($"{"test"}");
        //                     new BreakdownChart()
        //                         .Width(40)
        //                         .AddItem("Used", 1, Color.Green)
        //                         .AddItem("Days Left", 14, Color.Gray);
        //                     var opConsumptionNode = opTemplate.AddNode(new BreakdownChart()
        //                         .Width(40)
        //                         .AddItem("Used", 1, Color.Green)
        //                         .AddItem("Days Left", 14, Color.Gray));
        //                     // var opConsumptionNode = opTemplate.AddNode($"consumes: {ops[0].PartConsumed}\nproduces:{ops[0].PartProduced}");
        //                 }
        //             }
        //         }

        //         // AnsiConsole.Write(wcPanel);
        //         AnsiConsole.Write(myGrid);


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



                Grid myGrid = new Grid();
                myGrid.AddColumn(new GridColumn { Alignment = Justify.Left });
                myGrid.AddColumn(new GridColumn());





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

                    List<Operation> ops = await operationController.GetOperationById(opExecution.OperationId);

                    // if child op
                    // print ops

                    var opTemplate = opNode.AddNode($"operation:({ops[0].Id}){ops[0].Instruction}").Expand();
                    // var opConsumptionNode = opTemplate.AddNode($"{"test"}");
                    new BreakdownChart()
                        .Width(40)
                        .AddItem("Used", 1, Color.Green)
                        .AddItem("Days Left", 14, Color.Gray);
                    var opConsumptionNode = opTemplate.AddNode(new BreakdownChart()
                        .Width(40)
                        .AddItem("Used", 1, Color.Green)
                        .AddItem("Days Left", 14, Color.Gray));
                    // var opConsumptionNode = opTemplate.AddNode($"consumes: {ops[0].PartConsumed}\nproduces:{ops[0].PartProduced}");
                    myGrid.AddRow(tree, new Panel("--- TEST ---"));
                }
                // AnsiConsole.Write(myGrid);

            }

        }
        AnsiConsole.Write(wcPanel);
    }
    public void DisplayWorkCenterQueue()
    {






        // var tree = new Tree(new Panel("workcenter queue: bloom room row 1"));

        // var layer1_node1 = tree.AddNode("layer 1");
        // layer1_node1.AddNode("layer 2");

        int layer = 0;
        int max = 1;


        // mock input


        while (layer < 1)
        {


        Operation mock_op1 = new Operation()
        {
          Id = layer+20,
          Instruction = "Mock insrucitons"
        };
        OpExecution mock_opex1 = new OpExecution()
        {
            Id = layer+110,
            OperationId = layer+20,
            WorkOrderQueueId = 1,
            ExecutionLog = "dfasgagg",
            TimeStart = DateTime.Now,
            TimeStop = DateTime.Now
        
        };



    // public int Id { get; set; }
    // public int OperationId { get; set; }
    // public int WorkOrderQueueId { get; set; }
    // public string? ExecutionLog { get; set; }
    // public DateTime? TimeStart { get; set; }
    // public DateTime? TimeStop { get; set; }








        var tree = new Tree(new Panel("workcenter queue: bloom room row 1"));

            var node = tree.AddNode(new Panel($"Work Order Queue: {mock_opex1.OperationId}"));


            var workOrderInformation = node.AddNode(new Table()
            .AddColumn("workorder id: 112322")
            .AddColumn("1/2 ops complete")
            .AddColumn(" workorder - part produced: mushroom container 3.5qt(111004)"));

            // var parent = node.AddNode("Parent: info......");
            var parent1 = node.AddNode($"Instruction: {mock_op1.Instruction}");
            var opexinfo = parent1.AddNode("part Instance Produced: s/n 0000"+(layer+1));
            var child2 = parent1.AddNode("Execution Log");





            var bdChart = new BreakdownChart().Width(40);



            bdChart.AddItem("Op_ex % complete(days): ", RandomNumberGenerator.GetInt32(2, 8), Color.Green);
            bdChart.AddItem("Op_ex % remaining(days)", RandomNumberGenerator.GetInt32(10, 30), Color.Gray);
            var child6 = parent1.AddNode(new Panel(bdChart));

            var child5 = child6.AddNode("time estimated: 30 days");


            var child7 = child6.AddNode("next mock_op: opid12314");
            var nextOps = child6.AddNode(new Table()
                .AddColumn("next op")
                .AddColumn("next next op")
                .AddColumn("next next next op"));
            layer++;

        AnsiConsole.Write(new Panel(tree));

        }

        // AnsiConsole.Write(tree);





        // input: list of op_exs that all have the same workorderqueue id
        // input is sorted 
    }
}

