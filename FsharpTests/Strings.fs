module FsharpTests.Strings

open System.Text.RegularExpressions
open Xunit
open Xunit.Abstractions

type Tests(hlp:ITestOutputHelper) =
    
    [<Fact>]
    member _.``String triple quote formatting visual test``() : unit =
        let text =
            """
            select StoreID
            """
        let verbose = Regex.Replace(text, @"[ \t]", "*")
        hlp.WriteLine verbose
        
        
    [<Fact>]
    member _.``String normal quote formatting visual test``() : unit =
        let text =
            "
            select StoreID
            then else
            "
        hlp.WriteLine ("[" + text + "]")