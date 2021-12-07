using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DaySixPartTwo
{
    public class FishCensus
    {
        private long[] Ages { get; } = new long[9];
        private const int MaxAge = 8;

        public FishCensus(IEnumerable<long> initialBatch)
        {
            foreach (var age in initialBatch)
            {
                Ages[age]++;
            }
        }

        public long SimulateNumberOfDays(int daysToSimulate)
        {
            for (var day = 0; day < daysToSimulate; day++)
            {
                var lastAge = Ages[0];

                for (var ageFocus = 0; ageFocus < MaxAge; ageFocus++)
                {
                    Ages[ageFocus] = Ages[ageFocus + 1];
                }

                Ages[6] += lastAge;
                Ages[8] = lastAge;
            }

            return Ages.Sum();
        }
    }

    public static class Program
    {
        private const int DaysToSimulate = 18;

        public static void Main()
        {
            var inputs = File.ReadAllLines("input.txt");
            var splitInput = inputs.First().Split(',');
            var convertedInput = (from item in splitInput select Convert.ToInt64(item)).ToList();

            var fishCensus = new FishCensus(convertedInput);

            Console.WriteLine("Calculating...");

            var result = fishCensus.SimulateNumberOfDays(256);
           
            Console.WriteLine($"After {DaysToSimulate} day(s): {result}");
        }
    }
}