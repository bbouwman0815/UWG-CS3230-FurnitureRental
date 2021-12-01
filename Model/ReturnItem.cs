using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWG_CS3230_FurnitureRental.Model
{
    public class ReturnItem
    {
        public int RentalId { get; set; }
        public int FurnitureId { get; set; }
        public int? ReturnId { get; set; }
        public int Quantity { get; set; }
        public double DailyRentalRate { get; set; }
        public double TotalOwed => Quantity * DailyRentalRate;

        public static List<ReturnItem> itemsToBeReturned = new List<ReturnItem>();

        public override string ToString()
        {
            String output = "Furniture: " + FurnitureId + " Quantity: " + this.Quantity;
            return output;
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
    }
}