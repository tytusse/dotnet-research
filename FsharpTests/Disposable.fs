module FsharpTests.Disposable

open System
open Xunit
open Xunit.Abstractions

type Fixture(hlp:ITestOutputHelper) =
    [<Fact>]
    member _.``behavior of try-catch with failing disposable``() : unit =
        let makeFailingDisposable() = { new IDisposable with
            member _.Dispose() = failwith "dispose test fail" }
        try
            use d = makeFailingDisposable()
            failwith "regular fail"
        with x ->
            hlp.WriteLine $"error message: {x.Message}"