using Console = Praktyki.Utilities.ConsoleColors;
using static Praktyki.Utilities.AllowedColor;
using static Praktyki.Utilities.ConsoleMenu;
using System.Text.RegularExpressions;
using Praktyki.Utilities;
using System.Reflection;
using System;

internal class Program
{
    static bool exit = false;

    private static void Main(string[] args)
    {
        Console.Clear();

        while (exit != true)
        {
            Regex dateRegex = new Regex(@"([0-9]{1,2}\.[0-9]{1,2})|([0-9]{1,2}\.[0-9]{1,2}\.[0-9]{4})"); /* xx.xx.xxxx  x - [0-9]*/

            int option = 0;

            string? date = ListenForInput("Enter date of meeting", White, new[] { ("Exit", Red), (dateRegex.ToString(), Yellow) }, Gray, ref option);

            if (option == 1) // Exit [Option]
            {
                exit = true;
                continue;
            }

            if (option == -1) // Exit [Escape key]
            {
                if (string.IsNullOrWhiteSpace(date)) // Date is empty
                {
                    exit = true; // Actualy exit
                    continue;
                } else // Date is not empty
                {
                    continue; // Start loop again (clear date)
                }
            }

            if (string.IsNullOrWhiteSpace(date)) {
                Console.WriteLine("Date can't be null!", Red);
                continue;
            }

            if (dateRegex.IsMatch(date))
            {
                FormatDate(ref date);

                bool succes = LoadMeeting(date);

                if (!succes)
                {
                    Console.WriteLine("That date is not present in assembly", Red);
                    Console.NewLine();
                    continue;
                }

            } else
            {
                Console.WriteLine($"\"{date}\" is an invalid date.", Red);
                continue;
            }

            Console.Clear();
            //Console.NewLine();
        }

        Console.WriteLine("Exiting the program...", Red);
    }

    static void FormatDate(ref string date)
    {
        string[] dateParts = date.Split('.');

        if (dateParts[0].Length == 1) // Day of the month
        {
            dateParts[0] = string.Concat("0", dateParts[0]);
        }

        if (dateParts[1].Length == 1) // Month of the year
        {
            dateParts[1] = string.Concat("0", dateParts[1]);
        }

        if (dateParts.Length == 2) //No year
        {
            date = string.Concat(string.Join('.', dateParts), '.', DateTime.Today.Year); // Set to passed date with todays year
            return;
        }

        date = string.Join('.', dateParts); // Set to passed date with extended month and day numbers
    }

    static string ListenForInput(string entry,
                                AllowedColor entryColor,
                                (string, AllowedColor)[] targets,
                                AllowedColor standardColor,
                                ref int matchedIndex)
    {
        string input = "";

        bool exitListener = false;

        while (!exitListener)
        {

            Console.Write($"{entry}: ", entryColor);

            (string, AllowedColor) toWrite = ("", standardColor);

            int counter = 0;

            foreach ((string, AllowedColor) target in targets)
            {
                bool match = false;

                if (IsRegex(input))
                {
                    if (new Regex(target.Item1).IsMatch(input))
                    {
                        match = true;
                    }
                } else
                {
                    if (input == target.Item1)
                    {
                        match = true;
                    }
                }

                if (match)
                {
                    toWrite = (input, target.Item2);

                    matchedIndex = counter + 1;

                    break;
                }
                else
                {
                    toWrite = (input, standardColor);
                }

                counter++;
            }

            Console.Write(toWrite.Item1, toWrite.Item2);

            ConsoleKeyInfo keyInfo = Console.ReadKey();

            Console.NewLine();

            if (keyInfo.Key == ConsoleKey.Enter)
            {
                if (input.Length == 0)
                {
                    Console.Clear();
                    continue;
                }
                exitListener = true;
                Console.Clear();
                continue;
            } else if (keyInfo.Key == ConsoleKey.Backspace)
            {
                if (input.Length == 0)
                {
                    continue;
                }

                input = input.Remove(input.Length - 1);
            } else if (keyInfo.Key == ConsoleKey.Escape) {
                matchedIndex = -1;
                exitListener = true;
                Console.Clear();
                continue;
            } else
            {
                input += keyInfo.KeyChar;
            }

            Console.Clear();
        }

        return input;
    }

    static bool IsRegex(string str)
    {
        if (str.Length == 0)
        {
            return false;
        }

        try
        {
            new Regex(str);
        } catch
        {
            return false;
        }

        return true;
    }

    static bool LoadMeeting(string date)
    {
        Console.WriteLine($"Loading meeting from {date}...", Green);

        Assembly assembly = Assembly.GetExecutingAssembly();

        Console.NewLine();

        string[] dateSplited = date.Split('.');
        
        Type? meetingClass = assembly.GetType($"Praktyki.Meetings.M_{string.Join('_', dateSplited).Trim()}");

        if (meetingClass == null)
        {
            return false;
        }

        bool succes = SelectExercise(meetingClass);

        if (succes)
        {
            return true;
        } else
        {
            Console.WriteLine("Something went wrong while selecting exercises...", Red);
            return false;
        }
    }

    static bool SelectExercise(Type meetingClass, int startIndex = 0)
    {
        FieldInfo? getExercises = meetingClass.GetField("exercises");

        if (getExercises == null)
        {
            return false;
        }

        Dictionary<string, Delegate> exercises = (Dictionary<string, Delegate>)getExercises.GetValue(null);

        List<MenuOption> options = new List<MenuOption>();

        foreach (string exercise in exercises.Keys)
        {
            options.Add(new MenuOption(exercise));
        }

        options.Add(new MenuOption("Exit", DarkRed, Red));

        ConsoleMenu menu = new ConsoleMenu(options.ToArray());

        Delegate exerciseMethod = null;

        (int selectedIndex, string selectedOption) = menu.Show(startIndex);

        if (selectedOption == "Exit")
        {
            return true;
        }

        try
        {
            exerciseMethod = exercises[selectedOption];
        } catch
        {
            Console.WriteLine("Something went wrong while selecting exercise...", Red);
            return false;
        }

        if (exerciseMethod != null)
        {
            RunExercise(selectedOption, exerciseMethod.Method);

            SelectExercise(meetingClass, selectedIndex);
        }

        return true;
    }

    static void RunExercise(string name, MethodInfo method)
    {
        Console.Clear();

        Console.Write("Running ");
        Console.Write($"\"{name}\"", Green);
        Console.Write(" from method ");
        Console.Write($"\"{method.Name}()\"", Green);
        Console.WriteLine(":");
        Console.NewLine();

        method.Invoke(null, null);

        Console.StandardPause();
    }
}