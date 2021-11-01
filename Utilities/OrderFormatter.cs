using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UWG_CS3230_FurnitureRental.Model;

namespace UWG_CS3230_FurnitureRental.Utilities
{
    public class OrderFormatter
    {
        public static string CalculateFormatOrderCost(ObservableCollection<RentalItem> items)
        {
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
           
            double totalCost = 0.0;
            foreach (RentalItem item in items)
            {
                totalCost += item.TotalRentalRate;
            }
            return totalCost.ToString("C", nfi);
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
    }
}
