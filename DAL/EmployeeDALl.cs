using UWG_CS3230_FurnitureRental.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace UWG_CS3230_FurnitureRental.DAL
{
    public class EmployeeDAL
    {
        /// <summary>
        /// Verifies the employee login.
        /// </summary>
        /// <param name="fname">The fname.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public LoggedEmployee VerifyEmployeeLogin(string uname, string password)
        {
            List<LoggedEmployee> employeeList = new List<LoggedEmployee>();
            using (MySqlConnection connection = new MySqlConnection(Connection.connectionString))
            {
                connection.Open();
                String query = "select fname, lname, id, password, uname from employee where uname = @uname and password = @password;";

                using MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.Add("@uname", MySqlDbType.VarChar).Value = uname;
                command.Parameters.Add("@password", MySqlDbType.VarChar).Value = password;

                using MySqlDataReader reader = command.ExecuteReader();
                int fnameoridinal = reader.GetOrdinal("fname");
                int lnameordinal = reader.GetOrdinal("lname");
                int idordinal = reader.GetOrdinal("id");
                int pwordordinal = reader.GetOrdinal("password");
                int unameordinal = reader.GetOrdinal("uname");

                while (reader.Read())
                {
                    employeeList.Add(new LoggedEmployee
                    {
                        Fname = reader.GetFieldValueCheckNull<string>(fnameoridinal),
                        Lname = reader.GetFieldValueCheckNull<string>(lnameordinal),
                        Id = reader.GetFieldValueCheckNull<int>(idordinal),
                        Pword = reader.GetFieldValueCheckNull<string>(pwordordinal),
                        Uname = reader.GetFieldValueCheckNull<string>(unameordinal),
                    });

                }

            }

            if (employeeList.Count == 0)
            {
                return null;
            }    
            return employeeList[0];

        }

        public void CreateNewCustomer(Customer customer, int addressId)
        {
            using (MySqlConnection connection = new MySqlConnection(Connection.connectionString))
            {
                connection.Open();
                String query = "insert into member values(null, @fName, @lName, @gender, @bday, @registrationDate, @phone, @id);";

                using MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.Add("@fName", MySqlDbType.VarChar).Value = customer.fName;
                command.Parameters.Add("@lName", MySqlDbType.VarChar).Value = customer.lName;
                command.Parameters.Add("@gender", MySqlDbType.VarChar).Value = customer.gender;
                command.Parameters.Add("@bday", MySqlDbType.Date).Value = customer.birthday.Date;
                command.Parameters.Add("@registrationDate", MySqlDbType.Date).Value = customer.registrationDate.Date;
                command.Parameters.Add("@phone", MySqlDbType.VarChar).Value = customer.phoneNumber;
                command.Parameters.Add("@id", MySqlDbType.Int32).Value = addressId;
                command.ExecuteNonQuery();
            }
        }

        public void CreateNewAddress(Address address)
        {
            using (MySqlConnection connection = new MySqlConnection(Connection.connectionString))
            {
                connection.Open();
                String query = "insert into address values(null, @addr1, @addr2, @city, @state, @zip);";

                using MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.Add("@addr1", MySqlDbType.VarChar).Value = address.address1;
                command.Parameters.Add("@addr2", MySqlDbType.VarChar).Value = address.address2;
                command.Parameters.Add("@city", MySqlDbType.VarChar).Value = address.city;
                command.Parameters.Add("@state", MySqlDbType.VarChar).Value = address.state;
                command.Parameters.Add("@zip", MySqlDbType.VarChar).Value = address.zip;
                command.ExecuteNonQuery();
            }
        }

        public int GetAddressId(Address address)
        {
            int addressId = 0;
            using (MySqlConnection connection = new MySqlConnection(Connection.connectionString))
            {
                connection.Open();
                String query = "select id from address where address1 = @addr1 and address2 = @addr2 and city = @city and state = @state and zip = @zip;";

                using MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.Add("@addr1", MySqlDbType.VarChar).Value = address.address1;
                command.Parameters.Add("@addr2", MySqlDbType.VarChar).Value = address.address2;
                command.Parameters.Add("@city", MySqlDbType.VarChar).Value = address.city;
                command.Parameters.Add("@state", MySqlDbType.VarChar).Value = address.state;
                command.Parameters.Add("@zip", MySqlDbType.VarChar).Value = address.zip;

                using MySqlDataReader reader = command.ExecuteReader();
                int idordinal = reader.GetOrdinal("id");

                while (reader.Read())
                {
                    addressId = reader.GetFieldValueCheckNull<int>(idordinal);
                }
            }
            return addressId;

        }
    }
}
