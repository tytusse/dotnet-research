module FsharpTestsNullableEnableLongBuild.ExampleExtensions

open System.Runtime.CompilerServices
open System.Runtime.InteropServices

type TheExt() =
    [<Extension>]
    static member NullableEcho(
        [<OptionalArgument>]
        [<DefaultParameterValue(null)>]
        x:string|null) : string| null = x