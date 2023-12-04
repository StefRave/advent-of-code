namespace AdventOfCode2023;

public class Day04 : IAdvent
{
    record Card(HashSet<int> Winning, int[] Numbers);

    public void Run()
    {
        var input = Advent.ReadInput().SplitByNewLine()
            .Select(line =>
            {
                var data = line.Split(':')[1];
                var groups = data.Split('|');
                return new Card(
                    Regex.Matches(groups[0], @"\d+").Select(m => int.Parse(m.Value)).ToHashSet(),
                    Regex.Matches(groups[1], @"\d+").Select(m => int.Parse(m.Value)).ToArray());
            })
            .ToArray();

        int[] matchesPerCard = input.Select(card => card.Numbers.Count(n => card.Winning.Contains(n))).ToArray();
        int answer1 = matchesPerCard.Sum(m => m == 0 ? 0 : (int)Math.Pow(2, m - 1));
        
        Advent.AssertAnswer1(answer1, expected: 22488, sampleExpected: 13);


        int[] timesToProcess = Enumerable.Repeat(1, input.Length).ToArray();
        int total = 0;
        for (int i = 0; i < input.Length; i++)
        {
            int timesCurrentCard = timesToProcess[i];
            total += timesCurrentCard;

            for (int j = i + 1; j < Math.Min(input.Length, i + matchesPerCard[i] + 1); j++)
                timesToProcess[j] += timesCurrentCard;
        }
        Advent.AssertAnswer2(total, expected: 7013204, sampleExpected: 30);
    }
}
