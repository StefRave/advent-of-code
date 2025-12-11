namespace AdventOfCode2025;

public class Day11 : IAdvent
{
    public void Run()
    {
        // input has 2 pieces of sample data
        var input = Advent.ReadInput().SplitByDoubleNewLine()
            .Select(inp => inp
                .SplitByNewLine()
                .Select(line => line.Replace(":", "").Split(' '))
                .ToDictionary(line => line[0], line => line[1..]))
            .ToArray();

        var answer1 = Solve(input[0], "you", 1);
        Advent.AssertAnswer1(answer1, expected: 511, sampleExpected: 5);

        var answer2 = Solve(input[^1], "svr", 2);
        Advent.AssertAnswer2(answer2, expected: 458618114529380, sampleExpected: 2);
    }

    private static long Solve(Dictionary<string, string[]> input, string startNode, int part)
    {
        var visited = new Dictionary<string, long>();
        return SolveRecursive(startNode);


        long SolveRecursive(string node, bool dacInPath = false, bool fftInPath = false)
        {
            if (node == "out")
                return (part == 1) || (dacInPath && fftInPath) ? 1 : 0;

            if (node == "dac") dacInPath = true;
            if (node == "fft") fftInPath = true;

            if (visited.TryGetValue(node + dacInPath + fftInPath, out long result))
                return result;

            result = input[node].Sum(device => SolveRecursive(device, dacInPath, fftInPath));

            visited.Add(node + dacInPath + fftInPath, result);
            return result;
        }
    }
}
