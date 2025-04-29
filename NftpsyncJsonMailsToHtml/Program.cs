// See https://aka.ms/new-console-template for more information

if (args.Length == 0) {
    Console.WriteLine("Please provide path to json file as 1st argument");
    return;
}

var rootFolderPath = args[0];
if (!Directory.Exists(rootFolderPath)) {
    Console.WriteLine("Directory does not exist: " + rootFolderPath);
    return;
}

var htmlsPath = Path.Combine(rootFolderPath, "htmls");
if (!Directory.Exists(htmlsPath)) {
    Directory.CreateDirectory(htmlsPath);
}

var files = Directory.GetFiles(rootFolderPath, "*.email", SearchOption.TopDirectoryOnly);
Console.WriteLine($"Found {files.Length} files");

foreach (var filePath in files) {
    Console.WriteLine("Handling file " + filePath);
    string body;
    try {
        var json = File.ReadAllText(filePath);
        Console.WriteLine(json);
        var obj = System.Text.Json.JsonDocument.Parse(json);
        body = obj.RootElement.GetProperty("body").GetString() ?? "no body";
    }
    catch (Exception e) {
        body = "error reading file: " + e.Message;
        Console.Error.WriteLine(body);
    }

    try {
        var htmlPath = Path.Combine(htmlsPath, Path.GetFileName(filePath)+".html");
        if (File.Exists(htmlPath)) {
            File.Delete(htmlPath);
        }
        File.WriteAllText(htmlPath, body);
    }
    catch (Exception e) {
        Console.Error.WriteLine("Error writing file: " + e);
    }
}
