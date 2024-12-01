using System.Text;
using Xunit;

namespace AdventOfCode._2015;

public sealed class Day10
{
    public int Solve1(string input)
    {
        for (int i = 0; i < 40; i++) 
        {
            input = Process(input);    
        }

        return input.Length;
    }

    public int Solve2(string input)
    {
        for (int i = 0; i < 50; i++)
        {
            input = Process(input);
        }

        return input.Length;
    }

    private static string Process(string input)
    {
        var sb = new StringBuilder();

        int cnt = 1;
        for (int i = 1; i < input.Length; i++)
        {
            if (input[i] == input[i - 1])
            {
                cnt++;
            }
            else
            {
                sb.Append(cnt);
                sb.Append(input[i - 1]);
                cnt = 1;
            }
        }

        sb.Append(cnt);
        sb.Append(input[^1]);

        return sb.ToString();
    }

    [Fact]
    public void Test1() => Assert.Equal(492982, Solve1(Input));

    [Fact]
    public void Test2() => Assert.Equal(6989950, Solve2(Input));

    private const string Input = @"1321131112";
}
