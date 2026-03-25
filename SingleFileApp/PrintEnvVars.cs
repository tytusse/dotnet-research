#!/usr/bin/dotnet

#:property Nullable=enable
using System;

foreach (var env in Environment.GetEnvironmentVariables().Cast<System.Collections.DictionaryEntry>()) {
    Console.WriteLine($"{env.Key}={env.Value}");
}