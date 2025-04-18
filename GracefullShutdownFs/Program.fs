// For more information see https://aka.ms/fsharp-console-apps

open System
open System.Runtime.InteropServices
open System.Threading
let cts = new CancellationTokenSource()
let stoppedEvent = new AutoResetEvent(false)
let onStop() =
    cts.Cancel()
    stoppedEvent.Set() |> ignore
    
    
PosixSignalRegistration.Create(PosixSignal.SIGTERM, fun handler ->
    printfn $"received sigterm on thread {Environment.CurrentManagedThreadId}"
    handler.Cancel<- true
    onStop()
) |> ignore

Console.CancelKeyPress.Add(fun _ ->
    printfn $"received ctrl+c on thread {Environment.CurrentManagedThreadId}"
    onStop())

printfn "Will start async loop"
printfn $"Main code on thread {Environment.CurrentManagedThreadId}"

Async.Start(async {
    let rec loop n = async {
        printfn $"Loop no {n}"
        do! Async.Sleep 1000
        do! loop (n+1)
    }
    do! loop 0
}, cts.Token)

stoppedEvent.WaitOne() |> ignore
printfn "Bye!"