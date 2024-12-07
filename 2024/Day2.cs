﻿using Xunit;

namespace AdventOfCode._2024;

public sealed class Day2
{
    public int Solve1(string input)
    {
        var items = Parse(input);

        return items.Count(IsValid);
    }

    public int Solve2(string input)
    {
        var items = Parse(input);

        return items.Count(IsValid2);
    }

    private static bool IsValid(List<int> items)
    {
        bool isIncreasing = true;
        bool isDecreasing = true;

        for (int i = 0; i < items.Count - 1; i++)
        {
            isIncreasing &= items[i + 1] > items[i];
            isDecreasing &= items[i + 1] < items[i];
            var diff = Math.Abs(items[i + 1] - items[i]);

            if (!isIncreasing && !isDecreasing)
                return false;

            if (diff < 1 || diff > 3)
                return false;
        }

        return true;
    }

    private static bool IsValid2(List<int> items)
    {
        for (int i = 0; i < items.Count; i++)
        {
            var copy = new List<int>(items);
            copy.RemoveAt(i);
            if (IsValid(copy))
                return true;
        }

        return false;
    }

    private static List<List<int>> Parse(string input) =>
        input
            .Split(Environment.NewLine)
            .Select(x => Utils.ParseList<int>(x, ' '))
            .ToList();

    [Fact]
    public void Test1() => Assert.Equal(379, Solve1(Input));

    [Fact]
    public void Test2() => Assert.Equal(430, Solve2(Input));

