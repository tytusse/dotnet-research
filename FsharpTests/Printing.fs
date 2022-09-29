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

type Tests(hlp:ITestOutputHelper) =
    
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