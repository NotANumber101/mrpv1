using Spectre.Console;
using mrpv1.Pages;

if (AnsiConsole.Profile.Capabilities.Interactive)
{
    AnsiConsole.Clear();
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
    await new Page().MainMenu();
}
else
{
    await new FallbackPage().Display();
}