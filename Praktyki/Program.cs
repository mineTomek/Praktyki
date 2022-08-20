using System.Text.RegularExpressions;

internal class Program
{
    private static void Main(string[] args)
    {
        bool exit = false;

        while (exit != true)
        {
            Console.Write("Write date of meeting: ");

            string date = Console.ReadLine();

            if (date == null) {
                Console.WriteLine("Date can't be null!");
                continue;
            }

            Console.WriteLine(
                new Regex("^(0?[1-9]|1[0-2])[\\.](0?[1-9]|[12]\\d|3[01])[\\.]2\\d{3}\\b").IsMatch(date) ?
                $"Date {date} matches date regex" :
                $"Date {date} doesn't match date regex"
            );
        }
    }
}