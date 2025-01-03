﻿using Xunit;

namespace AdventOfCode;

public sealed class Template
{
    public int Solve1(string input)
    {
        return int.MinValue;
    }

    public int Solve2(string input)
    {
        return int.MinValue;
    }

    [Fact]
    public void Test1() => Assert.Equal(int.MinValue, Solve1(Input));

    [Fact]
    public void Test2() => Assert.Equal(int.MinValue, Solve2(Input));

    private const string Input = @"";
}
