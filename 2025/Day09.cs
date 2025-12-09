namespace AdventOfCode2025;

public class Day09 : IAdvent
{
    public void Run()
    {
        var input = Advent.ReadInputLines()
            .Select(line => line.GetInts())
            .Select(line => new V2(line[0], line[1]))
            .ToArray();

        var polygon = CreatePolygon(input);

        long maxArea1 = 0;
        long maxArea2 = 0;
        for (int i = 0; i < input.Length - 1; i++)
        for (int j = i + 1; j < input.Length; j++)
        {
            var a = input[i];
            var b = input[j];
            long sx = Math.Abs(a.x - b.x) + 1;
            long sy = Math.Abs(a.y - b.y) + 1;
            long area = sx * sy;
            
            if (area > maxArea1)
                maxArea1 = area;

            if (area > maxArea2 && IsInside(polygon, a, b))
                maxArea2 = area;
        }

        Advent.AssertAnswer1(maxArea1, expected: 4765757080, sampleExpected: 50);
        Advent.AssertAnswer2(maxArea2, expected: 1498673376, sampleExpected: 24);
    }

    private static List<V2> CreatePolygon(V2[] input)
    {
        var left = input.Skip(1).ToHashSet();
        var ordered = new List<V2>() { input[0] };

        while (left.Count > 0)
        {
            var last = ordered[^1];
            var match = left.Where(v => v.x == last.x || v.y == last.y)
                .OrderBy(m => (m - last).ManhattanDistance)
                .First();
            ordered.Add(match);
            left.Remove(match);
        }

        return ordered;
    }

    private static bool IsInside(IReadOnlyList<V2> polyLine, V2 point1, V2 point2)
    {
        int minX = Math.Min(point1.x, point2.x);
        int maxX = Math.Max(point1.x, point2.x);
        int minY = Math.Min(point1.y, point2.y);
        int maxY = Math.Max(point1.y, point2.y);

        var a = polyLine[^1];
        for (var i = 0; i < polyLine.Count; i++)
        {
            var b = a;
            a = polyLine[i];

            if (a.x == b.x)
            {
                int edgeMinY = Math.Min(a.y, b.y);
                int edgeMaxY = Math.Max(a.y, b.y);

                if (a.x > minX && a.x < maxX && edgeMinY < maxY && edgeMaxY > minY)
                    return false;
            }
            else
            {
                int edgeMinX = Math.Min(a.x, b.x);
                int edgeMaxX = Math.Max(a.x, b.x);

                if (a.y > minY && a.y < maxY && edgeMinX < maxX && edgeMaxX > minX)
                    return false;
            }
        }

        return true;
    }
}