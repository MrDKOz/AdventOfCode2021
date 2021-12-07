using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DaySevenPartOne
{
    public static class Program
    {
        public static void Main()
        {
            var input = File.ReadAllLines("input.txt");
            var initialPositions = input.First().Split(',').Select(number => Convert.ToInt32(number)).ToList();

            var maxPos = initialPositions.Max();

            var costs = new List<int>();

            for (var testPos = 0; testPos < maxPos; testPos++)
            {
                var posCost = initialPositions.Select(crabPos => crabPos > testPos
                        ? crabPos - testPos
                        : testPos - crabPos)
                    .ToList();

                costs.Add(posCost.Sum());
            }

            Console.WriteLine($"Lowest cost: {costs.Min()}");
        }
    }
}