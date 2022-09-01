using static Praktyki.Utilities.ConsoleColors;
using static Praktyki.Utilities.AllowedColor;
using Praktyki.Utilities;
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

        static void BubbleSorting()
        {
            Clear();

            WriteLine("Bubble Sorting:", Green);

            List<int> ints = GetIntsForSorting();

            exercisesVaribles["bs_delay"] = 300;

            WriteLine(StringifyList(ints), Blue);

            ReadKey(true);

            bool? swapped = null;

            while (swapped != false)
            {
                WriteLine(StringifyList(ints));

                swapped = BubbleEnumerate(ints);
            }

            ConsoleColors.allowPrinting = true;

            WriteLine("Final list: " + StringifyList(ints), Green);
        }

        static bool BubbleEnumerate(List<int> ints, int currentIndex = 0, bool swapped = false, bool skip = false)
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

            if (ints[currentIndex] > ints[currentIndex + 1])
            {
                swapped = true;
                (ints[currentIndex], ints[currentIndex + 1]) = (ints[currentIndex + 1], ints[currentIndex]); // Swap

                if (!skip)
                {
                    WriteFromString($"&y'{StringifyList(GetRange(ints, 0, currentIndex - 1))}&c'{ints[currentIndex]} {ints[currentIndex + 1]} &y'{StringifyList(GetRange(ints, currentIndex + 2, ints.Count - 1))}");
                }
            }
            else
            {
                if (!skip)
                {
                    WriteFromString($"&w'{StringifyList(GetRange(ints, 0, currentIndex - 1))}&c'{ints[currentIndex]} {ints[currentIndex + 1]} &w'{StringifyList(GetRange(ints, currentIndex + 2, ints.Count - 1))}");
                }
            }

            if (currentIndex == ints.Count - 2)
            {
                return swapped;
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

            return BubbleEnumerate(ints, currentIndex + 1, swapped: swapped, skip: skip);
        }

        static void InsertSorting()
        {
            Clear();

            WriteLine("Bubble Sorting:", Green);

            List<int> ints = GetIntsForSorting();

            WriteFromString("&r'(Not) &g'Final list: " + StringifyList(ints));
        }

        static void MergeSorting()
        {
            Clear();

            WriteLine("Bubble Sorting:", Green);

            List<int> ints = GetIntsForSorting();

            WriteFromString("&r'(Not) &g'Final list: " + StringifyList(ints));
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

