using System.Collections;
using System.Collections.Specialized;
using Xunit.Abstractions;

namespace PmImprovementWorkshop;

public class Person {
    public string Name { get; }
    public Person(string name) { Name = name; }
}

public class Week1Aka26(ITestOutputHelper outputHelper) {
    [Fact]
    public void Question1() {
        ArrayList names = new ArrayList();
        names.Add("John");
        names.Add("Jane");
        names.Add(42);

        foreach (string name in names) {
            Console.WriteLine(name);
        }
    }

    [Fact]
    public void Question2() {
        StringCollection names = new StringCollection();
        names.Add("John");
        names.Add("Jane");
        //names.Add(42);

        foreach (string name in names) {
            Console.WriteLine(name);
        }
    }

    [Fact]
    public void Question4() {
        var personsCollection = new Person[5];
        int index = 0;
        while (true) {
            if(index >= personsCollection.Length) {
                Console.WriteLine("I am sorry, collection is full.");
                break;
            }
            Console.Write("Enter new person name (or empty value to stop): ");
            string name = Console.ReadLine();
            if (string.IsNullOrEmpty(name)) break;
            personsCollection[index++] = new Person(name);
        }
    }
}