using Console = Praktyki.Utilities.ConsoleColors;
using static Praktyki.Utilities.AllowedColor;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Threading.Channels;

internal class Program
{
    private static void Main(string[] args)
    {
        bool exit = false;

        while (exit != true)
        {
            Console.Write("Enter date of meeting: ");

            string? date = Console.ReadLine(Yellow);

            if (string.IsNullOrWhiteSpace(date)) {
                Console.WriteLine("Date can't be null!", Red);
                continue;
            }

            Regex dateRegex = new Regex(@"[0-9]{1,2}\.[0-9]{1,2}\.[0-9]{4}"); /* xx.xx.xxxx  x - [0-9]*/
            //Regex dateRegex = new Regex(@"[0-9]{1,2}\.[0-9]{1,2}"); /* xx.xx  x - [0-9]*/

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
        }

        date = string.Join('.', dateParts); // Set to passed date with extended month and day numbers
    }

    static bool LoadMeeting(string date)
    {
        Console.WriteLine($"Loading meeting from {date}...", Green);

        Assembly assembly = Assembly.GetExecutingAssembly();

        Console.NewLine();

        string[] dateSplited = date.Split('.');
        
        Type? meetingClass = assembly.GetType($"Praktyki.Meetings.M_{string.Join('_', dateSplited)}");

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

    static bool SelectExercise(Type meetingClass)
    {
        FieldInfo? getExercises = meetingClass.GetField("exercises"); //.GetMethod("GetExercises");

        if (getExercises == null)
        {
            return false;
        }

        Dictionary<string, Delegate> exercises = (Dictionary<string, Delegate>)getExercises.GetValue(null);

        string exercise = "";

        while (exercise != "Exit")
        {
            int selectedIndex = 0;

            while (exercise == "")
            {
                Console.Clear();

                Console.WriteLine("Choose Exercise:");

                for (int i = 0; i < exercises.Count + 1; i++)
                {
                    if (i == exercises.Count)
                    {
                        if (i == selectedIndex)
                        {
                            Console.Write("\t> ", Yellow);
                            Console.WriteLine("Exit", Red);
                        }
                        else
                        {
                            Console.Write("\t  ");
                            Console.WriteLine("Exit", DarkRed);
                        }
                    } else
                    {
                        if (i == selectedIndex)
                        {
                            Console.Write("\t> ", Yellow);
                            Console.WriteLine(exercises.Keys.ToArray()[i]);
                        }
                        else
                        {
                            Console.Write("\t  ");
                            Console.WriteLine(exercises.Keys.ToArray()[i], Gray);
                        }
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
                        try
                        {
                            exercise = exercises.Keys.ToArray()[selectedIndex];
                        } catch //(IndexOutOfRangeException e)
                        {
                            exercise = "Exit";
                        }
                        break;
                    case ConsoleKey.Escape:
                        exercise = "Exit";
                        break;
                    default:
                        break;
                }

                selectedIndex = Math.Clamp(selectedIndex, 0, exercises.Count);
            }

            Console.NewLine();

            if (exercise != "Exit")
            {
                RunExercise(exercise, exercises[exercise].Method);

                exercise = "";
            }
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

        Console.NewLine();
        Console.Write("Press any key to continue...", Gray);
        Console.ReadKey();
    }
}