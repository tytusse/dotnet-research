module DatabaseFiles

open System
open System.Data.SqlClient
open System.Data
open System.IO
open System.Reflection

printfn "Resource names:"

for name in Assembly.GetExecutingAssembly().GetManifestResourceNames() do
    printfn $"{name}"
    
let connstr =
    let bld =
        SqlConnectionStringBuilder(
            IntegratedSecurity=true,
            DataSource= @".\sqlexpress",
            InitialCatalog="playground")
    bld.ToString()
    
let table = "Files"
let file =
    use str = Assembly.GetExecutingAssembly().GetManifestResourceStream("DatabaseFilesPoc.ninja.jpg")
    use mem = new MemoryStream()
    str.CopyTo mem
    mem.ToArray()
    
let conn = new SqlConnection(connstr)
conn.Open()

let timestampString = DateTime.Now.ToString("yyyyMMdd-HHmmss")

let modified =
    let cmd = conn.CreateCommand()
    cmd.CommandText <- @"insert into Files (Name, Contents) VALUES(@name, @contents)"
    cmd.Parameters.AddWithValue("@name", $"{timestampString}-ninja.jpg") |> ignore
    cmd.Parameters.AddWithValue("@contents", file) |> ignore
    cmd.ExecuteNonQuery()

printfn $"Modified N of rows: {modified}"

let filesRead =
    use cmd = conn.CreateCommand()
    cmd.CommandText <- @"select Id, Name, Contents FROM Files"
    use rdr = cmd.ExecuteReader()
    [
        while rdr.Read() do
            use str = rdr.GetStream("Contents")
            use mem = new MemoryStream()
            str.CopyTo mem
            
            {| Id = rdr["Id"] |> Convert.ToInt32
               Name = string(rdr["Name"])
               Contents = mem.ToArray() |}
    ]

printfn $"Files read back (total {filesRead.Length})"

for file in filesRead do
    printfn $"{file.Name} with id={file.Id}"
    
printfn $"Dumping files to {Environment.CurrentDirectory}"

for file in filesRead do
    let name = $"{file.Id}_{file.Name}"
    File.WriteAllBytes(name, file.Contents)