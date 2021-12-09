namespace D8P2;

public static class Program
{
    private class Entry
    {
        private List<string> SignalPatterns { get; }
        private List<string> Outputs { get; }

        public Entry(IEnumerable<string> signalPatterns, IEnumerable<string> outputValue)
        {
            SignalPatterns = SortPatterns(signalPatterns);
            Outputs = SortPatterns(outputValue);
        }

        /// <summary>
        /// Annoyingly the patterns aren't consistent in their ordering, so during initialization
        /// let's order the characters within the pattern to avoid issues later.
        /// </summary>
        /// <param name="patterns">The list of the patterns to order e.g. cdfgba</param>
        /// <returns>A list containing all patterns, but now sorted e.g. abcdfg</returns>
        private static List<string> SortPatterns(IEnumerable<string> patterns)
        {
            var result = new List<string>();

            foreach (var pattern in patterns)
            {
                var characters = pattern.ToCharArray();
                Array.Sort(characters);

                result.Add(new string(characters));
            }

            return result;
        }

        /// <summary>
        /// The most annoying class, this converts the 10 entries in our Signal Patterns into the numbers
        /// they're meant to be, we do this by eliminating each number using rules that rely on earlier findings.
        /// </summary>
        /// <returns>A dictionary with the number as the key, and its pattern as the value.</returns>
        private Dictionary<int, string> DecodeLeftSide()
        {
            // Get the ones we know
            var result = new Dictionary<int, string>
            {
                { 1, SignalPatterns.Single(p => p.Length == 2) },
                { 4, SignalPatterns.Single(p => p.Length == 4) },
                { 7, SignalPatterns.Single(p => p.Length == 3) },
                { 8, SignalPatterns.Single(p => p.Length == 7) }
            };

            // This gets 2, 3, and 5
            var lengthOfFive = SignalPatterns.Where(p => p.Length == 5).ToList();

            // This gets 0, 6, and 9
            var lengthOfSix = SignalPatterns.Where(p => p.Length == 6).ToList();

            //
            // We should now have all numbers, now to take 'em out one by one.
            //

            // 3 needs 5 segments, and contains all of 1's segments
            result.Add(3, lengthOfFive.Single(p => result[1].All(p.Contains)));
            lengthOfFive.Remove(result[3]);

            // 9 needs 6 segments, and contains all of 3's segments
            result.Add(9, lengthOfSix.Single(p => result[3].All(p.Contains)));
            lengthOfSix.Remove(result[9]);

            // 0 needs 6 segments, and contains all of 1
            result.Add(0, lengthOfSix.Single(p => result[1].All(p.Contains)));
            lengthOfSix.Remove(result[0]);

            // 6 should be the only one left with a length of 6
            result.Add(6, lengthOfSix.Single());
            lengthOfSix.Remove(result[6]);

            // 5 needs 5 segments, which are all contained within 6
            result.Add(5, lengthOfFive.Single(p => p.All(result[6].Contains)));
            lengthOfFive.Remove(result[5]);

            // 2 should be the only one left with a length of 5
            result.Add(2, lengthOfFive.Single());
            lengthOfFive.Remove(result[2]);

            if (lengthOfFive.Any() || lengthOfSix.Any())
            {
                throw new Exception("Something hasn't been detected... again");
            }

            return result;
        }

        /// <summary>
        /// Simple lookup of our output patterns against our "Rosetta Stone" and returning the values.
        /// </summary>
        /// <param name="rosetta">List of all 10 ints as well as their patterns.</param>
        /// <returns>The decoded output value.</returns>
        private int DecodeRightSide(Dictionary<int, string> rosetta)
        {
            var decoded = Outputs.Select(value => rosetta.Single(d => d.Value == value).Key).ToList();

            return decoded.Aggregate(0, (current, number) => 10 * current + number);
        }

        public int DecodeAndReturnSum()
        {
            var rosetta = DecodeLeftSide();
            var decodedOutput = DecodeRightSide(rosetta);

            return decodedOutput;
        }
    }

    public static void Main()
    {
        var inputs = File.ReadAllLines("input.txt");
        var converted = ConvertInput(inputs);
        var totalSumOfOutput = converted.Sum(entry => entry.DecodeAndReturnSum());

        Console.WriteLine($"Total sum of all output values: {totalSumOfOutput}");
    }

    private static IEnumerable<Entry> ConvertInput(IEnumerable<string> inputs)
    {
        return inputs.Select(input => input.Split('|')).Select(split =>
            new Entry(split[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList(),
                split[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList()
            ));
    }
}