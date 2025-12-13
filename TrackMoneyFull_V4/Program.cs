using System;
using System.Collections.Generic;
using System.Linq;

namespace TrackMoneyFull_V4
{
    internal class Program
    {
        static List<Item> Items = new List<Item>();

        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Items = DataService.LoadItems();

            while (true)
            {
                Console.Clear();
                Console.WriteLine(">> Welcome to TrackMoney (Table Edition)");

                decimal bal = ItemService.CalculateBalance(Items);

                ConsoleHelper.WriteInlineColor(">> Balance: ", ConsoleColor.Gray);
                ConsoleHelper.WriteLineColor(
                    $"{bal:N2} kr",
                    bal > 0 ? ConsoleColor.Green :
                    bal < 0 ? ConsoleColor.Red :
                    ConsoleColor.Cyan);

                Console.WriteLine("\nPick an option:");
                Console.WriteLine("(1) Show items");
                Console.WriteLine("(2) Add item");
                Console.WriteLine("(3) Edit/Remove item");
                Console.WriteLine("(4) Export to CSV");
                Console.WriteLine("(5) Monthly Summary");
                Console.WriteLine("(6) Search by Title");
                Console.WriteLine("(7) Save & Quit");
                Console.WriteLine("(8) Save only");

                string choice = ConsoleHelper.Prompt(">> ").Trim();

                switch (choice)
                {
                    case "1":
                        ShowItemsMenu();
                        break;

                    case "2":
                        AddItemMenu();
                        break;

                    case "3":
                        EditRemoveMenu();
                        break;

                    case "4":
                        DataService.ExportToCsv(Items);
                        ConsoleHelper.Prompt("Press ENTER...");
                        break;

                    case "5":
                        ItemService.ShowMonthlySummary(Items);
                        ConsoleHelper.Prompt("Press ENTER...");
                        break;

                    case "6":
                        SearchMenu();
                        break;

                    case "7":
                        DataService.SaveItems(Items);
                        return;

                    case "8":
                        DataService.SaveItems(Items);
                        ConsoleHelper.Prompt("Saved. Press ENTER...");
                        break;

                    default:
                        ConsoleHelper.WriteLineColor("Invalid choice.", ConsoleColor.Yellow);
                        break;
                }
            }
        }

        static void ShowItemsMenu()
        {
            Console.Clear();
            Console.WriteLine(">> All items:\n");

            ItemService.PrintTable(Items);

            ConsoleHelper.Prompt("\nPress ENTER to continue...");
        }

        static void AddItemMenu()
        {
            Console.Clear();
            Console.WriteLine(">> Add new item:");

            string title = ConsoleHelper.Prompt("Title: ").Trim();
            if (string.IsNullOrWhiteSpace(title)) title = "(No title)";

            decimal amount = ConsoleHelper.ReadDecimal("Amount: ");
            int month = ConsoleHelper.ReadIntInRange("Month (1-12): ", 1, 12);

            Console.Write("Is this an expense? (y/n): ");
            bool isExpense = ConsoleHelper.ReadYesNo();

            string category = ConsoleHelper.Prompt("Category: ").Trim();
            if (string.IsNullOrWhiteSpace(category)) category = "Uncategorized";

            Items.Add(new Item(title, amount, month, isExpense, category));

            ConsoleHelper.WriteLineColor("Item added.", ConsoleColor.Green);
            ConsoleHelper.Prompt("Press ENTER...");
        }

        static void EditRemoveMenu()
        {
            Console.Clear();
            Console.WriteLine(">> Edit / Remove items:\n");

            ItemService.PrintTable(Items);

            if (!Items.Any())
            {
                ConsoleHelper.Prompt("Press ENTER...");
                return;
            }

            int idx = ConsoleHelper.ReadIntInRange(
                "Select item number (0 to cancel): ", 0, Items.Count);

            if (idx == 0) return;

            var item = Items[idx - 1];

            Console.WriteLine("\n(1) Edit");
            Console.WriteLine("(2) Remove");
            string choice = ConsoleHelper.Prompt(">> ");

            if (choice == "1") EditItem(item);
            else if (choice == "2")
            {
                Console.Write("Confirm remove? (y/n): ");
                if (ConsoleHelper.ReadYesNo())
                {
                    Items.RemoveAt(idx - 1);
                    ConsoleHelper.WriteLineColor("Item removed.", ConsoleColor.Green);
                }
            }

            ConsoleHelper.Prompt("Press ENTER...");
        }

        static void EditItem(Item item)
        {
            Console.WriteLine("\nLeave blank to keep existing value.");

            string t = ConsoleHelper.Prompt($"Title [{item.Title}]: ");
            if (!string.IsNullOrWhiteSpace(t)) item.Title = t;

            string amt = ConsoleHelper.Prompt($"Amount [{item.Amount:N2}]: ");
            if (decimal.TryParse(amt, out decimal newAmt))
                item.Amount = newAmt;

            string m = ConsoleHelper.Prompt($"Month [{item.Month}]: ");
            if (int.TryParse(m, out int newMonth) && newMonth is >= 1 and <= 12)
                item.Month = newMonth;

            string exp = ConsoleHelper.Prompt($"Is Expense? (y/n, now {(item.IsExpense ? "Expense" : "Income")}): ");
            if (exp == "y") item.IsExpense = true;
            else if (exp == "n") item.IsExpense = false;

            string cat = ConsoleHelper.Prompt($"Category [{item.Category}]: ");
            if (!string.IsNullOrWhiteSpace(cat)) item.Category = cat;

            ConsoleHelper.WriteLineColor("Item updated.", ConsoleColor.Green);
        }

        static void SearchMenu()
        {
            Console.Clear();
            string q = ConsoleHelper.Prompt("Search keyword: ");

            var results = ItemService.SearchByTitle(Items, q);

            if (!results.Any())
            {
                ConsoleHelper.WriteLineColor("No results found.", ConsoleColor.Yellow);
            }
            else
            {
                ItemService.PrintTable(results);
            }

            ConsoleHelper.Prompt("\nPress ENTER...");
        }
    }
}
