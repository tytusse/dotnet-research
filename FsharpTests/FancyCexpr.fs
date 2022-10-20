module FsharpTests.FancyCexpr

type ZResult =
    | Zero
    | ZError of string

type Builder() =
    member _.Bind(m, f:'T -> Result<'R, _>) = 
        match m with
        | Ok x -> f x
        | Error err -> Error err
    member _.Bind(m, f:'T -> ZResult) = 
        match m with
        | Ok x -> f x
        | Error err -> ZError err
    member _.Return(x) = Ok x
    member _.ReturnFrom(m:Result<'T, _>) = m
    member _.Using(disp, f) =
        use disp = disp
        f disp
    member _.Zero() = Zero
    member this.Combine(m:Result<'T, _>, _) = m
    member this.Combine(m:ZResult, delayed:unit->Result<_,_>) =
        match m with
        | Zero -> delayed()
        | ZError e -> Error e
    member this.Combine(m:ZResult, delayed:unit->ZResult) =
        match m with
        | Zero -> delayed()
        | ZError e -> ZError e
    member _.Delay(f) = f
    member this.Run(f:unit->Result<'T, _>) = f()
    member this.Run(f:unit->ZResult) =
        let lifted() =
            match f() with
            | Zero -> Ok()
            | ZError e -> Error e
            
        this.Run lifted

let maybe = Builder()