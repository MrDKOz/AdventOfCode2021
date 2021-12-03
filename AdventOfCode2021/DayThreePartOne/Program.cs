using System;
using System.Collections.Generic;
using System.Text;

namespace DayThreePartOne
{
    public static class Program
    {
        public static void Main()
        {
            var inputs = System.IO.File.ReadAllLines("input.txt");

            var colSumArray = GetSumOfEachBitColumn(inputs);

            var gammaRate = CalculateBinaryAndConvert(colSumArray, inputs.Length);
            var epsilonRate = CalculateBinaryAndConvert(colSumArray, inputs.Length, false);

            Console.WriteLine($"Gamma: {gammaRate} | Epsilon: {epsilonRate}");
            Console.WriteLine($"Answer: {gammaRate * epsilonRate}");
        }

        private static int[] GetSumOfEachBitColumn(IReadOnlyList<string> inputs)
        {
            var colSumArray = new int[inputs[0].Length];

            foreach (var input in inputs)
            {
                for (var i = 0; i < input.Length; i++)
                {
                    var value = input[i].ToString();
                    colSumArray[i] += Convert.ToInt32(value);
                }
            }

            return colSumArray;
        }

        private static int CalculateBinaryAndConvert(IEnumerable<int> sumArray, int inputCount, bool mostCommon = true)
        {
            var binaryString = new StringBuilder();

            foreach (var sum in sumArray)
            {
                if (mostCommon)
                {
                    binaryString.Append(sum > inputCount / 2 ? 1 : 0);
                }
                else
                {
                    binaryString.Append(sum < inputCount / 2 ? 1 : 0);
                }
            }

            return Convert.ToInt32(binaryString.ToString(), 2);
        }
    }
}