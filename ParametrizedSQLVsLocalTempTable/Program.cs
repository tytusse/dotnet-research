// See https://aka.ms/new-console-template for more information

using System.Data.SqlClient;

Console.WriteLine("Hello, World!");

// replace conn string obviously
var connectionString = "????";

using var connection = new SqlConnection(connectionString);
connection.Open();

{
    var command = connection.CreateCommand();
    command.CommandText = @"
        SELECT TOP 10 * 
        INTO #result
        FROM tomasz_animals where Name Like @NameStarts + '%'";
    
    command.Parameters.AddWithValue("@NameStarts", "do");
    command.ExecuteNonQuery();
    Console.WriteLine("First query OK");
}
{
    var command = connection.CreateCommand();
    command.CommandText = "SELECT * FROM #result";
    using var reader = command.ExecuteReader();
    while (reader.Read())
    {
        Console.WriteLine(reader["ID"]);
        Console.WriteLine(reader["Name"]);
    }
}
