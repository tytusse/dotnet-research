#!/usr/bin/dotnet

#:package Microsoft.Data.SqlClient@6.0.1

#:property Nullable=enable

using System.Text.RegularExpressions;
using System.Linq;
using System.IO;
using Microsoft.Data.SqlClient;

var connectionString = args switch {
    [var arg, ..] => arg,
    _ => throw new ArgumentException("Please provide a valid connection string or a path to a file containing the connection string.")
};

Console.WriteLine($"Using connection string: {connectionString}");
using var connection = new SqlConnection(connectionString);
connection.Open();

var qry = "SELECT name FROM master.dbo.sysdatabases";
using var command = new SqlCommand(qry, connection);
using var reader = command.ExecuteReader();
while (reader.Read()){
    Console.WriteLine(reader.GetString(0));
}

Console.WriteLine("Sql connect check done.");