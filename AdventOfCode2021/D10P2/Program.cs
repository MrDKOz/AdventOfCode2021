namespace D10P2;

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
        var incompleteLines = ExtractIncompleteLines(inputs);
        var score = FinishIncompleteLines(incompleteLines);

        Console.WriteLine($"Result: {score}");
    }

    private static long FinishIncompleteLines(IEnumerable<string> incompleteLines)
    {
        var allScores = new List<long>();

        var currentStack = new Stack<char>();

        foreach (var incompleteLine in incompleteLines)
        {
            long lineScore = 0;

            // Build up current un-paired characters
            foreach (var character in incompleteLine)
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
                }
            }

            // Finish the line
            while (currentStack.Any())
            {
                var current = currentStack.Pop();

                lineScore *= 5;

                switch (current)
                {
                    case '(':
                        lineScore += 1;
                        break;
                    case '[':
                        lineScore += 2;
                        break;
                    case '{':
                        lineScore += 3;
                        break;
                    case '<':
                        lineScore += 4;
                        break;
                }
            }

            if (lineScore < 0)
            {
                Console.WriteLine("What?");
            }
            allScores.Add(lineScore);
        }

        allScores.Sort();

        return allScores[allScores.Count / 2];
    }

    private static IEnumerable<string> ExtractIncompleteLines(IEnumerable<string> input)
    {
        var currentStack = new Stack<char>();
        var invalidSyntax = new List<Pairing>();
        var incompleteLines = new List<string>();

        foreach (var line in input)
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
                        break;
                    }
                }
            }

            if (invalidSyntax.Any())
            {
                currentStack.Clear();
                invalidSyntax.Clear();
            }
            else
            {
                incompleteLines.Add(line);
            }
        }

        return incompleteLines;
    }
}