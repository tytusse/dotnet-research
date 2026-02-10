open System
open Spectre.Console
open Spectre.Console.Cli

type HelloSettings() =
    inherit CommandSettings()
    
    [<CommandOption("--text|-t")>]
    member val Text:string|null = null with get, set


type HelloCommand() =
    inherit Command<HelloSettings>()
    
    override _.Execute (context: CommandContext, settings: HelloSettings, cancellationToken: System.Threading.CancellationToken): int =
        let text =
            match settings.Text with
            | Null -> "Hello world"
            | NonNull x -> x
        
        AnsiConsole.MarkupInterpolated $"Saying: [bold]{text}[/]"
        
        0

let app = CommandApp()
app.Configure(fun cfg ->
    cfg.AddCommand<HelloCommand>("hello")
    |> ignore)

Environment.ExitCode <- app.Run(Environment.GetCommandLineArgs()[1..])
