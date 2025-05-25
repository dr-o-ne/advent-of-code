namespace AdventOfCode._2024;

using Point = (int Y, int X);

public sealed class Day8
{
    public object Solve1(string input)
    {
        var result = new HashSet<Point>();

        var matrix = Utils.ParseMatrix<char>(input);
        foreach (var values in ToMap(matrix).Values)
        {
            foreach ((Point p1, Point p2) in Utils.Pairs(values))
            {
                foreach (var a in GetPointForDirection1(matrix, p1, p2, 1))
                    result.Add(a);

                foreach (var a in GetPointForDirection2(matrix, p1, p2, 1))
                   result.Add(a);
            }
        }

        return result.Count;
    }

    public object Solve2(string input)
    {
        var result = new HashSet<Point>();

        var matrix = Utils.ParseMatrix<char>(input);
        foreach (var values in ToMap(matrix).Values)
        {
            foreach ((Point p1, Point p2) in Utils.Pairs(values))
            {
                result.Add(p1);
                result.Add(p2);

                foreach (var a in GetPointForDirection1(matrix, p1, p2))
                    result.Add(a);

                foreach (var a in GetPointForDirection2(matrix, p1, p2))
                    result.Add(a);
            }
        }

        return result.Count;
    }
    
    private static IEnumerable<Point> GetPointForDirection1(char[,] matrix, Point p1, Point p2, int count = int.MaxValue)
    {
        for(int i = 0; i < count; i++)
        {
            var point = ((i + 2) * p1.Y - (i + 1) * p2.Y, (i + 2) * p1.X - (i + 1) * p2.X);
            if (!IsValid(matrix, point))
                yield break;

            yield return point;
        }
    }

    private static IEnumerable<Point> GetPointForDirection2(char[,] matrix, Point p1, Point p2, int count = int.MaxValue)
    {
        for (int i = 0; i < count; i++)
        {
            var point = ((i + 2) * p2.Y - (i + 1) * p1.Y, (i + 2) * p2.X - (i + 1) * p1.X);
            if (!IsValid(matrix, point))
                yield break;

            yield return point;
        }
    }
    
    private static bool IsValid(char[,] matrix, Point p) =>
        p.Y >= 0 && 
        p.X >= 0 && 
        p.Y < matrix.GetLength(0) && 
        p.X < matrix.GetLength(1);

    private static Dictionary<char, List<(int, int)>> ToMap(char[,] matrix)
    {
        var map = new Dictionary<char, List<(int, int)>>();
        for (int y = 0; y < matrix.GetLength(0); y++)
        {
            for (int x = 0; x < matrix.GetLength(1); x++)
            {
                var value = matrix[y, x];
                if (value == '.')
                    continue;

                if (!map.ContainsKey(value))
                    map[value] = [];
                map[value].Add((y, x));
            }
        }

        return map;
    }

    [Fact]
    public void Test1() => Assert.Equal(249, Solve1(Input));

    [Fact]
    public void Test2() => Assert.Equal(905, Solve2(Input));

    private const string Input = @"..................................................
................2.................................
......6.........x.0..G............................
..............x5......0..................S........
.....0............................................
..................................y..............e
..........................G...............O.......
.....................0........GO...............d..
.........................8..........e.............
.........6....................................e...
......z6..5...N..x...................eY...........
................6.........5..........Y.E..........
.........X.....N....................E.a...S.....4.
...........................N.2......d.............
...s..................92.....a...................4
............s....................GO........4......
...........................................d.....S
.....................X....N.......................
.........A........................................
.s.....................A....E.......a.........Y...
.....g....s..................E.....Y..............
.............o....................................
...............................3...............O..
.g.................F.3.y..........................
.......F................y.....................d...
..................................X...............
..8....5............X..Z..........................
..g.....8.....na..................................
......................................3...........
.............J.......x............S.Z.............
..2J....h.A...............Z.......................
......A.............................3.............
............J.......n.............................
.8......o....n...........Z........................
..................o..............y................
..F.........................D...............9H....
.................................1.............9..
..................................................
.........h.....n......................f...........
.h....................z..........j.........9......
.......oF............................j............
..........h......z...........7.....1.f............
........................7.......1...H...j........f
........................................f.........
...........................7.......H..............
................................H.................
.............z...........D........................
..............J....................Dj.............
....................................D.............
....................7.......1.....................";
}
