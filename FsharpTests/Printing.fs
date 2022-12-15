module Printing

open System
open Xunit.Abstractions
open Xunit

type MyItem = {
    Name:string
    Id:int
}
with
    override this.ToString() =
        $"My Item named {this.Name} with id={this.Id}"
    static member Create(name, id) = {Name=name;Id=id}
    
let sizeLevels = [" Byte(s)"; "KB"; "MB"; "GB"]
let byteMagnitudeSize = 1024.0
let formatByteSize (size:int64) : string =
    let fmt (size:float) = size.ToString("0.##")
    let rec format (size:float) levels =
        match levels with
        | [] -> failwith "illegal: last level passed"
        | [ level ] -> $"{fmt size}{level}"
        | level :: levels ->
            if size >= byteMagnitudeSize then
                format (size / byteMagnitudeSize) levels
            else $"{fmt size}{level}"
    format (float size) sizeLevels

type Tests(hlp:ITestOutputHelper) =
    
    static member ``formatByteSize scenarios`` : seq<obj[]>=
        [   0L,              "0 Byte(s)"
            1023L,           "1023 Byte(s)"
            1024L,           "1KB"
            1024L+512L,       "1.5KB"
            1023L*1024L,      "1023KB"
            1024L*1024L*5L,    "5MB"
            1024L*1024L*1024L*5L,   "5GB"
            1024L*1024L*1024L*1025L,   "1025GB"
        ]
        |> Seq.map(fun (s, f) -> [|s;f|])
    
    [<Theory>]
    [<MemberData(nameof Tests.``formatByteSize scenarios``)>]
    member _.``byte size formated properly``(size:int64, expectedFmt:string) =
        let actual = formatByteSize size
        hlp.WriteLine($"{size} => {actual}")
        Assert.Equal(expectedFmt, actual)
    
    [<Fact>]
    member _.``to string on list calls to string on items too``() =
        let myItems = [MyItem.Create("John", 5)]
        hlp.WriteLine(string myItems)
        hlp.WriteLine $"%A{myItems}"
        hlp.WriteLine $"%O{myItems}"
        ()
        
    [<Fact>]
    member _.``to string on list calls to string on items too with tuples``() =
        let myItems = [
            (DateTime.UtcNow, MyItem.Create("John", 5))
        ]
        hlp.WriteLine(string myItems)
        hlp.WriteLine $"%A{myItems}"
        hlp.WriteLine $"%O{myItems}"
        ()