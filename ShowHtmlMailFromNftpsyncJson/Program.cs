// See https://aka.ms/new-console-template for more information

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

if (args.Length == 0) {
    Console.WriteLine("Please provide path to json file as 1st argument");
    return;
}

var hostBuilder = new WebHostBuilder()  
    .UseKestrel() //tiny web server. It can be replaced with any web server  
    .UseStartup<Startup>()  
    .Build();  
      
hostBuilder.Run();

public class Startup {
    public void Configure(IApplicationBuilder builder)  
    {  
        var path = Environment.GetCommandLineArgs()[1]; //1 as 0 is app path
        Console.WriteLine($"Using path: {path}");
        var json = File.ReadAllText(path);
        Console.WriteLine(json);
        var obj = System.Text.Json.JsonDocument.Parse(json);
        var body = obj.RootElement.GetProperty("body").GetString() ?? "No body provided";
        builder.Run(appContext => appContext.Response.WriteAsync(body));  
    } 
}