using Spectre.Console;
using mrpv1.Controllers;
using mrpv1.Models;

namespace mrpv1.Pages;

public class OrderPage() : Page

{
    WorkOrderController workOrderController = new ();
    PartController partController = new ();
    public async Task Display()
    {
        int workOrderPartId = AnsiConsole.Ask<int>("Please enter a part id");
        await CreateOrder(workOrderPartId);
    }
    public async Task CreateOrder(int partId)
    {
    // confirm part exists (can be amnufactured)
    Part ?partProduced = await partController.GetPartV2(partId);
    if (partProduced == null)
        {
            AnsiConsole.MarkupLine("[red]no part with that id was found[/]");
            return;
        }
    if (AnsiConsole.Confirm("Submit?"))
        {
            await workOrderController.CreateWorkOrder(partId);
        }
    }
    public async Task GetPart()
    {
        await partController.GetParts();
    }
}