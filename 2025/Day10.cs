
namespace AdventOfCode2025;

public class Day10 : IAdvent
{
    public void Run()
    {
        var lines = Advent.ReadInputLines()
            .Select(ParseInput)
            .ToArray();

        int answer1 = 0;

        foreach (var line in lines)
        {
            int minPresses = int.MaxValue;

            int times = 1 << (line.Buttons.Length + 1);
            for (int i = 1; i < times; i++)
            {
                var start = line.Indicators.ToArray();
                int presses = 0;
                for (int j = 0; j < line.Buttons.Length; j++)
                {
                    var buttons = line.Buttons[j];
                    if ((i & (1 << j)) != 0)
                    {
                        presses++;
                        foreach (var btn in buttons)
                            start[btn] = !start[btn];
                    }
                }
                if (start.All(b => !b) && presses < minPresses)
                    minPresses = presses;
            }
            if (minPresses < 1000)
                answer1 += minPresses;
        }
        Advent.AssertAnswer1(answer1, expected: 404, sampleExpected: 7);

        int answer2 = 0;
        Advent.AssertAnswer2(answer2, expected: 2, sampleExpected: 22);
    }

    private Hoi ParseInput(string line)
    {
        var parts = line.Split(['[', ']', '{', '}'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        var buttons = parts[1].Replace(" ", "").Split(['(', ')'], StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s.GetInts())
            .ToArray();

        return new Hoi(parts[0].Select(c => c == '#').ToArray(), buttons, parts[^1].GetInts());

    }

    private record Hoi(bool[] Indicators, int[][] Buttons, int[] Other);
}
