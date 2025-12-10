
namespace AdventOfCode2025;

public class Day10 : IAdvent
{
    public void Run()
    {
        var lines = Advent.ReadInputLines()
            .Select(ParseInput)
            .ToArray();

        var answer1 = DoPart1(lines);
        Advent.AssertAnswer1(answer1, expected: 404, sampleExpected: 7);


        int answer2 = 0;
        for (var index = 0; index < lines.Length; index++)
        {
            var line = lines[index];
            var buttons = line.Buttons;
            var counters = line.Counters;
            int minSteps = FindPart2MinSteps(counters, buttons);

            Console.WriteLine($" {index}  {minSteps}\r");
            answer2 += minSteps;
        }
        Advent.AssertAnswer2(answer2, expected: 16474, sampleExpected: 33);
    }

    private static int FindPart2MinSteps(int[] counters, int[][] buttons, int minExtraSteps = 1)
    {
        var equations = counters
            .Select((r, i) => new Equation(buttons.Select(b => b.Contains(i) ? 1 : 0).ToArray(), r))
            .ToArray();
        int[] indexes = Enumerable.Range(0, counters.Length).ToArray();

        var (solution, solvable) = IntEquationSolver.SolveIntegers(equations);
        if (solvable.Count == buttons.Length)
            return solution.Sum();

        int extraSteps = Math.Max(minExtraSteps, buttons.Length - counters.Length);

        int? index1 = indexes.FirstOrDefault(i => !solvable.Contains(i));
        int max1 = buttons[index1.Value].Min(ind => counters[ind]);
        int minCount = int.MaxValue;
        for (int i1 = 0; i1 <= max1; i1++)
        {
            var equations1 = equations
                .Concat([new Equation(buttons.Select((b, i) => i == index1 ? 1 : 0).ToArray(), i1)]).ToArray();

            (solution, solvable) = IntEquationSolver.SolveIntegers(equations1);

            if (extraSteps == 1)
            {
                if (solvable.Count == buttons.Length && solution.All(s => s >= 0))
                    minCount = Math.Min(minCount, solution.Sum());
            }
            else
            {
                int? index2 = indexes.FirstOrDefault(i => !solvable.Contains(i));
                int max2 = buttons[index2.Value].Min(ind => counters[ind]);
                for (int i2 = 0; i2 <= max2; i2++)
                {
                    var equations2 = equations1
                        .Concat([new Equation(buttons.Select((b, i) => i == index2 ? 1 : 0).ToArray(), i2)]).ToArray();
                    (solution, solvable) = IntEquationSolver.SolveIntegers(equations2);

                    if (extraSteps == 2)
                    {
                        if (solvable.Count == buttons.Length && solution.All(s => s >= 0))
                            minCount = Math.Min(minCount, solution.Sum());
                    }
                    else
                    {
                        int? index3 = indexes.FirstOrDefault(i => !solvable.Contains(i));
                        int max3 = buttons[index3.Value].Min(ind => counters[ind]);
                        for (int i3 = 0; i3 <= max3; i3++)
                        {
                            var equations3 = equations2
                                .Concat([
                                    new Equation(buttons.Select((b, i) => i == index3 ? 1 : 0).ToArray(), i3)
                                ]).ToArray();

                            (solution, solvable) = IntEquationSolver.SolveIntegers(equations3);
                            if (solvable.Count == buttons.Length && solution.All(s => s >= 0))
                            {
                                minCount = Math.Min(minCount, solution.Sum());
                            }
                        }
                    }
                }
            }
        }

        if (minCount == int.MaxValue)
            minCount = FindPart2MinSteps(counters, buttons, minExtraSteps + 1);
        return minCount;
    }

    private static int DoPart1(Hoi[] lines)
    {
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

        return answer1;
    }


