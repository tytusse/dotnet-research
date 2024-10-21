module FsharpTests.StaticInterfaceMethodsExample

#nowarn "3535" "3536"

open System
open Xunit

type IAnimal = static abstract TranslateFromEnglish: word:string -> string
type Dog() = interface IAnimal with static member TranslateFromEnglish(word:string) = $"{word}, woof! woof!"
let translateFromEnglish<'T & #IAnimal>(word:string) : string = 'T.TranslateFromEnglish(word)

type Fixture() =
    // !!!!!! no compiler error, runtime error !!!!!!
    [<Fact>]
    member _.``Dog.TranslateFromEnglish bad``() =
        ignore<|Assert.ThrowsAny(fun() -> ignore<|translateFromEnglish<IAnimal>("hello"))
    
    // !!!!!! no compiler error, runtime error !!!!!!
    [<Fact>]
    member _.``Dog.TranslateFromEnglish also bad``() =
        ignore<|Assert.ThrowsAny(fun() -> ignore<|translateFromEnglish("hello"))
        
    [<Fact>]
    member _.``Dog.TranslateFromEnglish good``() = translateFromEnglish<Dog>("hello")
        
        
        