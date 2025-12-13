

using System.Text;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.Json;

namespace TrackMoneyFull_V4



{
    public static class DataService
    {
        private static readonly string JsonFile = "moneydata.json";
        private static readonly string CsvFile = "moneydata_export.csv";
        private static readonly JsonSerializerOptions JsonOptions =
            new JsonSerializerOptions { WriteIndented = true };

        public static List<Item> LoadItems()
        {
            try
            {
                if (!File.Exists(JsonFile))
                    return new List<Item>();

                string json = File.ReadAllText(JsonFile);
                return JsonSerializer.Deserialize<List<Item>>(json) ?? new List<Item>();
            }
            catch
            {
                return new List<Item>();
            }
        }

        public static void SaveItems(List<Item> items)
        {
            try
            {
                string json = JsonSerializer.Serialize(items, JsonOptions);
                File.WriteAllText(JsonFile, json);
                ConsoleHelper.WriteLineColor("Data saved.", ConsoleColor.Green);
            }
            catch (Exception ex)
            {
                ConsoleHelper.WriteLineColor($"Failed to save data: {ex.Message}", ConsoleColor.Red);
            }
        }

        public static void ExportToCsv(List<Item> items)
        {
            try
            {
                var lines = new List<string> { "Title,Amount,Month,IsExpense,Category" };

                foreach (var it in items)
                {
                    string esc(string s) =>
                        s.Contains('"') || s.Contains(',') ? $"\"{s.Replace("\"", "\"\"")}\"" : s;

                    lines.Add(string.Join(",",
                        esc(it.Title),
                        it.Amount.ToString(CultureInfo.InvariantCulture),
                        it.Month,
                        it.IsExpense ? "Expense" : "Income",
                        esc(it.Category)
                    ));
                }

                File.WriteAllLines(CsvFile, lines);
                ConsoleHelper.WriteLineColor($"Exported to {CsvFile}", ConsoleColor.Green);
            }
            catch (Exception ex)
            {
                ConsoleHelper.WriteLineColor($"CSV export failed: {ex.Message}", ConsoleColor.Red);
            }
        }
    }
}