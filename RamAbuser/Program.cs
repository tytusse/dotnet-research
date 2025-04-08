// See https://aka.ms/new-console-template for more information
var list = new LinkedList<string>();
var pleaseRun = true;
var commands = new Dictionary<string, Action<ReadOnlySpan<string>>> {
    ["quit"]= _ => pleaseRun = false,
    ["exit"]= _ => pleaseRun = false,
    ["add"]= args => {
        const int stringSize = 1000;
        const int million = 1000 * 1000;
        uint megs = 1;
        if (args.Length >= 1) {
            if(!uint.TryParse(args[0], out megs))
            {
                Console.WriteLine("Error: Invalid argument: expected uint number of megs");
                return;
            }
        }
        Console.WriteLine($"will add {megs}M of big ass strings (size={stringSize}) to linked list");
        for (var i = 0; i < megs*million; i++) {
            // every half-million
            if (2*i % million == 0) {
                Console.WriteLine($"Allocated {(decimal) i / million}M strings");
            }
            // probably should be below LOH limit
            list.AddLast(new string('4', stringSize));
        }
    },
    ["add-small"]= _ => list.AddLast("small string"),
    ["clear"]= _ => list.Clear(),
    ["gccollect"]= _ => GC.Collect(),
    ["clear-and-collect"]= _ => {
        list.Clear();
        GC.Collect();
    },
};

Console.WriteLine("GC.GetConfigurationVariables()");
var gcConfigurationVariables = GC.GetConfigurationVariables();
foreach (var param in gcConfigurationVariables) {
    Console.WriteLine(param.Key + " = " + param.Value);
}


while (pleaseRun) {
    Console.WriteLine($"Available commands: {string.Join(",", commands.Keys)}");
    Console.WriteLine("Please enter command");
    var commandRaw = Console.ReadLine();
    if (commandRaw is null) {
        pleaseRun = false;
        continue;
    }
    var commandParts = 
        commandRaw.Split(" ", StringSplitOptions.TrimEntries|StringSplitOptions.RemoveEmptyEntries);
    var commandName = commandParts[0];
    switch (commands.GetValueOrDefault(commandName)) {
        case null: Console.WriteLine($"Unknown command: {commandName}"); break;
        case var command: command(commandParts.AsSpan()[1..]); break;  
    }
}

Console.WriteLine("Bye");