    private Hoi ParseInput(string line)
    {
        var parts = line.Split(['[', ']', '{', '}'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        var buttons = parts[1].Replace(" ", "").Split(['(', ')'], StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s.GetInts())
            .ToArray();

        return new Hoi(parts[0].Select(c => c == '#').ToArray(), buttons, parts[^1].GetInts());

    }

    private record Hoi(bool[] Indicators, int[][] Buttons, int[] Counters);
}
public class Equation
{
    public int[] Coefficients;
    public int Result;

    public Equation(int[] coefficients, int result)
    {
        Coefficients = coefficients;
        Result = result;
    }

    // Subtract factor * other from this equation
    public void SubtractMultiple(Equation other, int factor)
    {
        for (int i = 0; i < Coefficients.Length; i++)
            Coefficients[i] -= other.Coefficients[i] * factor;
        Result -= other.Result * factor;
    }
}

class IntEquationSolver
{
    public static (int[] solution, List<int> solvable) SolveIntegers(Equation[] eqs)
    {
        int numVars = eqs[0].Coefficients.Length;
        int numEqs = eqs.Length;

        // Create a working copy of equations
        var matrix = new Equation[numEqs];
        for (int i = 0; i < numEqs; i++)
            matrix[i] = new Equation((int[])eqs[i].Coefficients.Clone(), eqs[i].Result);

        // Track which column is the pivot for each row (-1 if no pivot)
        int[] pivotCol = new int[numEqs];
        for (int i = 0; i < numEqs; i++)
            pivotCol[i] = -1;

        // Track which row contains the pivot for each column (-1 if no pivot)
        int[] pivotRow = new int[numVars];
        for (int i = 0; i < numVars; i++)
            pivotRow[i] = -1;

        // Gaussian elimination to row echelon form
        int currentRow = 0;
        for (int col = 0; col < numVars && currentRow < numEqs; col++)
        {
            int pivotIdx = -1;
            int minAbs = int.MaxValue;

            for (int row = currentRow; row < numEqs; row++)
            {
                int val = Math.Abs(matrix[row].Coefficients[col]);
                if (val != 0 && val < minAbs)
                {
                    minAbs = val;
                    pivotIdx = row;
                }
            }

            if (pivotIdx == -1)
                continue; // No pivot in this column

            // Swap rows
            (matrix[currentRow], matrix[pivotIdx]) = (matrix[pivotIdx], matrix[currentRow]);

            pivotCol[currentRow] = col;
            pivotRow[col] = currentRow;

            // Eliminate this column in all other rows
            for (int row = 0; row < numEqs; row++)
            {
                if (row == currentRow)
                    continue;

                int targetCoeff = matrix[row].Coefficients[col];
                if (targetCoeff == 0)
                    continue;

                int pivotCoeff = matrix[currentRow].Coefficients[col];

                // Find GCD to minimize coefficient growth
                int g = Gcd(Math.Abs(targetCoeff), Math.Abs(pivotCoeff));
                int multTarget = pivotCoeff / g;
                int multPivot = targetCoeff / g;

                // Scale row and subtract: row = row * multTarget - pivotRow * multPivot
                for (int c = 0; c < numVars; c++)
                    matrix[row].Coefficients[c] = matrix[row].Coefficients[c] * multTarget - matrix[currentRow].Coefficients[c] * multPivot;
                matrix[row].Result = matrix[row].Result * multTarget - matrix[currentRow].Result * multPivot;

                // Reduce coefficients by their GCD to prevent overflow
                ReduceRow(matrix[row]);
            }

            currentRow++;
        }

        // Identify solvable variables (those with a pivot and no free variables in their row)
        var solution = new int[numVars];
        var solvable = new List<int>();

        for (int col = 0; col < numVars; col++)
        {
            int row = pivotRow[col];
            if (row == -1)
                continue; // Free variable

            // Check if this row has only the pivot variable (all other coefficients are 0)
            bool uniquelySolvable = true;
            for (int c = 0; c < numVars; c++)
            {
                if (c != col && matrix[row].Coefficients[c] != 0)
                {
                    uniquelySolvable = false;
                    break;
                }
            }

            if (uniquelySolvable)
            {
                int coeff = matrix[row].Coefficients[col];
                int constant = matrix[row].Result;

                // Check if it divides evenly (integer solution)
                if (constant % coeff == 0)
                {
                    solution[col] = constant / coeff;
                    solvable.Add(col);
                }
            }
        }

        solvable.Sort();
        return (solution, solvable);
    }

    private static int Gcd(int a, int b)
    {
        while (b != 0)
        {
            int temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }

    private static void ReduceRow(Equation eq)
    {
        int g = 0;
        foreach (int c in eq.Coefficients)
            g = Gcd(g, Math.Abs(c));
        g = Gcd(g, Math.Abs(eq.Result));

        if (g > 1)
        {
            for (int i = 0; i < eq.Coefficients.Length; i++)
                eq.Coefficients[i] /= g;
            eq.Result /= g;
        }
    }
}

