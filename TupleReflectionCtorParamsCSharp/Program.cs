using System.Collections;
using System.Diagnostics.CodeAnalysis;

void Analyze(
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]Type t)
{
    Console.WriteLine($"reflecting on type {t.FullName}");
    var constructorInfos = t.GetConstructors();
    Console.WriteLine($"type has {constructorInfos.Length} constructors");
    foreach (var constructorInfo in constructorInfos)
    {
        Console.WriteLine($"Showing ctor {constructorInfo}");
        var cps = constructorInfo.GetParameters();
        Console.WriteLine($"{t.Name} constructor has {cps.Length} parameters");
        var i = 0;
        foreach (var cp in cps)
        {
            Console.WriteLine($"param no {i++} name is {cp.Name}, type {cp.ParameterType.Name}");
            if (cp.Name is null)
            {
                Console.WriteLine("WARNING: param name is NULL!!");
            }
        }
    }
}

var dumbData = new System.Tuple<int, string>(1, "foobar");

Console.WriteLine($"dumb data {dumbData}");
var anon = new { Id = 1, Name = "Bob" };
var l = new ArrayList(4);
var ts = new[]
{
    l.GetType(),
};
foreach (var t in ts)
{
    Analyze(t);
}
