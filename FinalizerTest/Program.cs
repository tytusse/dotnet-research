// See https://aka.ms/new-console-template for more information

Console.WriteLine("Hello, World!");
for (int i = 0; i < 3; i++) {
    new Foobar().Baz();
}
Console.WriteLine($"Thread: {Thread.CurrentThread.ManagedThreadId}");
Console.WriteLine("type 'exit' to quit");
string res;
do {
    res = Console.ReadLine() ?? "";
    GC.Collect();
} while(res != "exit");

public class Foobar {
    public void Baz() {
        
    }
    
    ~Foobar() {
        Console.WriteLine($"finalizer thread running: {Thread.CurrentThread.ManagedThreadId}");
        throw new Exception("test");
    }
}