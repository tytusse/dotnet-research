open System
open System.IO
open System.Threading
open RStein.TOML
open Spectre.Console
open Spectre.Console.Cli

let cancellationTokenSource = new CancellationTokenSource()

type ValidateSettings() =
    inherit CommandSettings()
    
    [<CommandArgument(0, "[path]")>]
    member val Path:string = "" with get, set


type ValidateCommand() =
    inherit AsyncCommand<ValidateSettings>()
    
    override _.ExecuteAsync (context: CommandContext, settings: ValidateSettings, cancellationToken: System.Threading.CancellationToken) = task {
        AnsiConsole.MarkupInterpolated $"Path: [bold]{settings.Path}[/]"
        if not <| System.IO.Path.Exists(settings.Path)
        then
            failwith $"Path {settings.Path} does not exist"
        
        use str = File.Open(settings.Path, FileMode.Open, FileAccess.Read)
        let! table = TomlSerializer.DeserializeAsync(str)
        let v = table["notificationEmailAdresses"]["state"]
        for token in table.Tokens do
            AnsiConsole.WriteLine $"{token}"
        return 0
    }

Console.CancelKeyPress |> Event.add(fun e ->
    e.Cancel <- true // Prevent immediate process termination
    cancellationTokenSource.Cancel()
    AnsiConsole.MarkupLine("[yellow]Cancellation requested...[/]")
)

let app = CommandApp()
app.Configure(fun cfg ->
    cfg.AddCommand<ValidateCommand>("validate")
    |> ignore)

Environment.ExitCode <- app.RunAsync(Environment.GetCommandLineArgs()[1..], cancellationTokenSource.Token).Result
    
