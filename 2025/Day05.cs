namespace AdventOfCode2025;

public class Day05 : IAdvent
{
    public void Run()
    {
        var input = Advent.ReadInput().SplitByDoubleNewLine();
        var ingredientRanges = input[0].SplitByNewLine()
            .Select(line => line.Split('-').Select(long.Parse).ToArray())
            .Select(r => (start: r[0], end: r[1]))
            .ToArray();
        var ingredientIds = input[1]
            .SplitByNewLine()
            .Select(long.Parse)
            .ToArray();

        long freshIngredients = 0;
        foreach (long ingredient in ingredientIds)
        {
            foreach (var range in ingredientRanges)
            {
                if (ingredient >= range.start && ingredient <= range.end)
                {
                    freshIngredients++;
                    break;
                }
            }
        }
        Advent.AssertAnswer1(freshIngredients, expected: 726, sampleExpected: 3);


        ingredientRanges = ingredientRanges.OrderBy(r => r.start).ToArray();
        freshIngredients = 0;
        long pos = 0;
        foreach (var range in ingredientRanges)
        {
            pos = Math.Max(range.start, pos);
            if (pos > range.end)
                continue;
            freshIngredients += range.end - pos + 1;
            pos = range.end + 1;
        }

        Advent.AssertAnswer2(freshIngredients, expected: 354226555270043, sampleExpected: 14);
    }
}
