using Xunit.Abstractions;

namespace CSharpTests;

public class RegexResearch(ITestOutputHelper testOutputHelper) {
    string Replace(string inp) => 
        System.Text.RegularExpressions.Regex.Replace(inp, @"(?:[\n^])\s+(?:\r?[\n$])", "");

    [Fact]
    public void CheckRegex() {
        System.Text.RegularExpressions.Regex rx = new(@"^\s+$");
        var text =
            """
            
            aaaa1 = 1111
            aaaa2 = 2222
            
            aaaa3 = 3333
             
            aaaa4 = 4444
            aaaa5 = 5555
            """;
        testOutputHelper.WriteLine(Replace(text));
        testOutputHelper.WriteLine("-------------");
        testOutputHelper.WriteLine(rx.Replace(text, ""));
    }
}