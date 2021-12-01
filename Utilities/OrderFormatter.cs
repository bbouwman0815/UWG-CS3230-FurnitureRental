using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UWG_CS3230_FurnitureRental.DAL;
using UWG_CS3230_FurnitureRental.Model;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace UWG_CS3230_FurnitureRental.Utilities
{
    public class OrderFormatter
    {
        private readonly RentalTransactionDAL rdal = new RentalTransactionDAL();

        public static string CalculateFormatOrderCost(ObservableCollection<RentalItem> items, int rentalPeriod)
        {
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
           
            double totalCost = 0.0;
            foreach (RentalItem item in items)
            {
                totalCost += item.TotalRentalRate;
            }
            totalCost *= rentalPeriod;
            return totalCost.ToString("C", nfi);
        }

        public static double CalculateOrderCost(ObservableCollection<RentalItem> items, int rentalPeriod)
        {
            double totalCost = 0.0;
            foreach (RentalItem item in items)
            {
                totalCost += item.TotalRentalRate;
            }
            totalCost *= rentalPeriod;
            return totalCost;
        }

        public double CalculateReturnCost(ObservableCollection<ReturnItem> items)
        {
            double totalCost = 0.0;
            foreach (ReturnItem item in items)
            {
                totalCost += item.TotalOwed * this.rdal.GetTransactionDateDiff(item.RentalId);
            }
            return totalCost;
        }

        public static string FormatTotalCost(double totalCost)
        {
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            return totalCost.ToString("C", nfi);
        }

        public static bool VerifyPrice(string price)
        {
            Regex rgx = new Regex(@"^[+-]?([0-9]+\.?[0-9]*|\.[0-9]+)$");
            bool match = rgx.IsMatch(price);

            return match;
        }

        public static ObservableCollection<int> GetQuantityRange(int range)
        {
            ObservableCollection<int> quantity = new ObservableCollection<int>();
            for (int i = 0; i <= range; i++)
            {
                quantity.Add(i);
            }

            return quantity;
        }

        public static string GetOrderSummary(ObservableCollection<RentalItem> rentalItems, string total, string date)
        {
            FurnitureDAL fdal = new FurnitureDAL();
            string summary = Environment.NewLine;
            summary += Environment.NewLine;
            summary += Environment.NewLine;
            summary += "Order Summary";
            summary += Environment.NewLine;
            summary += Environment.NewLine;
            foreach (RentalItem currentRentalItem in rentalItems)
            {
                Furniture currentFurniture = fdal.GetFurnitureById(currentRentalItem.FurnitureId);
                summary += currentFurniture.Style + " " + currentFurniture.Description;
                summary += Environment.NewLine;
                summary += "Quantity: " + currentRentalItem.Quantity;
                summary += Environment.NewLine;
                summary += "Daily Rental Rate: $" + currentRentalItem.DailyRentalRate;
                summary += Environment.NewLine;
                summary += Environment.NewLine;
            }
            summary += Environment.NewLine;
            summary += total;
            summary += Environment.NewLine;
            summary += Environment.NewLine;
            summary += "Furniture due by: " + date;
            summary += Environment.NewLine;
            summary += Environment.NewLine;

            return summary;
        }

        public static string GetReturnSummary(ObservableCollection<ReturnItem> returnItems, string total)
        {
            FurnitureDAL fdal = new FurnitureDAL();
            string summary = Environment.NewLine;
            summary += Environment.NewLine;
            summary += Environment.NewLine;
            summary += "Order Summary";
            summary += Environment.NewLine;
            summary += Environment.NewLine;
            foreach (ReturnItem currentRentalItem in returnItems)
            {
                Furniture currentFurniture = fdal.GetFurnitureById(currentRentalItem.FurnitureId);
                summary += currentFurniture.Style + " " + currentFurniture.Description;
                summary += Environment.NewLine;
                summary += "Quantity: " + currentRentalItem.Quantity;
                summary += Environment.NewLine;
                summary += Environment.NewLine;
            }
            summary += Environment.NewLine;
            summary += total;
            summary += Environment.NewLine;
            summary += Environment.NewLine;
            summary += "Furniture returned on: " + DateTime.Today;
            summary += Environment.NewLine;
            summary += Environment.NewLine;

            return summary;
        }

        public static void FillDataGrid(DataTable table, DataGrid grid)
        {
            grid.Columns.Clear();
            grid.AutoGenerateColumns = false;
            for (int i = 0; i < table.Columns.Count; i++)
            {
                grid.Columns.Add(new DataGridTextColumn()
                {
                    Header = table.Columns[i].ColumnName,
                    Binding = new Binding { Path = new PropertyPath("[" + i.ToString() + "]") }
                });
            }

            var collection = new ObservableCollection<object>();
            foreach (DataRow row in table.Rows)
            {
                collection.Add(row.ItemArray);
            }

            grid.ItemsSource = collection;
        }


    }
}
