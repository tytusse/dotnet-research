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
    
    [<CommandOption("-t|--types")>]
    member val PrintTypes = false with get, set


type ValidateCommand() =
    inherit AsyncCommand<ValidateSettings>()
    
    let rec printToken(token:TomlToken, level: int, settings: ValidateSettings) : unit =
        let ident = String.init (level*4) (fun _ -> " ")
        match token with
        | :? TomlPrimitiveValue as v ->
            if settings.PrintTypes then
                AnsiConsole.MarkupInterpolated $"{v.Type} [green]{v.Value}[/]"
            else
                AnsiConsole.MarkupInterpolated $"[green]{v.Value}[/]"
        | :? TomlArray as a ->
            AnsiConsole.Write "["
            for item in a do
                AnsiConsole.WriteLine()
                AnsiConsole.Write ident
                printToken(item, level+1, settings)
            AnsiConsole.Write " ]"
        | :? TomlTable as table ->
            for key in table.Keys do
                let token = table[key]
                AnsiConsole.WriteLine()
                if settings.PrintTypes then
                    AnsiConsole.MarkupInterpolated $"{ident}{key.RawKey} [gray]({token.TokenType})[/]: "
                else
                    AnsiConsole.Write $"{ident}{key.RawKey}: "
                printToken(token, level+1, settings)
        | other ->
            AnsiConsole.MarkupLineInterpolated $"[green]{other}[/]"
    
    override _.ExecuteAsync (context: CommandContext, settings: ValidateSettings, cancellationToken: System.Threading.CancellationToken) = task {
        AnsiConsole.MarkupLineInterpolated $"Path: [bold]{settings.Path}[/]"
        if not <| System.IO.Path.Exists(settings.Path)
        then
            failwith $"Path {settings.Path} does not exist"
        
        use str = File.Open(settings.Path, FileMode.Open, FileAccess.Read)
        let! table = TomlSerializer.DeserializeAsync(str, cancellationToken)
        printToken(table, 0, settings)
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
    
