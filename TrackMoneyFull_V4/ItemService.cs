using System;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Globalization;using System.Linq;
using System.Linq;
using System.Text;

namespace TrackMoneyFull_V4
{
    public static class ItemService
    {
        public static decimal CalculateBalance(List<Item> items)
        {
            return items.Sum(i => i.IsExpense ? -i.Amount : i.Amount);
        }

        // ⭐ TABLE + COLOR (Expense=Red, Income=Green)
        public static void PrintTable(List<Item> items)
        {
            if (!items.Any())
            {
                ConsoleHelper.WriteLineColor("No items to show.", ConsoleColor.Yellow);
                return;
            }

            Console.WriteLine("----------------------------------------------------------------------------");
            Console.WriteLine("# | Title               | Amount (kr)         | Month | Type      | Category");
            Console.WriteLine("----------------------------------------------------------------------------");

            for (int i = 0; i < items.Count; i++)
            {
                var it = items[i];
                string type = it.IsExpense ? "Expense" : "Income";
                string amountStr = $"{it.Amount:N2} kr";

                // Print fixed-width table fields (non-colored)
                Console.Write($"{i + 1,-2}| {it.Title,-18}| ");

                // Color the amount (red if expense, green if income)
                if (it.IsExpense)
                    ConsoleHelper.WriteInlineColor($"{amountStr,18}", ConsoleColor.Red);
                else
                    ConsoleHelper.WriteInlineColor($"{amountStr,18}", ConsoleColor.Green);

                // Continue normal table printing
                Console.Write($" | {it.Month,5} | ");

                // Color the type label
                if (it.IsExpense)
                    ConsoleHelper.WriteInlineColor($"{type,-9}", ConsoleColor.Red);
                else
                    ConsoleHelper.WriteInlineColor($"{type,-9}", ConsoleColor.Green);

                Console.WriteLine($" | {it.Category}");
            }

            Console.WriteLine("----------------------------------------------------------------------------");
        }

        public static void ShowMonthlySummary(List<Item> items)
        {
            if (!items.Any())
            {
                ConsoleHelper.WriteLineColor("No items to summarize.", ConsoleColor.Yellow);
                return;
            }

            var groups = items.GroupBy(i => i.Month).OrderBy(g => g.Key);

            foreach (var g in groups)
            {
                decimal income = g.Where(i => !i.IsExpense).Sum(i => i.Amount);
                decimal expense = g.Where(i => i.IsExpense).Sum(i => i.Amount);
                decimal net = income - expense;

                Console.WriteLine($"\nMonth {g.Key}:");
                Console.Write("  Income:  ");
                ConsoleHelper.WriteLineColor($"{income:N2} kr", ConsoleColor.Green);

                Console.Write("  Expense: ");
                ConsoleHelper.WriteLineColor($"{expense:N2} kr", ConsoleColor.Red);

                Console.Write("  Net:     ");
                ConsoleHelper.WriteLineColor(
                    $"{net:N2} kr",
                    net > 0 ? ConsoleColor.Green :
                    net < 0 ? ConsoleColor.Red :
                    ConsoleColor.Cyan
                );
            }
        }

        public static List<Item> SearchByTitle(List<Item> items, string query)
        {
            query = query.Trim().ToLower();

            return items
                .Where(i => i.Title.ToLower().Contains(query))
                .OrderBy(i => i.Month)
                .ThenBy(i => i.Title)
                .ToList();
        }
    }
}