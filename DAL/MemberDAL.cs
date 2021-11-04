using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using UWG_CS3230_FurnitureRental.Model;

namespace UWG_CS3230_FurnitureRental.DAL
{
    public class MemberDAL
    {
        public Customer GetMemberById(int id)
        {
            Customer customer = new Customer();
            Address address;
            using (MySqlConnection connection = new MySqlConnection(Connection.connectionString))
            {
                connection.Open();
                String query = "select m.fname, m.lname, m.gender, m.birthday, m.registrationDate, m.phone, m.addressId, a.address1, a.address2, a.city, a.state, a.zip from member m join address a on a.id = m.addressId where m.id = @id";

                using MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;

                using MySqlDataReader reader = command.ExecuteReader();
                int fnameordinal = reader.GetOrdinal("fname");
                int lnameordinal = reader.GetOrdinal("lname");
                int phoneordinal = reader.GetOrdinal("phone");
                int genderordinal = reader.GetOrdinal("gender");
                int birthdayordinal = reader.GetOrdinal("birthday");
                int registrationdateordinal = reader.GetOrdinal("registrationDate");
                int addressidordinal = reader.GetOrdinal("addressId");
                int address1ordinal = reader.GetOrdinal("address1");
                int address2ordinal = reader.GetOrdinal("address2");
                int cityordinal = reader.GetOrdinal("city");
                int stateordinal = reader.GetOrdinal("state");
                int zipordinal = reader.GetOrdinal("zip");

                while (reader.Read())
                {
                    address = new Address {
                        id = reader.GetFieldValueCheckNull<int>(addressidordinal),
                        address1 = reader.GetFieldValueCheckNull<string>(address1ordinal),
                        address2 = reader.GetFieldValueCheckNull<string>(address2ordinal),
                        city = reader.GetFieldValueCheckNull<string>(cityordinal),
                        state = reader.GetFieldValueCheckNull<string>(stateordinal),
                        zip = reader.GetFieldValueCheckNull<string>(zipordinal)
                    };
                    customer = new Customer
                    {
                        id = id,
                        fName = reader.GetFieldValueCheckNull<string>(fnameordinal),
                        lName = reader.GetFieldValueCheckNull<string>(lnameordinal),
                        gender = reader.GetFieldValueCheckNull<string>(genderordinal),
                        birthday = reader.GetFieldValueCheckNull<DateTime>(birthdayordinal),
                        registrationDate = reader.GetFieldValueCheckNull<DateTime>(registrationdateordinal),
                        phoneNumber = reader.GetFieldValueCheckNull<string>(phoneordinal),
                        address = address
                    };

                }

            }
            return customer;

        }

    }
}
