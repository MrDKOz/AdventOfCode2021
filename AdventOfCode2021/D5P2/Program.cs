using System.Numerics;
using System.Text;

namespace D5P2
{
    public enum Direction
    {
        Horizontal,
        Vertical,
        Diagonal
    }

    public class Vent
    {
        public Vector2 Start { get; init; }
        public Vector2 End { get; init; }
        public Direction Direction { get; set; }

        public List<Vector2> CalculatePath()
        {
            var path = new List<Vector2>();

            switch (Direction)
            {
                case Direction.Horizontal:
                    if (Start.X < End.X)
                    {
                        for (var x = Start.X; x <= End.X; x++)
                        {
                            path.Add(new Vector2
                            {
                                X = x,
                                Y = Start.Y
                            });
                        }
                    }
                    else
                    {
                        for (var x = Start.X; x >= End.X; x--)
                        {
                            path.Add(new Vector2
                            {
                                X = x,
                                Y = Start.Y
                            });
                        }
                    }
                    break;
                case Direction.Vertical:
                    if (Start.Y < End.Y)
                    {
                        for (var y = Start.Y; y <= End.Y; y++)
                        {
                            path.Add(new Vector2
                            {
                                X = Start.X,
                                Y = y
                            });
                        }
                    }
                    else
                    {
                        for (var y = Start.Y; y >= End.Y; y--)
                        {
                            path.Add(new Vector2
                            {
                                X = Start.X,
                                Y = y
                            });
                        }
                    }
                    break;
                case Direction.Diagonal:
                    path.Add(Start);

                    var xIncrement = CalculateDirection(Start.X, End.X);
                    var yIncrement = CalculateDirection(Start.Y, End.Y);

                    var xCoord = Start.X;
                    var yCoord = Start.Y;

                    do
                    {
                        xCoord += xIncrement;
                        yCoord += yIncrement;

                        path.Add(new Vector2
                        {
                            X = xCoord,
                            Y = yCoord
                        });
                    } while (xCoord != End.X || yCoord != End.Y);
                    break;
            }

            return path;
        }

        private static int CalculateDirection(float start, float end)
        {
            if (start == end) return 0;
            if (start > end) return -1;
            if (start < end) return 1;

            throw new Exception("No");
        }
    }

    public static class Program
    {
        private const int Rows = 1000;
        private const int Columns = 1000;

        public static void Main()
        {
            var input = File.ReadAllLines("input.txt");

            var converted = ConvertRawInput(input);

            var map = MapVents(converted);

            var overlaps = CountOverlaps(map);

            DrawMap(map);

            Console.WriteLine($"There are {overlaps} places where >=2 vent lines overlap!");
        }

        private static List<Vent> ConvertRawInput(IEnumerable<string> input)
        {
            var vents = new List<Vent>();

            foreach (var line in input)
            {
                var temp = line.Split(" -> ");

                var tempStart = temp[0].Split(',');
                var tempEnd = temp[1].Split(',');

                var newVent = new Vent
                {
                    Start = new Vector2
                    {
                        X = int.Parse(tempStart[1]),
                        Y = int.Parse(tempStart[0])
                    },
                    End = new Vector2
                    {
                        X = int.Parse(tempEnd[1]),
                        Y = int.Parse(tempEnd[0])
                    }
                };

                newVent.Direction = DirectionOfLine(newVent.Start, newVent.End);

                vents.Add(newVent);
            }

            return vents;
        }

        private static Direction DirectionOfLine(Vector2 start, Vector2 end)
        {
            var vertical = start.X == end.X;
            var horizontal = start.Y == end.Y;

            if (horizontal)
            {
                return Direction.Horizontal;
            }
            else if (vertical)
            {
                return Direction.Vertical;
            }
            else
            {
                return Direction.Diagonal;
            }
        }

        private static int[,] MapVents(List<Vent> ventStartAndEnds)
        {
            var map = new int[Rows, Columns];

            foreach (var ventLine in ventStartAndEnds)
            {
                var path = ventLine.CalculatePath();

                foreach (var step in path)
                {
                    map[(int)step.X, (int)step.Y] += 1;
                }
            }

            return map;
        }

        private static int CountOverlaps(int[,] map)
        {
            var overlaps = 0;

            for (var x = 0; x < map.GetLength(0); x++)
            {
                for (var y = 0; y < map.GetLength(1); y++)
                {
                    var currentValue = map[x, y];

                    if (currentValue > 1)
                    {
                        overlaps++;
                    }

                }
            }

            return overlaps;
        }

        private static void DrawMap(int[,] map)
        {
            var mapper = new StringBuilder();

            for (var x = 0; x < map.GetLength(0); x++)
            {
                for (var y = 0; y < map.GetLength(1); y++)
                {
                    var currentValue = map[x, y];

                    mapper.Append(currentValue > 0 ? $" {currentValue}" : " .");
                }

                mapper.AppendLine();
            }

            File.WriteAllText(@"C:\\test\test.txt", mapper.ToString());
        }
    }
}