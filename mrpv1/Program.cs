using System;
using Spectre.Console;
using mrpv1.Helpers;
using mrpv1.Pages;
using Npgsql;

if (AnsiConsole.Profile.Capabilities.Interactive)
{
    // AnsiConsole.Clear();
    await AnsiConsole.Status().Spinner(Spinner.Known.Dots)
        .SpinnerStyle(Style.Parse("green"))
        .Start("Welcome!", async ctx =>
        {
            Thread.Sleep(300);
            ctx.Status("Loading configuration...");
            Thread.Sleep(300);
            ctx.Status("Starting services...");
            Thread.Sleep(300);
        });
    AnsiConsole.MarkupLine("LOG: Interactive mode detected.[green]Input Mode Enabled.[/]");
    await new Page().MainMenu();
}
else
{
    AnsiConsole.MarkupLine("[red]LOG: Interactive Mode Disabled... Input Mode Disabled.[/]");
    await new FallbackPage().Display();
}