using System.Drawing.Printing;

namespace AdventOfCode2025;

public class Day07 : IAdvent
{
    public void Run()
    {
        var input = Advent.ReadInputLines()
            .Select(line => line.ToCharArray())
            .ToArray();

        int start = Array.IndexOf(input[0], 'S');

        int timesSplit = 0;

        var beams = new Dictionary<int, long> { { start, 1 } };
        for (int y = 1; y < input.Length; y++)
        {
            var newBeams = new Dictionary<int, long>();
            foreach (var (x, times) in beams)
            {
                if (input[y][x] == '.')
                {
                    newBeams.Update(x, prev  => prev + times );
                }
                else
                {
                    timesSplit++;

                    newBeams.Update(x - 1, prev => prev + times);
                    newBeams.Update(x + 1, prev => prev + times);
                }
            }

            beams = newBeams;
        }
        long timeLines = beams.Values.Sum();

        Advent.AssertAnswer1(timesSplit, expected: 1507, sampleExpected: 21);

        Advent.AssertAnswer2(timeLines, expected: 1537373473728, sampleExpected: 40);
    }
}

public record V2(int x, int y)
{
    public readonly static V2 V0 = (0, 0);
    public static V2 operator -(V2 a, V2 b) => new V2(a.x - b.x, a.y - b.y);
    public static V2 operator +(V2 a, V2 b) => new V2(a.x + b.x, a.y + b.y);

    public static implicit operator V2((int x, int y) t) => new V2(t.x, t.y);
    public int ManhattanDistance => Math.Abs(x) + Math.Abs(y);
}
