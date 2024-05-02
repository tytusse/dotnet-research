namespace CSharpTests;

public static class ObjectExtensions {
    public static TRes Map<T, TRes>(this T input, Func<T, TRes> map) => map(input);
    public static T Do<T>(this T input, Action<T> doSomething) { 
        doSomething(input);
        return input;
    }
}