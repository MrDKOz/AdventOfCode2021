namespace D6P1;

public enum Action
{
    Create,
    Nothing
}

public class Family
{
    private HashSet<LanternFish> Fishes { get; } = new();

    public Family(IEnumerable<int> initialBatch)
    {
        foreach (var fish in initialBatch)
        {
            Fishes.Add(new LanternFish(fish));
        }
    }

    public void AdvanceDay()
    {
        var toCreate = Fishes.Count(fish => fish.StepTimer() == Action.Create);

        for (var i = 0; i < toCreate; i++)
        {
            Fishes.Add(new LanternFish(8));
        }
    }

    public string ReportCurrentState()
    {
        return $"{Fishes.Count} Total Fishes";
    }
}

public class LanternFish
{
    private int Timer { get; set; }

    public LanternFish(int initialTimer)
    {
        Timer = initialTimer;
    }

    public Action StepTimer()
    {
        Timer--;

        if (Timer >= 0) return Action.Nothing;
        Timer = 6;
        return Action.Create;
    }
}

public static class Program
{
    public static void Main()
    {
        var inputs = File.ReadAllLines("input.txt");
        var splitInput = inputs.First().Split(',');
        var convertedInput = (from item in splitInput select Convert.ToInt32(item)).ToList();

        var family = new Family(convertedInput);
        const int daysToSimulate = 80;

        Console.WriteLine($"Initial state: {family.ReportCurrentState()}");
        Console.WriteLine("Working...");

        for (var day = 1; day <= daysToSimulate; day++)
        {
            family.AdvanceDay();
        }

        Console.WriteLine($"After {daysToSimulate} day(s): {family.ReportCurrentState()}");
    }
}