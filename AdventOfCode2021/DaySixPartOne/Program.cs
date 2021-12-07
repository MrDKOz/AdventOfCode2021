using System;
using System.Collections.Generic;

namespace DaySixPartOne
{
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
            var toCreate = 0;

            foreach (var fish in Fishes)
            {
                if (fish.StepTimer() == Action.Create)
                {
                    toCreate++;
                }
            }

            for (int i = 0; i < toCreate; i++)
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
        public int Timer { get; set; }

        public LanternFish(int initialTimer)
        {
            Timer = initialTimer;
        }

        public Action StepTimer()
        {
            Timer--;

            if (Timer < 0)
            {
                Timer = 6;
                return Action.Create;
            }

            return Action.Nothing;
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
            var daysToSimulate = 80;

            Console.WriteLine($"Initial state: {family.ReportCurrentState()}");
            Console.WriteLine("Working...");

            for (int day = 1; day <= daysToSimulate; day++)
            {
                family.AdvanceDay();
            }

            Console.WriteLine($"After {daysToSimulate} day(s): {family.ReportCurrentState()}");
        }
    }
}