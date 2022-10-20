module FsharpTests.Dapper
open Dapper
open Dapper.FSharp
open Dapper.FSharp.MSSQL
open Xunit
open Xunit.Abstractions

type Status = Abba=1|Pater=2
type MyRec = {
    Id:int
    Name:string
    Status:Status
    Contents:byte[]
}

type Identity = { Id: int }

let recTable = table<MyRec>

do Dapper.FSharp.OptionTypes.register()

type Test(hlp:ITestOutputHelper) =
    
    [<Fact>]
    member _.``print deconstructed``() =
        let sql, values =
            insert {
                for r in recTable do
                value {Id=0; Name="Lucky"; Contents=Array.init 10 (byte); Status=Status.Abba}
                excludeColumn r.Id
            }
            |> Deconstructor.insertOutput<_, Identity>
            
        hlp.WriteLine $"sql: {sql}"
        hlp.WriteLine $"sql: {values}"
        
        
    [<Fact>]
    member _.``print deconstructed top x``() =
        let sql, values =
            select {
                for r in recTable do
                selectAll
                orderBy r.Id
                take 2
            }
            |> Deconstructor.select<Identity>
            
        hlp.WriteLine $"sql: {sql}"
        hlp.WriteLine $"sql: {values}"