using System.Text;
using Microsoft.Extensions.Logging;

Console.WriteLine("Hello, World!");


var lf = LoggerFactory.Create(builder => {
    builder.AddConsole().SetMinimumLevel(LogLevel.Trace);
});
var logger = lf.CreateLogger<Program>();

var sb = new StringBuilder();
sb.Append("the").Append(" ").Append("log");
logger.LogInformation("{contents}", sb);