namespace D4P1
{
    public class AllBoards
    {
        private readonly List<Board> _boards;

        public AllBoards(List<Board> allBoards)
        {
            _boards = allBoards;
        }

        public (bool won, int sumOfAllUnmarked, int lastNumber) SearchAndAddMatch(List<int> numbersToMatch)
        {
            foreach (var result in _boards.Select(board => board.SearchAndAddMatch(numbersToMatch))
                .Where(result => result.won))
            {
                return result;
            }

            return (false, -1, -1);
        }
    }

    public class Board
    {
        private int[,] Numbers { get; } = new int[5, 5];
        private int[,] Matches { get; } = new int[5, 5];
        private int MatchesCount { get; set; }
        public int Id { get; set; }

        public void AddNumber(int row, int col, int value)
        {
            Numbers[row, col] = value;
        }

        public (bool won, int sumOfAllUnmarked, int lastNumber) SearchAndAddMatch(List<int> numbersToMatch)
        {
            foreach (var number in numbersToMatch)
            {
                for (var col = 0; col < 5; col++)
                {
                    for (var row = 0; row < 5; row++)
                    {
                        if (Numbers[col, row] != number) continue;

                        Matches[col, row] = -1;
                        MatchesCount++;

                        if (!HasWon()) continue;

                        DrawBoard();
                        DrawMatches();

                        return (true, SumAllUnmarked(), number);
                    }
                }
            }

            return (false, -1, -1);
        }

        private int SumAllUnmarked()
        {
            var currentSum = 0;

            for (var row = 0; row < 5; row++)
            {
                for (var col = 0; col < 5; col++)
                {
                    var value = Matches[row, col];

                    if (value == 0)
                    {
                        currentSum += Numbers[row, col];
                    }
                }
            }

            return currentSum;
        }

        private bool HasWon()
        {
            if (MatchesCount < 5)
            {
                return false;
            }

            for (var row = 0; row < 5; row++)
            {
                if (Matches[row, 0] == -1 && Matches[row, 1] == -1 && Matches[row, 2] == -1 && Matches[row, 3] == -1 && Matches[row, 4] == -1)
                {
                    return true;
                }
            }

            for (var col = 0; col < 5; col++)
            {
                if (Matches[0, col] == -1 && Matches[1, col] == -1 && Matches[2, col] == -1 && Matches[3, col] == -1 && Matches[4, col] == -1)
                {
                    return true;
                }
            }

            return false;
        }

        private void DrawBoard()
        {
            Console.WriteLine($"Drawing board: {Id}");
            for (var row = 0; row < 5; row++)
            {
                Console.WriteLine($"{Numbers[row, 0]} {Numbers[row, 1]} {Numbers[row, 2]} {Numbers[row, 3]} {Numbers[row, 4]}");
            }
        }

        private void DrawMatches()
        {
            Console.WriteLine($"Drawing board: {Id}");
            for (var row = 0; row < 5; row++)
            {
                Console.WriteLine($"{Matches[row, 0]} {Matches[row, 1]} {Matches[row, 2]} {Matches[row, 3]} {Matches[row, 4]}");
            }
        }
    }

    public class BingoCaller
    {
        private readonly List<int> _numbers;
        private int _timesCalled;

        public BingoCaller(List<int> numbers)
        {
            _numbers = numbers;
        }

        public List<int> Call()
        {
            var numbersToCall = _numbers.Skip(5 * _timesCalled).Take(5).ToList();

            _timesCalled++;

            return numbersToCall;
        }
    }

    public static class Program
    {
        private static BingoCaller Caller { get; set; } = null!;

        public static void Main()
        {
            var boards = File.ReadAllLines("boards.txt");
            var numbers = File.ReadAllLines("numbers.txt");

            var convertedBoards = ConvertBoardsFromRaw(boards);
            var convertedNumbers = ConvertNumbersFromRaw(numbers);

            Caller = new BingoCaller(convertedNumbers);

            PlayBingo(new AllBoards(convertedBoards));

            Console.WriteLine($"Game Over! Thank you for playing.");
        }

        private static void PlayBingo(AllBoards boards)
        {
            var foundAWinner = false;
            var roundCounter = 1;

            while (!foundAWinner)
            {
                var numbersForRound = Caller.Call();

                Console.WriteLine($"[Bingo Caller] Numbers for round #{roundCounter} are {string.Join(", ", numbersForRound)}!");

                var (won, sumOfAllUnmarked, lastNumber) = boards.SearchAndAddMatch(numbersForRound);

                if (won)
                {
                    Console.WriteLine("[Bingo Caller] We have a winner!");
                    Console.WriteLine($"Unmarked Sum: {sumOfAllUnmarked} | Last Number: {lastNumber}");
                    Console.WriteLine($"Answer: {sumOfAllUnmarked * lastNumber}");
                }
                else
                {
                    Console.WriteLine($"[Bingo Caller] Onto round #{++roundCounter}!");
                }

                foundAWinner = won;
            }
        }

        private static List<int> ConvertNumbersFromRaw(IEnumerable<string> rawNumbers)
        {
            var numbers = new List<int>();

            var tempLine = rawNumbers.First().Split(',');

            numbers.AddRange(from number in tempLine
                             select Convert.ToInt32(number));

            return numbers;
        }

        private static List<Board> ConvertBoardsFromRaw(IEnumerable<string> rawBoards)
        {
            var boards = new List<Board>();
            var tempBoard = new Board();

            var row = 0;
            var boardId = 0;

            foreach (var line in rawBoards)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    var numbers = line.Replace("  ", " ").Split(' ', StringSplitOptions.RemoveEmptyEntries);

                    for (var col = 0; col < 5; col++)
                    {
                        tempBoard.AddNumber(row, col, Convert.ToInt32(numbers[col]));
                    }

                    row++;
                }
                else
                {
                    tempBoard.Id = boardId;
                    boards.Add(tempBoard);
                    tempBoard = new Board();

                    row = 0;
                    boardId++;
                }
            }

            boards.Add(tempBoard);

            return boards;
        }
    }
}