module WorkerServiceErrorHandling.Program

open System
open System.Collections.Generic
open System.Linq
open System.Threading.Tasks
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting

(*
sc create WorkerServiceErrorHandling binPath="C:\Projects\Research\CustomResearch\WorkerServiceErrorHandling\bin\Release\net7.0\publish\WorkerServiceErrorHandling.exe"
*)

let rec failEventually(iteration:int) = async {
    printfn $"iteration {iteration}"
    if iteration >= 3 then
        failwith "a failure test"
    do! Async.Sleep(2000)
    return! failEventually(iteration+1)
}

type Worker() =
    interface IHostedService with
        member this.StartAsync(cancellationToken) =
            Async.Start(failEventually 1)
            Task.CompletedTask
        member this.StopAsync(cancellationToken) = Task.CompletedTask

let createHostBuilder args =
    Host.CreateDefaultBuilder(args)
        .UseWindowsService()
        .ConfigureServices(fun hostContext services ->
            services.AddHostedService<Worker>() |> ignore)

[<EntryPoint>]
let main args =
    createHostBuilder(args).Build().Run()

    0 // exit code