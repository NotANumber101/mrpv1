using System;
using Spectre;
using Spectre.Console;
using mrpv1.Helpers;
using mrpv1.Queries;
using Npgsql;
using mrpv1.Controllers;

namespace mrpv1.Pages;

public class FallbackPage() : Page

{
    public async Task Display()
    {
        DbMetaController dbMetaController = new DbMetaController();
        List<string> dbTableNames = await dbMetaController.GetDbTableNames();
        List<string> dbFieldNames = await dbMetaController.GetTableFieldNames();

        Console.WriteLine("Interactive Mode is disabled.");
        Console.WriteLine("Nothing left to do.");
        Console.WriteLine("Exiting...");
    }
}