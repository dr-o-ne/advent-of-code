using Xunit;

namespace AdventOfCode;

public sealed class Template
{
    public int Solve1(string input)
    {
        throw null;
    }

    public int Solve2(string input)
    {
        throw null;
    }

    [Fact]
    public void Test1() => Assert.Equal(0, Solve1(Input));

    [Fact]
    public void Test2() => Assert.Equal(0, Solve2(Input));

    private const string Input = @"";
}
