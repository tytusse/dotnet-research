using System.ComponentModel;
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

    [Fact]
    public async Task EnumerableAsAsyncEnumerable() {
        const int slowEnumStepTime = 20;
        const int spamStepTime = 10;
        const int workerSteps = 10;
        using var mre = new ManualResetEvent(false);
        // this test only prints logs, does not assert anything (yet?)
        // Intent is to see if iteration over regular enum is really async if
        // it is wrapped in IAsyncEnumerable via `ToAsyncEnumerable`.
        // From its source code, it seems that not - the method just does 
        // `foreach` with `yield return` and I am *guessing* this just does sync enumeration.
        // However, I am not sure what C# compiler adds, but I again guess, that it does not add
        // anything that would cause the iteration to "give control" between steps, i.e.,
        // it does not have `await Task.Yield()` or anything like that.
        
        // this is also naive, as it starts separate task for the loop and hence will always 
        // be seen as Spam/work/spam.
        // To have clear picture one would need to somehow bind both to single thread (or something).
        var asAsync = SlowEnumerable().ToAsyncEnumerable();
        var shouldSpam = true;
        var t= Task.Run(async ()=> await Worker(mre));
        
        while (shouldSpam) {
            mre.Set();
            helper.WriteLine("Spamming ...");
            Thread.Sleep(spamStepTime);
        }

        await t;
        
        return;
        async Task Worker(ManualResetEvent mre) {
            mre.WaitOne();
            await foreach (var item in asAsync) {
                helper.WriteLine($"Item produced: {item}");
            }
            shouldSpam = false;
        }
        static IEnumerable<int> SlowEnumerable() {
            foreach (var item in System.Linq.Enumerable.Range(0, workerSteps)) {
                Thread.Sleep(slowEnumStepTime);
                yield return item;
            }
        }
    }
}