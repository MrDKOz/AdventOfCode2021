using System.Text;

namespace D3P2
{
    public static class Program
    {
        public static void Main()
        {
            var inputs = System.IO.File.ReadAllLines("input.txt");

            var oxygenRating = NarrowDown(inputs, true);
            var scrubberRating = NarrowDown(inputs, false);

            Console.WriteLine($"Oxygen: {oxygenRating} | CO2: {scrubberRating}");
            Console.WriteLine($"Answer: {oxygenRating * scrubberRating}");
        }

        private static int NarrowDown(IEnumerable<string> inputs, bool mostCommon)
        {
            var tempList = inputs.ToList();
            var columnIdx = 0;

            while (tempList.Count > 1)
            {
                var mask = GetMask(tempList, mostCommon);
                var matches = tempList.Count(m => m[columnIdx] == mask[columnIdx]);
                var equalAmount = matches == tempList.Count / 2;

                if (equalAmount)
                { // Equal number
                    tempList.RemoveAll(i => i[columnIdx] == (mostCommon ? '0' : '1'));
                }
                else
                { // Zero or One favoured
                    tempList.RemoveAll(i => i[columnIdx] != mask[columnIdx]);
                }

                columnIdx++;
            }

            return Convert.ToInt32(tempList.Single(), 2);
        }

        private static string GetMask(IReadOnlyList<string> inputs, bool mostCommon)
        {
            var sumArray = GetSumOfEachBitColumn(inputs);

            return ConvertSumToBinaryString(sumArray, inputs.Count, mostCommon);
        }

        private static IEnumerable<int> GetSumOfEachBitColumn(IReadOnlyList<string> inputs)
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

        private static string ConvertSumToBinaryString(IEnumerable<int> sumArray, int inputCount, bool mostCommon)
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

            return binaryString.ToString();
        }
    }
}