namespace AdventOfCode2025;

public class Day04 : IAdvent
{
    public void Run()
    {
        var input = Advent.ReadInputLines()
            .Select(line => line.ToCharArray())
            .ToArray();

        int removed = RemoveWithLessThan4Adjacent();
        Advent.AssertAnswer1(removed, expected: 1356, sampleExpected: 13);

        int totalRemoved = removed;
        while (removed > 0)
        {
            removed = RemoveWithLessThan4Adjacent();
            totalRemoved += removed;
        }

        Advent.AssertAnswer2(totalRemoved, expected: 8713, sampleExpected: 43);


        int RemoveWithLessThan4Adjacent()
        {
            char[][] newInput = new char[input.Length][];

            int adjacentTotal = 0;
            for (int y = 0; y < input.Length; y++)
            {
                newInput[y] = new char[input[y].Length];
                for (int x = 0; x < input[0].Length; x++)
                {
                    newInput[y][x] = input[y][x];
                    if (input[y][x] == '.')
                        continue;

                    int adjacent = 0;
                    for (int dy = -1; dy <= 1; dy++)
                    for (int dx = -1; dx <= 1; dx++)
                    {
                        if (dx == 0 && dy == 0)
                            continue;

                        int py = y + dy;
                        int px = x + dx;
                        if (px < 0 || py < 0 || px >= input[0].Length || py >= input.Length)
                            continue;
                        if (input[py][px] == '@')
                            adjacent++;
                    }

                    if (adjacent < 4)
                    {
                        adjacentTotal++;
                        newInput[y][x] = '.';
                    }
                }
            }
            input = newInput;

            return adjacentTotal;
        }
    }
}
