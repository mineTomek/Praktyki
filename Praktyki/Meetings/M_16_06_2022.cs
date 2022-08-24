using static Praktyki.Utilities.ConsoleColors;
using static Praktyki.Utilities.AllowedColor;

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
    }
}

