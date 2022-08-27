using static Praktyki.Utilities.ConsoleColors;
using static Praktyki.Utilities.AllowedColor;
using Praktyki.Utilities;
using System.Text;

namespace Praktyki.Meetings
{
    public static class M_16_06_2022
    {
        public static Dictionary<string, Delegate> exercises = new Dictionary<string, Delegate>
        {
            { "Bool z int'a (0 / 1)", BoolFromInt },
            { "Switch'e z case'ami bez breake'a", SwitchesWithoutBrakes },
            { "Int poza granicami", IntBeyondBoundries },
            { "Sortowanie cyfr", Sorting }
        };

        static void BoolFromInt()
        {
            WriteFromString(@"
&y'Wynik testów:&'

Nie ma możliwości konversji pomiędzy typem &g'bool&', a &g'int&' w tą i odwrotną stronę.
Ani niejawna ani jawna konwersja nie działają.
Zwracany jest błąd kompilacji, więc nawet blok &b'try&' nie pomaga.
");
        }

        static void SwitchesWithoutBrakes()
        {
            WriteFromString(@"
&y'Kod zwracający &r'błąd&y':&'

&m'switch&'(someInt)
{
    &b'case &y'1&':
        &y'WriteLine&'(&c'""Zdał test na \""1\""&');
    &b'case &y'2&':
        &y'WriteLine&'(&c'""Zdał test na \""2\"");
    &b'default&':
        &y'WriteLine&'(&c'""Wszystko inne.""&');
}

&y'Kod, który się &g'kompiluje&y':

&m'switch&'(someInt)
{
    &b'case &y'1&':
        &y'WriteLine&'(&c'""Zdał test na \""1\""&');
        &m'break&';
    &b'case &y'2&':
        &y'WriteLine&'(&c'""Zdał test na \""2\"");
        &m'return&';
    &b'default&':
        &y'WriteLine&'(&c'""Wszystko inne.""&');
        &m'break&';
}

&y'Wyniki testów:&'

Da się użyć bloku &b'switch&' z &m'case&''ami bez &m'break&''ów, ale wtedy nazeży użyć &m'return&' jako zastępnika.
Wtedy jednak po bloku &b'switch&' nie zostanie wywołany żaden kod &dgr'(będący w tej samej metodzie)&'.
Kompilator zwraca błąd przy użyciu &gr'niczego&' lub &m'continue&'.
");
        }

        static void IntBeyondBoundries()
        {
            int someInt = 0;

            unchecked
            {
                someInt = int.MaxValue + 1;
            }

            WriteFromString(@"
&y'Obserwacje:&'

Pisząc taki kod:

    &b'int&' someInt = &b'int&'.MaxValue + &y'1&';

nie uda się przypisać zmiennej &g'int&' wartości większej niż jej maksymalna wartość &gr'(&y'" + int.MaxValue + @"&gr')&'.

Kompilator zwraca wtedy tu błąd:

    &b'int&' someInt = &b'int&'.MaxValue + &y'1&';
                  &r'^^^^^^^^^^^^^^^^&'
Można jednak otoczyć tą linijkę kody blokiem &b'unchecked&'.
Wtedy kompilator nie sprawdzi tego czy dana operacja prowadzi do wyjścia poza maksymalną wartość typu &gr'(np. &y'" + int.MaxValue + @"&gr' dla &g'int&gr').

Napisać można taki kod:

&b'int&' someInt = &y'0&';

&b'unchecked&' {
    someInt = &b'int&'.MaxValue + &y'1&';

    &g'Console&'.&y'WriteLine&'(someInt);
}

i zwróci on taką odpowiedź:

&g'" + someInt + @"&'
Jest to minimalna wartość &g'int&''a + 1.
Stało się tak dlatego, że operacja doszła do maksymalnej wartości i kontynuowała liczenie od najmniejszej wartości.
");
        }

        #region Sorting

        static void Sorting()
        {
            ConsoleMenu.MenuOption[] options = new[]
            {
                new ConsoleMenu.MenuOption("Bubble Sorting", DarkCyan, Cyan),
                new ConsoleMenu.MenuOption("Insert Sorting", DarkYellow, Yellow),
                new ConsoleMenu.MenuOption("Merge Sorting"),
                new ConsoleMenu.MenuOption("Exit", DarkRed, Red)
            };

            (int selectedIndex, _) = new ConsoleMenu(options).Show();

            switch (selectedIndex)
            {
                case 0:
                    BubbleSorting();
                    break;
                case 1:
                    InsertSorting();
                    break;
                case 2:
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

            return ints;
        }

        static void BubbleSorting()
        {
            Clear();

            WriteLine("Bubble Sorting:", Green);

            List<int> ints = GetIntsForSorting();

            bool? swapped = null;

            while (swapped != false)
            {
                swapped = false;

                WriteLine(StringifyList(ints));

                for (int i = 0; i < ints.Count - 1; i++)
                {
                    
                    if (ints[i] > ints[i + 1])
                    {
                        swapped = true;
                        (ints[i], ints[i + 1]) = (ints[i + 1], ints[i]); // Swap

                        WriteFromString($"&y'{StringifyList(GetRange(ints, 0, i - 1))}&c'{ints[i]} {ints[i + 1]} &y'{StringifyList(GetRange(ints, i + 2, ints.Count - 1))}");
                    } else
                    {
                        WriteFromString($"&w'{StringifyList(GetRange(ints, 0, i - 1))}&c'{ints[i]} {ints[i + 1]} &w'{StringifyList(GetRange(ints, i + 2, ints.Count - 1))}");
                    }
                }
            }


            WriteLine("Final list: " + StringifyList(ints), Green);
        }

        static void BubbleEnumerate(List<int> ints, int delay, int curentIndex = 0)
        {
            var t = Task.Run(async delegate
            {
                await Task.Delay(delay);
            });

            BubbleEnumerate(ints, delay, curentIndex + 1);
        }

        static void InsertSorting()
        {
            Clear();

            WriteLine("Bubble Sorting:", Green);

            List<int> ints = GetIntsForSorting();

            WriteLine("Final list: " + StringifyList(ints), Green);
        }

        static void MergeSorting()
        {
            Clear();

            WriteLine("Bubble Sorting:", Green);

            List<int> ints = GetIntsForSorting();

            

            WriteLine("Final list: " + StringifyList(ints), Green);
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

