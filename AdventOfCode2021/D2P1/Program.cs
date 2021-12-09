namespace D2P1
{
    public enum Directions
    {
        forward,
        up,
        down
    }

    public class Instruction
    {
        public readonly Directions Direction;
        public readonly int Value;

        public Instruction(Directions direction, int value)
        {
            Direction = direction;
            Value = value;
        }
    }

    public static class Program
    {
        public static void Main()
        {
            var inputs = System.IO.File.ReadAllLines("input.txt");

            var horizontal = 0;
            var depth = 0;

            foreach (var input in inputs)
            {
                var instruction = ConvertFromString(input);

                switch (instruction.Direction)
                {
                    case Directions.forward:
                        horizontal += instruction.Value;
                        break;
                    case Directions.up:
                        depth -= instruction.Value;
                        break;
                    case Directions.down:
                        depth += instruction.Value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            Console.WriteLine($"Answer: {horizontal * depth}");
        }

        /// Convert to string into its two parts, and return them.
        private static Instruction ConvertFromString(string input)
        {
            var split = input.Split(" ");

            if (Enum.TryParse<Directions>(split[0], out var direction) &&
                int.TryParse(split[1], out var value))
            {
                return new Instruction(direction, value);
            }

            throw new Exception("Direction/Value was not valid");
        }
    }
}