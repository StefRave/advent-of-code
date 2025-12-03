namespace AdventOfCode2025;

public class Day03 : IAdvent
{
    public void Run()
    {
        var input = Advent.ReadInputLines()
            .Select(line => line.Select(c => c - '0').ToArray())
            .ToArray();

        var answer1 = GetTotalJolts(input, 2);
        Advent.AssertAnswer1(answer1, expected: 16842, sampleExpected: 357);

        long answer2 = GetTotalJolts(input, 12);
        Advent.AssertAnswer2(answer2, expected: 167523425665348, sampleExpected: 3121910778619);
    }

    private static long GetTotalJolts(int[][] input, int digits)
    {
        long totalJolts = 0;
        foreach (var line in input)
        {
            long jolts = 0;
            int maxPos = 0;
            int maxPrev = -1;
            for (int digit = digits; digit > 0; digit--)
            {
                int max = -1;
                for (int i = maxPrev + 1; i < line.Length - digit + 1; i++)
                {
                    if (line[i] > max)
                    {
                        max = line[i];
                        maxPos = i;
                    }
                }
                maxPrev = maxPos;
                jolts = jolts * 10 + max;
            }
            totalJolts += jolts;
        }

        return totalJolts;
    }
}
