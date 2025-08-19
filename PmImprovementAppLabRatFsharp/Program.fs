open System
open System.Linq
type Xxx = {
    Name:string
    Age:int
}

let zzz = [
    {Name="bbbbbb"; Age=1111}
    {Name="aaaaaa"; Age=2222}
]

let xxx = zzz.OrderBy(id).ToList() |> List.ofSeq

printfn $"{xxx}"