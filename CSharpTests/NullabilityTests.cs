namespace CSharpTests;

public class NullabilityTests {
    public enum TestEnum {
        Foo, Bar, Baz
    }
    
    
    [Fact]
    public void Enumbrable_Cast_Nullability() {
        var vals =  Enum.GetValues<TestEnum>();
    }
}