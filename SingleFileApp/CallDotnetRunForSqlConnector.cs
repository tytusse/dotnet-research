#!/usr/bin/dotnet

#:property Nullable=enable
// no way for dotnet single-file to know its own source path.
// only paths known are after it was compiled and published to some tmp dir in `.local/share/dotnet`

foreach(var arg in args) {
    Console.WriteLine($"Arg: {arg}");
}

var (path, connectionString) = args switch {
    [var argPath, var argConnectionString, ..] => (argPath, argConnectionString),
    _ => throw new ArgumentException("Please provide a valid script and connection string as arguments.")
};


var process = new System.Diagnostics.Process {
    StartInfo = new System.Diagnostics.ProcessStartInfo {
        FileName = "dotnet",
        Arguments = $"run --file \"{path}\" -- \"{connectionString}\"",
        RedirectStandardOutput = true,
        UseShellExecute = false,
        CreateNoWindow = true
    }
};
process.Start();

while (!process.StandardOutput.EndOfStream) {
    Console.WriteLine(process.StandardOutput.ReadLine());
}
process.WaitForExit();
Console.WriteLine($"Process exited with code {process.ExitCode}");