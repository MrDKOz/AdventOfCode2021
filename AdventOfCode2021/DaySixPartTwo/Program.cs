using System;
using System.Linq;
using System.Text;

namespace DaySixPartTwo
{
    public class FishRecord
    {
        public int Id { get; set; }
        public int Age { get; set; }

        public long Count { get; set; }
    }

    public class FishCensus
    {
        private const int maxAge = 8;

        public Stack<FishRecord> FishAges { get; set; } = new Stack<FishRecord>();

        public FishCensus(IEnumerable<int> initialBatch)
        {
            for (int i = 0; i <= maxAge; i++)
            {
                FishAges.Push(new FishRecord
                {
                    Id = i,
                    Age = i,
                    Count = 0
                });
            }

            foreach (var fish in initialBatch)
            {
                AddFish(fish);
            }
        }

        public void AddFish(int currentTimer)
        {
            FishAges.Single(fa => fa.Age == currentTimer).Count++;
        }

        public void AdvancedDay()
        {
            for (int groupId = 0; groupId < FishAges.Count; groupId++)
            {
                var fishAgeGroup = FishAges.Single(fa => fa.Id == groupId);

                switch (fishAgeGroup.Age)
                {
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                    case 6:
                    case 7:
                    case 8:
                    default:
                        break;
                }
            }
        }
    }

    public static class Program
    {
        public static void Main()
        {
            var inputs = File.ReadAllLines("input.txt");
            var splitInput = inputs.First().Split(',');
            var convertedInput = (from item in splitInput select Convert.ToInt32(item)).ToList();


           
            Console.WriteLine($"After day(s):");
        }
    }
}