using Console = Praktyki.Utilities.ConsoleColors;
using static Praktyki.Utilities.AllowedColor;
using System;

namespace Praktyki.Utilities
{
    public class ConsoleMenu
    {
        readonly AllowedColor arrowColor;
        readonly MenuOption[] options;


        public ConsoleMenu(MenuOption[] _options, AllowedColor _arrowColor = Yellow)
        {
            arrowColor = _arrowColor;
            options = _options;
        }

        public (int, string) Show(int startIndex = 0)
        {
            bool selected = false;

            int selectedIndex = startIndex;

            while (!selected)
            {
                Console.Clear();

                Console.WriteLine("Choose Exercise:");

                for (int i = 0; i < options.Length; i++)
                {
                    if (i == selectedIndex)
                    {
                        Console.Write("\t> ", arrowColor);
                        Console.WriteLine(options[i].name, options[i].selectedColor);
                    }
                    else
                    {
                        Console.Write("\t  ");
                        Console.WriteLine(options[i].name, options[i].standardColor);
                    }
                }

                ConsoleKey key = Console.ReadKey().Key;

                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        selectedIndex--;
                        break;
                    case ConsoleKey.DownArrow:
                        selectedIndex++;
                        break;
                    case ConsoleKey.Enter:
                        return (selectedIndex, options[selectedIndex].name);
                    case ConsoleKey.Escape:
                        return (-1, string.Empty);
                    default:
                        break;
                }

                if (selectedIndex < 0)
                {
                    selectedIndex = options.Length - 1;
                } else if (selectedIndex >= options.Length) {
                    selectedIndex = 0;
                }
            }

            Console.NewLine();

            return (selectedIndex, options[selectedIndex].name);
        }

        public class MenuOption
        {
            public string name;
            public AllowedColor standardColor;
            public AllowedColor selectedColor;

            public MenuOption(object _name, AllowedColor _standardColor = Gray, AllowedColor _selectedColor = White)
            {
                name = _name.ToString();
                standardColor = _standardColor;
                selectedColor = _selectedColor;
            }
        }
    }
}

