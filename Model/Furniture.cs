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
            FurnitureDAL dal = new FurnitureDAL();
            string style = dal.getStyleTypeById(this.StyleId);
            string category = dal.getCategoryTypeById(this.CategoryId);
            return category + " " + style + " " + this.Description;
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

        public bool CheckQuantityAvailable(int request)
        {
            if (this.Available >= request)
            {
                return true;
            }

            return false;
        }

        public void UpdateQuantity(int rented)
        {
            if (this.CheckQuantityAvailable(rented))
            {
                this.Available = this.Available - rented;
            }
        }
    }
}
