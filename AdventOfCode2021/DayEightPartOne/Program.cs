using System;
using System.Collections.Generic;
using System.Linq;

namespace DayEightPartOne
{
    public static class Program
    {
        private class Entry
        {
            private List<string> SignalPatterns { get; }
            private List<string> OutputValue { get; }

            public Entry(List<string> signalPatterns, List<string> outputValue)
            {
                SignalPatterns = signalPatterns;
                OutputValue = outputValue;
            }

            public int HowManyUnique()
            {
                return OutputValue.Count(o => o.Length is 2 or 3 or 4 or 7);
            }
        }

        public static void Main()
        {
            var inputs = System.IO.File.ReadAllLines("input.txt");
            var converted = ConvertInput(inputs);

            var uniqueNumbers = converted.Sum(x => x.HowManyUnique());

            Console.WriteLine($"Digits 1, 4, 7, 8 appear {uniqueNumbers} times in the output signals.");
        }

        private static IEnumerable<Entry> ConvertInput(IEnumerable<string> inputs)
        {
            return inputs.Select(input => input.Split('|')).Select(split =>
                new Entry(split[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList(),
                    split[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList()
                ));
        }

    }
}