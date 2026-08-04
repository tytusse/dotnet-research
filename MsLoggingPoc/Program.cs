using System.Text;
using Microsoft.Extensions.Logging;

Console.WriteLine("Hello, World!");


var lf = LoggerFactory.Create(builder => {
    builder
        .AddSimpleConsole(x => {
            // x. = true;
            x.IncludeScopes  = true;
        })
        .SetMinimumLevel(LogLevel.Trace);
});
var logger = lf.CreateLogger<Program>();

var sb = new StringBuilder();
sb.Append("the").Append(" ").Append("log");
logger.LogInformation("{contents}", sb);
{
    using var scope = logger.BeginScope("Scope log {Value}", 42);
    using var scope2 = logger.BeginScope("Scope 2");
    logger.LogInformation("What now: {SecondValue}", 84);
    logger.LogInformation("Nothing");
}
logger.LogInformation("Something else: {AnotherValue}", 666);
logger.LogInformation("Now is: {NowValue:yyyyMMdd HH:mm:ss}", DateTime.Now);
