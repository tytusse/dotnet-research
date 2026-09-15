module FsharpTests.Exceptions

open System.Diagnostics
open System.Runtime.InteropServices
open Xunit
open Xunit.Abstractions

type TestException(msg:string, [<DefaultParameterValue(null); OptionalArgument>]inner:exn|null) =
    inherit exn(msg, inner)
    new() = TestException("test error")
    
type FailInProperty() =
    member val Foobar : string = raise(TestException())

let inline someInline f = f()

[<StackTraceHidden>]
let someExcluded f = f()

[<StackTraceHidden>]
let someExcludedCurried f x = f x

[<StackTraceHidden>]
let someExcludedThrowing f =
    try f()
    with x -> raise(TestException("wrapped inner", x))

[<StackTraceHidden>]
type ExcludeFromTrace() =
    member _.Run(f) = f()
    member _.RunThrowing(f) =
        try f()
        with x -> raise(TestException("rethrow", x))

type Fixture(output:ITestOutputHelper) =
    [<Fact>]
    member _.``reraise() in finally``() : unit =
        let test() : unit =
            try failwith "test error"
            with x ->
                try ()
                finally reraise()
                
        let z = Assert.Throws<exn>(test)
        Assert.Equal("test error", z.Message)
        
    [<Fact>]
    member _.``val property exception will be thrown from constructor``() : unit =
        let exc = Assert.Throws<TestException>(fun () -> FailInProperty() |> ignore)
        output.WriteLine $"Exception: {exc}"
        
    [<Fact>]
    member _.``stack trace behavior``() : unit =
        let excludeMeths = ExcludeFromTrace()
        let fail() : unit =
            excludeMeths.Run(fun() ->
                someInline(fun() ->
                    someExcluded(fun() ->
                        "A" |> someExcludedCurried(fun x ->
                            someExcludedThrowing(fun ()->
                                raise(TestException($"foo: {x}")))))))
            
        let fail2() : unit =
            excludeMeths.Run(fun() ->
                excludeMeths.Run(fun() ->
                    excludeMeths.RunThrowing(fun ()->
                        raise(TestException("foo")))))
 
        let exc = Assert.Throws<TestException>(fail)
        output.WriteLine $"Exception: {exc}"
        let exc2 = Assert.Throws<TestException>(fail2)
        output.WriteLine $"Exception: {exc2}"
        
