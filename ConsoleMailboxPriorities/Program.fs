open System

type MyMessage = {Id:int}

type Work<'Prio> = {
    Run:unit->unit
    Priority:'Prio
}

type PriorityManagerMessage<'Prio> =
    | RunnerDone
    | QueueWork of Work<'Prio>
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

let buffer = MailboxProcessor.Start(fun mailbox ->
    let rec loop (isBusy:bool) waiting n = async {
        let! (msg:PriorityManagerMessage<_>) = mailbox.Receive()
        logger.Post $"[{n}] enqueued: {msg}, isBusy: {isBusy} waiting: {waiting}"
        
        let runWithPriority (waiting:Work<_> list) =
            logger.Post $"runWithPriority {waiting}"
            match waiting |> List.sortBy(fun x -> x.Priority) with
            | work :: waiting ->
                work.Run()
                loop true waiting (n+1)
            | [] -> loop isBusy [] (n+1)
        
        match msg with
        | QueueWork work ->
            let waiting = work::waiting
            if isBusy
            then return! loop isBusy waiting (n+1)
            else return! runWithPriority waiting
        | RunnerDone -> return! runWithPriority waiting       
    }
    loop false [] 0)

let runner = MailboxProcessor.Start(fun mailbox ->
    let rec loop n = async {
        let! (msg:MyMessage) = mailbox.Receive()
        logger.Post $"runner[{n}]handling msg [{msg.Id}]"
        do! Async.Sleep jobLengthMs
        logger.Post $"runner[{n}]handled msg [{msg.Id}]"
        buffer.Post RunnerDone
        return! loop (n+1)
    }
    loop 0)

for i=1 to 10 do
    printfn $"posting {i}"
    let item = {Id=i}
    let priority = (item.Id % 2, item.Id)
    buffer.Post(QueueWork {Run = (fun() -> runner.Post(item)); Priority=priority})
    
System.Console.ReadKey() |> ignore

printfn "good bye"