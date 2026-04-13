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
    // WorkCenterPage wcPage = new();
    // await wcPage.Display();

    await new PartPage().Display();
}
else
{
    AnsiConsole.MarkupLine("[red]LOG: Interactive Mode Disabled... Input Mode Disabled.[/]");
    // FallbackPage fallbackPage = new();
    await new FallbackPage().Display();
}



// var choice = AnsiConsole.Prompt(
//     new SelectionPrompt<string>()
//         .Title("[green]What would you like to do?[/]")
//         .AddChoices("Design", "Manufacture"));

// AnsiConsole.MarkupLine($"Page: [blue]{choice}[/]");

// if (choice == "Design")
// {
// AnsiConsole.MarkupLine($"[black]DESIGN[/]");
// AnsiConsole.MarkupLine($"[gray]- design m stack[/]");
// AnsiConsole.MarkupLine($"[gray]- design m part[/]");
// } else if (choice == "Manufacture")
// {
// AnsiConsole.MarkupLine($"[black]MANUFACTURE[/]");
// AnsiConsole.MarkupLine($"[gray]- view WC[/]");

// }


/// design
/// manufacture


//// view work centers
///         wcq
///             ops
/// 
/// 
/// view work orders
/// view inventory
/// view manufacturing catalog 
/// 
/// 
/// 

// well I would like it to help me keep track of things i am buiulding
// those things may take months to produce

// I would also like the app to help me design

// and actually what this app does is help prototype the design and manufacture of products
