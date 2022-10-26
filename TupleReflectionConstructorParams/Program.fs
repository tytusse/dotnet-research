// For more information see https://aka.ms/fsharp-console-apps

open System

let dumbData = System.Tuple<int, string>(1, "foobar")
// use it "to be sure"
printfn $"dumb data: {dumbData}"
let analyze (t:Type) =
    printfn $"reflecting on type {t.FullName}"
    let c = t.GetConstructors()
    printfn $"type has {c.Length} constructors"

    let cps = c[0].GetParameters()
    printfn $"{t.Name} constructor has {cps.Length} parameters"

    for (i, cp) in cps |> Seq.indexed do
        printfn $"param no {i} name is {cp.Name}, type {cp.ParameterType.Name}"
        if isNull cp.Name then
            printfn "WARNING: param name is NULL!!"
            
[
  dumbData.GetType()
  typeof<{| Id:int; Code:string |}>
]
|> List.iter analyze