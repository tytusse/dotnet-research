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
    cmd.Parameters.AddWithValue("@contents", file.AsMemory()) |> ignore
    cmd.ExecuteNonQuery()

printfn $"Modified N of rows: {modified}"

let read(rdr:IDataReader) (col:int) =
    let sizeReported = rdr.GetBytes(col, 0L, null, 0, 0)
    if sizeReported > System.Int32.MaxValue
    then failwith $"sizes over {System.Int32.MaxValue} are not supported"
    
    let sizeReported = int sizeReported // truncate
    let buffer:byte[] = Array.zeroCreate (int sizeReported)
    let sizeRead = rdr.GetBytes(col, 0L, buffer, 0, sizeReported)
    if sizeRead <> sizeReported
    then failwith $"sizeRead <> sizeReported ({sizeRead}<>{sizeReported}) "
    
    buffer

let filesRead =
    use cmd = conn.CreateCommand()
    cmd.CommandText <- @"select Id, Name, Contents FROM Files"
    use rdr = cmd.ExecuteReader()
    let contentsIdx = rdr.GetOrdinal("Contents")
    
    [
        while rdr.Read() do
            // use str = rdr.GetStream("Contents")
            // use mem = new MemoryStream()
            // str.CopyTo mem
            // let contents = mem.ToArray()
            
            let contents = read rdr contentsIdx
            
            {| Id = rdr["Id"] |> Convert.ToInt32
               Name = string(rdr["Name"])
               Contents = contents |}
    ]

printfn $"Files read back (total {filesRead.Length})"

for file in filesRead do
    printfn $"{file.Name} with id={file.Id}"
    
printfn $"Dumping files to {Environment.CurrentDirectory}"

for file in filesRead do
    let name = $"{file.Id}_{file.Name}"
    File.WriteAllBytes(name, file.Contents)