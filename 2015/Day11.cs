using Xunit;

namespace AdventOfCode._2015;

public sealed class Day11
{
    public string Solve1(string input) =>
        GetNextValidPassword(input);

    public string Solve2(string input) =>
        GetNextValidPassword(GetNextValidPassword(input));

    private static string GetNextValidPassword(string input)
    {
        var password = input.ToCharArray();

        do
        {
            GetNextPassword(password);
        }
        while ( !(Rule1(password) && Rule2(password) && Rule3(password)) );

        return new string(password);
    }

    private static void GetNextPassword(char[] password) => 
        GetNextPassword(password, 7);

    private static void GetNextPassword(char[] password, int i)
    {
        if(i < 0 || i > 7)
            throw new ArgumentOutOfRangeException(nameof(i));

        password[i]++;

        if(password[i] == 'z' + 1)
        {
            password[i] = 'a';
            GetNextPassword(password, i - 1);
        }
    }

    private static bool Rule1(char[] password)
    {
        for(int i = 0; i < password.Length - 2; i++)
        {
            if (password[i] == password[i + 1] - 1 && password[i] == password[i + 2] - 2)
                return true;
        }

        return false;
    }

    private static bool Rule2(char[] password) => 
        !password.Any(x => x == 'i' || x == 'o' || x == 'l');

    private static bool Rule3(char[] password)
    {
        int i = 0;
        for (; i < password.Length - 1; i++)
        {
            if (password[i] == password[i + 1])
                break;
        }

        for(int j = i + 2; j < password.Length - 1; j++)
        {
            if (password[j] == password[j + 1] && password[j] != password[i])
                return true;
        }

        return false;
    }

    [Fact]
    public void Test1() => Assert.Equal("hepxxyzz", Solve1(Input));

    [Fact]
    public void Test2() => Assert.Equal("heqaabcc", Solve2(Input));

    private const string Input = @"hepxcrrq";
}
