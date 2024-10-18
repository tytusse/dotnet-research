module FsharpTests.StaticInterfaceMethods

#nowarn "3535" "3536"

open Xunit

type IWithStatics =
    abstract Id: int
    static abstract TranslateFromEnglish: word:string -> string
    
type Dog() =
    interface IWithStatics with
        member _.Id = 42
        static member TranslateFromEnglish (word:string) = $"{word}, woof! woof!"
        
        
let resolve<'Animal & #IWithStatics>(word:string) : string =
    'Animal.TranslateFromEnglish(word)
    
type Fixture() =
    [<Fact>]
    member _.``Dog.TranslateFromEnglish bad``() =
        Assert.Equal("hello, woof! woof!", resolve("hello"))
        
    [<Fact>]
    member _.``Dog.TranslateFromEnglish good``() =
        Assert.Equal("hello, woof! woof!", resolve<Dog>("hello"))
        
        
        