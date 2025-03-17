using Xunit.Abstractions;

namespace CSharpTests;

public class Enumerable(ITestOutputHelper helper) {
    [Fact]
    public void EmptyCollectionMoveNextReturnsFalse() {
        var empty = Array.Empty<int>();
        using var enumerator = (empty as IEnumerable<int>).GetEnumerator();
        Assert.False(enumerator.MoveNext());
    }
    
    [Fact]
    public void CollectionCurrentWillCrashIfMoveNextNotCalled() {
        int[] items = [1, 2, 45];
        using var enumerator = (items as IEnumerable<int>).GetEnumerator();
        var exception = Assert.Throws<InvalidOperationException>(() => enumerator.Current);
        helper.WriteLine($"Exception is {exception}");
    }
    
    [Fact]
    public void EmptyCollectionCurrentWillCrashAfterFirstMoveNext() {
        int[] empty = [];
        using var enumerator = (empty as IEnumerable<int>).GetEnumerator();
        enumerator.MoveNext(); // ignore
        var exception = Assert.Throws<InvalidOperationException>(() => enumerator.Current);
        helper.WriteLine($"Exception is {exception}");
    }
}