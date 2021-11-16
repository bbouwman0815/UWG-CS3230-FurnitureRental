using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWG_CS3230_FurnitureRental.DAL;
using UWG_CS3230_FurnitureRental.Utilities;

namespace UWG_CS3230_FurnitureRental.Model
{
    public class RentalItem
    {
        public int? RentalId { get; set; }
        public int FurnitureId { get; set; }
        public int Quantity { get; set; }
        public double DailyRentalRate { get; set; }
        public double TotalRentalRate => Quantity * DailyRentalRate;

        public static RentalItem SelectedRentalItem { get; set; }

        public RentalItem(int furnitureId, int quantity, double dailyRentalRate)
        {
            this.FurnitureId = furnitureId;
            this.Quantity = quantity;
            this.DailyRentalRate = dailyRentalRate;
        }

        public RentalItem()
        {
          
        }

        public ObservableCollection<int> GetQuantityRange()
        {
            ObservableCollection<int> quantity = new ObservableCollection<int>();
            for (int i = 0; i <= this.Quantity; i++)
            {
                quantity.Add(i);
            }

            return quantity;
        }

        public override string ToString()
        {
            FurnitureDAL dal = new FurnitureDAL();
            Furniture furniture = dal.GetFurnitureById(this.FurnitureId);
            string description = furniture.Description;
            string style = furniture.Style;
            string category = furniture.Category;
            string totalCost = OrderFormatter.FormatTotalCost(this.TotalRentalRate);
            string dailyRate = OrderFormatter.FormatTotalCost(this.DailyRentalRate);

            String output = Environment.NewLine;
            output += category + " " + style + " " + description;
            output += Environment.NewLine;
            output += "Daily rate: " + dailyRate + " x " + this.Quantity + " = " + totalCost;
            output += Environment.NewLine;

            return output;
        }
    }
}
