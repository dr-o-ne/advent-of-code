namespace AdventOfCode.Common;

public sealed class UnionFindTests
{
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(10)]
    public void Constructor_Initial_State_Is_Correct(int size)
    {
        var uf = new UnionFind(size);

        Assert.Equal(size, uf.Components);

        for (int i = 0; i < size; i++)
        {
            Assert.True(uf.Connected(i, i));
            Assert.Equal(1, uf.ComponentSize(i));
        }
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 1)]
    [InlineData(2, 2)]
    public void Union_With_Self_Is_NoOp(int a, int b)
    {
        var uf = new UnionFind(3);

        uf.Union(a, b);

        Assert.Equal(3, uf.Components);
        Assert.Equal(1, uf.ComponentSize(a));
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 2)]
    [InlineData(0, 2)]
    public void Connected_Symmetric(int a, int b)
    {
        var uf = new UnionFind(3);

        uf.Union(a, b);

        Assert.True(uf.Connected(a, b));
        Assert.True(uf.Connected(b, a));
    }

    [Theory]
    [InlineData(new[] { 0, 1 }, 2)]
    [InlineData(new[] { 0, 1, 2 }, 3)]
    [InlineData(new[] { 1, 2, 3, 4 }, 4)]
    public void Chain_Unions_Form_One_Component(int[] chain, int expectedSize)
    {
        var uf = new UnionFind(5);

        for (int i = 0; i < chain.Length - 1; i++)
            uf.Union(chain[i], chain[i + 1]);

        Assert.Equal(expectedSize, uf.ComponentSize(chain[0]));
    }

    [Theory]
    [InlineData(0, 1, 2)]
    [InlineData(1, 2, 0)]
    [InlineData(2, 3, 4)]
    public void ComponentSize_Same_For_All_Members(int a, int b, int c)
    {
        var uf = new UnionFind(5);

        uf.Union(a, b);
        uf.Union(b, c);

        var size = uf.ComponentSize(a);

        Assert.Equal(size, uf.ComponentSize(b));
        Assert.Equal(size, uf.ComponentSize(c));
    }

    [Theory]
    [InlineData(5, 0)]
    [InlineData(5, 1)]
    [InlineData(5, 4)]
    public void Find_Is_Idempotent(int size, int index)
    {
        var uf = new UnionFind(size);

        uf.Union(0, 1);
        uf.Union(1, 2);

        var r1 = uf.Find(index);
        var r2 = uf.Find(index);

        Assert.Equal(r1, r2);
    }

    [Theory]
    [InlineData(3, 2)]
    [InlineData(5, 4)]
    [InlineData(10, 9)]
    public void Components_Equals_Size_Minus_Unions(int size, int unions)
    {
        var uf = new UnionFind(size);

        for (int i = 0; i < unions; i++)
            uf.Union(i, i + 1);

        Assert.Equal(size - unions, uf.Components);
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 2)]
    public void Repeated_Unions_Do_Not_Change_State(int a, int b)
    {
        var uf = new UnionFind(3);

        uf.Union(a, b);
        var componentsAfterFirst = uf.Components;

        uf.Union(a, b);
        uf.Union(b, a);

        Assert.Equal(componentsAfterFirst, uf.Components);
    }

}
