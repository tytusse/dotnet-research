using Xunit.Abstractions;

namespace CSharpTests;

public class LocalMethodVsOOrderOfVarInits(ITestOutputHelper testOutputHelper) {

    [Fact]
    public void CanUseVarBeforeInitialization() {
        // C# is smart enough to detect uninitialized variables in local methods.
        // Error: Use of unassigned local variable 'foo'
        // LocalMethod(); // <-- Compile ERROR!

        var foo = "bar";

        void LocalMethod() {
            testOutputHelper.WriteLine($"Foo -> {foo}");
            testOutputHelper.WriteLine($"Foo is null? {foo is null}");
        }
    }
}