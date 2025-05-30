﻿namespace AdventOfCode._2024;

using Node = (Cell2D Position, Vector2D Direction);

public sealed class Day16
{
    private static readonly Vector2D Up = new(-1, 0);
    private static readonly Vector2D Right = new(0, 1);
    private static readonly Vector2D Down = new(1, 0);
    private static readonly Vector2D Left = new(0, -1);

    private static readonly Vector2D[] Directions = { Up, Right, Down, Left };

    public object Solve1(string input)
    {
        var grid = new Grid2D<char>(input);
        var startPosition = FindStartPosition(grid);
        var endPosition = FindEndPosition(grid);

        (var Costs, var _) = GetShortestPaths(grid, (startPosition, Right));

        return Directions
            .Select(x => Costs[(endPosition, x)])
            .Min();
    }

    public object Solve2(string input)
    {
        var grid = new Grid2D<char>(input);
        var startPosition = FindStartPosition(grid);
        var endPosition = FindEndPosition(grid);

        (var Costs, var Predecessors) = GetShortestPaths(grid, (startPosition, Right));

        var result = new HashSet<Cell2D> { endPosition };
        var queue = new Queue<Node>();

        var minCost = Directions
            .Select(x => Costs[(endPosition, x)])
            .Min();

        foreach(var direction in Directions) {

            if (Costs[(endPosition, direction)] != minCost)
                continue;

            foreach (var node in Predecessors[(endPosition, direction)])
                queue.Enqueue(node);
        }

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            result.Add(current.Position);
            foreach (var node in Predecessors[current])
                queue.Enqueue(node);
        }

        return result.Count;
    }

    // Solve using Deijkistra's algorithm
    private static (Dictionary<Node, int>, Dictionary<Node, HashSet<Node>>) GetShortestPaths(Grid2D<char> grid, Node start)
    {
        Dictionary<Node, int> costs = [];
        costs[start] = 0;

        Dictionary<Node, HashSet<Node>> predecessors = [];
        predecessors[start] = [];

        var positions = FindOpenPositions(grid);
        foreach (var node in positions.SelectMany(position => Directions.Select(direction => (position, direction))))
        {
            costs[node] = int.MaxValue;
            predecessors[node] = [];
        }

        var pq = new PriorityQueue<Node, int>();

        pq.Enqueue(start, 0);

        while (pq.TryDequeue(out var currentNode, out var currentCost))
        {
            if (costs[currentNode] < currentCost)
                continue;

            foreach (Node nextNode in NextNodes(grid, currentNode.Position))
            {
                var newCost = currentNode.Direction == nextNode.Direction ?
                    currentCost + 1 :
                    currentCost + 1001;

                // using >= instead of > in order to find all shortest paths
                if (costs[nextNode] >= newCost)
                {
                    if (costs[nextNode] == newCost)
                        predecessors[nextNode].Add(currentNode);
                    else
                        predecessors[nextNode] = [currentNode];

                    costs[nextNode] = newCost;
                    pq.Enqueue(nextNode, newCost);
                }
            }
        }

        return (costs, predecessors);
    }

    private static IEnumerable<Node> NextNodes(Grid2D<char> grid, Cell2D currentPosition)
    {
        foreach (var direction in Directions)
        {
            var newPosition = currentPosition.Add(direction);

            if (!grid.IsInRange(newPosition) || grid[newPosition] == '#')
                continue;

            yield return (newPosition, direction);
        }
    }

    private static IEnumerable<Cell2D> FindOpenPositions(Grid2D<char> grid) => grid
        .Where(x => x.Value != '#')
        .Select(x => new Cell2D(x.Y, x.X));

    private static Cell2D FindEndPosition(Grid2D<char> grid) => grid
        .Where(x => x.Value == 'E')
        .Select(x => new Cell2D(x.Y, x.X))
        .Single();

    private static Cell2D FindStartPosition(Grid2D<char> grid) => grid
        .Where(x => x.Value == 'S')
        .Select(x => new Cell2D(x.Y, x.X))
        .Single();

    [Fact]
    public void Test1() => Assert.Equal(123540, Solve1(Input));

    [Fact]
    public void Test2() => Assert.Equal(665, Solve2(Input));

    private const string TestInput = @"#################
