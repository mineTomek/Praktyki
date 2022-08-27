using System.Text.RegularExpressions;

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

        public static void NewLine(int repeats = 1)
        {
            for (int i = 0; i < repeats; i++)
            {
                Console.WriteLine();
            }
        }

        public static void Clear() => Console.Clear();

        public static void StandardPause()
        {
            NewLine();
            Write("Press any key to continue...", AllowedColor.DarkGray);
            ReadKey();
        }

        public static void WriteFromString(string str)
        {
            str = str.Trim();

            Dictionary<string, AllowedColor> textWithColors = new Dictionary<string, AllowedColor>();

            Regex colorCodeRegex = new Regex(@"&[a-z\.]{1,2}'");

            AllowedColor printingColor = AllowedColor.White;

            ColorParser parser = new ColorParser(str);

            parser.Print();
        }

        public class ColorParser
        {
            char currentChar;
            string str;
            int index;
            AllowedColor printColor;

            public ColorParser(string _str)
            {
                str = _str;
            }

            public void Print(AllowedColor color = AllowedColor.White)
            {
                index = 0;

                printColor = color;

                Advance();

                while (currentChar != '\0')
                {
                    if (currentChar == '&')
                    {
                        LookForColorStamp();
                    } else
                    {
                        Write(currentChar, printColor);
                    }

                    Advance();
                }

                NewLine();
            }

            public void Advance()
            {
                if (index >= str.Length)
                {
                    currentChar = '\0';
                    return;
                }

                currentChar = str[index];

                index++;
            }

            void LookForColorStamp()
            {
                string colorStamp = "";

                while (currentChar != '\'')
                {
                    Advance();
                    colorStamp += currentChar;
                }

                colorStamp = colorStamp.TrimEnd('\'');

                switch (colorStamp)
                {
                    case "":
                        printColor = AllowedColor.White;
                        break;
                    case "w":
                        printColor = AllowedColor.White;
                        break;
                    case "bl":
                        printColor = AllowedColor.Black;
                        break;
                    case "b":
                        printColor = AllowedColor.Blue;
                        break;
                    case "c":
                        printColor = AllowedColor.Cyan;
                        break;
                    case "db":
                        printColor = AllowedColor.DarkBlue;
                        break;
                    case "dc":
                        printColor = AllowedColor.DarkCyan;
                        break;
                    case "dgr":
                        printColor = AllowedColor.DarkGray;
                        break;
                    case "dg":
                        printColor = AllowedColor.DarkGreen;
                        break;
                    case "dm":
                        printColor = AllowedColor.DarkMagenta;
                        break;
                    case "dr":
                        printColor = AllowedColor.DarkRed;
                        break;
                    case "dy":
                        printColor = AllowedColor.DarkYellow;
                        break;
                    case "gr":
                        printColor = AllowedColor.Gray;
                        break;
                    case "g":
                        printColor = AllowedColor.Green;
                        break;
                    case "m":
                        printColor = AllowedColor.Magenta;
                        break;
                    case "r":
                        printColor = AllowedColor.Red;
                        break;
                    case "y":
                        printColor = AllowedColor.Yellow;
                        break;
                    default:
                        printColor = AllowedColor.White;
                        NewLine();
                        WriteLine("Invalid color stamp", AllowedColor.Red);
                        NewLine();
                        break;
                }
            }
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

