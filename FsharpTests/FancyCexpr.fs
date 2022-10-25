module FsharpTests.FancyCexpr

type Returned<'T> = Returned of 'T
    

type Builder() =
    member _.Bind(m, f:'T -> Returned<_>) = 
        match m with
        | Ok x -> f x 
        | Error err -> Returned(Error err)
        
    member _.Bind(m, f:'T -> Result<_,_>) = 
        match m with
        | Ok x -> f x 
        | Error err -> Error err
        
    member _.Return(x) = Returned(Ok x)
    member _.ReturnFrom(m:Result<'T, _>) = Returned m
    member inline _.Zero() = Ok()
    member inline this.Combine(m:Returned<_>, _) = m
    member this.Combine(m:Result<unit,_>, delayed:unit->Result<_,_>) = Result.bind delayed m
    member this.Combine(m:Result<unit,_>, delayed:unit->Returned<_>) =
        match m with
        | Ok _ -> delayed() 
        | Error _ -> Returned m
    member _.Delay(f) = f
    member this.Run(f:unit->Result<_, _>) = f()
    member this.Run(f:unit->Returned<_>) = match f() with Returned r -> r

let maybe = Builder()

type Tracing(traceImpl) =    
    let mutable indent = 0
    let doTrace msg = traceImpl $"{System.String(' ', 3*indent)}{msg}"
    
    member _.Capture(name, f) =
        doTrace $"{name} {{"
        indent <- indent+1
        
        try f()
        finally
            indent <- indent-1
            doTrace "}"
            
    member _.Write msg = doTrace msg

type PlainBuilder(tracing:Tracing) =
    let capt n f = tracing.Capture(n, f)
    member _.Bind(m, f) = capt "Bind" (fun() ->
        tracing.Write $"m=%A{m}"
        let r =
            match m with
            | Ok x -> f x
            | Error e -> Error e
        tracing.Write $"returns %A{r}"
        r)
    // member _.Bind(i, f) = capt "Bind int as unit" (fun() ->
    //     tracing.Write $"m=%A{i}"
    //     let r = f i
    //     tracing.Write $"returns %A{r}"
    //     r)
    member _.Return x = capt "Return" (fun () ->
        tracing.Write $"returns %A{x}"
        Ok x)
    member _.Delay f = capt "Delay" (fun() -> f)
    member _.Zero() = capt "Zero" (fun() -> Ok())
    member this.Combine(m:Result<unit, _>, delayed) = capt "Combine" (fun () -> Result.bind delayed m)
    member this.Run(f) = capt "Run" f
        