using System.Text.RegularExpressions;
using Xunit;

namespace AdventOfCode._2015;

public sealed class Day9
{
    public int Solve1(string input) => Solve(input).Item1;

    public int Solve2(string input) => Solve(input).Item2;

    private static (int, int) Solve(string input)
    {
        var graph = new Dictionary<Edge, int>();
        var nodes = new HashSet<string>();

        foreach ((string From, string To, int Distance) item in Parse(input))
        {
            graph[new Edge(item.From, item.To)] = item.Distance;
            graph[new Edge(item.To, item.From)] = item.Distance;

            nodes.Add(item.From);
            nodes.Add(item.To);
        }

        var routes = Utils.Permute(nodes.ToList());

        var min = int.MaxValue;
        var max = int.MinValue;
        foreach (var route in routes)
        {
            var distance = GetDistance(route, graph);
            min = Math.Min(min, distance);
            max = Math.Max(max, distance);
        }

        return (min, max);
    }

    private static int GetDistance(List<string> route, Dictionary<Edge, int> graph)
    {
        int result = 0;

        for (int i = 0; i < route.Count - 1; i++)
        {
            result += graph[new Edge(route[i], route[i + 1])];
        }

        return result;
    }

    private static IEnumerable<(string, string, int)> Parse(string input) => input
        .Split(Environment.NewLine)
        .Select(ParseLine);

    private static (string, string, int) ParseLine(string line)
    {
        var m = Regex.Match(line, @"(\w+) to (\w+) = (\w+)");
        
        if (!m.Success)
        {
            throw new ArgumentException(line);
        }

        return (m.Groups[1].Value, m.Groups[2].Value, int.Parse(m.Groups[3].Value));
    }

    internal sealed record Edge(string From, string To);

    [Fact]
    public void Test1() => Assert.Equal(117, Solve1(Input));

    [Fact]
    public void Test2() => Assert.Equal(909, Solve2(Input));

    private const string Input = @"Faerun to Tristram = 65
Faerun to Tambi = 129
Faerun to Norrath = 144
Faerun to Snowdin = 71
Faerun to Straylight = 137
Faerun to AlphaCentauri = 3
Faerun to Arbre = 149
Tristram to Tambi = 63
Tristram to Norrath = 4
Tristram to Snowdin = 105
Tristram to Straylight = 125
Tristram to AlphaCentauri = 55
Tristram to Arbre = 14
Tambi to Norrath = 68
Tambi to Snowdin = 52
Tambi to Straylight = 65
Tambi to AlphaCentauri = 22
Tambi to Arbre = 143
Norrath to Snowdin = 8
Norrath to Straylight = 23
Norrath to AlphaCentauri = 136
Norrath to Arbre = 115
Snowdin to Straylight = 101
Snowdin to AlphaCentauri = 84
Snowdin to Arbre = 96
Straylight to AlphaCentauri = 107
Straylight to Arbre = 14
AlphaCentauri to Arbre = 46";
}
