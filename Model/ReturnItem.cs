using System;
using System.Collections.Generic;
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

        public override string ToString()
        {
            String output = "Furniture: " + FurnitureId + " Quantity: " + this.Quantity;
            return output;
        }
    }
}
