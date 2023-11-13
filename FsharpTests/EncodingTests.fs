module FsharpTests.EncodingTests

open System.IO
open System.Text
open Xunit
open Xunit.Abstractions
let bomBytesUtf8 = [| 0xEFuy;0xBBuy;0xBFuy |]
let encoding = Encoding.UTF8
let defaultBufferSize = 1024

type Fixture(hlp:ITestOutputHelper) =
    [<Fact>]
    member _.``Stream writing adds BOM``() :unit =
        use stream = new MemoryStream()
        use wr = new StreamWriter(stream, encoding, defaultBufferSize, true)
        wr.Write("foobar")
        wr.Flush()
        let arr = stream.ToArray()
        hlp.WriteLine($"Result: %A{arr}")
        Assert.Equal<byte>(bomBytesUtf8, arr[0..2])
        
    [<Fact>]
    member _.``GetBytes does NOT add BOM``() :unit =
        let arr = encoding.GetBytes("foobar")
        hlp.WriteLine($"Result: %A{arr}")
        Assert.NotEqual<byte>(bomBytesUtf8, arr[0..2])
        