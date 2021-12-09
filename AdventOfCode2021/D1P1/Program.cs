namespace D1P1;

public static class Program
{
    public static void Main()
    {
        var inputs = System.IO.File.ReadAllLines("input.txt");
        var depthIncreaseCount = 0;
        var previousValue = 0;
        var firstValueDone = false;

        foreach (var recordedDepth in inputs)
        {
            if (int.TryParse(recordedDepth, out var value))
            {
                if (firstValueDone)
                {
                    depthIncreaseCount += value > previousValue ? 1 : 0;
                }
                else
                {
                    firstValueDone = true;
                }

                previousValue = value;
            }
        }

        Console.WriteLine($"Depth increased: {depthIncreaseCount} time(s)");
    }
}