namespace AdventOfCode2025;

public class Day06 : IAdvent
{
    public void Run()
    {
        var lines = Advent.ReadInputLines();
        var numberLines = Init.Array(() => new List<string>(), lines.Length - 1);
        var operations = lines[^1].Replace(" ", "");

        int start = 0;
        while (start < lines[0].Length)
        {
            int end = start + 1;
            while (end < lines[0].Length && lines[^1][end] == ' ')
                end++;
            if (end >= lines[0].Length - 1)
                end++;
            for (int i = 0; i < numberLines.Length; i++)
                numberLines[i].Add(lines[i][start..(end - 1)]);
            start = end;
        }

        BigInteger answer1 = 0;
        for (int i = 0; i < operations.Length; i++)
        {
            BigInteger ans1 = operations[i] == '*' ? 1 : 0;

            for (int j = 0; j < numberLines.Length; j++)
            {
                var num = int.Parse(numberLines[j][i]);
                if (operations[i] == '*')
                    ans1 *= num;
                else
                    ans1 += num;
            }

            answer1 += ans1;
        }
        Advent.AssertAnswer1(answer1, expected: 4693159084994, sampleExpected: 4277556);


        BigInteger answer2 = 0;
        for (int i = 0; i < operations.Length; i++)
        {
            BigInteger ans2 = operations[i] == '*' ? 1 : 0;
            for (int j = 0; j < numberLines[0][i].Length; j++)
            {
                var strNum = "";
                for (int k = 0; k < numberLines.Length; k++)
                    strNum += numberLines[k][i][j];

                var num = int.Parse(strNum);
                if (operations[i] == '*')
                    ans2 *= num;
                else
                    ans2 += num;
            }
            answer2 += ans2;
        }
        Advent.AssertAnswer2(answer2, expected: 11643736116335, sampleExpected: 3263827);
    }
}
