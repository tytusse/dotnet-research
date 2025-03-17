using Xunit.Abstractions;

namespace CSharpTests;

public class CollectionInitializers(ITestOutputHelper testOutputHelper) {
    [Fact]
    public void TypeOfEnumerableCollectionInitializer() {
        IEnumerable<int> enumerable = [1, 2, 3];
        testOutputHelper.WriteLine(enumerable.GetType().FullName);
    }
}