namespace CSharpTests;

public class StaticInterfaceMembersResearchTests {
    public interface IAnimal {
        static abstract string FromEnglish(string english);
        string Name { get; }
    }

    public class Dog(string name) : IAnimal {
        public string Name { get; } = name;
        public static string FromEnglish(string english) => $"{english}, woof!";
    }

    static string FromEnglish<T>(string english) where T : IAnimal =>
        T.FromEnglish(english);

    static string GenericName<T>(T animal) where T : IAnimal => animal.Name;
    static string InterfaceName(IAnimal animal) => animal.Name;

    [Fact]
    public void CallStaticViaExternalMethod() {
        // C# compiler protects from calling static method via interface
        //Assert.Equal("dog, woof!", FromEnglish<IAnimal>("dog"));
        Assert.Equal("dog, woof!", FromEnglish<Dog>("dog"));
    }

    [Fact]
    public void CallInstanceViaExternalMethod() {
        // C# compiler still disallows passing interface as type parameter.
        // Note, that "Name" method does not use static interface member, but
        // it is not possible for compiler to know the context of the call, hence all it can do
        // is to disallow it basing on fact, that .
        //compiler error: Assert.Equal("Rolf", Name<IAnimal>(new Dog("Rolf")));

        // infers the type of T from the argument
        Assert.Equal("Rolf", GenericName(new Dog("Rolf")));

        IAnimal animal = new Dog("Rolf");
        // type inferred would be IAnimal, hence it is not allowed to use the interface.
        //compiler error: Assert.Equal("Rolf", Name(animal));

        // however, non-generic method can be used
        Assert.Equal("Rolf", InterfaceName(animal));
    }
}