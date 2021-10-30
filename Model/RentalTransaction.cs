using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWG_CS3230_FurnitureRental.Model
{
    public class RentalTransaction
    {
        public int? id { get; set; }
        public double cost { get; set; }

        public DateTime transactionDate { get; set; }

        public DateTime dueDate { get; set; }

        public int employeeId { get; set; }

        public int memberId { get; set; }

    }
}
