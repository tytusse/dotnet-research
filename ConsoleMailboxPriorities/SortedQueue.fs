namespace ConsoleMailboxPriorities

type SortedQueue<'T> private (getPrio, storage) =
    // uses list as storage, might consider optimizing:
    // - Dequeue usage of List.rev
    // - Enqueue usage of List.insertAt
    // We dont expect big queues here so probably wont worth it.
    new getPrio = SortedQueue(getPrio, [])
    member this.TryDequeue(): ('T option * SortedQueue<'T>) =
        match List.rev storage with
        | [] -> None, this
        | q :: rest -> (Some q), SortedQueue<_>(getPrio, List.rev rest)
    member _.Enqueue (toAdd:'T) =
        let rec add toAdd toCheck skipped =
            match toCheck with
            | [] -> toAdd :: skipped |> List.rev // empty queue or all has lower prio
            | next :: remaining ->
                if getPrio next < getPrio toAdd // found lower prio - skip next, keep searching
                then add toAdd remaining (next::skipped)
                else // next is same or more prio - enqueue after next
                    let tail = toAdd :: next :: remaining
                    // dont forget skipped
                    (List.rev skipped) @ tail 
        
        let newStorage = add toAdd storage []
        
        SortedQueue(getPrio, newStorage)
    override _.ToString() = $"{List.rev storage}"

