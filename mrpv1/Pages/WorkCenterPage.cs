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

    public async Task Display(WorkCenter workCenter)
    {

        await DisplayWorkCenter(workCenter);
        // await DisplayWorkOrderQueue(workCenter.Id);
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


                var workorderqueuenodePanel = new Tree($"[blue]WorkOrderQueue: {workOrderQueue.Id} | Serial Number: {workOrderQueue.PartProducedSerialNumber}[/]");


                var workorderqueuenode = wcNode.AddNode(workorderqueuenodePanel);
                List<OpExecution> opExecutions = await operationController.GetWorkOrderOpExecutions(workOrderQueue.Id);

                foreach (OpExecution opExecution in opExecutions)
                {

                    DateOnly date = DateOnly.FromDateTime((DateTime)opExecution.TimeStart);
                    // DateOnly today = new DateTime(DateOnly);
                    DateOnly today2 = DateOnly.FromDateTime((DateTime)DateTime.Now);
                    // Console.WriteLine((opExecution.TimeStart - opExecution.TimeStart));


                    // Declare two dates
                    var prevDate = new DateTime(2021, 7, 15); //15 July 2021
                    var startDate = opExecution.TimeStart;
                    var endDate = opExecution.TimeStop;
                    var today = DateTime.Now;


                    var diffOfDatesStart = today - startDate;
                    var diffOfDatesEnd = endDate - today;
                    // Console.WriteLine("Dsssifference in Days: {0}", diffOfDatesStart.Days);
                    // Console.WriteLine("Dsssifference in Days: {0}", diffOfDatesEnd.Days);

                    var opNodePanel = new Panel(
                        $"op_execution Id: {opExecution.Id}\nExecution Log: {opExecution.ExecutionLog}\nstart time: {date}"
                    );
                    
                    var opNode = workorderqueuenode.AddNode(opNodePanel);
                    List<Operation> ops = await operationController.GetOperationById(opExecution.OperationId);
                    if (ops.Count < 1)
                    {
                        opNode.AddNode("No ops found");
                    }
                    else
                    {

                        var opTemplate = opNode.AddNode($"operation:({ops[0].Id}){ops[0].Instruction}").Expand();

                        // var opConsumptionNode = opTemplate.AddNode($"{"test"}");
                        // new BreakdownChart()
                        //     .Width(40)
                        //     .AddItem("Used", 1, Color.Green)
                        //     .AddItem("Days Left", 14, Color.Gray);
                        var opConsumptionNode = opTemplate.AddNode(new BreakdownChart()
                            .Width(40)
                            .AddItem("Used", diffOfDatesStart.Days, Color.Green)
                            .AddItem("Days Left", diffOfDatesEnd.Days, Color.Gray));
                        // var opConsumptionNode = opTemplate.AddNode($"consumes: {ops[0].PartConsumed}\nproduces:{ops[0].PartProduced}");
                        myGrid.AddRow(tree, new Panel("--- TEST ---"));
                    }

                }
                // AnsiConsole.Write(myGrid);

            }

        }
        AnsiConsole.Write(wcPanel);
    }
    public async Task DisplayWorkOrderQueue(int workOrderQueueId)
    {






        // var tree = new Tree(new Panel("workcenter queue: bloom room row 1"));

        // var layer1_node1 = tree.AddNode("layer 1");
        // layer1_node1.AddNode("layer 2");



        Operation mock_op1 = new Operation()
        {
            Id = 1,
            Instruction = "Mock insrucitons"
        };
        // DateOnly startdate = new DateOnly(2026, 4, 19);
        DateTime startDate = new DateTime(2026, 4, 19);
        // DateOnly startDate = new DateOnly(2026, 4, 19);
        // DateOnly stopDate = new DateOnly(2026, 5, 19);
        DateTime stopDate = new DateTime(2026, 5, 19);

        // DateTime start = new DateTime(12451513);
        OpExecution mock_opex1 = new OpExecution()
        {
            Id = 1,
            OperationId = 20,
            WorkOrderQueueId = 1,
            ExecutionLog = "dfasgagg",
            TimeStart = startDate,
            TimeStop = stopDate

        };

        // {newStatusDate.ToString("MMMM dd yyyy hh:mm:ss")}' "

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
        var opexinfo = parent1.AddNode("part Instance Produced: s/n 0000" + (1));
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

        AnsiConsole.Write(new Panel(tree));
        Grid ggr = new Grid().AddColumn().AddRow(new Panel(tree));



        // AnsiConsole.Write(tree);





        // input: list of op_exs that all have the same workorderqueue id
        // input is sorted 
    }
}

