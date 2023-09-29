using System.Collections;
using Xunit.Abstractions;

namespace CSharpTests;

public class EnvironmentVariablesTests
{
    private readonly ITestOutputHelper _output;

    public EnvironmentVariablesTests(ITestOutputHelper output)
    {
        _output = output;
    }
    
    [Fact] 
    public void PrintAll()
    {
        foreach (DictionaryEntry de in Environment.GetEnvironmentVariables())
        {
            _output.WriteLine($"{de.Key} = {de.Value}");
        }
    }
    
    [Fact]
    public void Foo_env_var_NOT_read_from_launchSettings_json()
    {
        var value = Environment.GetEnvironmentVariable("FOO");
        Assert.Null(value);
    }
    
}