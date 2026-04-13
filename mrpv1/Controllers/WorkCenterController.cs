using System;
using mrpv1.Helpers;
using mrpv1.Models;
using mrpv1.Queries;
using Npgsql;
using Spectre.Console;

namespace mrpv1.Controllers;

public class WorkCenterController()
{
    private static readonly string multiHost = "db,localhost";
    readonly NpgsqlDataSourceBuilder dbBuilder = new DbSourceBuilder(multiHost).Builder();
    readonly string getWorkCentersQuery = WorkCenterQueries.GetWorkCenters();

    public async Task<List<string>> GetWorkCenters()
    {
        AnsiConsole.MarkupLine("[gray]Fetching data...[/]");
        AnsiConsole.MarkupLine("    -> [gray]Fetching work centers..[/]");
        List<string> workcenters = [];
        try
        {
            await using var dataSource = dbBuilder.BuildMultiHost();
            await using (var getWorkCentersCommand = dataSource.CreateCommand(getWorkCentersQuery))

            await using (var reader = await getWorkCentersCommand.ExecuteReaderAsync())

                while (await reader.ReadAsync())
                {
                    string wcName = reader.GetString(0);
                    workcenters.Add(wcName);
                }
            AnsiConsole.MarkupLine($"        -> [green]Done. [/][gray]Work centers found.[/]");
            return workcenters;
        }
        catch (NpgsqlException e)
        {
            AnsiConsole.MarkupLine($"        -> [red]Failed. [/][gray]Could not fetch work centers.[/]");
            Console.WriteLine(e.Message);
        }
        return workcenters;
    }
}