    private const string Input = @"24 25 28 31 28
41 44 45 48 49 50 50
5 8 10 13 15 16 17 21
11 13 16 17 19 26
79 81 78 79 82 84
16 19 20 18 20 22 25 22
84 87 90 92 94 97 96 96
86 87 88 91 88 91 95
40 43 41 44 49
8 10 10 11 13
91 94 95 95 92
18 19 20 20 21 23 25 25
15 17 19 19 23
35 37 37 39 40 43 50
2 5 6 10 12
82 85 87 88 91 93 97 94
30 32 35 38 42 43 43
64 66 68 72 76
73 74 77 80 84 86 91
64 67 72 73 74 76
72 73 75 81 82 81
24 25 32 35 37 38 38
59 60 62 64 65 70 74
44 46 47 48 50 52 58 63
54 52 53 56 57 58
77 76 77 80 82 81
48 45 48 49 52 54 54
74 71 74 76 78 81 85
20 18 19 21 27
88 86 88 91 92 89 92
17 16 14 17 18 15
95 92 91 93 95 96 96
7 5 6 9 8 11 14 18
15 12 9 12 14 16 21
3 2 2 5 6
58 55 55 57 58 57
36 35 36 36 36
73 72 72 75 79
80 77 78 78 79 82 87
20 19 23 25 27 28 31 32
92 90 91 95 97 96
86 84 86 90 93 96 98 98
36 33 34 38 41 45
82 81 85 88 89 92 99
70 69 71 77 78 81
51 49 50 57 60 58
28 27 30 33 35 40 40
40 37 38 45 49
53 52 54 55 56 61 67
27 27 29 31 34
82 82 84 86 89 92 89
91 91 93 96 96
55 55 58 60 64
80 80 81 82 83 85 88 95
84 84 87 84 85
81 81 80 82 79
25 25 22 24 24
71 71 74 76 77 74 78
86 86 87 86 87 88 93
41 41 43 43 46
70 70 70 72 71
12 12 13 13 14 15 15
23 23 23 26 28 31 35
84 84 86 88 88 89 91 98
4 4 5 7 8 9 13 14
31 31 35 36 37 36
19 19 23 26 26
80 80 84 85 88 90 94
51 51 53 54 58 60 61 66
35 35 36 42 43
9 9 12 15 18 24 21
2 2 8 11 14 15 15
25 25 30 33 36 38 39 43
13 13 18 19 26
10 14 15 17 19
35 39 40 43 45 46 48 45
41 45 47 50 50
16 20 23 25 28 31 34 38
10 14 15 18 20 25
42 46 49 46 48
25 29 32 31 34 31
7 11 14 11 14 14
90 94 92 94 98
27 31 34 32 35 37 44
50 54 54 56 59 61 63
48 52 52 53 50
84 88 89 90 91 91 91
48 52 54 54 56 60
85 89 89 91 98
9 13 15 17 21 23
43 47 49 52 53 57 55
76 80 82 83 86 90 93 93
5 9 11 15 16 20
53 57 61 62 65 66 73
65 69 70 73 76 81 82 83
15 19 21 27 24
51 55 57 63 65 65
15 19 22 25 27 33 37
1 5 6 7 9 11 16 22
50 56 58 60 63 65 68 70
36 42 43 44 41
36 42 44 47 50 52 54 54
79 84 87 88 90 92 94 98
35 42 45 46 47 49 52 58
74 80 83 80 81 82 84 85
60 67 69 72 73 71 72 71
85 92 94 97 96 98 98
17 22 25 27 25 28 32
22 28 26 28 31 33 40
51 57 60 61 61 62 63
7 13 16 16 19 18
80 87 87 89 89
70 76 79 81 84 84 88
39 45 46 49 49 51 52 58
38 43 45 48 52 55 57 58
79 84 86 89 93 94 92
39 45 48 52 52
58 63 67 69 70 74
76 83 86 90 97
13 18 25 27 29 31
60 66 69 72 75 80 82 81
21 26 29 31 33 38 38
31 37 42 45 46 50
15 21 23 24 30 32 38
51 48 45 43 44
45 43 41 40 38 37 36 36
66 64 61 60 57 53
89 86 83 81 76
36 33 30 33 30 29 28 27
91 88 91 89 86 85 83 85
12 10 12 11 11
31 30 27 30 28 27 23
35 32 31 34 32 26
11 10 10 9 6 5 4 3
44 41 38 38 37 35 34 36
80 78 76 73 72 71 71 71
45 43 40 37 37 33
97 94 91 88 87 86 86 80
58 56 53 51 47 44
66 63 62 60 58 56 52 53
73 72 71 68 65 61 61
95 92 88 86 82
19 17 16 12 11 10 5
80 77 74 71 66 64 62 61
64 62 59 56 51 52
47 46 43 42 39 38 33 33
45 44 41 39 34 30
95 92 89 87 82 81 80 74
94 96 94 91 88
89 91 90 88 89
40 41 38 36 36
70 72 70 68 67 63
22 25 24 21 19 18 13
41 42 39 40 38 37
35 36 35 32 31 29 32 35
31 34 31 34 34
88 90 89 91 90 88 85 81
54 57 60 59 56 50
83 85 82 82 80
66 68 68 65 62 61 62
35 36 36 34 32 30 30
79 80 80 78 76 74 70
44 45 45 43 37
37 40 37 33 31
63 65 63 59 56 55 54 56
19 22 21 17 15 13 11 11
94 95 91 90 88 84
22 23 19 18 16 14 7
29 32 30 23 22
64 67 64 58 56 57
29 30 27 24 18 18
33 36 34 27 24 22 20 16
50 52 49 48 46 39 37 31
31 31 28 25 22 21 19 16
47 47 44 43 40 37 38
88 88 85 84 84
41 41 38 36 32
73 73 70 69 67 66 60
80 80 81 80 79 77 76 75
36 36 34 32 34 35
59 59 57 60 59 58 56 56
30 30 27 28 25 23 19
9 9 7 6 5 8 3
89 89 87 86 85 85 82
52 52 49 46 46 47
60 60 59 59 59
21 21 19 19 15
17 17 17 16 13 12 10 4
33 33 29 28 26 25
67 67 63 61 58 60
27 27 25 24 20 20
78 78 76 74 72 68 64
62 62 59 55 49
47 47 44 38 36 33 31
31 31 28 21 23
81 81 80 79 78 72 72
75 75 69 66 62
97 97 91 89 87 85 78
35 31 28 27 26 25
35 31 28 27 25 22 21 23
37 33 32 31 28 28
73 69 67 65 62 59 55
67 63 60 58 53
52 48 47 44 47 46 43
73 69 70 68 69
18 14 13 16 16
91 87 84 81 82 79 77 73
96 92 90 92 91 88 81
40 36 35 35 34 33 31
93 89 86 83 82 82 80 81
86 82 79 79 78 77 77
20 16 13 13 12 8
30 26 23 23 21 19 17 10
76 72 69 66 62 60 57 54
57 53 49 46 48
64 60 58 54 54
20 16 15 12 8 5 1
40 36 34 31 27 25 19
25 21 20 18 16 14 9 6
48 44 43 40 38 36 29 31
96 92 90 89 83 81 80 80
45 41 34 32 28
35 31 28 27 21 19 14
35 30 27 26 23
73 66 63 61 64
60 55 53 51 50 50
68 62 61 60 56
55 50 49 46 43 42 37
29 23 22 25 22 20 18 16
96 90 88 85 84 86 83 85
47 41 39 36 37 37
72 66 69 68 66 62
52 47 49 46 43 36
77 71 68 68 67 66
99 94 94 93 96
45 38 35 33 31 31 31
23 16 13 10 10 6
33 26 25 25 23 20 18 11
44 37 33 30 28 27
90 85 81 79 80
91 85 82 79 77 73 70 70
98 91 88 86 82 78
68 63 61 58 54 52 45
64 58 57 50 49
66 59 56 49 52
89 82 77 76 73 70 70
80 73 72 67 63
73 66 60 59 57 50
11 14 15 17 20 23 21
47 50 52 55 56 56
16 17 18 19 20 23 27
82 83 85 88 90 91 94 99
59 61 59 61 64 67 70 73
32 33 36 37 38 36 39 38
86 89 90 91 89 92 94 94
67 68 71 68 71 73 76 80
28 30 31 33 34 32 39
3 4 6 6 7 10 12 13
61 64 64 66 69 71 68
49 50 52 52 53 55 55
8 11 14 16 16 20
59 62 62 63 70
43 45 49 52 55 57
80 82 86 88 89 92 93 91
81 82 86 87 88 91 94 94
38 41 45 48 51 53 57
35 38 40 41 45 47 54
6 9 12 14 15 17 22 23
81 82 83 85 91 90
74 76 83 84 87 87
24 27 28 35 37 41
69 71 72 77 80 85
12 9 12 13 15 17 20 23
77 75 76 77 78 81 78
60 57 59 61 61
13 10 11 14 16 20
81 80 81 84 85 90
6 5 6 7 5 7 10 13
54 51 54 52 54 55 54
72 71 70 73 73
89 86 88 90 91 88 90 94
8 6 3 5 10
10 7 8 10 12 12 15 16
14 11 12 14 16 16 19 18
39 37 38 38 39 42 44 44
13 11 12 13 15 15 16 20
10 8 8 11 14 20
42 40 42 45 49 50
67 66 70 73 76 78 77
32 30 34 35 37 37
47 44 45 49 51 55
36 35 39 40 43 45 47 53
68 66 67 68 73 74
40 37 38 39 40 46 47 45
28 26 27 30 35 35
45 42 48 50 54
44 43 45 48 54 59
61 61 62 63 64 66 67
57 57 58 61 59
66 66 69 71 73 75 78 78
59 59 61 64 67 71
28 28 29 30 31 33 36 41
41 41 42 41 44 45
11 11 10 13 15 12
37 37 40 38 41 41
41 41 40 42 46
68 68 69 72 69 71 78
95 95 95 97 98 99
9 9 9 10 13 14 17 16
81 81 82 85 88 88 88
86 86 86 89 92 96
58 58 59 62 62 67
80 80 81 85 87
84 84 87 91 88
3 3 7 10 11 12 12
74 74 75 79 81 82 85 89
26 26 28 29 31 35 37 44
38 38 39 46 49
77 77 80 81 87 88 85
75 75 78 81 87 90 92 92
28 28 31 37 40 43 47
11 11 12 18 20 27
15 19 21 22 23 26
58 62 64 65 66 69 71 69
56 60 61 63 63
46 50 53 56 58 62
67 71 73 76 81
52 56 58 56 58 60
6 10 11 8 7
81 85 86 89 87 88 88
72 76 79 76 79 82 86
39 43 44 46 43 49
38 42 45 45 46
41 45 48 51 51 48
79 83 86 86 86
24 28 29 31 33 33 34 38
55 59 59 61 62 65 67 74
73 77 80 82 86 87 90
45 49 50 53 57 54
80 84 88 90 92 92
14 18 20 22 24 28 32
61 65 69 70 73 76 82
50 54 57 59 62 63 69 70
75 79 81 82 83 89 92 89
45 49 52 59 61 63 63
38 42 44 45 48 55 57 61
62 66 69 71 74 80 86
28 35 37 40 41 43
25 30 32 33 34 37 35
14 19 20 21 22 22
8 14 17 20 23 27
23 28 29 31 38
68 74 77 78 76 79 80
14 21 18 20 22 23 25 22
18 23 20 22 22
65 72 71 74 76 80
8 14 15 17 14 17 20 25
59 65 68 70 70 73
59 65 68 68 71 74 71
34 41 43 43 43
20 25 25 28 32
43 48 50 50 53 56 58 64
38 45 49 52 55 58
23 30 34 35 33
4 9 11 14 18 20 21 21
51 57 61 63 67
67 74 78 81 82 88
20 26 29 36 37
9 14 15 18 20 26 29 28
73 79 82 87 90 90
9 16 18 24 28
8 14 19 22 29
80 77 75 73 76
93 90 88 85 84 81 78 78
25 22 20 18 15 12 11 7
51 48 47 44 42 40 37 31
77 74 77 75 73 72
94 91 92 90 88 89
28 27 26 24 21 22 20 20
94 91 88 87 86 89 85
91 90 87 85 82 80 81 74
59 56 56 54 52 50
89 88 88 87 84 82 85
96 95 93 90 89 86 86 86
25 22 22 19 15
39 36 34 34 33 31 24
53 52 49 47 43 40
93 92 88 85 87
82 80 76 75 72 69 66 66
57 55 52 49 47 43 39
43 40 36 33 26
33 30 28 21 18
72 70 67 66 59 61
26 23 22 19 14 12 10 10
25 22 20 18 13 10 9 5
33 30 27 21 19 16 13 6
96 98 97 95 93 90 89
47 48 46 45 47
10 12 9 8 7 5 3 3
48 49 48 47 43
25 28 26 25 22 15
2 4 1 4 3
59 62 65 64 65
24 25 22 19 20 17 17
53 54 52 54 51 48 46 42
21 23 21 18 19 18 12
48 49 46 46 43 41 40
58 61 59 57 57 59
84 85 83 80 79 78 78 78
20 22 22 19 17 15 11
87 89 87 86 86 81
97 99 95 94 91
15 18 16 15 11 13
54 55 53 49 49
28 30 26 25 23 21 17
34 37 34 32 28 21
33 36 33 31 28 21 18 15
66 69 68 63 61 58 55 58
93 94 93 87 85 85
17 18 13 12 8
81 83 80 78 71 68 63
68 68 66 65 63
65 65 62 61 58 57 60
28 28 26 23 23
48 48 46 45 44 43 39
16 16 13 11 6
30 30 29 26 29 28
23 23 20 22 20 17 20
41 41 43 41 41
39 39 36 39 36 32
34 34 36 35 30
88 88 87 85 85 84
6 6 4 3 3 5
41 41 38 38 38
14 14 11 8 8 7 5 1
75 75 72 72 70 64
59 59 55 52 51
91 91 88 84 81 79 80
95 95 92 88 85 85
62 62 60 59 55 51
17 17 13 11 4
67 67 66 65 63 58 56
63 63 56 55 57
46 46 45 39 39
42 42 39 38 32 28
91 91 86 84 78
30 26 24 22 21 20 19
58 54 53 50 53
10 6 5 4 4
93 89 88 86 85 83 81 77
52 48 46 43 38
89 85 84 81 83 82 79
46 42 41 39 42 44
48 44 41 44 43 42 39 39
61 57 56 53 54 51 47
55 51 54 52 50 43
23 19 16 16 15 13 11 8
23 19 17 16 14 14 13 14
23 19 19 17 14 11 9 9
24 20 19 16 14 14 10
94 90 88 85 85 84 77
47 43 41 39 36 32 30
54 50 46 45 42 43
88 84 81 77 77
29 25 21 20 18 15 12 8
67 63 59 58 51
16 12 6 3 1
68 64 61 59 53 51 50 53
70 66 60 58 58
84 80 75 73 70 66
92 88 86 85 78 77 71
93 86 83 82 79 77 74 73
56 50 48 46 48
25 20 19 16 13 13
79 74 73 72 70 69 67 63
77 71 70 67 64 62 56
80 74 71 68 67 69 68 65
36 29 27 26 28 31
88 82 83 82 82
13 6 5 7 3
17 11 12 9 6 1
87 82 81 80 80 78 75
23 17 14 11 11 12
70 64 61 58 58 58
96 91 90 87 87 85 81
92 85 85 83 80 74
23 17 13 12 11
70 63 59 56 55 53 54
85 80 79 76 73 70 66 66
81 75 74 73 72 68 64
69 62 59 55 54 49
68 62 60 58 52 49
81 75 74 71 69 64 66
71 65 58 55 55
39 32 31 26 24 22 19 15
53 47 42 40 33
39 40 43 44 46 48 55 61
41 37 35 31 29 25
33 28 26 23 23 24
53 49 48 45 43 39
45 45 46 47 49 49 50 57
55 52 51 50 47 46 46 42
43 40 39 33 27
31 37 42 44 46 46
38 36 34 33 30 29 27 30
25 31 33 35 32 38
74 74 80 82 83 87
34 40 40 41 44 45 50
75 68 66 63 61
72 69 67 64 61 54 54
23 27 27 29 34
53 57 58 60 60
47 40 35 34 35
28 24 22 20 18 16 16
55 58 56 52 52
9 9 11 14 15 12 16
38 33 29 27 26 21
19 18 19 19 20 24
59 59 55 54 53 52 46
9 13 16 18 20 21 22 26
36 32 25 24 21 15
40 41 44 45 47 46 49 53
2 4 7 7 9 11 18
16 21 20 22 23 25
90 84 82 79 75 72 68
74 74 78 79 81 81
59 61 64 63 61 59 52
76 76 75 68 67 64 65
85 85 85 86 85
34 37 35 32 29 25 24
86 84 84 83 80 77 75 68
15 16 20 21 23 26 29
65 71 75 76 73
95 95 94 91 88 90
81 87 88 91 94 96 97 97
4 8 11 13 14 12 15
34 40 42 44 47 45 43
34 36 38 41 48 51 55
44 46 44 44 41 40 38 38
95 98 97 96 93 93
54 53 50 48 44 38
25 25 23 20 17 15 11
82 82 81 81 81
52 56 58 62 64 68
39 39 36 35 34
83 86 88 85 87
46 39 41 40 34
18 24 27 34 37
34 30 28 26 24 23 24
65 67 70 72 74 77 76
18 23 24 27 34 36 37 36
10 10 11 14 16 21
2 6 9 12 13 13 15 14
46 51 52 56 57 59 63
69 75 80 81 85
60 61 64 61 58 57 60
84 87 86 80 79 79
76 73 75 73 73
72 66 65 62 65 63 60
10 14 16 17 18 25 27 31
15 16 17 17 18
94 94 91 86 82
39 46 47 49 52 56 58
1 1 2 2 2
3 7 8 10 17 20 20
26 26 24 19 17 14 12
43 43 42 44 42 37
62 62 59 60 59 58 55 55
19 23 27 28 30 36
41 44 42 44 41 41
12 10 10 12 13 15 17 22
52 53 54 55 62 65 67 67
21 22 21 20 20 19 16 17
50 46 45 43 41 40 40 40
69 70 74 77 77
83 87 89 89 90 94
14 11 9 11 15
86 83 84 88 89 92 93 97
47 44 44 47 47
83 87 89 89 92
65 61 57 55 57
65 69 71 75 78 81 81
72 72 73 80 81 84 82
31 28 30 34 35 36 39 41
94 89 83 82 81 80 79
55 55 52 48 45 45
64 66 64 61 59 54 50
7 6 11 14 17 21
83 79 75 73 72 71 70 70
37 36 36 33 31 29 28 31
92 94 93 92 91 86
71 70 69 68 63
80 83 82 83 82 79 77
51 55 57 61 58
53 50 47 46 42 42
89 89 90 96 99
34 33 36 39 40 44 42
77 73 69 68 66
10 10 13 14 16 13
76 77 74 74 69
91 92 93 95 96 99 99 99
17 15 11 10 9 6 5
47 47 44 41 39 40 41
21 25 28 30 28 35
52 55 52 48 49
44 44 42 45 43 39
36 36 37 41 42 44 46 44
43 43 40 40 37 40
19 22 19 15 10
61 62 59 56 55 51
31 37 38 41 45 45
1 5 8 9 10 15
42 38 40 38 37 30
60 61 59 57 55
36 34 36 33 30
38 40 41 42 40 42 44 43
58 55 53 52 51 49 46 42
91 88 91 89 88 89
53 53 51 50 50
89 91 92 95 92 99
18 13 15 13 11 11
64 65 66 68 69 70
71 74 77 80 83 84 86
40 42 45 48 51 52 54 57
29 31 32 34 37 39 42
13 14 15 18 19 22 25 27
20 18 16 13 12
21 19 16 13 12 10 7
88 91 94 95 97
25 22 21 18 15 13
55 53 51 48 45 42 40 38
63 62 61 58 55 52 50 48
61 58 56 55 54 52 49
59 60 62 65 67 69
68 69 72 74 76 77
73 70 68 65 63 61
88 89 92 95 96
37 36 34 33 30
50 51 52 55 56 59 62 63
67 70 71 73 76 78
90 92 93 95 96
41 42 43 45 46
60 59 56 54 52 50 47
39 38 36 35 32 31 28 27
14 17 19 20 21 24 25
60 58 56 55 53 51
11 10 7 6 5
10 11 14 15 17 19
14 11 8 5 4 3
64 62 61 58 55 52 51
17 14 13 12 11 10 7
72 70 69 68 66 64 61
74 77 80 82 85 86 88 89
26 29 32 35 36 38
62 60 59 57 56 53 50
26 25 23 22 20 19 16 13
7 10 13 16 17 20 21
71 70 69 67 65 62 61
11 13 14 15 18 21
22 20 17 14 11 9 6 5
3 4 6 8 11 12 14 15
2 4 6 9 12
81 84 86 88 91 94 97 98
32 33 36 37 38 41
78 77 76 74 73
71 72 74 76 78
44 45 48 49 51 53 54
78 75 74 73 70 69 66 64
75 74 71 69 66 63
84 82 79 78 75 74 71 69
62 61 58 56 54 52 51 48
98 95 92 89 88 87 85
3 5 7 9 10 12 14
89 87 85 83 82
80 79 77 75 73 71 68
61 62 64 66 69 72 74 76
51 49 46 44 43 40 39 38
70 67 64 63 62
35 34 33 31 29 27 24 23
42 44 47 50 51 52
52 55 58 59 62 64 65
67 69 72 74 77 80
14 12 11 9 8
53 51 48 45 43
57 58 60 63 65 68 70
30 32 35 36 38 41 43
99 97 96 94 91 89 88
68 65 63 61 59 57 56
92 90 88 86 85
44 46 47 49 52
49 52 53 54 56 57 59
28 27 25 22 21 20 17 16
48 46 43 41 40
79 80 82 85 87 88 89
58 56 54 53 52 51 49 47
54 57 60 61 62 64 65
40 39 38 36 35 33
32 30 29 27 24 23 22
17 14 11 9 7 5
91 89 88 87 86 83
19 16 14 11 9 8
40 41 42 45 46 49 51 52
86 84 81 80 77 74 71 68
42 41 39 37 36 34
36 33 32 29 26 23 20
24 21 20 17 16
12 9 8 5 4
76 79 82 85 88 91 92
64 66 69 72 75
35 37 38 41 44 46 49 51
94 92 89 87 86 85
66 68 71 74 76
81 80 79 77 75 74 72 71
83 86 89 92 94
43 40 37 34 31
36 37 38 41 43
31 28 25 23 20 18
45 44 42 41 38 36
86 85 84 82 80 77 74
58 57 55 54 53 50 49 47
70 71 73 75 78
73 76 79 80 82
85 83 81 78 75 72
37 38 39 42 45 48 50
64 67 68 71 72
66 63 61 58 56
61 64 65 67 68 69 72 74
95 94 91 90 89 87 85 84
84 87 89 91 94
63 65 68 71 72 73
40 39 38 37 35 34 32
88 87 86 83 82 81 80
71 70 68 65 63 62
92 90 88 85 82 81 80
76 74 71 68 65 64
9 11 12 14 17
74 75 77 78 80 82
30 33 36 38 39 42 44 45
59 62 63 65 67 69
83 86 88 91 94 95
29 28 26 23 21
70 71 74 77 78 80 82
70 72 74 75 76
6 9 12 15 18 21 24 25
32 31 29 26 23 22
76 75 74 72 70 68
93 92 91 90 89
56 58 59 60 63 64
72 73 74 76 78 80 83 85
96 95 92 89 86 84
98 95 94 93 90
27 29 31 33 36 37 39 40
48 47 44 41 39 38 37 36
20 19 16 14 13
98 97 96 95 92
71 72 74 75 78 80
49 51 53 55 57
63 60 59 56 53
78 80 82 83 84
80 79 77 75 73 71 69 66
24 25 27 30 33 34 36
81 82 84 86 89
35 36 39 42 43 44 46
14 15 16 18 21 23 25 28
83 84 86 89 92 93
82 81 78 76 73
19 21 24 27 28 30 32
61 60 58 56 53
7 8 10 12 14
69 67 66 64 61 59
29 28 25 23 21
44 46 49 52 54 55 57 60
6 7 9 10 12 14 15
27 25 22 19 18 17 14
50 52 54 56 59 60 63 65
40 39 36 33 32
10 11 12 14 16
30 27 26 24 23 21 19 18
24 22 20 19 18 16 14
89 91 93 95 97 98 99
60 57 54 53 51 48 47
1 3 6 7 8 10 13
14 13 11 10 8
57 55 53 51 50
48 46 45 42 40 39 36 35
42 43 45 47 50
92 90 88 86 85 82
94 91 90 88 87 85 84 82
45 46 48 50 53
24 21 20 17 14 11 9 7
65 64 62 60 59 56 55 53
54 51 50 48 47 44 43
49 47 45 42 39 38 37 34
37 34 32 30 27 26 25
87 88 91 92 94 97
86 84 81 79 76 73 71 70
59 60 63 64 65
39 42 43 44 46
14 16 17 18 21 23 24 27
5 6 7 9 12 14 16 19
64 65 67 70 73
90 89 88 85 84 81 78
83 85 88 90 92 93 96 97
83 81 79 76 74 73
68 71 74 77 78 80 83 86
41 40 38 35 34
34 31 28 27 24
87 88 90 92 93 94 96
52 55 56 57 60 62
17 20 23 24 25 27
30 28 27 24 21 18 15 12
28 27 26 25 22 19
72 69 68 67 66 64
59 56 55 54 52 49 47 45
26 28 31 32 35 36 38
52 49 46 45 42 41 39
9 10 11 12 14 15 17
35 36 37 39 40 42 44 47
59 58 55 54 53 52
11 10 9 8 6 4 3
18 20 22 23 24 27
10 8 7 6 5 4 3
68 71 72 74 76 77
64 63 62 59 56 54 51
37 36 35 33 31 28
98 97 96 95 93 92 90 87
18 19 21 24 27
23 24 26 29 30 33 36 39
52 51 49 47 46 45 42 41
50 48 47 44 43
40 43 46 49 52 54 55 58
45 43 40 37 35 33 31 29
35 33 30 27 24 23 20 18
82 83 84 86 88
19 16 13 10 9
37 34 33 32 29
86 83 80 77 76
77 78 79 81 84 87 88 90
13 15 18 21 22 25 27
6 7 9 11 13 14 17 18
51 50 48 47 46 44 41 40
79 80 82 83 85
74 72 69 67 65 63 62 59
58 57 55 53 50
54 57 60 62 64 65 68
28 30 32 34 37 38
10 13 15 18 20 22 24 27
80 79 77 74 73 71 70
21 20 18 15 13 11 9
21 23 26 29 31 33 35
74 75 76 78 81 83
61 58 56 55 54 53 51 50
54 55 56 57 60 62 64 65
45 43 42 41 38 37 35 34
47 50 53 56 57 59
98 95 94 93 92 90
13 12 9 7 6 4 2
68 70 73 76 79 80 81 84
59 62 65 68 69 72 75
80 82 84 87 89 92 95
63 65 68 70 71 72 73 76
92 90 87 86 85 82 80 77
20 18 15 13 11 9 7
60 59 58 55 54 51 48 47
18 16 15 13 11 9
71 68 65 62 61 59 57
69 68 65 62 60 57 56
75 74 73 72 70 67 65 62
17 20 22 25 28 31
43 45 46 48 50 51
28 25 22 21 19 17 15
36 33 30 29 27 24 22 21
13 14 15 18 20 23
84 82 81 78 77 76 75
23 26 27 30 33
56 53 50 47 46
25 27 28 30 31 32 34 37
90 92 94 95 96 99
82 83 85 88 90 92 95
49 46 45 43 42 40
55 58 61 63 65 68 69 72
61 62 65 66 68 69
49 52 54 55 56 58 59 61
41 43 44 46 49
74 71 70 68 65 62 61 58
26 29 30 31 32 35 38 40
11 12 14 15 18 21
79 76 74 72 71 68 66
17 20 23 25 27 30 33 35
52 53 55 58 60
6 7 9 11 12
88 85 84 83 81 79 77
71 74 76 77 79
70 72 75 78 79
54 56 58 59 62
97 96 94 91 90
6 9 10 11 14 17 20 22
23 21 18 15 14
99 98 96 94 91 88 87
96 93 92 90 89 88
91 89 88 85 84 82 80 77
86 87 88 91 94
3 4 7 10 11 12
56 59 61 64 67
40 41 44 46 48 49 52 53
35 34 32 31 30 27 24 22
48 47 44 43 42 40 38 36
31 29 26 23 20 19 16
25 27 28 29 30 32
4 7 10 13 15
36 39 40 42 43 45 48
20 23 25 27 28 31 34
40 42 45 48 49 52 54
14 12 10 9 7 4 2 1
76 74 71 68 66 64
25 27 29 30 31
34 31 29 28 25 24 22
63 64 67 70 72 73
95 93 90 89 87 85
29 27 25 22 20 18
28 30 32 33 34 37
53 52 49 46 43 42
41 40 38 36 33 31 30
81 79 77 75 74 73
97 96 95 94 93 90
77 79 81 82 83
67 64 62 59 56 54
55 54 52 50 48 46 45
74 73 70 69 67
90 88 85 82 80
16 14 13 12 11
32 31 29 26 24 23
17 18 21 24 27
21 18 16 14 11 9 6
37 39 41 42 45 47
6 8 9 11 13
35 34 32 30 28 27 25 23
80 78 77 75 73 71
4 5 6 9 12 15
48 51 52 53 55 56 57
31 33 34 35 37 38 40 42
75 77 78 80 83 86 87
39 40 43 46 49 51
70 71 72 74 75 77 79
95 92 89 87 85 84 82
5 7 10 12 14 17 18 20
68 67 66 65 62
61 63 64 66 68 69 70 73
61 59 57 55 53 51
38 36 33 32 31 30 29
83 82 79 76 75 74 72 69
17 20 22 23 26 28
81 83 86 87 90 93 94
85 82 81 79 77
7 10 11 12 13
94 93 90 87 84 82
63 64 66 69 70 71 73
35 36 37 39 42 45
48 47 45 44 41
93 92 91 89 88 86
47 50 52 54 55
33 35 38 40 43 46 48 49
70 73 76 77 80
96 95 92 91 88 87 84 83
14 17 20 21 22 23 25 28
14 15 16 17 18 20 21
72 71 70 68 65 63 60 59
57 60 61 64 67 68
18 17 16 13 10
49 47 44 42 40 38
30 32 34 37 38
70 69 68 65 63 62 61
37 40 43 44 47 48
55 54 53 52 50 47 45
90 88 86 85 83 81 80
90 88 85 83 80
38 40 43 45 46
64 67 70 73 75
91 93 95 98 99
18 15 12 9 8 6 4 1
80 77 75 73 70 67 64 62
29 26 24 21 19 16 15
61 63 64 67 69 72
60 59 57 54 53
63 61 59 57 56 54 53
49 50 52 53 54
57 59 60 63 66
61 63 64 65 66 67 69 70
24 21 19 18 16 14
42 39 36 35 34 33
95 94 92 91 89 86 83
72 75 77 78 81 83 86
4 5 6 8 9 12 14
20 22 25 28 30 33
65 68 70 71 73 74
1 3 4 6 9 11 12 15
75 77 79 81 83 85
21 22 25 28 29 31 34
8 7 4 3 2
39 37 34 31 28";
}
