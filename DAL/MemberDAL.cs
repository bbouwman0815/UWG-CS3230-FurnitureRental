using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
                    address = new Address
                    {
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

        public ObservableCollection<Customer> GetMemberByName(string name)
        {
            ObservableCollection<Customer> customers = new ObservableCollection<Customer>();
            Address address;
            using (MySqlConnection connection = new MySqlConnection(Connection.connectionString))
            {
                connection.Open();
                String query = "select m.id, m.fname, m.lname, m.gender, m.birthday, m.registrationDate, m.phone, m.addressId, a.address1, a.address2, a.city, a.state, a.zip from member m join address a on a.id = m.addressId where CONCAT(m.fname, ' ', m.lname) = @name";

                using MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.Add("@name", MySqlDbType.VarChar).Value = name;

                using MySqlDataReader reader = command.ExecuteReader();
                int fnameordinal = reader.GetOrdinal("fname");
                int idordinal = reader.GetOrdinal("id");
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
                    address = new Address
                    {
                        id = reader.GetFieldValueCheckNull<int>(addressidordinal),
                        address1 = reader.GetFieldValueCheckNull<string>(address1ordinal),
                        address2 = reader.GetFieldValueCheckNull<string>(address2ordinal),
                        city = reader.GetFieldValueCheckNull<string>(cityordinal),
                        state = reader.GetFieldValueCheckNull<string>(stateordinal),
                        zip = reader.GetFieldValueCheckNull<string>(zipordinal)
                    };
                    customers.Add(new Customer
                    {
                        id = reader.GetFieldValueCheckNull<int>(idordinal),
                        fName = reader.GetFieldValueCheckNull<string>(fnameordinal),
                        lName = reader.GetFieldValueCheckNull<string>(lnameordinal),
                        gender = reader.GetFieldValueCheckNull<string>(genderordinal),
                        birthday = reader.GetFieldValueCheckNull<DateTime>(birthdayordinal),
                        registrationDate = reader.GetFieldValueCheckNull<DateTime>(registrationdateordinal),
                        phoneNumber = reader.GetFieldValueCheckNull<string>(phoneordinal),
                        address = address
                    });

                }

            }
            return customers;

        }

        public ObservableCollection<Customer> GetMemberByPhone(string phone)
        {
            ObservableCollection<Customer> customers = new ObservableCollection<Customer>();
            Address address;
            using (MySqlConnection connection = new MySqlConnection(Connection.connectionString))
            {
                connection.Open();
                String query = "select m.id, m.fname, m.lname, m.gender, m.birthday, m.registrationDate, m.phone, m.addressId, a.address1, a.address2, a.city, a.state, a.zip from member m join address a on a.id = m.addressId where m.phone = @phone";

                using MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.Add("@phone", MySqlDbType.VarChar).Value = phone;

                using MySqlDataReader reader = command.ExecuteReader();
                int fnameordinal = reader.GetOrdinal("fname");
                int idordinal = reader.GetOrdinal("id");
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
                    address = new Address
                    {
                        id = reader.GetFieldValueCheckNull<int>(addressidordinal),
                        address1 = reader.GetFieldValueCheckNull<string>(address1ordinal),
                        address2 = reader.GetFieldValueCheckNull<string>(address2ordinal),
                        city = reader.GetFieldValueCheckNull<string>(cityordinal),
                        state = reader.GetFieldValueCheckNull<string>(stateordinal),
                        zip = reader.GetFieldValueCheckNull<string>(zipordinal)
                    };
                    customers.Add(new Customer
                    {
                        id = reader.GetFieldValueCheckNull<int>(idordinal),
                        fName = reader.GetFieldValueCheckNull<string>(fnameordinal),
                        lName = reader.GetFieldValueCheckNull<string>(lnameordinal),
                        gender = reader.GetFieldValueCheckNull<string>(genderordinal),
                        birthday = reader.GetFieldValueCheckNull<DateTime>(birthdayordinal),
                        registrationDate = reader.GetFieldValueCheckNull<DateTime>(registrationdateordinal),
                        phoneNumber = reader.GetFieldValueCheckNull<string>(phoneordinal),
                        address = address
                    });

                }

            }
            return customers;

        }

        public void UpdateMember(Customer customer, int addressId)
        {
            using MySqlConnection connection = new MySqlConnection(Connection.connectionString);
            connection.Open();
            String query = "UPDATE member SET fname = @fname, lname = @lname, birthday = @birthday, gender = @gender, phone = @phone, addressId = @addressId where id = @id;";

            using MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.Add("@id", MySqlDbType.Int32).Value = customer.id;

            command.Parameters.AddWithValue("@fName", customer.fName);
            command.Parameters.AddWithValue("@lName", customer.lName);
            command.Parameters.AddWithValue("@gender", customer.gender);
            command.Parameters.AddWithValue("@birthday", customer.birthday);
            command.Parameters.AddWithValue("@registrationDate", customer.registrationDate);
            command.Parameters.AddWithValue("@phone", customer.phoneNumber);
            command.Parameters.AddWithValue("@addressId", addressId);
            command.ExecuteNonQuery();
        }

    }
}
