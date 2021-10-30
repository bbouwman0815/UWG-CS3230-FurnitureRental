﻿using System;
using System.Collections.Generic;
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

        public override string ToString()
        {
            FurnitureDAL dal = new FurnitureDAL();
            string style = dal.getStyleTypeById(this.StyleId);
            string category = dal.getCategoryTypeById(this.CategoryId);
            return category + " " + style + " " + this.Description;
        }
    }
}
