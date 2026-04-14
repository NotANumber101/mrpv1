using System;
using Spectre.Console;
using mrpv1.Helpers;
using mrpv1.Pages;
using Npgsql;

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
if (AnsiConsole.Profile.Capabilities.Interactive)
{
    // AnsiConsole.Clear();
    await AnsiConsole.Status().Spinner(Spinner.Known.Dots)
        .SpinnerStyle(Style.Parse("green"))
        .Start("Welcome!", async ctx =>
        {
            Thread.Sleep(550);
            ctx.Status("Loading configuration...");
            Thread.Sleep(700);
            ctx.Status("Starting services...");
            Thread.Sleep(700);
        });
    AnsiConsole.MarkupLine("LOG: Interactive mode detected.[green]Input Mode Enabled.[/]");
    await new Page().MainMenu();
}
else
{
    AnsiConsole.MarkupLine("[red]LOG: Interactive Mode Disabled... Input Mode Disabled.[/]");
    await new FallbackPage().Display();
}