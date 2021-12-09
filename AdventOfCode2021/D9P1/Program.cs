namespace D9P1;

public static class Program
{
    private class HeightMap
    {
        private static int[,] Map { get; set; } = null!;
        private static int Rows => Map.GetLength(0);
        private static int Cols => Map.GetLength(1);
        public int RiskLevel { get; private set; }

        public HeightMap(IReadOnlyList<string> input)
        {
            Map = ConvertInput(input);
        }

        private static int[,] ConvertInput(IReadOnlyList<string> input)
        {
            var result = new int[input.Count, input[0].Length];

            for (var row = 0; row < input.Count; row++)
            {
                for (var col = 0; col < input[0].Length; col++)
                {
                    result[row, col] = Convert.ToInt32(input[row][col].ToString());
                }
            }

            return result;
        }

        public void CalculateRiskLevel()
        {
            for (var row = 0; row < Rows; row++)
            {
                for (var col = 0; col < Cols; col++)
                {
                    var (result, riskLevel) = IsLowPoint(row, col);

                    if (result)
                    {
                        RiskLevel += riskLevel;
                    }
                }
            }
        }

        private (bool result, int riskLevel) IsLowPoint(int row, int col)
        {
            var currentValue = Map[row, col];

            var up = row > 0 ? Map[row - 1, col] : currentValue + 1;
            var down = row + 1 < Rows ? Map[row + 1, col] : currentValue + 1;
            var left = col > 0 ? Map[row, col - 1] : currentValue + 1;
            var right = col + 1 < Cols ? Map[row, col + 1] : currentValue + 1;

            return (currentValue < up && currentValue < down && currentValue < left && currentValue < right, currentValue + 1);
        }
    }

    public static void Main()
    {
        var inputs = File.ReadAllLines("input.txt").ToList();

        var heightMap = new HeightMap(inputs);
        heightMap.CalculateRiskLevel();

        Console.WriteLine($"The risk level is: {heightMap.RiskLevel}");
    }


}