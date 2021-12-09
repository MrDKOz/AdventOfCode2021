namespace D9P2;

public static class Program
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    /// <summary>
    /// Vector2 uses float which is annoying in instances where you only need X and Y int precision.
    /// </summary>
    public class Vector2Int
    {
        public int X { get; }
        public int Y { get; }

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
                Direction.Right => new Vector2Int(X, Y + 1),
                _ => throw new Exception("This shouldn't happen...")
            };
        }
    }

    private class HeightMap
    {
        internal int[,] Map { get; }
        public int Rows => Map.GetLength(0);
        public int Cols => Map.GetLength(1);

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

    private class Measurer
    {
        private readonly HeightMap _map;
        private readonly List<Vector2Int> _lowPoints;

        private readonly HashSet<Vector2Int> _visited = new();
        private readonly Queue<Vector2Int> _toVisit = new();

        public Measurer(HeightMap map, List<Vector2Int> lowPoints)
        {
            _map = map;
            _lowPoints = lowPoints;
        }

        public int Calculate()
        {
            var basinMeasurements = _lowPoints.Select(MeasureBasin).ToList();

            basinMeasurements.Sort();
            basinMeasurements.Reverse();

            return basinMeasurements.Take(3).Aggregate(1, (current, size) => current * size);
        }

        private int MeasureBasin(Vector2Int startingPoint)
        {
            _toVisit.Enqueue(startingPoint);
            GetAvailableNeighbours(startingPoint);

            while (_toVisit.Any())
            {
                var current = _toVisit.Dequeue();
                _visited.Add(current);

                GetAvailableNeighbours(current);
            }

            var result = _visited.Count;

            ResetTracking();

            return result;
        }

        private void ResetTracking()
        {
            _visited.Clear();
            _toVisit.Clear();
        }

        private void GetAvailableNeighbours(Vector2Int startingPoint)
        {
            foreach (var direction in Enum.GetValues(typeof(Direction)).Cast<Direction>())
            {
                if (!CheckIfValidMove(direction, startingPoint)) continue;

                var newLocation = startingPoint.Look(direction);

                if (!_visited.Any(v => v.X == newLocation.X && v.Y == newLocation.Y) &&
                    !_toVisit.Any(v => v.X == newLocation.X && v.Y == newLocation.Y))
                {
                    _toVisit.Enqueue(newLocation);
                }
            }
        }

        private bool CheckIfValidMove(Direction direction, Vector2Int startingPoint)
        {
            var newCoord = startingPoint.Look(direction);

            return CanReach(direction, newCoord)
                   && _map.Map[newCoord.X, newCoord.Y] != 9
                   && _map.Map[newCoord.X, newCoord.Y] > _map.Map[startingPoint.X, startingPoint.Y];
        }

        private bool CanReach(Direction direction, Vector2Int check)
        {
            return direction switch
            {
                Direction.Up => check.X >= 0,
                Direction.Down => check.X < _map.Rows,
                Direction.Left => check.Y >= 0,
                Direction.Right => check.Y < _map.Cols,
                _ => throw new Exception("This shouldn't happen...")
            };
        }
    }

    public static void Main()
    {
        var inputs = File.ReadAllLines("input.txt").ToList();

        var map = new HeightMap(inputs);
        var measure = new Measurer(map, map.FindLowPoints());

        Console.WriteLine($"Answer: {measure.Calculate()}");
    }
}