namespace AdventOfCode2025;

public class Day01 : IAdvent
{
    public void Run()
    {
        var input = Advent.ReadInput().SplitByNewLine()
            .Select(x => new { Rot = x[0], Times = int.Parse(x[1..]) });

        int pos = 50;
        int answer1 = 0;
        int answer2 = 0;
        foreach (var item in input)
        {
            for (int i = 0; i < item.Times; i++)
            {
                if (item.Rot == 'L')
                    pos--;
                else
                    pos++;

                if (pos < 0)
                    pos = 99;
                if (pos == 100)
                    pos = 0;

                if (pos == 0)
                    answer2++;
            }
            if (pos == 0)
                answer1++;
        }
        Advent.AssertAnswer1(answer1, expected: 1081, sampleExpected: 3);
        Advent.AssertAnswer2(answer2, expected: 6689, sampleExpected: 6);
    }
}
