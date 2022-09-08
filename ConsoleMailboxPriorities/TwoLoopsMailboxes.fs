module ConsoleMailboxPriorities.TwoLoopsMailboxes

open System

type Work = {
    Run:unit->unit
    Priority:IComparable
}

type Command<'T> =
    | RunnerDone
    | QueueWork of 'T

type Agent<'T>(logger:string->unit, getPrio:'T->IComparable, run:'T->Async<unit>) =
    let buffer = MailboxProcessor.Start(fun mailbox ->
        let rec loop (isBusy:bool) waiting n = async {
            let! (msg:Command<_>) = mailbox.Receive()
            logger $"[{n}] enqueued: {msg}, isBusy: {isBusy} waiting: {waiting}"
            
            let runWithPriority (waiting:Work list) =
                logger $"runWithPriority {waiting}"
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
            let! (msg:'T) = mailbox.Receive()
            logger $"runner[{n}]handling msg [{msg}]"
            do! run msg
            logger $"runner[{n}]handled msg [{msg}]"
            buffer.Post RunnerDone
            return! loop (n+1)
        }
        
        loop 0)
    
    member _.Post(item) =
        buffer.Post(QueueWork {Run = (fun() -> runner.Post(item)); Priority=getPrio item})