namespace CSharpTests.StaticInterfaceMembersResearch;

public class Dog(string name) : IAnimal {
    public string Name { get; } = name;
    public static string FromEnglish(string english) => $"{english}, woof!";
}