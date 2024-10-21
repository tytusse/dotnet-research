namespace CSharpTests.StaticInterfaceMEthodExamples;

public interface IAnimal {
    static abstract string FromEnglish(string english);
}

public class Dog : IAnimal {
    public static string FromEnglish(string english) => $"{english}, woof!";
}

public class StaticInterfaceMembersResearchTests {
    static string FromEnglish<T>(string english) where T : IAnimal => T.FromEnglish(english);
    // C# compiler requires to add `where T : IAnimal` here.
    // F# compiler silently drops the constraint...
    //compiler error: static string FromEnglish2<T>(string english) => FromEnglish<T>(english);

    [Fact]
    public void CallStaticViaExternalMethod() {
        // C# compiler protects from calling static method via interface
        //Assert.Equal("dog, woof!", FromEnglish<IAnimal>("dog"));
        Assert.Equal("dog, woof!", FromEnglish<Dog>("dog"));
    }
}