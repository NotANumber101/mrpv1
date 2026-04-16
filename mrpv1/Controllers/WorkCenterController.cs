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

    public async Task<List<WorkCenter>> GetWorkCenters()
    {
        AnsiConsole.MarkupLine("[gray]Fetching data...[/]");
        AnsiConsole.MarkupLine("    -> [gray]Fetching work centers..[/]");
        List<WorkCenter> workcenters = [];
        try
        {
            await using var dataSource = dbBuilder.BuildMultiHost();
            await using (var getWorkCentersCommand = dataSource.CreateCommand(getWorkCentersQuery))

            await using (var reader = await getWorkCentersCommand.ExecuteReaderAsync())

                while (await reader.ReadAsync())
                {
                    
                    int id = reader.GetInt32(0);
                    string name = reader.GetString(2);
                    string location = reader.GetString(1);
                    WorkCenter wc = new WorkCenter() {Id=id, Name=name, Location=location};
                    workcenters.Add(wc);
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
    public async Task<int> GetWorkCenterQueue(int workCenterId)
    {
        int wcqid = 1000000000;
        try
        {
            await using var dataSource = dbBuilder.BuildMultiHost();
            await using (var cmd = dataSource.CreateCommand($"SELECT * FROM work_center_queue WHERE workCenterId={workCenterId};"))

            await using (var reader = await cmd.ExecuteReaderAsync())

                while (await reader.ReadAsync())
                {
                    wcqid = reader.GetInt32(0);
                }
        }
        catch (NpgsqlException e)
        {
            Console.WriteLine("Failed.");
            Console.WriteLine(e.Message);
        }
        return wcqid;
    }
}