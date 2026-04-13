using System;
using mrpv1.Helpers;
using mrpv1.Models;
using mrpv1.Queries;
using Npgsql;
using Spectre.Console;

namespace mrpv1.Controllers;

public class DbMetaController()
{
    private static readonly string multiHost = "db,localhost";
    readonly NpgsqlDataSourceBuilder dbBuilder = new DbSourceBuilder(multiHost).Builder();
    readonly string getDbNamesQuery = DbMetaQueries.GetDbNamesQuery();
    readonly string getDbTableNamesQuery = DbMetaQueries.GetDbTableNamesQuery();

    public async Task<List<string>> GetDbTableNames()
    {
        AnsiConsole.MarkupLine("[gray]Fetching data...[/]");
        AnsiConsole.MarkupLine("    -> [gray]Fetching db table names...[/]");
        List<string> dbTableNames = [];
        try
        {
            await using var dataSource = dbBuilder.BuildMultiHost();
            await using (var getDbNamesCommand = dataSource.CreateCommand(getDbNamesQuery))
            await using (var dbTableNameReader = await getDbNamesCommand.ExecuteReaderAsync())

                while (await dbTableNameReader.ReadAsync())
                {
                    string dbTableName = dbTableNameReader.GetString(0);
                    dbTableNames.Add(dbTableName);
                }
            AnsiConsole.MarkupLine($"        -> [green]Done. [/][gray]Db table names found[/]");
            return dbTableNames;
        }
        catch (NpgsqlException e)
        {
            AnsiConsole.MarkupLine($"        -> [red]Failed. [/][gray]Could not fetch db table names: {dbTableNames.Count}[/]");
            Console.WriteLine(e.Message);
        }
        return dbTableNames;
    }
    public async Task<List<string>> GetTableFieldNames()
    {
        AnsiConsole.MarkupLine("[gray]Fetching data...[/]");
        AnsiConsole.MarkupLine("    -> [gray]Fetching db field names...[/]");
        var queryTree = new Tree("QueryTree: GetTableFieldNames");
        List<string> fieldNames = [];
        try
        {
            await using var dataSource = dbBuilder.BuildMultiHost();
            await using (var getDbTableNamesCommand = dataSource.CreateCommand(getDbTableNamesQuery))
            await using (var dbTableNameReader = await getDbTableNamesCommand.ExecuteReaderAsync())

                while (await dbTableNameReader.ReadAsync())
                {
                    string dbTableName = dbTableNameReader.GetString(0);
                    var treeNode = queryTree.AddNode(dbTableName);
                    string getTableFieldNamesQuery = DbMetaQueries.GetTableFieldNamesQuery(dbTableName);

                    await using (var getTableFieldNamesCommand = dataSource.CreateCommand(getTableFieldNamesQuery))
                    await using (var dbFieldNameReader = await getTableFieldNamesCommand.ExecuteReaderAsync())

                        while (await dbFieldNameReader.ReadAsync())
                        {
                            string dbFieldName = dbFieldNameReader.GetString(0);
                            treeNode.AddNode(dbFieldName);
                            fieldNames.Add(dbFieldName);
                        }
                }
            AnsiConsole.MarkupLine($"        -> [green]Done. [/][gray]Db field names found.[/]");
        }
        catch (NpgsqlException e)
        {
            AnsiConsole.MarkupLine($"        -> [red]Failed. [/][gray]Cound not fetch db field names. {fieldNames.Count}[/]");
            Console.WriteLine(e.Message);
        }
        AnsiConsole.Write(queryTree);
        return fieldNames;
    }
}