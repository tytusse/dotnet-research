module FsharpTests.Exceptions

open Xunit

type Fixture() =
    [<Fact>]
    member _.``reraise() in finally``() : unit =
        let test() : unit =
            try failwith "test error"
            with x ->
                try ()
                finally reraise()
                
        let z = Assert.Throws<exn>(test)
        Assert.Equal("test error", z.Message)            