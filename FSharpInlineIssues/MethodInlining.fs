open System.Diagnostics

type Foobar() =
  member inline _.ShouldSkip_BazInline() =
    printfn "BazInline"
    failwith "BazInline"
  
  [<StackTraceHidden>]
  member inline _.ShouldSkip_BazInlineAttribute() =
    printfn "BazInlineAttribute"
    failwith "BazInlineAttribute"

  [<StackTraceHidden>]
  member _.ShouldSkip_BazAttribute() =
    printfn "BazAttribute"
    failwith "BazAttribute"

[<StackTraceHidden>]
type WholeClass() =
  member inline _.ShouldSkip_Inline() = 
    printfn "WholeClass->Inline"
    failwith "WholeClass->Inline"

  member _.ShouldSkip_Normal() = 
    printfn "WholeClass->Normal"
    failwith "WholeClass->Normal"

let f = Foobar()
let tryOne o body =
  try body o
  with x ->
    printfn $"StackTrace:\n{x.StackTrace}"

tryOne f _.ShouldSkip_BazInline()
tryOne f _.ShouldSkip_BazInlineAttribute()
tryOne f _.ShouldSkip_BazAttribute()
let w = WholeClass()
tryOne w _.ShouldSkip_Inline()
tryOne w _.ShouldSkip_Normal()

