using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UWG_CS3230_FurnitureRental.Model
{
    public class Customer
    {
        public int? id;
        public string fName;
        public string lName;
        public string gender;
        public DateTime birthday;
        public DateTime registrationDate;
        public string phoneNumber;
        public Address address;
    }
}
