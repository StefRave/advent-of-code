namespace AdventOfCode2025;

public class Day02 : IAdvent
{
    public void Run()
    {
        var input = Advent.ReadInput().Split(',')
            .Select(line => line.Split('-').Select(long.Parse).ToArray())
            .ToArray();

        long answer1 = 0;
        long answer2 = 0;
        foreach (var line in input.Skip(0))
        {
            for (long i = line[0]; i <= line[1]; i++)
            {
                string val = i.ToString();
                bool found = false;
                for (int len = 1; len < val.Length; len++)
                {
                    if ((val.Length % len) != 0)
                        continue;
                    string firstPart = val[0..len];
                    bool match = true;
                    for (int j = len; j < val.Length; j += len)
                        match &= firstPart == val[j..(j + len)];
                    if (match)
                    {
                        if (!found)
                            answer2 += i;
                        found = true;

                        if (len * 2 == val.Length)
                        {
                            answer1 += i;
                        }
                    }
                }
            }
        }

        Advent.AssertAnswer1(answer1, expected: 26255179562, sampleExpected: 1227775554);
        Advent.AssertAnswer2(answer2, expected: 31680313976, sampleExpected: 4174379265);
    }
}
