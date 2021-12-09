namespace D1P2;

public static class Program
{
    public static void Main()
    {
        var inputs = File.ReadAllLines("input.txt");

        var previousValue = 0;
        var depthIncreaseCount = 0;
        var firstValueDone = false;

        for (var i = 0; i <= inputs.Length - 3; i++)
        {
            var calculated = Calculate(inputs[i], inputs[i + 1], inputs[i + 2]);

            if (firstValueDone)
            {
                depthIncreaseCount += calculated > previousValue ? 1 : 0;
            }
            else
            {
                firstValueDone = true;
            }

            previousValue = calculated;
        }

        Console.WriteLine($"Depth increased: {depthIncreaseCount} time(s)");
    }

    private static int Calculate(string val1, string val2, string val3)
    {
        return Convert.ToInt32(val1) + Convert.ToInt32(val2) + Convert.ToInt32(val3);
    }
}