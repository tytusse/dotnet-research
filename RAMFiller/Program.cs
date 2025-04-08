// See https://aka.ms/new-console-template for more information

var pleaseRun = true;
// ReSharper disable once NotAccessedVariable
// ReSharper disable once CollectionNeverQueried.Local
var arrays = new List<string[]>();
const long mega = 1000 * 1000;
var commands = new Dictionary<string, Action<ReadOnlySpan<string>>> {
    ["quit"]= _ => pleaseRun = false,
    ["exit"]= _ => pleaseRun = false,
    ["walk"]= _ => {
        for (int arrNum = 0; arrNum < arrays.Count; arrNum++) {
            for (int i = 0; i < arrays[arrNum].Length; i++) {
                var x = arrays[arrNum][i];
                
            }
        }
    },
    ["add"]= args => {
        var megaRaw = "";
        if (args.Length == 0) {
            while (megaRaw == "") {
                Console.WriteLine("How many Megs to allocate?");
                megaRaw = Console.ReadLine() ?? "";    
            }
        }
        else {
            megaRaw = args[0];
        }
        
        if (!int.TryParse(megaRaw, out var megaToAllocate) || megaToAllocate < 0) {
            Console.WriteLine("Invalid number - please provide positive integer");
            return;
        }
        
        Console.WriteLine($"Allocating {megaToAllocate} Megs of objects");
        var i = 0;
        for (; i < megaToAllocate; i++) {
            try {
                var gigaArray = new string[mega];
                gigaArray.Initialize();
                arrays.Add(gigaArray);
            }
            catch (OutOfMemoryException) {
                Console.WriteLine("oops - out of memory");
                break;
            }
        }
        Console.WriteLine($"Allocated {i} Megs objects");
    },
    ["clear"]= _ => {
        arrays.Clear();
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