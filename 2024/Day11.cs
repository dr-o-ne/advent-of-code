namespace AdventOfCode._2024;

public sealed class Day11
{
    public object Solve1(string input) =>Solve(input, 25);

    public object Solve2(string input) => Solve(input, 75);

    private static long Solve(string input, int steps)
    {
        var cache = new Dictionary<(long, int), long>(); // (value, currentStep) -> count

        return Utils.ParseList<long>(input, " ").Sum(x => Count(x, 0));

        long Count(long value, int currentStep)
        {
            if (currentStep == steps)
                return 1;

            if (cache.TryGetValue((value, currentStep), out long result))
                return result;

            var length = (int)Math.Log10(value) + 1;

            if (value == 0)
                result = Count(1, currentStep + 1);
            else if (length % 2 == 0)
                result = 
                    Count(value / (long)Math.Pow(10, length / 2), currentStep + 1) + 
                    Count(value % (long)Math.Pow(10, length / 2), currentStep + 1);
            else
                result = Count(value * 2024, currentStep + 1);

            cache[(value, currentStep)] = result;

            return result;
        }
    }

    [Fact]
    public void Test1() => Assert.Equal(193899L, Solve1(Input));

    [Fact]
    public void Test2() => Assert.Equal(229682160383225, Solve2(Input));

    [Theory]
    [InlineData(1, "125", 0)] // 125
    [InlineData(1, "125", 1)] // 253000
    [InlineData(2, "125", 2)] // 253 0
    [InlineData(2, "125", 3)] // 512072 1
    [InlineData(3, "125", 4)] // 512 72 2024
    [InlineData(5, "125", 5)] // 1036288 7 2 20 24
    public void SolveTest(long expected, string input, int steps) =>
        Assert.Equal(expected, Solve(input, steps));

    private const string Input = @"0 89741 316108 7641 756 9 7832357 91";
}
