#!/usr/bin/dotnet

#:property Nullable=enable
using System;
using System.IO;

Console.WriteLine("Current dir:");
Console.WriteLine(Path.GetFullPath("."));
Console.WriteLine("Parent dir:");
Console.WriteLine(Path.GetFullPath(".."));

var thisFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
Console.WriteLine("This file path:");
Console.WriteLine(thisFilePath);