namespace AdventOfCode;

public class UtilsTests
{
    [Fact]
    public void GetSubsets_EmptyArray_ReturnsSingleEmptySubset()
    {
        List<List<int>> result = Utils.Subsets<int>([]);

        Assert.Single(result);
        Assert.Empty(result[0]);
    }

    [Fact]
    public void GetSubsets_SingleElement_ReturnsTwoSubsets()
    {
        List<List<int>> result = Utils.Subsets([42]);

        Assert.Equal(2, result.Count);
        Assert.Contains(result, x => x.Count == 0);
        Assert.Contains(result, x => x.SequenceEqual([42]));
    }

    [Fact]
    public void GetSubsets_TwoElements_ReturnsFourSubsets()
    {
        List<int> input = [1, 2];
        List<List<int>> expected =
        [
            [],
            [1],
            [2],
            [1, 2]
        ];

        List<List<int>> result = Utils.Subsets(input);

        Assert.Equal(4, result.Count);
        foreach (List<int> subset in expected)
        {
            Assert.Contains(result, x => x.SequenceEqual(subset));
        }
    }

    [Fact]
    public void GetSubsets_ThreeElements_HasEightUniqueSubsets()
    {
        List<char> input = [ 'a', 'b', 'c' ];
        var result = Utils.Subsets(input);

        Assert.Equal(8, result.Count);

        var unique = result
            .Select(subset => string.Join(",", subset))
            .Distinct()
            .Count();

        Assert.Equal(8, unique);
    }

}
