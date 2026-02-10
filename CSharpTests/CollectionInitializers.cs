using Microsoft.VisualBasic;
using Xunit.Abstractions;

namespace CSharpTests;

public class CollectionInitializers(ITestOutputHelper testOutputHelper) {
    [Fact]
    public void TypeOfEnumerableCollectionInitializer() {
        IEnumerable<int> enumerable = [1, 2, 3];
        testOutputHelper.WriteLine(enumerable.GetType().FullName);
    }

    [Fact]
    public void DictionaryInitializer() {
        MyFunc(
        new Dictionary<int, int>{
            [1] = 2,
            [3] = 42,
        }); 
        MyFunc([    
            // ??
        ]); 
        
        MyFunc2([
            (3, 42)
        ]);
        
        return;
        
        static void MyFunc(Dictionary<int, int> inp) {};
        static void MyFunc2(List<(int, int)> inp) {}
    }
}