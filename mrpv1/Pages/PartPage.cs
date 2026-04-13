using System;
using Spectre;
using Spectre.Console;
using mrpv1.Helpers;
using mrpv1.Queries;
using Npgsql;
using mrpv1.Controllers;
using mrpv1.Models;

namespace mrpv1.Pages;

public class PartPage() : Page

{
    PartController partController = new();
    public async Task Display()
    {
        Console.WriteLine("part page");
        await partController.CreatePart();
        var res = await partController.GetParts();
        foreach (Part part in res)
        {
            Console.WriteLine(part.Id.ToString());
            Console.WriteLine(part.Name);
            Console.WriteLine("");
        }
    }
}