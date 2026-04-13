using System;
using Spectre;
using Spectre.Console;
using mrpv1.Helpers;
using mrpv1.Queries;
using Npgsql;
using mrpv1.Controllers;

namespace mrpv1.Pages;

public class WorkCenterPage() : Page

{
    WorkCenterController workCenterController = new();
    public async Task Display()
    {
        Console.WriteLine("wc page");
        var res = await workCenterController.GetWorkCenters();
        if (res.Any())
        {
                    Console.WriteLine($"res[0]: {res[0]}");
        } else
        {
            Console.WriteLine("No workcenters");
        }
    }
}