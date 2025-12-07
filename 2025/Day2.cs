namespace AdventOfCode._2025;

public sealed class Day2
{
    internal sealed record Range(long From, long To);

    public object Solve1(string input) => 
        Solve(input, (id) => IsInvalid(id, 2));

    public object Solve2(string input) => 
        Solve(input, IsInvalid);

    private long Solve(string input, Func<string, bool> isInValidFunc)
    {
        long result = 0;

        foreach (Range range in Parse(input))
        {
            for (long i = range.From; i <= range.To; i++)
            {
                string id = i.ToString();
                if (isInValidFunc(id))
                    result += i;
            }
        }

        return result;
    }

    private static bool IsInvalid(string id, int partsCount)
    {
        if (id.Length % partsCount != 0)
            return false;

        string part = id[..(id.Length / partsCount)];

        return id == string.Concat(Enumerable.Repeat(part, partsCount));
    }

    private static bool IsInvalid(string id)
    {
        for(int i = 2; i <= id.Length; i++)
            if(IsInvalid(id, i))
                return true;

        return false;
    }

    private static IEnumerable<Range> Parse(string input) => 
        input.Split(",").Select(x =>
        {
            var pair = x.Split("-");
            return new Range(long.Parse(pair[0]), long.Parse(pair[1]));
        });

    [Fact]
    public void Test1() => Assert.Equal(1227775554L, Solve1(TestInput));

    [Fact]
    public void Test2() => Assert.Equal(38437576669L, Solve1(Input));

    [Fact]
    public void Test3() => Assert.Equal(4174379265L, Solve2(TestInput));

    [Fact]
    public void Test4() => Assert.Equal(49046150754, Solve2(Input));

    private const string TestInput = @"11-22,95-115,998-1012,1188511880-1188511890,222220-222224,1698522-1698528,446443-446449,38593856-38593862,565653-565659,824824821-824824827,2121212118-2121212124";

    private const string Input = @"3737332285-3737422568,5858547751-5858626020,166911-236630,15329757-15423690,753995-801224,1-20,2180484-2259220,24-47,73630108-73867501,4052222-4199117,9226851880-9226945212,7337-24735,555454-591466,7777695646-7777817695,1070-2489,81504542-81618752,2584-6199,8857860-8922218,979959461-980003045,49-128,109907-161935,53514821-53703445,362278-509285,151-286,625491-681593,7715704912-7715863357,29210-60779,3287787-3395869,501-921,979760-1021259";

}
