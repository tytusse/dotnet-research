module FsharpTests.Exceptions

open Xunit
open Xunit.Abstractions

type TestException(msg:string) =
    inherit exn(msg)
    new() = TestException("test error")
    
type FailInProperty() =
    member val Foobar : string = raise(TestException())

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