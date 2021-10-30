using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
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
    }
}