#...#...#...#..E#
#.#.#.#.#.#.#.#.#
#.#.#.#...#...#.#
#.#.#.#.###.#.#.#
#...#.#.#.....#.#
#.#.#.#.#.#####.#
#.#...#.#.#.....#
#.#.#####.#.###.#
#.#.#.......#...#
#.#.###.#####.###
#.#.#...#.....#.#
#.#.#.#####.###.#
#.#.#.........#.#
#.#.#.#########.#
#S#.............#
#################";

    private const string Input = @"#############################################################################################################################################
#.....#.....#...#.................#...#.......#.........#...#.......#...#.......#.......#...#...#.....#...#.......................#...#....E#
#.###.#.#.#.###.#.###.###.#######.#.#.#.#.###.#.###.###.#.#.#.###.#.###.#.#.#####.###.#.###.#.#.#.#.#.#.#.#.#####.#.###.#.#######.#.#.#.###.#
#...#.#.#.#...#.....#...#.#.....#...#...#.#...#.#...#...#.#.#...#.#...#.#.#...#...#...#.....#.#.#.#.#...#...#.....#.#.#.....#.....#.#.#...#.#
#.#.###.#.###.#.#####.#.#.###.###########.#.#####.###.###.###.###.###.#.#.#.#.#.###.#######.#.#.#.#.#########.#####.#.#.#####.#####.#.###.#.#
#.#.....#.#.#.#...#...#.#...#.....#.....#.#.....#.#.#.#.........................#.#.......#.#.#...#.....#...#.#...#.....#...#.#...#.#.....#.#
#.#######.#.#.#.###.#.#.###.#.#.#.#.#.###.#####.#.#.#.#.###.###.###.#.###.#.#.#.#.#######.###.#.#####.#.#.#.#.###.###.#.#.#.#.#.#.#.#######.#
#...#...#.#...#.#...#.#...#.#.#.#...#.#.......#...#...#...#...#.#.....#...#.#...#.......#...#...#.....#...#.#.....#...#...#...#.#.........#.#
###.#.###.#.#####.###.###.#.###.#.#####.###.#.#####.###.#.###.#.###.###.###.#####.#########.#.#.###.#.###.#.#####.#.###.#######.###########.#
#...#...#.#.#.....#...#...#.#...#.#...#.#...#.#...#...#.#.....#...#...#.#...........#.........#.#...#.#...#.........#.#.#.......#...........#
#.#####.#.#.#.#######.#.###.#.###.#.#.#.#.###.#.#####.#########.#.#.###.#.#####.###.#.#####.#.#.#.###.###.###.#####.#.#.#.#######.#########.#
#.#.....#.#...#.......#...#.#.#.....#.#...#.#.#...............................#...#.#...#...#...#.#.#.....#.#.....#...#.#.#.....#.......#...#
#.#.###.#.#####.#.###.###.#.#.#.#.###.#.###.#.###############.#.#.#.#.#####.#.#####.###.#####.#.#.#.#####.#.#####.###.#.#.#.###.#######.###.#
#.#...#.#.....#.#.....#...#...#.#...#.#.....#.#.....#.................................#...#...#...#...#...................#...#...#...#...#.#
#.###.#.#####.###.#.#.#.#####.#.#.#.#.#######.#.###.#.#######.###.#.#.#.#.###.#.#########.#.#.#.#.#.###.#.#####.#.###.#.#.###.#.#.###.###.#.#
#.#...............#.#...#...#.#...#.........#.....#.#.#.......#...#.#.#.#.#...#.#.....#...#.#.....#.....#.....#.#.....#.#.#...#.#...#...#...#
#.#.#####.#.#####.#.#####.#.#.###.#########.#.#####.#.#.#######.###.#.###.#.###.#.###.#.#.#.#.#########.#.###.#########.#.#####.###.###.###.#
#.#.#.....#.....#.#.#.....#.....#.....#...#.#.#.....#.#.#.......#.#.#.....#.#...#.#...#.#.#.#.........#...#.#.........#.#.........#...#...#.#
#.#.#.#####.#####.#.###.#######.#####.###.#.###.#####.#.#####.###.#.#######.#.#.#.#.#.#.###.#.#######.#.###.###.#.###.#.#########.###.#.#.#.#
#.#.......#...#...#...#...#...#.....#...#.#.#...#.#.....#.....#.....#...#...#.#...#.#.#.....#.#.......#.#.......#...#.#.....#.......#.#.....#
#.#######.###.#.#####.#.###.#.#########.#.#.#.###.#.#####.###.#######.#.#.#########.#.###.###.#.#######.#.#####.###.#.#####.#.###.###.#.#.#.#
#.....#...#...#...#.#.#.#...#.#.........#.#...#...#...#...#.#.#.......#...#.......#.#.#.......#.........#.........#.#...#.#.#...#.....#.#.#.#
#####.###.#.#.###.#.#.###.###.#.#########.#####.#####.#.#.#.#.#.#############.###.#.#.###########.#######.#######.#.#.#.#.#.###.#######.#.#.#
#...#...#.#.#.#...#.......#.#.#.#.......#.......#.....#.#.#...#.............#.#...#.#...#.......#.........#...#...#...#...#...#...#.....#.#.#
#.#.###.#.#.#.#.###########.#.#.###.###.#.#.#####.#####.#.#####.#.#########.#.#.###.###.#.###.#.###########.###.#####.###.#.#####.#.#####.#.#
#.#.#.#.#.#.#.#.....#.....#...#...#.#.....#.#.....#.#...#.....#.#.#.......#.#.#...#.#.....#...#.#.....#.....#...#.......#.#.................#
#.#.#.#.###.#.#####.#.###.#.#####.#.#######.#.#####.#.#####.#.#.#.#####.###.#.###.#.#########.#.#.###.#.###.#.###.#####.#.###.###.###.#.###.#
#.....#.#...#.....#...#...#...#...#.#.....#.#...#.#...#.....#...#.#.....#...#.#...#...#.......#...#...#...#.#.#...#.....#.#.#.........#...#.#
#.#####.#.#######.#####.#####.#.###.#.###.#####.#.#.#####.#.#####.#.#####.###.#.#####.#.###.#######.#######.#.###.#.#####.#.###############.#
#.#.....#...#...#...#...#.....#.#...#.#.#.......#.#.......#.....#.#.....#.#...#.......#.#...#.......#.....#.#.....#.....#.......#.........#.#
#.#.#######.#.#.###.#.#######.#.#.###.#.#########.#############.#.#.###.#.#.###.#########.#.#.#########.#.#.#####.#####.#####.###.#######.#.#
#...#.....#...#...#.#.......#...#...#...#.........#.......#...#...#...#.#.#...#.#...#...#.#.#.#.....#...#.#.....#.....#.....#.#...#.....#...#
#.###.#.#.#.#####.#########.###.###.###.###.###.#.#.###.#.#.#.#########.#.#.#.###.#.#.#.#.###.#.###.#.###.#.#.#######.#####.#.#.###.#.#####.#
#.#.....#.#.#...#...#.......#.............#.#.#.#.#.#...#...#.#.........#.#.#.....#.#.#.#.#.....#.....#.#.#.#...........#...#.#...#.#.#...#.#
#.#.#.###.###.#.###.#.#########.#####.###.#.#.#.#.#.#.#########.#.#####.#.#########.#.#.#.#.#######.###.#.###########.###.###.###.#.###.#.#.#
#...#...#.....#.....#.#.........#.....#...#.....#.#.#.#.....#...#.#...#.#.#.......#...#.#...#.#...#...#...#.............#.#...#...#.....#.#.#
#####.#.#######.#####.#.#.#############.#####.#.###.#.#.###.#.###.#.#.###.#.#####.#####.#.###.#.#.#.#.#.###.###########.#.###.#.###.#####.#.#
#.#...#.......#.#.....#.#.#...#.............#.#.#...#.....#.....#...#...#.#...#.#...#...#.#...#.#.#.#.#...#.#...#.....#.#.#.....#...#...#...#
#.#.#.###.#####.#.#####.###.#.#.###########.###.#.#############.#######.#.###.#.###.#.###.#.###.#.#.#.#.#.#.#.#.#.#####.#.#.#.###.###.#.#####
#...#...#...#...#.#.#.......#.#.#.......#.....#.....#.........#.........#...#.#.....#...#...#...#.#.#.#.....#.#.#.........#.#.#...#...#.#...#
#.###.#.###.#.###.#.#.#######.#.#.#####.#####.#.#####.#######.#.#####.#.###.#.#.#######.###.#.###.#.###.###.#.#.#.#########.#.#.#####.#.#.#.#
#.......#...#...#.#.........#...#...#.#.....#.#...#...#.#.....#.#.....#...#...#...#...#.....#...#.#.....#.....#.#.#.........#.#.......#.#.#.#
#.###.#.#.###.#.#.###.#########.###.#.#####.#.#####.###.#.#######.#######.#######.#.#.#.###.###.#.#.#####.#####.#.#.#########.#########.#.###
#.#.....#.....#.#...#.#.......#.#...#.....#.#.......#.....#.......#...#.....#...#...#.#.#...#...#.#.#.....#...#.#.#.#.........#.....#...#...#
#.#.#.#######.#.#.#.###.#.###.###.###.#####.#.#.###.#.#########.#.#.#.#.#####.#.###.#.###.#.#.###.###.#####.#.#.###.#.#####.###.###.#.###.#.#
#.#.#.....#...#.#.#.#...#...#.#...#.....#...#.........#.........#.#.#.#.#.....#.....#.#...#...#.#...........#.#...#.#.#...#.......#.#...#.#.#
#.#.#.###.#.#.#.#.#.#.#####.#.#.#######.#.###.#########.#.###.#####.#.###.#####.#####.#.###.###.#######.#########.#.###.#.#####.#.#.###.###.#
#.#.#.....#.#.#.#.#.#.......#.#.#...#...#.#.#...#.#.....#.#...#.........#...#...#...#.#.#...#...#.....#.#.....#...#.....#.#.....#.#.........#
#.#.#.###.#.#.#.#.#.#.#.#.#.#.#.#.#.#.###.#.###.#.#.#####.#####.###.###.###.#.###.#.#.#.#.###.#.#.###.###.###.#.#.#####.#.#.###.#.#########.#
#.#.......#.#.#...#.#.#.#.#.#.#...#.#.#...#.#...#...#...#.#...#...#...#...#.#.....#.#.#.#.#...#.#.#...#...#.#.#.#.........#...#.#.....#...#.#
#.#######.#.#.###.#.#.#.#.###.#####.#.#.###.#.#####.#.#.#.#.#.###.###.###.#.#######.#.#.#.#.#####.#.###.###.#.#.#####.#.#####.#.#######.#.#.#
#.#.......#.#...#.#.#...#...#.#.......#.#...#.....#.#.#...#.#...#.....#.#.#.......#...#.....#.....#...#.#.....#.....#.#.#.....#.........#.#.#
#.#.#####.#.###.#.#.#.#.###.#.#.#######.#.#.#####.#.#.###.#.###.#.###.#.#.#######.###########.#######.#.#.#########.###.#.###############.#.#
#.#...#...#.#...#.#...#.#...#...#.#.....#.#.#.....#.#.#...#...#.........#...................#.#.....#.#.#.........#...#...#.......#.......#.#
#.###.#.#.#.#####.###.###.#.#####.#.#####.#.#.#####.#.#.#####.#########.###################.#.#.#####.#.###.###.#.###.#####.#######.#######.#
#...#...#...#.....#.......#.....#.#.#.....#...#.........#...#...#.....#...#...........#.#.....#.......#...#.....#.......#...#.....#.#.....#.#
###.###.#.###.#####.###########.#.#.#.###########.#.#####.#.###.#.###.###.#.#########.#.#.#######.#####.#.###########.#.#.#.#.###.#.###.###.#
#...#...#...........#.#.....#.....#.#.#.......#...#.#.....#.....#.#.#.#...#.#.#.....#.#.....#...#.....#.#.................#.#.#.#.#...#.....#
#.#.#.#####.###.#####.#.###.#.###.#.#.###.###.#.#####.#######.###.#.#.#####.#.#.#.###.#.#####.#.#####.#####.#########.#####.#.#.#.#.#.###.###
#.#.#.#...#.#.......#.....#.....#.#.#.#...#.#.#.......#.....#...#.#.#...#.......#.....#...#...#.#...#.....#.........#...#...#.#...#.#...#...#
#.###.#.#.###.###.#.###########.#.#.#.#.###.#.#.#.#####.###.###.#.#.###.#.#########.#######.###.###.#####.#########.###.#.###.#.#######.#####
#.#...#.#.....#...#.#.........#...#.#...#...#.#.....#...#...#...#.#.#...#.........#.........#.#.#...#...#...#.....#...#.#...#.#...#...#.....#
#.#.###.#.#####.#.#.#.#######.#.###.#####.###.#.###.#.###.#######.#.#.###.#######.###########.#.#.###.#.###.#.#####.###.#####.###.#.#.#.###.#
#...#...#.....#.#...#.#.....#.#.#...#.....#...#.....#...#.........#.#.#.........#.......#...#...#.....#...#.#...........#...#...#...#.......#
#####.###.#.###.#####.#.#.###.###.###.#####.#######.###.###.#######.#.#########.#.#.###.###.#.#########.###.#########.###.#.###.#######.###.#
#.....#...#.....#.....#.#.......................#...#...#.........#.#.......#...#.#...#...#.#.......#...#...#.........#...#...#.......#.....#
#.#####.#.#######.#################.#########.#.#.###.###.#####.#.#.#######.#####.###.###.#.#.#.###.#.#.#.###.###.#####.#####.#######.#.#####
#.#.#...#...#.................#.#...#.........#.......#.#.#.#...#...#...#...#...#.......#.#.#.#...#...#.#.....#...#...#...#.........#.......#
#.#.#.#.#.###.###.###########.#.#.###.#################.#.#.#.#####.#.#.#.###.#.#########.#.#.###.#####.#######.###.#.###.#########.###.###.#
#.#.#...#.........#.....#.....#.#.#.....#...#.............#.#.#.....#.#.......#.........#.#.#...#.........#.......#.#...#...#.....#.#.....#.#
#.#.###.#.#######.#####.#.#####.#.#.###.#.#.###.###.###.#.#.#.#######.#################.#.#.###.#.#.#####.#.#######.###.###.#.###.#.#.###.#.#
#.#...#.#.....#.........#.#.....#.#.#...#.#...#...#...#.#.....#.......#.........#.......#.#...#.#.#.....#.#.#...#...#...#...#...#.#...#.#.#.#
#.#.###.#.###.###########.#.###.#.###.###.###.#######.###.#.#.#.###############.#.#######.#.#.#.###.###.#.#.#.#.#.###.#.#.#####.#.#####.#.#.#
#.#.....#.#...............#.#...#...#...#...#.#.....#.......#...........#.....#.#.#.......#.#.#.....#.#.#.#...#.#.#.......#.....#.......#...#
#.#.###.#.#.#################.###.#.#.#.###.#.#.###.#######.###########.#.###.#.#.#.###.#.###.###.#.#.#.###.#####.#.###########.#.#####.###.#
#.#...#...#.#...#...#.........#...#.#.#...#.#...#.#.#.....#.#.....#...#.#...#...#...#.#...#...#...#...#.....#.....#.....#.......#.....#...#.#
#.###.#.#####.#.#.#.#####.#.###.#.#.###.#.#.#####.#.#.###.###.###.#.#.#.###.#########.#####.#.#.#####.###.#.#.#######.###.#####.#####.###.#.#
#.#...#.......#.#.#...#...#.#...#...#.....#...#...#...#.#.....#.#...#.#...#.#.#.......#.......#.....#.......#.#.#.....#...#...#.#.....#...#.#
#.#.###.###.###.#.###.#.###.#.#######.#.#####.#.#.#####.#######.#####.###.#.#.#.###.#.#.#####.#####.#########.#.#.#####.###.#.#.###.###.#.#.#
#.#.#.....#.....#.#.....#.#.#.........#.......#.#.........#.............#.#.#.....#.#.#.#.....#.....#.......#.#...#...#.#...#.#...#...#.#.#.#
#.#.#.#.#.#.#####.#######.#.#####.#####.#.#########.###.#.#.#####.#####.#.#.#######.#.###.###.#.#.###.#####.#.###.#.#.#.###.#.#.#.#.#.#.#.#.#
#.#...#.#...#...............#...#.#...#.#.#.......#...#.#.#...#.......#.#.#.#.......#.#...#.#.#.#.......#...#.......#.......#...#.#.#...#.#.#
#.#####.#.#.###.#######.#####.#.#.#.#.###.#.#####.#####.#.#.#.#.#.###.#.#.#.#.#######.#.###.#.#.#######.#.###.###########.#####.#.#.#######.#
#.....#...#...#.#...#...#.....#...#.#.#...#.#...........#.#...#.#...#.#.#.#.#...#...#...#.#...#.........#.#.....#...#.............#.#.......#
#.#.#.#.###.#.###.#.#.###.#.#######.#.#.###.#.#.#########.#.#.#.###.#.#.#.#.#.#.#.#.#####.#.#########.###.#.#####.#.#.#########.###.#.#######
#.#.#.......#.#...#.#.....#.......#.#...#...#.#.#.......#...#.#.#...#...#.#...#...#.#.........#.......#...#...#...#.#.#...#.....#...#...#...#
#.#.###.#####.#.###.#.#########.###.#######.#.###.#####.###.###.#.###.#######.#####.#.#######.###.#.###.#####.#.###.#.#.###.###.#.#.###.###.#
#...#...........#.#.#...........#...#.......#.........#.....#...#...#.#.....#.....#.......#...#...#.#...#.....#.#.....#...#...#.#.#...#.....#
#.###.#.###.#.#.#.#.###########.#.###.#.#################.###.#####.#.#.###.#####.#.#####.#.###.###.#.#####.###.#######.#.###.#.#.###.#####.#
#.....#.....#.....#.......#.....#...#.#...#.............#...#.....#.#.#...#.....#.#.....#.#.....#...#.#...#...#.....#...#.....#.#.#.#.....#.#
#.#######.###########.###.#.#######.#.###.#.###.#.#####.#########.#.#.###.###.#.#.###.###.#######.###.#.#.#.#######.#.#########.#.#.###.#.#.#
#.#.....#.......#...#...#.#...#.........#...#.#...#...#...........#.....#...#.....#...#...#.........#...#.#.#.......#.#.#.....#...#...#.#.#.#
#.#.###.#########.#.###.#.###.#.#######.#####.#.###.#########.#.#.#########.#######.###.###.#######.#####.###.#######.#.#.###.#.###.#.#.###.#
#.#.#.#...........#.#...#.#...#...#...#...........#.....#...#.#.#...#.....#.....#.....#.#.....#...#.#...#.....#.....#.#.....#.......#.#.#...#
#.#.#.#############.#####.#.#####.#.###.#.#########.###.#.#.#.#####.#.#.#####.#.#######.###.###.#.#.#.#.#######.###.#.#############.###.#.###
#.#.#...#...#.......#.....#.#.#...#.#...#.......#.....#.#.#.#.....#...#.....#...#.....#.....#...#.#...#...#.....#...#.............#...#...#.#
###.#.###.#.#.#######.#####.#.#.#.#.#.#####.###.#.###.#.#.#.#.#.#########.#.#.###.###.###.#.#.###.#.#####.###.###.###############.###.#####.#
#...#...#.#.#...#...#.........#.#.........#...#...#...#.#.#.#.#.............#.....#.#...#.#...#...#.#...#.....#...#.......#.....#...#.......#
#.###.#.#.#.#.#.#.#.###.#####.#.#########.#.#.#####.#.#.#.#.#######.###.#.#########.###.#.#####.#.###.#.#######.###.#####.#.#.#.###.#.#####.#
#.................#...#.....#.#.....#.#...#.#.....#.#.#.#.#.........#...#.....#.......#.#.#.....#.#...#.#.........#.....#.#...#.#.#...#...#.#
#.#.#.#.#####.#.#.###.#####.#######.#.#.###.#####.#.#.#.#.#########.#######.#.#.#####.#.#.#.#####.#.###.#.#.#####.#####.###.#.#.#.#####.###.#
#...........#...#.#.........#...#...#.#...#.....#...#.#.#...#.....#.#.....#.#...#.#...#.#.#.#.....#.#.#.#.#.#...............#.#.......#.....#
#.###.#.###.#.###.#########.#.#.#.###.###.#####.#####.#.###.#####.#.#.###.#.###.#.#.#.#.#.#.#.#####.#.#.###.#.###########.###.#########.#####
#.#.........#.....#.........#.#.#.#.#.....#...#...#.#.....#.....#...#.#...#.#.....#...#.#.#.#.#.....#.#...#.#.#...#.......#.#...#.......#...#
#.###.#.#.###.###.#.#####.###.#.#.#.#.#####.#.#.#.#.#####.#####.#.###.#.###.#.###.###.#.###.#.#.#####.###.#.#.#.###.#######.###.#.#########.#
#.......#...#.......#...#.#...#.#.#...#.....#.#.#.#.....#.#...#.#...#.#.#...#...#...#.#...#.#.#.#.......#.#.....#...#.....#...#...#.#.......#
#####.#.###.#.#####.#.###.#.###.#.###.###.###.###.#.###.#.#.#.#.###.#.#.###.###.#####.###.#.#.#.#.#.###.#.#######.#####.#.###.#####.#.###.#.#
#.......#.....#.....#...#.#.#...#...#.....#.#.#...#...#.#.#.#.#.#...#.#.......#.#.....#...#.#.#.#.#...#.#...#.....#.....#...#...#.......#...#
#.#####.#######.#####.#.#.#.#.#####.#######.#.#.#####.#.#.###.#.#####.#.#.###.#.#.#####.###.###.###.#.#.###.#.#####.#######.#.###.#########.#
#.#.....#.......#.....#.#.#.#.......#...........#...#.#.#.....#.....#...#.....#.#.....#.#.#...#.....#.....#.........#.....#.#.....#.....#...#
#.#.###.#.#######.#.###.#.#.#########.###########.#.#.#.#####.#####.###.#######.#.#####.#.#.#.#######.#####.###.#.###.#.###.#.#.###.###.#.#.#
#.#.#.....#.....#.#...#.#.#.....#...#...#...#.#...#.#.#.....#.#.......#.....#.....#...#.#.#.#.........#.....#.#.#...#.#.....#...#...#.#...#.#
#.#.#.#.###.#####.#.#.#.#.#####.#.#.#####.#.#.#.###.#.###.###.#.###.#.#####.#######.#.#.#.#.###.#####.#.###.#.#.#.#.#.###.#.###.#.###.#####.#
#.#.#.#.....#...#...#.#.#.....#...#.......#.#.#.#.#.#.#.#.....#.#.......#.#.#.....#.#.....#.#.......#.#.#...#.#.#...#...#.#...#...#.......#.#
#.#.#.#.#####.#.#.#.#.#.###.###.###########.#.#.#.#.#.#.#######.#.#####.#.#.#.###.#.#####.#.#.#####.#.#.#.###.#.#.#.#####.###.#.###.#####.#.#
#...#.#.......#.#...#.#.....#...#.........#.#.#...#.#.......#.#.#.#...#...#...#...#...#...#.#.....#...#.#.....#.#.#.....#...#...#.#.#.......#
#.###.#.#######.#####.###.#.#.#.#.#####.###.#.###.#.#######.#.#.###.#.###.#####.#.###.#.#.#.###.#.###.#.#####.#.#.#####.#.#.###.#.#.#.#.###.#
#.#...#.#...#...#...#...#.#...#.#.....#.....#.....#.......#...#.....#.#.#.#.....#...#.#.#...#...#.#.......#...#.#.....#.#.........#.#.#.....#
#.#.###.#.#.#.#.#.#.###.#.#####.#.#.#.#######.#####.#.#####.#.#######.#.#.#.#########.#.#####.#.#.#.#####.#.#.#.#####.#.###.#.#.###.#.#####.#
#.#.#...#.#.#.#.#.#.....#...#...#...#.#.......#...#.#.#.....#...#...#.#...#.....#...#.#.......#.#.........#.#.#.......#...#.#...#...#.......#
#.#.#.###.#.#.#.#.#######.###.#.#####.#########.#.###.#.#####.#.###.#.#########.#.#.#.###.#.#####.#########.#.#######.#.#.#####.#.#########.#
#...#.....#.#.#.#...#...#...#.#.....#.........#.#...#.#.#.....#.....#.#.........#.#...#.#.#.....#.#...#.....#.......#.#.#...#...#.#.....#...#
###########.#.#.###.#.#.###.#.###.###########.#.###.#.#.#.#######.###.#.#######.#.#####.#.#.###.#.#.###.###.#######.#.#.###.#.#.#.#.#.#.#.#.#
#.#.......#.#.#...#.#.#...#.#.....#.......#.#.#.......#.#.........#...#.........#.....#...#.#.....#.#.....#.......#.#.#...#.#.#...#.#.#.#.#.#
#.#.#####.#.#.###.#.#.#.#.#.#####.#.#.###.#.#.#.#.###.#.###.###.#.#.#.#######.#####.#.#.###.###.###.#.#####.#####.#.#.###.#.#.#.#####.#.#.#.#
#.......#.#.#...#.#.#.#.#.#...#.....#...#...#.#.#.#...#...#...#...#.#.#.....#.#.........#.#...........#...#...#.#...#.#...#.#.......#.#...#.#
#########.#.###.#.#.#.#.#.#.#.#.#####.#.#.###.#.#.#######.#.#.#####.#.#.#.###.#.#####.###.#####.#######.#.###.#.#####.#.###.#######.#.#####.#
#.......#...#...#.#.#.#.#.#.#.........#...#...#.#.#...#...#.#.....#.#.#.#...#...#...#.#...#...#.......#.#.#.........#.#...#...#.#...........#
#.#####.#####.###.#.#.#.###.#########.###.#.#####.#.#.#.#########.#.#.#.###.#####.#.#.#.#.#.#.#.#####.#.#.###########.#######.#.#.###.#.#####
#...#.........#...#.#.#...#.#.......#...#...#.....#.#...#.........#.#.#...#.#.....#.#.#.#.#.#.......#.#.#.........#...#.....#.#.............#
#.#.###########.###.###.#.#.#######.#.#.#####.#####.#####.#######.#.#.#.#.#.#.###.#.#.#.#.#.#######.#.#.#########.#.###.###.#.#.#.#####.###.#
#.#.#.......#.............#.....#...#.....#.......#.#.....#...#...#.#.#...........#.#.#.#.#.#...#...#...#.#.......#.#...#.......#.........#.#
###.#.#####.###.#.###.###.###.#.#.###.#.###.#####.#.#.###.#.#.#.###.#.#.#######.###.#.###.#.###.#.#######.#.#######.#.#.#######.#######.#.#.#
#...#.....#...#.....#.#...#...#.#.....#...#...#...#.#.#...#.#.#...#.#.#...#.#...#...#...#.#.....#.....#.......................#...#.....#.#.#
#.#######.###.#.###.#.#.#.#.#.#.#.###.#.#.###.#.###.#.#.###.#.###.#.#.###.#.#.#########.#.###.#####.#.###.#.#.###.#.#########.#.#.#.###.#.#.#
#.......#.#.#...#...#...#...........................#.#.#...#.#...#.#...#...#.....#...#.#.....#.....#...#.#.#...................#.#.#...#...#
#.#.###.#.#.###.#.#.#########.#.###.#.#.###.#.#########.#.#.###.###.#######.#####.#.#.#.#.#####.#######.#.#.###.#.#########.#.#.###.#.#.###.#
#.#.#.#...#...#.#.#.......................#.#.#.........#.#.#...#.#.#.....#.#...#...#...#...#...#.......#.#.....#...#.......#.#.#...#.....#.#
#.#.#.#####.#.#.###.###########.#########.#.#.#####.#####.#.#.###.#.#.#.###.#.#.#############.###.#######.#########.#.#######.#.#.#####.#.#.#
#S..........#.#...........................#.........#.....#.......#...#.......#...............#...................#...........#.........#...#
#############################################################################################################################################";
}
