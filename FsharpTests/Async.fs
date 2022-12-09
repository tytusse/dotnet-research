module FsharpTests.Async

open System.Threading
open Xunit
open Xunit.Abstractions

type Fixture(hlp:ITestOutputHelper) =
    let log = hlp.WriteLine
    
    [<Fact>]
    member _.``can cancel loop workflow``() =
        let cancSource = new CancellationTokenSource()
        let rec loop n = async {
           log $"loop {n}"
           do! Async.Sleep(250)
           // self-cancel after 2 loops
           // the twist: without "do" and scope here
           // the "finishing something probably very important" is NOT logged after "will cancel"
           // with `do` however, "main: cancel requested" is called BEFORE loop finishes
           // I will guess is this is due to F# using async.Combine when ommiting `do`
           // which handles cancellation by not calling continuation.
           // However, this is not the most consistent behavior IMHO, as the code "looks" the same.
           // I'd call it a gotcha.
           do
               if n = 2 then
                   log "will cancel"
                   cancSource.Cancel()
               else ()
           
           log "finishing something probably very important"
           return! loop (n+1)
        }
        
        
        let e = new ManualResetEvent(false)
        Async.Start(async {
            use! hld = Async.OnCancel(fun() ->
                log "main: cancel requested"
                e.Set() |> ignore
            )
            do! loop 0
            ()
        }, cancSource.Token)
        
        let signaled = e.WaitOne(5000)
        Assert.True(signaled)
        Thread.Sleep(1000)