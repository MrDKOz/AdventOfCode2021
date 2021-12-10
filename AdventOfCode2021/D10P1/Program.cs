namespace D10P1;

public static class Program
{
    private record Pairing(int Id, int Score, char Open, char Close);

    private static readonly List<Pairing> Pairings = new()
    {
        new Pairing(1, 3, '(', ')'),
        new Pairing(2, 57, '[', ']'),
        new Pairing(3, 1197, '{', '}'),
        new Pairing(4, 25137, '<', '>')
    };

    private static void Main()
    {
        var inputs = File.ReadAllLines("input.txt");

        var currentStack = new Stack<char>();
        var invalidSyntax = new List<Pairing>();

        foreach (var line in inputs)
        {
            foreach (var character in line)
            {
                if (Pairings.Any(o => o.Open == character))
                {
                    currentStack.Push(character);
                }
                else if (Pairings.Any(o => o.Close == character))
                {
                    if (Pairings.Single(p => p.Open == currentStack.Peek()).Id ==
                        Pairings.Single(p => p.Close == character).Id)
                    {
                        currentStack.Pop();
                    }
                    else
                    {
                        invalidSyntax.Add(Pairings.Single(p => p.Close == character));
                        Console.WriteLine($"Syntax error, expected '{Pairings.Single(p => p.Open == currentStack.Peek()).Close}' but got '{character}'");
                        break;
                    }
                }
            }

            currentStack.Clear();
        }

        var points = (from pairing in Pairings
            let count = invalidSyntax.Count(x => x.Id == pairing.Id)
            select count * pairing.Score).ToList().Aggregate(0, (current, point) => current + point);

        Console.WriteLine($"Result: {points}");
    }
}