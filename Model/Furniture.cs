using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWG_CS3230_FurnitureRental.DAL;

namespace UWG_CS3230_FurnitureRental.Model
{
    public class Furniture
    {
        public int Id { get; set; }
        public int StyleId { get; set; }

        public int CategoryId { get; set; }

        public string Description { get; set; }

        public int Available { get; set; }

        public int Rented { get; set; }

        public override string ToString()
        {
            string output = "";
            FurnitureDAL dal = new FurnitureDAL();
            output += Environment.NewLine;
            output += dal.GetStyleTypeById(this.StyleId);
            output += " ";
            output += dal.GetCategoryTypeById(this.CategoryId);
            output += " ";
            output += this.Description;
            output += Environment.NewLine;
            if (this.CheckIfAvailable())
            {
                output += "Currently in stock: " + this.Available;
            }
            else
            {
                output += "Currently out of stock";
            }
            output += Environment.NewLine;
            return output;
        }

        public ObservableCollection<int> GetQuantityRange()
        {
            ObservableCollection<int> quantity = new ObservableCollection<int>();
            for (int i = 0; i <= this.Available; i++)
            {
                quantity.Add(i);
            }

            return quantity;
        }

        public bool CheckQuantityAvailableForRequest(int request)
        {
            if (this.Available >= request)
            {
                return true;
            }

            return false;
        }

        public bool CheckIfAvailable()
        {
            return this.Available > 0;
        }

        public void RemoveQuantity(int rented)
        {
           if (this.CheckQuantityAvailableForRequest(rented))
            {
                this.Available -= rented;
            } 
        }

        public void AddQuantity(int rented)
        {
            this.Available += rented;
        }
    }
}
