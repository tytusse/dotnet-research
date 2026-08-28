type Foobar() =
    member inline _.Baz() =
        printfn "Baz"
        failwith "test"

let f = Foobar()
try f.Baz()
with x ->
    printfn $"StackTrace:\n{x.StackTrace}"

