﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UWG_CS3230_FurnitureRental.DAL;
using UWG_CS3230_FurnitureRental.Model;

namespace UWG_CS3230_FurnitureRental.Utilities
{
    public class OrderFormatter
    {
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


    }
}
