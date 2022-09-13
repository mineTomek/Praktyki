using static GreatConsole.ConsoleColors.AllowedColor;
using static GreatConsole.ConsoleColors;
using GreatConsole;
using System.Text;

namespace Praktyki.Meetings
{
    public static class M_30_06_2022
    {
        static Dictionary<string, object> exercisesVaribles = new Dictionary<string, object>();

        public static Dictionary<string, Delegate> exercises = new Dictionary<string, Delegate>
        {
            { "Sortowanie cyfr cd.", Sorting }
        };
        #region Sorting

        static void Sorting()
        {
            ConsoleMenu.MenuOption[] options = new[]
            {
                new ConsoleMenu.MenuOption("Insert Sorting", DarkYellow, Yellow),
                new ConsoleMenu.MenuOption("Merge Sorting", DarkGreen, Green),
                new ConsoleMenu.MenuOption("Exit", DarkRed, Red)
            };

            (int selectedIndex, _) = new ConsoleMenu(options).Show();

            switch (selectedIndex)
            {
                case 0:
                    InsertSorting();
                    break;
                case 1:
                    MergeSorting();
                    break;
                default:
                    return;
            }

            if (exercisesVaribles.ContainsKey("exit"))
            {
                return;
            }

            StandardPause();

            Sorting();
        }

        static List<int> GetIntsForSorting()
        {
            Write("Enter numbers: ", Gray);

            string rawString = ReadLine(Yellow);

            if (string.IsNullOrWhiteSpace(rawString))
            {
                Clear();
                WriteLine("Entry cannot be empty", Red);
                return GetIntsForSorting();
            }

            if (rawString.ToLower().StartsWith("r") && rawString.Split(' ').Length > 0)
            {
                List<int> randomInts = new List<int>();

                int amount = int.Parse(rawString.Split(' ')[1]);

                if (amount < 2)
                {
                    Clear();
                    WriteLine("List would be too small", Red);
                    return GetIntsForSorting();
                }

                for (int i = 0; i < amount; i++)
                {
                    randomInts.Add(new Random().Next(-200, 200));
                }

                return randomInts;
            }

            bool isBad = false;

            List<int> ints = new List<int>();

            rawString.Split(' ').ToList().ForEach((string str) =>
            {
                foreach (char chr in str)
                {
                    if (!(char.IsDigit(chr) || chr == '-'))
                    {
                        isBad = true;
                    }
                }

                if (!isBad)
                {
                    ints.Add(int.Parse(str));
                }
            });

            if (isBad)
            {
                Clear();
                WriteLine("This is not fully a number", Red);
                return GetIntsForSorting();
            }

            if (ints.Count < 2)
            {
                Clear();
                WriteLine("This list is too small", Red);
                return GetIntsForSorting();
            }

            return ints;
        }

        static void InsertSorting()
        {
            Clear();

            WriteLine("Insert Sorting:", Green);

            List<int> ints = GetIntsForSorting();

            exercisesVaribles["bs_delay"] = 300;

            WriteLine(StringifyList(ints), Blue);

            ReadKey(true);

            ints = InsertEnumerate(ints);

            WriteFromString("&g'Final list: " + StringifyList(ints));
        }

        static List<int> InsertEnumerate(List<int> ints, int currentIndex = 0, bool skip = false)
        {
            if ((int)exercisesVaribles["bs_delay"] > 0)
            {
                var t = Task.Run(async delegate
                {
                    await Task.Delay((int)exercisesVaribles["bs_delay"]);
                });
                t.Wait();
            } else if ((int)exercisesVaribles["bs_delay"] < 0)
            {
                skip = true;
            }

            int j = currentIndex;

            while (j > 0 && ints[j - 1] > ints[j]) {
                (ints[j], ints[j - 1]) = (ints[j - 1], ints[j]);

                j -= 1;

                if (!skip)
                {
                    WriteFromString($"&'{StringifyList(GetRange(ints, 0, j - 1))}&g'{ints[j]} &c'{ints[j + 1]} &'{StringifyList(GetRange(ints, j + 2, ints.Count - 1))}");
                }

                var t = Task.Run(async delegate
                {
                    await Task.Delay((int)exercisesVaribles["bs_delay"]);
                });

                t.Wait();
            }

            if (!skip)
            {
                WriteFromString($"&'{StringifyList(GetRange(ints, 0, currentIndex - 1))}&c'{ints[currentIndex]} &'{StringifyList(GetRange(ints, currentIndex + 1, ints.Count - 1))}");
            }

            if (currentIndex == ints.Count - 1)
            {
                return ints;
            }

            if (Console.KeyAvailable)
            {
                ConsoleKey key = ReadKey(true).Key;

                if (key == ConsoleKey.Escape)
                {
                    exercisesVaribles["bs_delay"] = -1;
                    ConsoleColors.allowPrinting = false;
                } else
                {
                    if ((int)exercisesVaribles["bs_delay"] == 0)
                    {
                        exercisesVaribles["bs_delay"] = 300;
                    }
                    else if ((int)exercisesVaribles["bs_delay"] == 300)
                    {
                        exercisesVaribles["bs_delay"] = 0;
                    }
                }
            }

            return InsertEnumerate(ints, currentIndex + 1, skip);
        }

        static void MergeSorting()
        {
            WriteLine("Not Implemented!", Red);

            exercisesVaribles["exit"] = true;

            return;

            Clear();

            WriteLine("Merge Sorting:", Green);

            List<int> ints = GetIntsForSorting();

            exercisesVaribles["bs_delay"] = 300;

            WriteLine(StringifyList(ints), Blue);

            ReadKey(true);

            ints = InsertEnumerate(ints);

            WriteFromString("&g'Final list: " + StringifyList(ints));
        }

        static string StringifyList<T>(List<T> list, string separator = " ")
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (T item in list)
            {
                stringBuilder.Append(string.Concat(item.ToString(), separator));
            }

            string output = stringBuilder.ToString();

            try
            {
                output.Substring(0, output.Length - 2);
            } catch
            {
                return "";
            }

            return stringBuilder.ToString();
        }

        static List<T> GetRange<T>(List<T> list, int fromIndex, int toIndex)
        {
            return list.GetRange(fromIndex, toIndex - fromIndex + 1);

            /*
            
            2 5 => 4
                v     v
            0 1 2 3 4 5 6 7 8 9
                ^^^^^^^

            5 - 2 + 1

            */ 
           
        }

        #endregion
    }
}

