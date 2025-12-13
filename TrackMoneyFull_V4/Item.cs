using System;
using System.Text;
using System.Globalization;
using System.Collections.Generic;


namespace TrackMoneyFull_V4
{
    public class Item
    {
        public string Title { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public int Month { get; set; }
        public bool IsExpense { get; set; }
        public string Category { get; set; } = "Uncategorized";

        public Item() { }

        public Item(string title, decimal amount, int month, bool isExpense, string category = "Uncategorized")
        {
            Title = title;
            Amount = amount;
            Month = month;
            IsExpense = isExpense;
            Category = string.IsNullOrWhiteSpace(category) ? "Uncategorized" : category.Trim();
        }

        public override string ToString()
        {
            string type = IsExpense ? "Expense" : "Income";
            return $"{Title} | {Amount.ToString("N2", CultureInfo.InvariantCulture)} kr | " +
                   $"Month {Month} | {type} | {Category}";
        }
    }
}