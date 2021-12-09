namespace D7P2;

public static class Program
{
    public static void Main()
    {
        var input = File.ReadAllLines("input.txt");
        var initialPositions = input.First().Split(',').Select(number => Convert.ToInt32(number)).ToList();

        var maxPos = initialPositions.Max();

        var costs = new List<long>();

        for (var testPos = 0; testPos < maxPos; testPos++)
        {
            var posCost = initialPositions.Select(initialPosition => initialPosition > testPos
                    ? ConvertCost(initialPosition - testPos)
                    : ConvertCost(testPos - initialPosition))
                .ToList();

            costs.Add(posCost.Sum());
        }

        Console.WriteLine($"Lowest cost: {costs.Min()}");
    }

    private static int ConvertCost(int cost)
    {
        var convertedCost = 0;

        for (var i = 1; i <= cost; i++)
        {
            convertedCost += i;
        }

        return convertedCost;
    }
}