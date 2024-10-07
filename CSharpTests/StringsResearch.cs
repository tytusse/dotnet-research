namespace CSharpTests;

public class StringsResearch
{
    [Fact]
    public void Raw_string_Handles_trailing_Spaces_nicely()
    {
        const string exampleString = 
            """
            SELECT genre
            FROM bands
            WHERE name = 'Abba'
            """;
        Assert.StartsWith("SELECT", exampleString);
        Assert.EndsWith("'Abba'", exampleString);
    }
}