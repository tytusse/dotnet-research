(*
CREATE TABLE [dbo].[Files](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](1024) NOT NULL,
	[Status] int NOT NULL,
	[Contents] [varbinary](max) NOT NULL,
 CONSTRAINT [PK_Files_1] PRIMARY KEY CLUSTERED ([Id] ASC))

*)

open System
open System.Data.SqlClient
open System.IO
open System.Reflection
open System.Threading.Tasks
open Dapper.FSharp
open Dapper.FSharp.MSSQL

let connString = @"Data Source=.\sqlexpress;Initial catalog=playground;Integrated security=True"

OptionTypes.register()

type Status = Active=1|Completed=2
type File = {
    Id:int
    Name:string
    Status:Status
    Contents:byte[]
}

type Identity = {Id:int}

for name in Assembly.GetExecutingAssembly().GetManifestResourceNames() do
    printfn $"Resource found: {name}"


let filesTable = table'<File> "Files"
let file =
    use str = Assembly.GetExecutingAssembly().GetManifestResourceStream("DatabaseFilesDapperPoc.ninja.jpg")
    use mem = new MemoryStream()
    str.CopyTo mem
    mem.ToArray()
    
let conn = new SqlConnection(connString)
conn.Open()
    
    
let timestampString = DateTime.Now.ToString("yyyyMMdd-HHmmss")

let inline waitTask (t:Task<_>) = t.Wait()
let inline result (t:Task<_>) = t.Result

let t = task {
    let! _ =
        insert {
            for f in filesTable do
            excludeColumn f.Id
            value {Id=0; Name= $"ninja-{timestampString}.jpg"; Contents=file; Status=Status.Active}
        }
        |> conn.InsertOutputAsync<_ , Identity>
    let! allFiles = select { for f in filesTable do selectAll } |> conn.SelectAsync<File>
    let! onlyIds = select { for f in filesTable do selectAll } |> conn.SelectAsync<Identity>
    return allFiles, onlyIds 
}

let filesRead, idsRead =
    let allFiles, onlyIds = t.Result
    List.ofSeq allFiles, List.ofSeq onlyIds

printfn $"Files read back (total {filesRead.Length}), contents excluded"

for file in filesRead do
    let file = {file with Contents=Array.empty}
    printfn $"{file}"
    
printfn $"Ids read back (total {idsRead.Length})"

for identity in idsRead do printfn $"{identity}"    
    
printfn $"Dumping files to {Environment.CurrentDirectory}"

for file in filesRead do
    let name = $"{file.Id}_{file.Name}"
    File.WriteAllBytes(name, file.Contents)
    
// try changing status for all files
update {
    for f in filesTable do
    setColumn f.Status Status.Completed
}
|> conn.UpdateAsync |> waitTask


// try select top X
let ids =
    select {
        for f in filesTable do
        orderBy f.Id
        take 2
        selectAll
    }
    |> conn.SelectAsync<Identity>
    |> result
    |> List.ofSeq
    
printfn $"top 2 ids: {ids}"    


    



