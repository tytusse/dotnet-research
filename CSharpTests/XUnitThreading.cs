using Xunit.Abstractions;

namespace CSharpTests;

public class XUnitThreading(ITestOutputHelper output) {
    [Fact]
    public void ErrorInSeparateThread() {
        output.WriteLine($"{Thread.CurrentThread.ManagedThreadId}: Starting test");
        using var stoppedEvent = new AutoResetEvent(false);
        var t = new Thread(_ => {
            try {
                output.WriteLine($"{Environment.CurrentManagedThreadId}: Starting thread");
                throw new Exception("test other thread exception");
            }
            finally {
                output.WriteLine($"{Environment.CurrentManagedThreadId}: Stopping thread");
                // ReSharper disable once AccessToDisposedClosure
                stoppedEvent.Set();
            }
        });
        t.Start();
        stoppedEvent.WaitOne();
        output.WriteLine("sleeping 1s");
        Thread.Sleep(1000);
        output.WriteLine($"{Environment.CurrentManagedThreadId}: Stopping test");
    }
}