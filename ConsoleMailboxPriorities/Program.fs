open System
open ConsoleMailboxPriorities

type MyMessage = {Id:int}

let jobLengthMs = 1000
let postTimeoutMs = 5000
let start = DateTime.Now
let logger = MailboxProcessor.Start(fun mailbox ->
    let rec loop() = async {
        let! (msg) = mailbox.Receive()
        let now = DateTime.Now
        let span:TimeSpan = now-start
        printfn $"[{span.TotalMilliseconds}] %s{msg}"
        return! loop()
    }
    loop())

let runner = TwoLoopsMailboxes.Agent<MyMessage>(
    logger.Post,
    (fun item -> (item.Id % 2, item.Id)),
    (fun _ -> Async.Sleep jobLengthMs))

for i=1 to 10 do
    printfn $"posting {i}"
    let item = {Id=i}
    let priority = (item.Id % 2, item.Id)
    runner.Post(item)
    
System.Console.ReadKey() |> ignore

printfn "good bye"