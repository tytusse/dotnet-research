module FsharpTests.CompExpr

open Xunit
open Xunit.Abstractions

type Tests(hlp:ITestOutputHelper) =
    [<Fact>]
    member _.``sasddssf``() =
        FancyCexpr.maybe {
            return 4
            return "adas"
            ()
        }
        |> ignore
        
    [<Fact>]
    member _.``plain non fancy cexpr: surprise Return when ending cexpr with do!``() =
        let t = FancyCexpr.Tracing(hlp.WriteLine)
        // return should be called once ffs
        let r = FancyCexpr.PlainBuilder t {
            t.Write "1"
            if 1 = 1 then
                t.Write "2"
                let! x = Ok 5
                t.Write "`do! Ok ()` and ..."
                do! Ok () // will call `Return` = ffs?            
            t.Write "4"
            return 4
        }
        ()
        
    [<Fact>]
    member _.``plain non fancy cexpr: calls zero as expected``() =
        let t = FancyCexpr.Tracing(hlp.WriteLine)
        // return should be called once ffs
        let r = FancyCexpr.PlainBuilder t {
            t.Write "1"
            if 1 = 2 then
                t.Write "never happens"
                do! Ok()
            t.Write "4"
            return 4
        }
        ()
        
    // does not even compile        
    // [<Fact>]
    // member _.``cexpr not working Zero after do!``() =
    //     let r = FancyCexpr.maybe {
    //         if 1 = 1 then
    //             // No Zero after that(!!)
    //             // Comp expr calls Return here 
    //             // hence at the moment there is no way to distinguish zero branch from return branc
    //             // (facepalm)
    //             do! Ok ()
    //         
    //         return 4
    //     }
    //     //Assert.Equal(Ok 4, r) -> error - result is not int but unit ....
    //     ()
            
        
    [<Fact>]
    member _.``cexpr not working Zero after do! - how it really works (skipped delay)``() =
        
        let inner = 
            if 2 = 1
            // here instead of zero the compexpr resolves to `Return()` (with unit as param) for some reason.
            // hence there is now ay to distinguish zero from actual `return whatever` with this.
            // hence there is no way to implement return semantics
            // we are left with best we can do - allow to combine only Result<unit,_> and to interpret
            // `return ()` same way as zero. 
            then FancyCexpr.maybe.Bind(Ok(), (fun () -> FancyCexpr.maybe.Return()))
            else FancyCexpr.maybe.Return() 
            
        let r =      
            FancyCexpr.maybe.Run(fun() ->
                FancyCexpr.maybe.Combine(
                    inner,
                    fun() -> FancyCexpr.maybe.Return(4))
            )
            
        ()