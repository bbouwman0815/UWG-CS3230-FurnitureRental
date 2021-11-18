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

        public override string ToString()
        {
            string output = "";
            output += Environment.NewLine;
            output += "Transaction Id: " + this.id;
            output += Environment.NewLine;
            output += "Cost: " + this.cost;
            output += " ";
            output += "Transaction Date: " + this.transactionDate;
            output += Environment.NewLine;
            output += "Due Date: " + this.dueDate;
            output += Environment.NewLine;
            output += "Member Id: " + this.memberId;
            return output;
        }
    }
}
