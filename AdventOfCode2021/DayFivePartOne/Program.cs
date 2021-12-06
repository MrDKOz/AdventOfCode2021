﻿using System.Numerics;
using System.Text;

namespace DayFivePartOne
{
    public enum Direction
    {
        Horizontal,
        Vertical,
        Diagonal
    }

    public class Vent
    {
        public Vector2 Start { get; set; }
        public Vector2 End { get; set; }
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
                    throw new NotImplementedException();
                default:
                    break;
            }

            return path;
        }
    }

    public static class Program
    {
        const int Rows = 1000;
        const int Columns = 1000;

        public static void Main()
        {
            var input = File.ReadAllLines("input.txt");

            var converted = ConvertRawInput(input);

            MapVents(converted);

            Console.WriteLine($"Game Over! Thank you for playing.");
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
                        Y=  int.Parse(tempEnd[0])
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

        private static void MapVents(List<Vent> ventStartAndEnds)
        {
            var map = new int[Rows, Columns];
            var overlaps = 0;

            foreach (var ventLine in ventStartAndEnds)
            {
                if (ventLine.Direction != Direction.Diagonal)
                {
                    var path = ventLine.CalculatePath();

                    foreach (var step in path)
                    {
                        map[(int)step.X, (int)step.Y] += 1;
                    }
                }
            }

            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    var currentValue = map[x, y];

                    if (currentValue > 1)
                    {
                        overlaps++;
                    }

                }
            }



            var mapper = new StringBuilder();

            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    var currentValue = map[x, y];

                    mapper.Append(currentValue > 0 ? $" {currentValue}" : " .");
                }

                mapper.AppendLine();
            }

            File.WriteAllText(@"C:\\test\test.txt", mapper.ToString());

            //for (int x = 0; x < 1000; x++)
            //{
            //    for (int y = 0; y < 1000; y++)
            //    {
            //        var currentValue = map[x, y];

            //        if (currentValue > 1)
            //        {
            //            overlaps++;
            //        }
            //    }
            //}

            Console.WriteLine(overlaps);
        }
    }
}