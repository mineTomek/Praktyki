using System;

namespace Praktyki.Utilities
{
    public static class ConsoleColors
    {
        public static void WriteLine(object? obj = null, AllowedColor? color = null)
        {
            if (obj == null)
            {
                Console.WriteLine();
            } else if (color == null)
            {
                Console.WriteLine(obj);
            } else
            {
                ConsoleColor lastColor = Console.ForegroundColor;

                Console.ForegroundColor = (ConsoleColor)((int)color);

                Console.WriteLine(obj);

                Console.ForegroundColor = lastColor;
            }
        }

        public static void Write(object obj, AllowedColor? color = null)
        {
            if (color == null)
            {
                Console.Write(obj);
            }
            else
            {
                ConsoleColor lastColor = Console.ForegroundColor;

                Console.ForegroundColor = (ConsoleColor)color;

                Console.Write(obj);

                Console.ForegroundColor = lastColor;
            }
        }

        public static string? ReadLine(AllowedColor? color = null)
        {
            string? output = null;

            if (color == null)
            {
                output = Console.ReadLine();
            }
            else
            {
                ConsoleColor lastColor = Console.ForegroundColor;

                Console.ForegroundColor = (ConsoleColor)color;

                output = Console.ReadLine();

                Console.ForegroundColor = lastColor;
            }

            return output;
        }

        public static ConsoleKeyInfo ReadKey()
        {
            return Console.ReadKey();
        }

        public static void NewLine()
        {
            Console.WriteLine();
        }

        public static void Clear() => Console.Clear();

        public static void WriteFromString(string text)
        {
            throw new NotImplementedException();
        }
    }

    public enum AllowedColor
    {
        Black,
        DarkBlue,
        DarkGreen,
        DarkCyan,
        DarkRed,
        DarkMagenta,
        DarkYellow,
        Gray,
        DarkGray,
        Blue,
        Green,
        Cyan,
        Red,
        Magenta,
        Yellow,
        White
    }
}

