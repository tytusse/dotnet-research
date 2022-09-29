module ConsoleMailboxPriorities.SelfPostingMailbox

open System

module PrioritizeOnDequeue =
    let tryDequeue getPrio storage : ('T option * list<'T>) =
        match storage with
        | [] -> None, []
        | xs ->
            let (idx, itm) = 
                xs
                |> List.indexed
                |> List.maxBy (snd>>getPrio)
            let storage = storage |> List.removeAt idx
            (Some itm), storage

type private Msg<'T> =
    | ItemAdd of 'T
    | Clear

type Agent<'T>(logger:string->unit, getPrio:'T->IComparable, run:'T->Async<unit>) =
    
    let mailbox = MailboxProcessor.Start(fun mailbox ->
        let rec loop itms = async {
            let! msg = mailbox.Receive()
            match msg with
            | ItemAdd itm ->
                
                return! loop (itm::itms)
            | Clear -> return! loop []
            
        }
        loop [])
    
    member _.Post(item) =
        mailbox.Post(ItemAdd item)
        //buffer.Post(QueueWork {Run = (fun() -> runner.Post(item)); Priority=getPrio item})