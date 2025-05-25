namespace AdventOfCode._2024;

public sealed class Day15
{
    private static readonly Vector2D Up = new(-1, 0);
    private static readonly Vector2D Right = new(0, 1);
    private static readonly Vector2D Down = new(1, 0);
    private static readonly Vector2D Left = new(0, -1);

    public object Solve1(string input)
    {
        (Grid2D<char> grid, string moves) = Parse(input, false);

        var position = GetStartPosition(grid);

        foreach(var move in moves)
            position = TryMove(grid, position, move);

        return CalculateResult(grid, 'O');
    }

    public object Solve2(string input)
    {
        (Grid2D<char> grid, string moves) = Parse(input, true);

        var position = GetStartPosition(grid);

        foreach (var move in moves)
        {
            if (CanMove(grid, position, move))
                position = DoMove(grid, position, move);
        }

        return CalculateResult(grid, '[');
    }

    private static bool CanMove(Grid2D<char> grid, Cell2D position, char move)
    {
        if (!TryGetNewPosition(grid, position, move, out var newPosition))
            return false;

        var newValue = grid[newPosition];

        return (newValue, move) switch
        {
            ('.', _) => true,
            ('#', _) => false,
            ('[', '>') =>
                CanMove(grid, newPosition.Add(Right), move),
            ('[', '^') or ('[', 'v') => 
                CanMove(grid, newPosition.Add(Right), move) && 
                CanMove(grid, newPosition, move),
            (']', '<') => 
                CanMove(grid, newPosition.Add(Left), move),
            (']', '^') or (']', 'v') => 
                CanMove(grid, newPosition.Add(Left), move) && 
                CanMove(grid, newPosition, move),
            _ => throw new ArgumentException()
        };
    }

    private static Cell2D DoMove(Grid2D<char> grid, Cell2D position, char move)
    {
        if (!TryGetNewPosition(grid, position, move, out var newPosition))
            throw new ArgumentException();

        var newValue = grid[newPosition];

        switch (newValue)
        {
            case ']':
                if (move == '<')
                {
                    DoMove(grid, newPosition.Add(Left), move);
                    Swap(grid, newPosition.Add(Left), newPosition);
                }
                else if (move == '^' || move == 'v')
                {
                    DoMove(grid, newPosition.Add(Left), move);
                    DoMove(grid, newPosition, move);
                }
                break;
            case '[':
                if (move == '>')
                {
                    DoMove(grid, newPosition.Add(Right), move);
                    Swap(grid, newPosition.Add(Right), newPosition);
                }
                else if (move == '^' || move == 'v')
                {
                    DoMove(grid, newPosition.Add(Right), move);
                    DoMove(grid, newPosition, move);
                }
                break;
        }

        Swap(grid, position, newPosition);
        return newPosition;
    }

    private static void Swap(Grid2D<char> grid, Cell2D position, Cell2D newPosition)
    {
        var temp = grid[newPosition];
        grid[newPosition] = grid[position];
        grid[position] = temp;
    }

    private static Cell2D TryMove(Grid2D<char> grid, Cell2D position, char move)
    {
        if( !TryGetNewPosition(grid, position, move, out var newPosition) )
            return position;

        if (grid[newPosition] == 'O')
            TryMove(grid, newPosition, move);

        if (grid[newPosition] == '.')
        {
            Swap(grid, position, newPosition);
            return newPosition;
        }

        return position;
    }

    private static bool TryGetNewPosition(Grid2D<char> grid, Cell2D position, char move, out Cell2D newPosition)
    {
        newPosition = move switch
        {
            '^' => position.Add(Up),
            '>' => position.Add(Right),
            'v' => position.Add(Down),
            '<' => position.Add(Left),
            _ => throw new ArgumentException(nameof(move))
        };

        return 
            newPosition.Y >= 0 &&
            newPosition.Y < grid.Rows &&
            newPosition.X >= 0 &&
            newPosition.X < grid.Columns;
    }

    private static Cell2D GetStartPosition(Grid2D<char> grid)
    {
        for (int y = 0; y < grid.Rows; y++)
            for (int x = 0; x < grid.Columns; x++)
                if (grid[y, x] == '@')
                    return new Cell2D(y, x);

        throw new ArgumentException();
    }

    private static int CalculateResult(Grid2D<char> grid, char value)
    {
        var result = 0;

        for (int y = 0; y < grid.Rows; y++)
            for (int x = 0; x < grid.Columns; x++)
                if (grid[y, x] == value)
                    result += 100 * y + x;

        return result;
    }

    private static (Grid2D<char>, string) Parse(string input, bool doubleSize)
    {
        var delimiter = Environment.NewLine + Environment.NewLine;
        var index = input.IndexOf(delimiter);
        var data = input[..index];
        if (doubleSize)
        {
            data = data
                .Replace("#", "##")
                .Replace("O", "[]")
                .Replace(".", "..")
                .Replace("@", "@.");
        }

        var grid = new Grid2D<char>(data);
        var moves = input[(index + delimiter.Length)..]
            .Replace(Environment.NewLine, string.Empty);

        return (grid, moves);
    }

    [Fact]
    public void Test1() => Assert.Equal(1429911, Solve1(Input));

    [Fact]
    public void Test2() => Assert.Equal(1453087, Solve2(Input));

