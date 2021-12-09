namespace D9P2
{
    public static class Program
    {
        private class Vector2Int
        {
            public int X { get; set; }
            public int Y { get; set; }

            public Vector2Int(int x, int y)
            {
                X = x;
                Y = y;
            }

            public Vector2Int Look(Direction direction)
            {
                return direction switch
                {
                    Direction.Up => new Vector2Int(X - 1, Y),
                    Direction.Down => new Vector2Int(X + 1, Y),
                    Direction.Left => new Vector2Int(X, Y - 1),
                    Direction.Right => new Vector2Int(X, Y + 1)
                };
            }
        }

        public enum Direction
        {
            Up,
            Down,
            Left,
            Right
        }

        private class HeightMap
        {
            private static int[,] Map { get; set; } = null!;
            private static int Rows => Map.GetLength(0);
            private static int Cols => Map.GetLength(1);

            public HeightMap(IReadOnlyList<string> input)
            {
                Map = ConvertInput(input);
            }

            private static int[,] ConvertInput(IReadOnlyList<string> input)
            {
                var result = new int[input.Count, input[0].Length];

                for (var row = 0; row < input.Count; row++)
                {
                    for (var col = 0; col < input[0].Length; col++)
                    {
                        result[row, col] = Convert.ToInt32(input[row][col].ToString());
                    }
                }

                return result;
            }

            public List<Vector2Int> FindLowPoints()
            {
                var lowPoints = new List<Vector2Int>();

                for (var row = 0; row < Rows; row++)
                {
                    for (var col = 0; col < Cols; col++)
                    {
                        if (IsLowPoint(row, col))
                        {
                            lowPoints.Add(new Vector2Int(row, col));
                        }
                    }
                }

                return lowPoints;
            }

            public HashSet<Vector2Int> Visited = new();
            public Queue<Vector2Int> ToVisit = new();

            public List<Vector2Int> MeasureBasin(Vector2Int startingPoint)
            {
                ToVisit.Enqueue(startingPoint);
                GetAvailableNeighbours(startingPoint);

                while (ToVisit.Any())
                {
                    var current = ToVisit.Dequeue();
                    Visited.Add(current);

                    GetAvailableNeighbours(current);
                }

                return Visited.ToList();
            }

            private void GetAvailableNeighbours(Vector2Int startingPoint)
            {
                foreach (var direction in Enum.GetValues(typeof(Direction)).Cast<Direction>())
                {
                    if (CheckIfValidMove(direction, startingPoint))
                    {
                        var newLocation = startingPoint.Look(direction);

                        if (Visited.Count(v => v.X == newLocation.X && v.Y == newLocation.Y) == 0 &&
                            ToVisit.Count(v => v.X == newLocation.X && v.Y == newLocation.Y) == 0)
                        {
                            ToVisit.Enqueue(newLocation);
                        }
                    }
                }
            }

            private bool CheckIfValidMove(Direction direction, Vector2Int startingPoint)
            {
                var newCoord = startingPoint.Look(direction);

                return CanReach(direction, newCoord) && Map[newCoord.X, newCoord.Y] != 9 && Map[newCoord.X, newCoord.Y] > Map[startingPoint.X, startingPoint.Y];
            }

            private bool CanReach(Direction direction, Vector2Int check)
            {
                return direction switch
                {
                    Direction.Up => check.X >= 0,
                    Direction.Down => check.X < Rows,
                    Direction.Left => check.Y >= 0,
                    Direction.Right => check.Y < Cols,
                    _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
                };
            }

            private bool IsLowPoint(int row, int col)
            {
                var currentValue = Map[row, col];

                var up = row > 0 ? Map[row - 1, col] : currentValue + 1;
                var down = row + 1 < Rows ? Map[row + 1, col] : currentValue + 1;
                var left = col > 0 ? Map[row, col - 1] : currentValue + 1;
                var right = col + 1 < Cols ? Map[row, col + 1] : currentValue + 1;

                return currentValue < up && currentValue < down && currentValue < left && currentValue < right;
            }
        }

        public static void Main()
        {
            var inputs = File.ReadAllLines("input.txt").ToList();

            var map = new HeightMap(inputs);
            var lowPoints = map.FindLowPoints();

            var basins = new List<List<Vector2Int>>();

            foreach (var lowPoint in lowPoints)
            {
                var basinPoints = new List<Vector2Int>();
                basinPoints.AddRange(map.MeasureBasin(lowPoint));

                basins.Add(basinPoints);

                map.Visited = new HashSet<Vector2Int>();
                map.ToVisit = new Queue<Vector2Int>();
            }

            var top = basins.OrderByDescending(b => b.Count).Take(3).ToList();

            var answer = top.Aggregate(1, (current, basin) => current * basin.Count);

            Console.WriteLine($"Answer: {answer}");
        }
    }
}