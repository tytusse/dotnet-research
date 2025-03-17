namespace CSharpTests;

public class XUnitAsserts {
    [Fact]
    public void ArrayEqualWithStandardComparer() {
        int[][] actual = [[1, 2, 3], [4, 5, 6]];
        
        Assert.Equal<int[][]>([[1, 2, 3], [4, 5, 6]], actual);
    }
}