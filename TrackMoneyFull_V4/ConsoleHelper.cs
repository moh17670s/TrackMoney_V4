using System;
using System.Collections.Generic;
using System.Text;

namespace TrackMoneyFull_V4
{
    public static class ConsoleHelper
    {
        public static void WriteLineColor(string text, ConsoleColor color)
        {
            var prev = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = prev;
        }

        public static void WriteInlineColor(string text, ConsoleColor color)
        {
            var prev = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ForegroundColor = prev;
        }

        public static string Prompt(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine() ?? string.Empty;
        }

        public static int ReadIntInRange(string prompt, int min, int max)
        {
            while (true)
            {
                string s = Prompt(prompt).Trim();
                if (int.TryParse(s, out var val) && val >= min && val <= max)
                    return val;

                WriteLineColor($"Please enter a number between {min} and {max}.", ConsoleColor.Yellow);
            }
        }

        public static decimal ReadDecimal(string prompt)
        {
            while (true)
            {
                string s = Prompt(prompt).Trim();
                if (decimal.TryParse(s, System.Globalization.NumberStyles.Number,
                    System.Globalization.CultureInfo.InvariantCulture, out var val) && val >= 0)
                    return val;

                WriteLineColor("Please enter a valid positive number.", ConsoleColor.Yellow);
            }
        }

        public static bool ReadYesNo()
        {
            while (true)
            {
                string r = Prompt(">> ").Trim().ToLower();
                if (r == "y" || r == "yes") return true;
                if (r == "n" || r == "no") return false;

                WriteLineColor("Please answer 'y' or 'n'.", ConsoleColor.Yellow);
            }
        }
    }
}