    private const string Input = @"##################################################
#OOO#..O.O#O#...#.O.OO...............OO.....O....#
#O#O...#...#...O............OO.#...OO..O..OO...O.#
#..O....O#.O..O.O..O#O##.OO..O..........#O....OO.#
#...O#.OOO.O.O.O......O.OO.O..#.O.OO#O.#OO.O.O#..#
#OO..O....O....#....OO#OO..O..OO..O.....#O.O..O.O#
#O.....#.O.O...#..OO.......OO....O.#O.O..#..O..O.#
#..OO.O.#O.O.O..##O..O#OO....#.O..O...O.OO..#OOO##
#....OO...OO..O..O.#.OOO..O..#O....OO#O..O..#.O..#
#....OO.#...O#..O...#.....O......#.....#.....#.O.#
#..O.....O.O.O..O....OO#.O.#.O......#..#O.O..OO..#
#.#..OO....O........O.#....OO...#.O.OO...O.......#
#OO...OO.OO...#.OOO........O#...O.#.OO....OO...OO#
#.O..O..OO.OOOO....#..O.#OO.O..O#.......O.#....###
#O...O.......OO....#......OO.O.#O.O..O.O.O.#..O.O#
#OO#OO....#O...O#....#...OOO.O.O..O..#...O.O#O..O#
#....OOO#OO.......#.OO....#...O..#.O...#.O.......#
#OOOO....O...O.OOOO.....OO.......O#..O.OO.....O.O#
#..#..O.#.O..OO.OOOO...OO#...OOO...O.##.O#.O..OOO#
#O......O..O#...#....O...O.OO....O.O....OO..#....#
#.....OOO..O.O......O...#...O...........#.OO..#O.#
#...#.....O.O..#O#....O.#O#..O#....O..O....#OOO###
####O.....O...O.#.......O.O.O...O.....O.O...O...O#
#O...O...O.OO.O.....O.O......O.##.#O#.O.....O..OO#
#...#..#...#...OO..#OO.O@.O......O............OO.#
#O........OOO.O.O.O..#..#O...O.O..#.OO...#.OO..#.#
#.O.#..O...O...OO...#..OOO......OO.#O#......O.OO.#
#.O...OO#O...O..O......O..#O....O.....O..O..O.O..#
#.O..O...O..#..OO.....O..O.........O#...#O#..OO.##
#.....O..#..##........O.#..#...O..##..OO.....O...#
#.O..O#...#.....O.O#....OOO.#..#.OO..O..OO.OOO...#
#.#OO..O...O.....O.O#.....##..O.O#.O.O....O....#.#
#....#..#.....#O.#..O..#......O..O.....OO#..O#.OO#
#....O.O...#OO...#O.O..O..........OO..#O....O..O##
#O...#..#.....O.......O.O.O....O.O.O..O.O.#...####
#.OO........#.#..........#..O....OO#..#O.O.....O.#
#.#..O.O....OO....#O.......#.O..O.#O.O..OO#......#
#O...O.....O.OO#...#O.O.O.....O..OOO..O##.O...O..#
#O...OO.#O#.O..O..O...O.O......O..##............##
#O.O.#...........OO.....OO...O.O......#O.OO....OO#
#...OO...OO#.O..O.O...O..OO......O.......O.O.OO..#
#.#....O..O..O...O.......#...O.#O..O..O.O.O......#
#O....O......O#.O......##.....#.....O.O.O#O......#
#.OO.OO..O......OOOO..O#.O...O.O......#.....OO..O#
#.O......O.O.OO.O..#..O....O.....#....O.#.OO#...O#
#.O..#....#...O......#.O.....OO.O.....O.....O....#
#.#..OO.....#.....#O.O.#O......O......OO....OOO.O#
#..OO.O....O...O.O......O.O.OO...#.....O.......O.#
#..O..O...#.O.O#........OO....O...O.OO...#.......#
##################################################

^>v<v><>^<>v^<vvv<v>v<><>^^v>^^<<^<v^v>v<>v^<^<^v^v^>>v<><>^<^^^^<v^><v<^^>v>vv>v^><<vv>>^><v<^<^v^v>v>>^><>^^vv^<<<>v>v>>vvvv^<^><v>^^><>v>^v^^<>>^<^<v<v^^<v><v><<>>>v^<<>>^^^<^>><v>>v>^>>v^^v^^^>>^v<vv^>>vvv<<>vv<^>>>^^^v<<<^^<v^^<><v>v<v<vvv^>><><>>>^v>^vv>^><^>>vv^^<^<v^<>>v^^vv^v^^v<v^<v^<><>>>>v><<vv<><^vv^vv><^^^vv><v^<^>><<<v^v<>v><>>v^^<^^>><>^vvv<<><<^v<<^>^v^^v><^<^vvv>v<^^vv^<><vvv<><>>>>v<^^v^<vv<v>^v>vv^v>><<v^><<><<>^v>>><^<^<vv>^^^v><^>vv>>v<^<<v>vv<<>>^>v<>^^<^<><^<>vvv><>><<v^^>>>v^>^<<^^>v^>>>vv<^v><>v<<^>>><<><v^><<<<>^>^^v^<>^>v>v><<<>><<>>vv>v><<^><<><vv>vv<v^v^>vv<<v>v^v^v^v<<^v^v><>v>v<^v^>><>><^<>v^^^^v>^^<^v><v<>^<<>^><^^^><>vvv>>>><<vv^v^<v^>vvv^<^^v>><<<v^v^<vv>v^^^^>v<<^<>v<^>^^>^<<v<^^^^>v>>^^<>v>v<<^<><v^^v<v>v>^<^>><<>v>>v>v>^v<<^<<^<vv<>>>vv>^<>v<v^^v<<vv^v>>^v^v>^>^>^>vv><^<<^<><<^v<v<v<v^^vv<<v^>v>v<^>v<vvv^<>^><vv>^v<^>>vvv>v><v>^<>^<^^>><<^><v^>>^v>>><v<^v^<<<<vvvv>^<<<>^>>>v<><><v<v^v>><<^>><v>v^<vv^^<>>^>><<<<^^>v>^^<<v^^>vvv^><v>v^<<>^<<<^<>>^<><
<<^^^v<v^>^v^>v<^>vv^v<>^<v^<<>>^vv>v><^>^>^<<vv><>v>>v^<^<v<^vvv>>v^><>>>v<>>v<><v>><^^<^^v<<<<v<^^<v^>^^^vv>^v<v<>v<^><v<^^>>v^>v^><^><<>>^vvv>>v<>v<^<^>>v<^<^^<>^v^<^>^^>^v<><^><<>^>v>v>v^vv^^<^v^^<>><<v>v<^>>>>v^v<^>><v<vvv>v>v<<^vvv>>^>>^^>><^>vvv>><^v>>v<<<>v<>v>^<^>vv^^<<<<<<<vv^^^<><^<<vv<<^v><vv>>>><>v<>^>>^^v><^v>v<<vv<^>v>^><^>^v<v>^<v<v^<^^v<^<>>^v^<^<<<^^<<<vv><<<v<^><vv>^>v^<^<>^^^vv><>^^<^v<>vv^>^^^^^<vv<vvv>^^v>^><><v^<^<<><<^v^vvvv>>^v>>^<vv^v^<>^<v<>v<^v>vvvvv>>>vvv<v<<<>v<<><<^^^^v<<vv><<^^>><>>>vv>v<^<v>>v^><v<>^<<<v<<^<^<<^>>^<><^v^v^>^^^^v>v>^v<>^>><^>v><><^<^v>^><<>^^v^v>><><v<^^^>v^vv^v>^^^<>^v<<<^>v^^v>^vv<<vv^^<>vvv<>v><^v>^<><^^>v<<^<>v^^<<>>^v<^>vv^<v<>^v<^v><v>>v^<^v><v^vv>><<v<<vv>><<v^v^^<^>^>>>>>>^<vv><>vvv<<<^>>v<>vv<><>^><<v<v>^<<>^>>^<<><>v<^<^<vv>><vv<^^>^><<v>>^<>><^<^v><<><vv>^><vvvv<v<v<v>>^<v^<<>>^v<><>v^>^^>^>v>vv<^^^>v>^>^vv<^vv^v<<>>>^v^>>>vv^^v<^v^<>v<^<>>v^<^v^^<vv<^<^>>><><<>v<^<<<>>>>><<<>^v<<^^^^^^^>v^^^^^^<^>^>^<>^<>^^v^^<<>>^<><^<^<>^^<
vv^>^><<<<^v^^<>>><^v^vv^^>^<^<v<^>>^><vv><^<><<v>v^>v<v<>>^>v^<><^vv<><>>^^v^<^v<<^>>^^^>v>v>><>>v><<^>v^^v<v^>>v<v><>v<vv<v^^^v^>>>^^<^^<^v<^v>^><^<v^^v>v^^<<<^>^^<v<<^^><v>>^>^<^^^<>^v<v<>^<>>^>vvvv^^^^v>^v<>^<<<<>^>>^^^^>^^v><v>>>v^v>>><^v^vvvv^v>>^^v^^<vv<<^^<^v<<>^<<vv<v^<>^vv>^><<<>v<v>v^<v<>v>^<><v<<^v<^<>v^<v^^^v<<^<<vv<>vv^<v<>^^^<^<>^vv>^v><^<<^^^vvvv^v<^>vv>>^v^vvvv>><^>>^^<v<<^^<vv<v^<>^<><<v>>vv^v<^v<^<v^><>v<<^v<v>v^>>v<v^^<>>>v^vv^<><>vvvv<>v^<^v^>>vv^v^>v<<^vv>^v^^v<<<v><<^vv><<^>>^^v<v>v<>v>^^>>^^><v>>v^<>vvv^>v<^<>v^><><<^<v^^<v^^^^v<v><<v>v>>>v^^<><v^<>^<>>^<>vv>><^><v^^^<v^^>v<^<v<^<v^vv>^<v^v^>>>v<^vvv>><<>^^>^<^v^^>><>v^^>><^><<vv>>v<^>vv>>v>><v<^><vvv^<^^^v>^^<^v><>^v<^vv^^>v><^v>^>><<<<>^v>><><^v^v>><>v>>><>^^^^<^^v^<<v<>v>v<^^^v<^<<>v<><><vv><v><^^>>^>^v>^^><><^v>^<>>^<<v<^v^v^><^^>^><<v^^v<<><^^>vv>vv^<v^<<<vv^<<>^vv<^<<<><vv><<^v^v<<v>>v^v>^<^^>>>>v^^^<>><v^<v^><><<>^v<^<v<^<<<v^v<v<v^v<^<^<v<><^<>^<<>v<<^<v^vvv>><^^^^><<<^^<<vv><^v<^v^^<^>^^<>>>^>><^^>^^>><
<<<<<>v^>^<^vv>^v^>^>^v<v<<<^v^v>vv><>>>><^><>>>^<vvv>v^<>^v^<<v^>>v^v^vvv>^<>v<>>v^<v<^vv><^>>^>v<<v^<^><>^<^>>v<^^vvv^<<<<^vv^vv^>>^><^>v^>><v><v^^>>><v<^<^v^<v^>v>><v>v^<^>vv<<^^^^^<<>vv><^<^<><^<>^<^>^v^^<v<<>vv<^v>vv<<<vvv^<^>vv><v>>><v<>>v>^^v^^<>>^v<>><>v<>^v^>>>>^^^v^^^><v^v>^^<<<^<<^>^><^><<^vv^^v>v><>^<><^>^<<>>^<v^vv^^vv<^<^^>v>v>^vv<^v>>^>>v>><vv><^<<<<vv>vv>>^^^><<v<<<>v<<<>>v^<<^vv>>>^>><<>^^<<<v^<<><<><<<v^^^>><^v<<<v>v^v^<<v<<v<v^vv<<^vv<v><^>^<v<<v^<v<>^^<^^v><>^v<v<^^v^<><^<v<v^<^v>>v<vv<^^^<^^>v>v^^vv<<<^<vv>^<<^<<^<^^v^v>^v^>v<>>>^v^v<<v<^vv>>^^^>>^<<><^^><v^^^<vv^^^v<<<v<^v<>^^>v<^v<^vv<v<>^vv>^^^>^v^^<>v>^^>v^vv^v><<<^<>v>vvvvvv>vv<>^>^^>>^>^<<>^v>v^<^vvv>^^^^^v<>><v><vv<<<^v>>^v<>>>v><>v>^>v<>^>>vvvv<><><v<^>>vv^^<>>><v>^^v<v<<^^^^v<<><v^<^<vv<<<^<v>><<<>v<v<<<<<>^<>><v>^^><v>^>^^^><>v^v<^v^vvvvv<^^<v>v^v>>>>v^^^<>v<>><<><^<^v^v^<v^v<>v>>v>>^vvv>>>^^>>><^v<<<^>^vv>>v>>>^><v<^>>^<<v>>v<<<^vv>^v<>^v<^><^vv>v^v^vv><vv>v<^^>v>v><><><^><v^v^<v<>>^^><<^<v>v^>v^>^v<^v<>
>v>>><>^<^v<<<>^v>v^^v>>^<<^v>><^<v^^^<<<<>^^v<v^>><^^<<<<<v^vv^v<v^^>^>vv^v^<<<^<>v^>^<<<vv^<<<<vv><^v^<^<><v<v^v^<^vv><>^>v<>v>v<<vv<^>^>>v^v<^>v>^^v>v<^^v<<<<^^^<<^<v^<>^>v<vv>><vv<^^<>^>v>v^vv<>^<v^^>vv^>>v^^<>>>><v<>vv^>vv><^^>vv^v^^>^^^^v^v^<v<><^v>v<^^<><>vvvv>>v<v><<<v<<<>^v<^v^v^^>vv^^<v><<^<^vv^^<<<<><^<<>>vv<v>^<^^^<<v^^v^^>><>v^>^>>><<<v>>>>^vv>v>vv>^<<^v^^^^>>^>v<^^>>^v>>^v<>>v<v<>>>v^><>^v<<v>>>^v<>>v>>><<>vvvv^>><><^v>^>>vv<vv^>>^<><^v><^^v><<<<>vv<<^^v<^>vv<^^v<<^><<vv^^<^vv>v<<^v^<^v<<vvv>v<><><^<<>>v^><>^^^vv^>><<vv>^v^^<>v<<vv<^^^^><vv^v>>^^v<v<^<^<v>v<<<v<v<v>>vv^>>>^>v<<vv>>>v^v^v^^v^v>vv>><>vv<<^<^<>vvvv>^^>^<v^<<v<<v^^<^>v<<^^^><^v<^<>^>><^<>vv^>^<<^>>v^<<>v<>^v^v^^<^<>>^><>>^>>^><>v>vv^><^<<^>>><vvv^<^>^^><^^vv^<v<<>v^<^>>v>^<v>v^<v^^<<<v^>>^<<>>>^>v<vv<^vvv>><<><>^v><<^^>^>v^v>vv^vv^<>vvv^<^>^<v^><>v>^>>>v^><<v^>>vv^<v^v<<v<v>v^>^>^v^v^^vv>^v^<v^><<^>v^>v<><vv^^>v<^>^v>^v<><<>^>v^<^vvv><^^>v><^><^>>v>><<v><^^><<<<^^^<^>vv^v>v^<<<>>^v^v<>^<^<v^^^<^>><v^<<v^<^^^<
v>v><>^^^><<^v^>v<<>^<<<<v>><<^>^vvv^><v<^<^<<v^v^vv^v^><>v>^^v^<^<^<v>v^><<>^>><^vvvv^^^^v^vvv^^>>^<vv^<^<v^<>^^<^<<^v>>>>v<vv^>^^>v<^><^v^^><^^^v>><>><vvv>v^<<v^vv>v<^><vv>v>v^<><>>v<^>^^v>v<v>><>v<v^^<>v>v>^v>^>^<<<^^vv<<<v^v<vv<><v>>>>v>v<>v<>>v><<><>^^>v<^v>v<^v<^^^^>^^^^v<>v^v^vv<^^^^>^^<^<<v<v>>>><^^>v<<^^^v^^^v^v>^><vv>^><<<v>^<<>v>v>v<^<>>^<<<v><^^<^^v><>^v>v^>v<v>v<v^><><<^v^v<<>^>v^v^<^v^v>v^v>>^v<^vv<^<v<v>>^vv^^^>^<^^^v^>^>>^vvv^^<vvv>vv^>^v<<<<v<v<v>v^^^vvv>^>^^^>v><vv<<<>>><><v>^>^vv>vv>^<v^>>^^<>v>v<<>^>^^v>><><>^>vv>>v>>v^<<>>^v<>vv<><><<>^v>>>^v><^<^>vv>^>><<>v>^^v>v>^>v>>v<<<^^><^v>v>><<^<<^v^>^>>v>^v>v<<><<^^v>><v<<^>v^^<v^^^^^>>>^v^<v^vvv^<<<>>>v<vv^v<^v^<^v^vvv^^<v>><<vv><<^>v<^<^vv<^>v>^v^^<<>v<<>v^<>v<^>>>vv>^vv>vv><<>^>>v<>v<v<>v<^>^<^^^v>>^>v<v^>^^>^^<>><^>^<v^><v>v<>>v^^^^>>v^v^v>v<vv^v><>v<v<^^^<^<v^^v^>vvvv>v^<>^v<v>v<v<v^>>v>v><>^><vvv>v>^^v><<>v^<<^>v><<<><<^>>v^>v>^>^v^<<<>>v^<<><^^vv>v>v^>v<^^v^<v^v>^>>>><<>v>^<>^vv^^>^>v<^v><>v<>>^v><v<>^<><v^>><>><><<
^^v^<v^><><<vv^v^>><^v<<<><>>^vv><v<^^<>><^>^^<^>>^v^>v><<v<^>>vv>>>^>^^^vv><>><^<>v>><>>^^^>><v^v<><v^^>^v><<^vv>>^v<^v>>^vvv^^<>v>>><>>^^<^vv><<<<^v<^v^<<>^<<<<><vvvv^v<<^v^<><^>>v^>>>v<^v^<>^<<v>>v<^<>v><>>v>vv^^<^<<^>>^vvv><<<^<^^v<>>v><<<<>>>v>v<<>>>>^^v>><>^^v>vvvv<><<vv^<^^^v<vv<<v^^^v<v>^<vv^<vv<<<v>><^vv>>><>^<<v>^^^^^v><vvv<<<v^vvv>^<><^v^>>>>^v<^v><><^<<<v<<vvv<^v^^<v^><v^><^>^^^<vv>>>^<>>>><<^<v>v>^>>><^<<>v<^<>>^<^v<v^><v>vvv^^v^vv><vv<>v^>^>vv>^<<^^><v>^vv>v<<>^>v<^v<^v<>^v>v>^>^<^^<v<>vvvv^<<^<<>vv^^>>><^v^v<^v<v>^v<<v<vv^<^vv>v<^>>>>><<<<<<^>><>^^^^^^<>v>v<^<^<<vv>v^^^<^v<>^v<>><^><>^<v<^<<<>><>v><>>v<><^><^^v<>^>>v>^<><vv^><vv><><^<>>^<^^^><^<^v<^<v<>vv<vv<>^v><^vv^<^><>vvv^<v>>^>vvv<^v<^v<^<^v<>^v><vv^<<>><><<>>v>^v<v>v<^>vv><v^>>v>>>^^^^v<vv<>vv^^>^^<>>^<^<^><>^v<<>^v^^v<v<^<<vvv<vvv>v<vv<<<^<<v^<>><><>^^><v<>^>>^^<><v>>^<<>vv<^v^^<<>^>v^><v^>^^>v>v<^^v<^^v><>^v<vv^>^><v<<<v<>^v>>^^v^^<>vvv^v^>^v^<><>vv^<^>^v>>^^<<^<>v>v<^^v<>v^^^<v<<^^>^>>>v<>^v><v>>vv^vv^v<v<v><^<<
^^<>vvv>v<vvv^<^<><vv<vv^>>^<v>>^>>v<^<v^<^>>v^vv^<^>^^>^vv>v>^^<><<<^^<>>><<<<><v^v><^>v>v^v>v<v^vv>^>vvv>>^>v>><<vv<v^<><v>^v<><^<vv<<>>>v<^<^<v^<v><<<v<<<vvv<<v>^<^>vv<^v<<^>><>v>v^>><v<vvv>>^<^v^>><>v^>v>vvv^>>>vv^v><<v<>><v<<^v>>^vv^<><<<<>^><^v<>^v^<^>^vv^vv<^><^<<^^>v<v^><>^>><^^<<^>>>v<>><>^<^^>vv^>^>vvv>v<<>>^v<vv>vv<<<^>v^^vv>>^<>vv^vv<<v^^<<<^<<<<>^>v<<v>>>v<>^<>>>^>v^^<<v^v<^^v^><<<<^vv<^<^<^>^>^>>v<^v^<<>>>^<vv<^>^v^><v^<<>><<><v^<^^<<vvvv>v^v^>>^vvv<<<><<<^<<^<^v^<v<v<>v><<vv^<v<v<<<><>>^^v>^<<^<>v<^<v>>^>v><><^<<<>vvv<v^v><<^<<^^>^<>^>v<<^v<v>v<<^^v<v><v^v>>vv>vv<>>>^v>^v<^^>v^><^<v^<<^<>>^v>>^<v<<>>><<^<>v<^^^>^^^v^^^vv>^<><^v<<<vvv<^>v^v^v^^>v^<>vvv<^<<vv<v<^vv^v<<<vvv^>><>^v<<>>>><<^>vv^v^<><^>><>^>>v<>v>^>>v>><vv^v><^<^>>v<<><>v>^<<>>vvvv>><<^^v<vv>v^<^<>v^v<>^><><<<^vv><v>v>^^><<^<^^>>^vv<^>>v^>>vv<>>>^>>^<<<>v<>v^<>^vv><<^>^^>><<^^v^v^>v>>>>><^v>v^^<vvv^><^>>>>^v>^>^<<^^v^<v<>^^<^v>^vvv<^^<<>^><^<^^^<>v^>^<^^<v><v>^><<v<^>^>>v<<v>>^v^>^<<vv><>^>^vv><<v><<>^<>vvv^>^
vv^^<v><<v>>v<>>^v^v>^^><<<<><>>><>>v^>>>>v^^<<<v>>^v>v^v<>><^v<>^^><>^<<^v>>>^^<^><^<<<<v^^>^<^vv<v<v>v^>>v<v<>v^^vvv<^>>>>vv>^vv>^v>>^^^^v^>^><^^v>v^>v<<<^^><^>v^<^v>>^<^vvv^<><^^v>><v<^<v^<<^vvv^^<>^<>>v^>v^<v^><<<>^^>vv^>>v^v^>^^v^<^^>>v>v><><><<v<^^v<^^<^^^>v<v^v><^><^v<v><^v^^<>^^v<vv^vv<<^>^^^>vvvvvv>^^><v>^v>>>>>v<v^>>vv>^^^<<<<>^><<<v^vv><^>v<^>><<v><>vv>^<v<^<<<><><v^<^<>^<<^>^^^v^vv<v>^^><><>vv>>v<<><>vv^^^>>>^>>>v>>vvv^<^^^<v^>vv>>^>>v^>v<v<>^vv^v>>^<v><vv^>>^<v>^><>><><<v<^v<v^<v><v>^<><><v^v>^<>v^><^^<^vv<v^v^^v><>vv<>v<^^v<<^<v^>^<^<>v^>vvv<<^v<>v^><^<>v^<>^<v<<<<v^>vvv>^v^<^^^^>^v<v^>^v>v>><v^v<><><<<v^<<<v^<>^>><><vv<>v>^vv<vv<^>>><v^vv>^vvv>^^<^v^<>v><vv<v^^v<>v>^v>>^v><vv^v<>><<<<>>v>^<vv>v^<<>><<^><><^v>><<^>v<v<^^vv<<>>v^<^>vv^^<^^>^><^><<v>v>^>>><v>^<>^<v>v^>^^<><v<^<^^^^><>><vv<>^><<<>>v<<v<<v^^v<<<^>vv><<>>>^<<v<<v^<v^<v<^<^^>>v<<<<>vv>^^^><<vv<^^v>^<^v^vv>>^^v>^>^>^^>^v><^><>^v^<<^<v<>vv<<v>v<>^<<^^^<<<v<>>^>^v><>v<^<<<<^>>^<<v<<>^^^^v<vvvvv<<^<<^>>v^<^v^><<^^>
<^v^^<<>vvv^<^v<<><v<v^^^v^^>>v^^^^<v<>vv<^^vv>^^<v><<v^^^v^^v^<<>>v^^<<^v^<<^<<>><^>>^^>><^><^>vvvv<^>>>>v<v^^<v<<<vv^><v^^v><<>>vvvvv^v><>>^<^<^vv><v<^vvvv<<v<v^<<^v<^<v^v^><<>vvvv<^>^<^><^v^vv^v<^^>>vvv^v><>^>v^<v<^^<<v<<><^><^>^v^v^<^>>vv<^vv^><v>v>>><v<<>^<^^>^>>^<>^v<>^^v^^>>^^^^><>>^>>vv>^<^^<v<v<^<^^<<<^<^^><v^<>^><v>^v^<<>^^vv<v^^>v^v^^v<<<<<>^>v<<^<<vvv>>v>>v<vvvvvvv<>>v^>>>>v<v>^v^<^^><>v><vv<>v^>v^^<>vvv<v>^>^^^^<^<>^vv><^v<^>v^<v>^v^<<<^v><vv^>><^^>^><<v>>>vvvvvv>vv<>><^<^>^v><<>>v>>v><^>><<<^<v<<<<<v^v>><><^vv^vv<<<vv<<>>v>><^vvv^v>^>^v^^^vv><><^><^<<v<<^<>><>^<><vvv><^^><<<><^vv<<<>v<<^vv>^v^v<<^^^^>>^^>>>v<v>^>v<>^^<^>^>^v^v<^<v><^<v^<<<v<<^^^>>^<>v><^v>><v^>>^<v<vv<v><><<<v><^><v<v<<<<vv^<^><^^^v><v^v>v>^vv>^^v^>v<><vv^>v<v<<v>^<vvv<v^v<<<>v<>^>^>v<<v>v^vv^^^>v^<<v>>^<vv^^<^<^^v><^^vv><vv<<^>>v><><^v^vv<v^>vv><<vv<^>>v>^vv^^<>vv>v<<>v<v>^v^v^v><v><>><<^^<>>^^<v<>v<v<<v<><<><v^>v<^><v<<vv^^v>><v^<>v>>^>>>vv^><><<v<>^v><>^><^>vv^^>>v<v<<^^<<><<><>>><^>>><><v^v>v<vvv<v>v^
v>^<^<>^>>^^>^^v<^>^vv^v>v>v<^v>^<v<v^<v^v>>>^^><vv^<^<>vv<>><v^<<>>^<^^v>>>v^<<<^v^<v^<v><<^^^v>^v^<v^v<^v<vvvvv>^<^v^<<<^>^<<v>^<>><<^^<^vvv>v>^^^>><v<><<<<v^>v^^<<^^<<><>^^>^<vv>^^>^>><<vv^v^<><>^>v><^<^<^^v<>^>v><^^v<v>^<vv^<vv^>^<<<><^<>vv^<^<<v>v<<<<>><<^^^v><>v>^>^<<vv^<<><^>v<<<<vv<v>v><<v<<<>^^v>>><<<vv<<<<v>>vv>><v<>><^<>>v^>^v^v<>v<^<>v><<^<^^v>>><>^^^vvvvvv>v<>><<^v<v>><^v><>v<^v><<>v<^^<<v^^>vv^<v<^>>^^^v><<<^<^^vvv^<><<^v^v^<>^^<<<>v<^vv^^^<vv^v>^^<<v>^v^^^^^vvv<<<v^^<<<v^>>v^><^^^v<v>v^>v<vv><vvvv<>v>^vv^v<<v>v>>^v>^vvv^>>>>^>v>>^^vv>>v^v>v^^<<^<^^><^^v^<<>^vvv<^<><^<v>v^><>>vv><v<<>v^^<>vv^<><<<v<<>vv<><<<><<v<>><^>^<^<>>v<<v>vv<>vvv>>^v^v<<>^<^>^v<<^>>>^^>v^<v^v^v>^^<<v^>v<>^v^vvv<<>>>^vv<v><^<><^<<^<><<^vvv>>><v^v>><>>^<<><^<v>><vv<<^<^>v<>>^><>v<<v^><>^>^^>v<^<vv>^v<<<^<^>v^<<v<>^vv^<^^^v^<v<vv<vv<^>^v<^v>><^>vvv^^<<<^^<<<><>^vv^>^<^^^>v^v<>^>^><v>v^>>>><<>^>>>^<><vv<<^<>v>v^v<<<<<^v<^<<^v<v<>>vvvv<<<<<v<<v^^vv^>^<>v>^v^>^^>^><<v<^^><>><<^^^>vv<<^<^^<<>^<>^v>^v<v^v<<
<vv>^<v>>>>><v^^>><<v>v^><^v^><<>^^><vv^^^><^^<^<>>^<v>vvv<>v<<><<^>vv^v^^vvv>v>>v<<<><>>v<<>v^^^<v>v>>v^<^^<>vv^<>v><v><^><><<v<^^<<>v<v^<vv<^v^>^^><^>^<v^^vv><<v<<<^^>^>^v<<v>><>^v>v><<<v>^>v^<><<<>^^><><v^v^v^v<v<^vv<<v<<>^<<v>vv<^<<^>>>v<v^>^^<^<v><^v><^v<<^^>>vvvv^v>>><^>>v^^^<>>>^^>>>>>^<>v>^v>vv><^><^v<<<v^^<<^>v^^v>><v<v^><>>^>^>^^>^vv<<>>>^^^v^^v^<vv^>>vv<vv^<><>v<>^^vv>^<vv^><<><<<<><>v>v>>>>^><>>>>vv><<^^>v^><^>v^^>>v><v^>v<>^<<^>>v<^<>^<><>><<>><vv>^<<^^^>v>^>>v<^v>^vv<<><v<>v><v<vvvv>vv<v>>v>^^>>v<v<v^<v<v>><>^v^<v><>>>^<^<^><v<>v>^<^<^<^^<<^^<^^^^^v>v^>^>^<^><^vv>vv><^<><<<<><><^^^^^><^><<>v><vv^>vv<^^<^vv>><^^v<<v^>^^<v<v<<<v^^v<^^v^v<^vv<v>^^<vv<<vv<vv<v>^<>^<v^v><vv<vvvv>^<<<<<><v>v>^v^v<^^v>v>v<<>>>>v<v<<><<^<>v>><^<^^>>><^v^^v^>>^vv>>^v^v^^<>^><v<><<<^>^>^><>^>>^v^^vv^^^^^<^vv^^^^v>>^<<^^<<^<v<>^v^^vv^^v<v<^<>v^<v<^<><<>>v>vv<><^>^v<<^^^^<<^vv^v^><>^v>v><v><v<<><^<<^<<v>v><>^v>>^><vvv<>vv><<>^>^^<>vv<>vv<><v<^<<^^>vv>vv^^<^v^v><<<<v^>v^>v^^<v^^>^>^>>>^><v^^><<<>^>vv<
>><>><<^^v>v<v^><<<<^v><^^vv>^<>v^>>vvv^<^>vv<<^v<v^>v^<>^<v<v^v^<^<>>vvv<<<>v^vv^^><v><^^>^^vv^^^><v^>^>v<<vvvv>v^<<vv^><^v^^<<>^<v^<>>^vv^v^^<^^vv<v>v>^<^>>^^^v<>v^v>>^>^^^^><<<<^>>v<<><><>><^^<vv^v<>^<v<><v^<^^^v<<vvv<^^>>v>v<<v<<<<<v^>v<vv><<<vv><v^^<^<><>v^vv<^<<<^<^>>^>^<^<^vvv^v<<v<^<<<vv^>^^v^^<^^v>>v<^<>>v^^^v^>^<^v>>v^>><>vv^^<<v>>v>>v^vv^<>>^<^>^vv<^v^>v><^^^>^v<>^>>v<^<<>^vv>>>><>^<>>>^^vv>v^^^^<v<v>v^^^>>v^<>^><>v^^<v^>^>>>>^^<v>^^>v^><^>^v<v>^>>>>>v<^><v<^v^>>v<<<^^^>^>^>>^>>v^^>v<><<v<>><<><>v^><v^vv<><^v^><^v<>v^^<>^>>><v>>^><<v>^vv><<^v^vv>^<><^^vv^>><v>>v<v>^<>^v><><<^>v<^<>><^vv<v><v<>>><>v<<<^>>>^v<>><>^><>>^^<>^>^<^><v^<^^<^><>>v>>><<<^^<^<^><^>v>><^<v>><v>^>vvv>^^<>v<<v^>v>^^>v>^vv><<<^^^v^vv<v>>><<>>^v^v^^><>>>>v<v^v><v<v<<v<>>^v^^v^^^v^v^^^^>>^>v^^v>>>>>><<>v^v^>^>>^<^><^<^<>v><<<>^vv^^>^<v^<v^<><v<^<v<<<^v<>v<>^v^<v^><^>>vvv^<^<^v<^>>^>v<^^>>v^^vv>v<<v<>vv^<<<<v>><<>^><<^v<>><>v>v^<v<<<^^v<>><><^^<^v^^^<<^^^>v^>>vv<^^v<<<<<^><^v^<v^>^^vv>v<^v<vv>v>v><^<^^v>>><^
<v^v^v<v^<>>>^v>><><>^><^v>vv<^^^>^vv<v<<>^v>^>>^>>v>^>^^^^^v<<vv>^^^<^<>^>>v^>>^>v<<>^v>^<<v^v<^^v<v>>^v<v^v>v>>^>><><vvv<^^v>^<v>>><<><v^^>^>^>^^^^^><<<>>>vv>v<^v^v^^>v>^><<vvv<^<^><^^v^^^^>^^v<^^>>^v^v^^v<><<^v<^^v^><v>>v<<^^v<<>>^v<>^<>v^>^>^>v>>^v>v^v^<><vvv<>v>v<><>v^^v>>>v<^<<v<<v^^<v>>v><^<<<<<^<<v<^<^>v^v<^^>^v>>^v>vvv>>>>^^v<v^v<>><<>vv>^<><^vv^^>>>v^<v^>^v^^v<<v^<v^<v>^v^<<^vvvv<^v^<^>^<>vv>^^>vvvvvv^>><v><^v^^<v<<^<vvv<><^>^^<^<^<vvvv<^^vv>vv^>v^><<vv<<v^v^>v<v<<v>v>v^vv^v^<^^>v^<<<>v^>^><><<^v<<>v>>^<><^<^v^^>v^^^v^<<^<>>^^>^<<<^>v<>^^^vv>^>^vv^>^vv^><<^>^v^^v^>>^v<^v<>v<>^>v^<^<v><<<v>>v>^^v<^v<^^v^>v^>v<<^^<<^^vv^>>^<^v>^>>v^^v^v^^<<v>^vvv>^<vvv^^^>><v^>>>>v^^>v<v>vv^^v<v^^><^>>v<<<v^v>^v><^>>v^^^vvvv^<^><<^^<>^>>v<vv><^vv>>><v>^<>^v^^v<^>>v<><><>>v>v<^^<vv<<v^>>vv^<<^v>^v>^^v<>>><<vv><^<v<<vv<<>><><<>vvvvvv^<^v<vv<<<v><>vv<<<>^v<^<v<v^^>>vv>^<<v^<^vv<^>v^^^>^vv><<<v^^>^<>^vv^>^<v<<^>>^>^><^>v^v<<^<v<^v>>^<<>v<^^^^>^>^^>v>v<<v>^vv<vv<^^>^<>v<v<<<^<<<v>^><>><<<>>v>^v^v>v>
>>>^vv<<^^>^>v>>>v>v>>^^><>v^><<>^<v^<^>^<><^vv^<>^<^^v>^<>^^^<><v^<^v^v>>v^v^<<<v<v<vvv^<>>^^^vv>>><<^<><<<^^^^<><v<<>>>^>^v^^^<>^>>^^>v>v>^<>><^^^<^^>v^v<^<>>><<v^vv^>^<>v^^<v^v^^^^><vv^<v<^<>v^><>vv<v>v<>^^><v>^^v^v^v><>>v>v<^>^><v<>^>>^v>>>>>v^^v>^>>v<<^vvv<^v>^^v<^>v>^v><<vv><v<<<^>^<>^v<v^<vvvv^^^^^^v>v<>>^v^^^^^^<^v^>^^^><><<<<v^^>v^^^<>>vvv<<<>v<>^^v<^<<>^^>^^v^<><^^<<v<>vv^>>^>>^>v^>>><v^vv<^^><>v>><<^<<v<>>^v^><^>v>>v^v<>>><v^>>v<><^>^^v><^^<>>>v<v<><>^^<>^><^>><v<<v<<v>^v<>^vv^>^<<><>>^v<>v<v^<>>v^v><^^<>^>v<>>><^>vv<^>vv<v^^<<^v^<v^v>^><v^^v><^v^<<<>^>v>><>v^v<^<>v<^^><>v><v>vv<v<<v<v^>v^<vv^>v^vv<><>^>^v^^<v<v<^v^<^<<^v^<><vv><>v>v^^<<>vv>><v><<vv<<^^>^<^^<>^^><>>v>>v<v<^>^<<<v<^<vv^<<vv^v<>v^><^><v^<vvv><vvvv<>>><v>>><<^v<^vv>v^<v>^<<<<^^>^>vv>^>>^<vv><>>^<v<vv^><<<<<^v<<^v^^v<^<^vv>^><^<<^><^^>><<>v<>v>^>v>>^<v^>v<<<>v<>>v<v<^><^^^^v<v><>v^v<<v>><^<><^<>^>^^>>>><>^<<>>^<v><>><^^<^^<<vvv^<<<<v>>^^v<<>^v>^<^^>v^vv>vv>><^<^>^^<<>>>v<<<>^>>^v<>^>v><vv^^v^<^>^<v>v><>^v^><<v^<
^>v>>>>>vv^^>^^^v>^^v<>v>v><^^v><><v<>v>><<v>>^>^>^^>><^>^>v><v>>v^>^>>>>v>^v>>^<<^<<>>><>><^^^>^v<v>>^>>>>vv>^><>>vv^<<<>^>^^><v><>^><<<<^v>>><v>v^<v^^>>vv<>>^v^<v^vv>v^<vv<>^>^<><>^<^<>v>>vv^v<^v^>v<>>v>><v^>^^>^^^v<><^>><vv^<<^>^^^^^>>^<<^v^^<>><^>^v<<^^>v<>^<^vvv<^^vv^v>vv>v>v<>v>^<v>>^vv<^>>^<><^^<>v^><^v<^^<^v>vv^v^>^^v><v^^v<^<v<^^^<>v^<><v<<<><^vv><^vvvv>v>>>^v>^<>^^^v^^>v^>v>>vv<<v<^v^v<vv><^^>><^v^>^>^<^><v^v>^^>^v^v<>>vv<vvv>><>^v>vvv><v<<>>vv<>>v>^<>vv>^v^^>>>>>><<v^^><><v^>><^>v>^^vv<<v<^<><>^v^>^<v>^vv^<^v^^>v^<vv^<v><>^>><>><vv^<v>>v^<>^>^v^v<>v>v<v^<^vv><<^v>>><>>^<^>>v^v^v^<^^<<^v<^<>^^>>^^vv<<<>^vvv<v^^<<v<>^^^<vvv<vv>vv>v<v><v^<<<vvv^<v^><>>v^>vv><<>>v^vv><^^>v^>v>v<^vvv<^v^>>>>>^>>>><v>><vv><v>^>>^v><^v^>><v><<^>>><>>vv<^>>^v^<>>^<<^<<v^<^<vv>vv<<vvvvv><^^<^>>v>vvv>^v>^<<v<^>^>>^>v<<vv<^<^>vv>>vvv^>v>><^>v^v>^vvv^v>><<<v^^^>^><>^><<^<^<><v^<v^>v^^<v^v>v>v^v<>><v<vv>^^<<><>v>v<<^>><v^<vvvvv<^>><v><vv><<<>^v^vv<>^<><<^^v<>><>>^<<>v<<<v^<>>v^>v<><^^v^<^v<v^v^<><<>^v>v>
^<vv><>^>vv^>vv^><^<<>^v<<v^v>v^^^v<>>v>>>><<<<v^<<v<^>^>v>v^>><<<v<<>^><v^<<<vv^<^^><><^^>^^><v<^v>^^v^>^<>vv<<<>>>^v^v>>v<^^>v^vv><^v>v><v^v<><^><^^v><^^>^>>^^>>^<<^^>v^<<<^<vvv^^vvv<>v^v^v>>>^><v>^<v^>^v>^v>v<><v^^<<^v^^<v<<<>>^<>^^^>>^^>vvv><>>^<><<<<v>v^^<^<<>v^v<^^><<^^<>^>v^<<<v>>>^>>^^>^<<<>vv^>^v<vv^<vv^<>vv<^>v^v^v>>^^^v<^>>v<>><>v><v<v>^v>^v<>^^<>>vvv><<<^^<>>v<<^v^<>^^v^^>^vv>^v^<<vvv^<<<>>^vv^^^v<<^^vv>^^v><^^^<v><<^^v>^<^<^^>v^v<^>v>><v>><>><v>>><^><vvv<v^v>v<v>^^v^^^v^^v>vv>^<><>^^<<^<vv><><v^<^<vvv^^<vv>^>vvv<v>>>>>>^>>>>><><<v^^^<^^^>><v^^>>^<^^<>vvvv><>^vv<><>><<>>v^<><>v^>>^<^>v<^><v^<>^>>v><<>><>>^^vvv><>vv>>^>^>>>v^^<<<>>v^v>^^v<>^v^<vvv><<<^^v<>>>>^v<v>^>^^v^v<>v>^v<><>v<v<>>vv<>^^^>^<v<vvv>^v<>^v^<<>v<<^v^^v><^>v>>>>v>^<<vvv><<^<v<<v^v^vv^v<<>><^v^v>>>v>^^^^v^^>^v^<<<^vv^<v<^v^>><^>v<<v><><>><vvvv<<vv^^<v<^v>^<>><v><v^>v<^v^v^^v^>^vvv^^<v<>>v^^vv<v^>><^<^>^<><<^<>>vv><>^><^v<<<<^^^v>><^>>^>><>v<>^v<vv^^><<><<v>v>^><vvv><vv><^>v>><vv>v<<<>>>>v<>vv><v<^<v<>>vvv^<vv
<<^<<<^>^>>^^^>><^<^^<>v^>^<^vv<^<>vv<><><v^<>^>vvv^<<<v>^<><^<>><v<>^>^v^>v^^<<<<>^<<^v^<>vv^^v<>>v^<>^v^^^<^v^vvvv>vvvvv<^^^^v><>>^>^v^vv>>>^^<><^<>^v<<<>v<^v^v>><>><><>vv^>v^^^<v<v>^<^v^<>^<v<>vv>^^<^v><v<^<v>v<^^^>^><vv^<<v><<^>>v^^>><>v^vv><^<<><^><<^v><><v^<><v<>v>v>^>v<vvv>>>v<<><^^>^v<v>^<><>^>vv<<^>v>v^>v><^<v<<<<^^v>^v>^>>v^<v^<<<^<<^^^>^v<<v<^vv>>^><<>>><<<v>>>vv>>v><v>v>^^>>^>><>v>>^v<>>><><<<<>^>^>v<^<v>>v<v>v^<^<><<^<>>^^<^^v^vvv>>^v>v<^^>^v><<^v>^^^^^<v<>>v<v^^><^vvv^<>v>>^^>v^^v>v^^<<>vv^v>^^v<vvvv<>vvv>>vv>v>>^>>^^>v<>v^<<^>v^^><>v<><^>>^^v>^<^<>^<<v><>^^^^^v^>^<^^vv^v>>>>vvv^>v>vv^^^vv><v>vv<^^>v<>^v>>^^^v>^^^<>>v<^>^<<^>^>><>><^<>^^^vv^<^<^<<^>^<>><^vv<^<<><<<><^<>><^v^>>^v<^^><>v<^^^v^^>>^^vv>^<<vv>>>v>>>v^vvv>^^v^^>^>^^<>>>^<><>^^>^^v>^><^><v<>^>>^>>^<^>^<^<^vv>^>v><v^^>^>v<>>vv><<>><<><<>v<>>v><v>^v<^<<><^v<^^^><>v>>vv<^<<<^v^<vvvv^v<^<>^^^^vvv^<^^<>v<^^^>><^<v^><v^v<<^^><>^^^>^>^vv^^>>v>>^vv<<<v^v<vv><>vv^><<^v<v>^<v<>^^^<v<v^v<^>v^vv>v^vv><vv^v>>^^vvvvv>^<^vv><^
>^<v^v>v>>vv^^>v>^^^^<>v><<vv><<^^v<><<v^^^^>vv>>^>v^v<<^>vvv<<^v<>v><>^v<^vv>>^^>>v<>v>^^^<v<^>>><<^v^^<v^vv<<<^^>>>vv<vv<v>><>^>^<v<<><<vv<vv>vvv><v>^<vv<><^v><>v>^^v>v<vv><v^^v^<^<v<>vv>><<<><<^v><>^>^v>v^^>vv>vv<>v<<^<v^^^>^>>^^v<^^^><v><<v^<>><><vvvv^v^^v^^<^v^>><^>v^><^^<>v<^v<<>>><^<<<^^<^v>v^^<><v<^^<>vvv^><v^v<<><v><v>^<^<^<v<^><<<>>vv<^<vv<<v>vv^^<v^^<<>v^vvv><>v>>^<^v<v><<v<<v<^^vvv>>^<^<^>>^vv>^<>^^<^>v<v>^>v^><><>vv<<>v<^^<>^^><^^^v^vv><v^^^^><^>vv<^><^^v^>>>vv^^>>v>^vv<<<<v^<v^>>>vv^v>^><><>>>^^vv^v^>v<^v>^<<^>>vvvv<<v<^v<^<>^<^>><>>^>^>^>^<<v>v^>>v>^<v>^vv<^><^^v^<<><^v^^^>v^<<>v<vv<vv>v>><>v<^<v^>v<^v>^<<vv^>vvv<><<<^<>>v>^vv<^v>^v><>^<v^v<vvvv^<<^v^v<<^^v^^^>^<v^><^^<>^^<vvv^^^^>><^>>v<>^>v>^v^v^<^^^v<vv<v<><vv<<<vv>v>^><<>vv<<>v>^vv<><^<^^^>^>v^v>^v^<v>^v>^^<^^<>>^<>>><><<<^<<<vvv>^<^<<<^vvv^v<<>><v^v<>^>>^v<^v^v^vvv<v<><v>^<^v<<<^<>>^^>^>>^^>^>v^^<<vv<vv>><^vvv^<<<v>^<><v><<v><<^^<<>><<v^vvv>vv><>><<^<v><<^>>^^vv<vvvv>^>>>>vv<^<<<>v<<<>>>>vv>^^^>^<>>^^<v>v><><^><^>>>
v><vv^><>>v<v^>^^vv<vv^^<vvvv^^^v^^^vv^><>>^^vv^>v^<>vvv>vv<>^v>>v<^<>>^v><^<<v<v>>v>v^<<>^<v>v>^^><^^>>vv<v>^v^<><^<>v<^><^v>vv<^vv^<<vv<<<<v<>v<>v^v^>>^>^<^>>^v<^v<v^<<<<>vv>><>vv>^<vvvv<<<<<^<^v<>v<^v<^><^^^>v^^>>><<<>><<v<<>>^vv^<>^<><v<^<v>^^v^^<<>^<^><v>v^^>v<v^vvv>v<<><<^<<<v<<^^><<v<>v<^v^<>v<>vvv<^^vv><<>><>v><>>^>>v<^vv^v<<^v^^>v>v><v^v>vv^^v>><<<^vvvv>^^^<v<<<<>^<<<><^<^>^v^<^v<<vv<<v<<>^<v^^>v^^><^<<^<v>^>v^vv<><^>^vv^v<<<>^vvv<<>>^vv^vvv<vv<^<^v>^^>><>v^^<^v^^^>^>v^<><><>>><v>^v>v^<>>>vv<><^>v>^v<<<^>v^<>><^v<>^><^^v><v^<vv><^^><>^^>vv^vv<<^<v^v<<v<>^v<^<v>^><>v^>v^<^^<^v>^^^>v^vv>^>v^<^^><^<<<>v>^><^>>vvv^^>>>>^>^<^><>^^v<<><v>v^>^<v^>^^<><vv>><>vv^^^v>>v^^v<v<v^<<><^v<<^^<<>v>v>>v<><>^>^^<v>^>vvv<><<>><<v<<<^>^<^v<<<^<>^^>>v^><v<<<<><<>>^^^^^^vvv<><<v<vvv>^^>^v>>>^^>>>^<<<>^vv<^><<<<>^v^>>>v<>>v>><<^<vv><^^<><><v><>^^<vv^<<vvvvvv>vv^^v^v<>>^v<<^v^^>^vv^v>vv<><vv^<v^^>v^^<<<>>vvv^><>v^<v>^<^><><>>v>^><v><^<^^>v>vv<^v^^>^>^v^^>v<vv>v><v^v>>>>vvv^^v^>v>>^^<<<v><<v>v>v<<>^^v";
}
