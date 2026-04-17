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

        try
        {
            var dbsb = new DbSourceBuilder("db,localhost");
            await using var dataSource = dbsb.Builder().BuildMultiHost();
            // FETCH DATA
            await using var connection = await dataSource.OpenConnectionAsync();
            // SELECT 8 verifies Logging
            await using var loggingCommand = new NpgsqlCommand("SELECT 8", connection);
            _ = await loggingCommand.ExecuteScalarAsync();
            AnsiConsole.MarkupLine("LOG: Database Connection: [green]OK![/]");
        }
        catch (NpgsqlException e)
        {
            AnsiConsole.MarkupLine("[red]WARN: Unable to retrieve data...[/]");
            AnsiConsole.MarkupLine("[red]ERROR: Database server connection has failed.[/]");
            Console.WriteLine(e.Message);
        }
        DbMetaController dbMetaController = new DbMetaController();
        List<string> dbTableNames = await dbMetaController.GetDbTableNames();
        List<string> dbFieldNames = await dbMetaController.GetTableFieldNames();

        Console.WriteLine("Interactive Mode is disabled.");
        Console.WriteLine("Nothing left to do.");
        Console.WriteLine("Exiting...");
    }
}