using System.Diagnostics;

namespace AdventOfCode2025;

public class Day08 : IAdvent
{
    public void Run()
    {
        var input = Advent.ReadInputLines()
            .Select(line => line.GetInts())
            .Select(line => new V3(line[0], line[1], line[2]))
            .ToArray();

        var distances = new List<(double distance, int p1, int p2)>();

        for (int i = 0; i < input.Length - 1; i++)
            for (int j = i + 1; j < input.Length; j++)
                distances.Add((distance: (input[i] - input[j]).Length(), i, j));
        
        distances = distances
            .OrderBy(r => r.distance)
            .ToList();

        var networks = input.Select((_, i) => new HashSet<int>() {i}).ToHashSet();
        int connections = 0;
        int maxConnections = Advent.UseSampleData ? 10 : 1000;
        foreach (var distance in distances)
        {
            connections++;

            var n1 = networks.First(n => n.Contains(distance.p1));
            var n2 = networks.First(n => n.Contains(distance.p2));
            if (n1 != n2)
            {
                networks.Remove(n2);
                foreach (var i in n2)
                    n1.Add(i);
            }

            if (connections == maxConnections)
            {
                var answer1 = networks
                    .Select(n => n.LongCount())
                    .Distinct()
                    .OrderByDescending(s => s)
                    .Take(3)
                    .Aggregate((a, b) => a * b);
                Advent.AssertAnswer1(answer1, expected: 50760, sampleExpected: 40);
            }

            if (networks.Count == 1)
            {
                long answer2 = (long)input[distance.p1].X * input[distance.p2].X;
                Advent.AssertAnswer2(answer2, expected: 3206508875, sampleExpected: 25272);
                break;
            }

        }
    }
}

[DebuggerDisplay("{X},{Y},{Z}")]
public record V3(int X, int Y, int Z)
{
    public double Length() => Math.Sqrt((double)X * X + (double)Y * Y + (double)Z * Z);

    public static V3 operator -(V3 a, V3 b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
    public static V3 operator +(V3 a, V3 b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
}
