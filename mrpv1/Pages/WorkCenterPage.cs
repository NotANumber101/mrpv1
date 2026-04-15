using mrpv1.Controllers;
using mrpv1.Helpers;
using mrpv1.Models;

namespace mrpv1.Pages;

public class WorkCenterPage() : Page

{
    readonly WorkCenterController workCenterController = new();
    readonly InventoryController inventoryController = new();
    public async Task Display()
    {
        Console.WriteLine("wc page");
        // var res = await workCenterController.GetWorkCenters();
        await inventoryController.GetIlocations();
        // if (res.Any())
        // {
        //             Console.WriteLine($"res[0]: {res[0]}");
        // } else
        // {
        //     Console.WriteLine("No workcenters");
        // }
    }
}



// // Reading
// await using (var cmd = new NpgsqlCommand("SELECT my_enum, my_composite FROM some_table", conn))
// await using (var reader = cmd.ExecuteReader()) {
//     reader.Read();
//     var enumValue = reader.GetFieldValue<Mood>(0);
//     var compositeValue = reader.GetFieldValue<InventoryItem>(1);
// }