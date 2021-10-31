using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWG_CS3230_FurnitureRental.DAL;
using UWG_CS3230_FurnitureRental.Utilities;

namespace UWG_CS3230_FurnitureRental.Model
{
    public class RentalItem
    {
        public int RentalId { get; set; }
        public int FurnitureId { get; set; }
        public int Quantity { get; set; }
        public double DailyRentalRate { get; set; }
        public double TotalRentalRate => Quantity * DailyRentalRate;

        public RentalItem(int furnitureId, int quantity, double dailyRentalRate)
        {
            this.FurnitureId = furnitureId;
            this.Quantity = quantity;
            this.DailyRentalRate = dailyRentalRate;
        }

        public override string ToString()
        {
            FurnitureDAL dal = new FurnitureDAL();
            Furniture furniture = dal.GetFurnitureById(this.FurnitureId);
            string description = furniture.Description;
            string style = dal.getStyleTypeById(furniture.StyleId);
            string category = dal.getCategoryTypeById(furniture.CategoryId);
            string totalCost = OrderFormatter.FormatTotalCost(this.TotalRentalRate);
            string dailyRate = OrderFormatter.FormatTotalCost(this.DailyRentalRate);

            return category + " " + style + " " + description + "   " + dailyRate + " x " + this.Quantity + " = " + totalCost;
        }
    }
}
