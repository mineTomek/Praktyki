using static Praktyki.Utilities.ConsoleColors;
using static Praktyki.Utilities.AllowedColor;
using static Program;
using static System.Net.Mime.MediaTypeNames;

namespace Praktyki.Meetings
{
    public static class M_16_06_2022
    {
        public static Dictionary<string, Delegate> exercises = new Dictionary<string, Delegate>
        {
            { "Bool z int'a (0 / 1)", BoolFromInt },
            { "Switch'e z case'ami bez breake'a", SwitchesWithoutBrakes },
            { "Int poza granicami", IntBeyondBoundries }
        };

        static void BoolFromInt()
        {
            WriteLine("Wynik testów:", Yellow);

            Write("Nie ma możliwości konversji pomiędzy typem ");
            Write("bool", Green);
            Write(", a ");
            Write("int", Green);
            WriteLine(" w tą i odwrotną stronę.");

            WriteLine("Ani niejawna ani jawna konwersja nie działają.");

            Write("Zwracany jest błąd kompilacji, więc nawet blok ");
            Write("try", Blue);
            WriteLine(" nie pomaga.");
        }

        static void SwitchesWithoutBrakes()
        {
            WriteLine("Kod zwracający błąd:", Yellow);
            WriteLine(@"
switch(someInt)
{
    case 1:
        WriteLine(""Zdał test na \""1\"");
    case 2:
        WriteLine(""Zdał test na \""2\"");
    default:
        WriteLine(""Wszystko inne."");
}
", Red);
            WriteLine("Kod, który się skompiluje:", Yellow);
            WriteLine(@"
switch(someInt)
{
    case 1:
        WriteLine(""Zdał test na \""1\"");
        break;
    case 2:
        WriteLine(""Zdał test na \""2\"");
        return;
    default:
        WriteLine(""Wszystko inne."");
        break;
}
", Green);

            WriteLine("Wynik testów:", Yellow);

            Write("Da się użyć bloku ");
            Write("switch", Blue);
            Write(" z ");
            Write("case", Magenta);
            Write("'ami bez ");
            Write("break", Magenta);
            Write("'ow, ale należy wtedy użyć ");
            Write("return", Magenta);
            WriteLine(" jako zastępnika.");

            Write("Wtedy jednak kod po bloku ");
            Write("switch", Blue);
            WriteLine(" nie zostanie wywołany żaden kod (będący w tej samej metodzie).");

            Write("Kompilator zwraca błąd przy użyciu");
            Write("niczego", Gray);
            Write(" lub ");
            Write("continue", Magenta);
            WriteLine(".");

        }

        static void IntBeyondBoundries()
        {
            WriteLine("To zadanie nie zostało jeszcze zrobione...", Red);
        }
    }
}

