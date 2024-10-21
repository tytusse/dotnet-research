namespace CSharpTests.StaticInterfaceMembersResearch;

public interface IAnimal {
    static abstract string FromEnglish(string english);
    string Name { get; }
}