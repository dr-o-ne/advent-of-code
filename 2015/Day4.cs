namespace AdventOfCode._2015;

public sealed class Day4
{
    public int Solve1(string input) => Solve(input, "00000");

    public int Solve2(string input) => Solve(input, "000000");

    private static int Solve(string input, string prefix)
    {
        for (int i = 0; i < int.MaxValue; i++)
        {
            var hash = GetHash(input + i);
            if (hash.StartsWith(prefix))
                return i;
        }

        return int.MinValue;
    }

    private static string GetHash(string input)
    {
        using MD5 md5 = MD5.Create();

        byte[] inputBytes = Encoding.ASCII.GetBytes(input);
        byte[] hashBytes = md5.ComputeHash(inputBytes);

        return Convert.ToHexString(hashBytes);
    }

    [Fact]
    public void Test1() => Assert.Equal(254575, Solve1(Input));

    [Fact]
    public void Test2() => Assert.Equal(1038736, Solve2(Input));

    private const string Input = @"bgvyzdsv";
}
