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
    member _.``adasda adadas``() =
        let r = FancyCexpr.maybe {
            if 2 = 1 then
                do! Error "dsdsa"
                //()
                
            return 4
        }
        Assert.Equal(Ok 4, r)
        ()
        
    [<Fact>]
    member _.``adasda ddddd``() =
        FancyCexpr.maybe.Run(fun() ->
            FancyCexpr.maybe.Combine(
                (
                    if 2 = 1
                    then FancyCexpr.maybe.Bind(Error "dsdsa", (fun () -> FancyCexpr.maybe.Zero()))
                    else FancyCexpr.maybe.Zero()
                ),
                fun() -> FancyCexpr.maybe.Return(4))
        )
        |> ignore