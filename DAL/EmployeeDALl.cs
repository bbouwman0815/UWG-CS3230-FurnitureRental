using UWG_CS3230_FurnitureRental.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace UWG_CS3230_FurnitureRental.DAL
{
    public class EmployeeDAL
    {
        /// <summary>
        /// Get all the employees of the given department
        /// </summary>
        /// <param name="dno">department number</param>
        /// <returns> all the employees of the given department</returns>
        public LoggedEmployee GetEmployeeByLoginInformation(string fname, string password)
        {
            LoggedEmployee employeeList = new LoggedEmployee();
            using (MySqlConnection connection = new MySqlConnection(Connection.connectionString))
            {
                connection.Open();
                String query = "select fname, lname, id, password from employee where fname = @fname and password = @password;";

                using MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.Add("@fname", MySqlDbType.VarChar).Value = fname;
                command.Parameters.Add("@password", MySqlDbType.VarChar).Value = password;

                using MySqlDataReader reader = command.ExecuteReader();
                int fnameoridinal = reader.GetOrdinal("fname");
                int lnameordinal = reader.GetOrdinal("lname");
                int idordinal = reader.GetOrdinal("id");
                int pwordordinal = reader.GetOrdinal("password");

                while (reader.Read())
                {
                    employeeList = new LoggedEmployee
                    {
                        Fname = reader.GetFieldValueCheckNull<string>(fnameoridinal),
                        Lname = reader.GetFieldValueCheckNull<string>(lnameordinal),
                        Id = reader.GetFieldValueCheckNull<int>(idordinal),
                        Pword = reader.GetFieldValueCheckNull<string>(pwordordinal),
                    };

                }

            }
            return employeeList;

        }

        /// <summary>
        /// Verifies the employee login.
        /// </summary>
        /// <param name="fname">The fname.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public bool VerifyEmployeeLogin(string fname, string password)
        {
            List<LoggedEmployee> employeeList = new List<LoggedEmployee>();
            using (MySqlConnection connection = new MySqlConnection(Connection.connectionString))
            {
                connection.Open();
                String query = "select fname, lname, id, password from employee where fname = @fname and password = @password;";

                using MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.Add("@fname", MySqlDbType.VarChar).Value = fname;
                command.Parameters.Add("@password", MySqlDbType.VarChar).Value = password;

                using MySqlDataReader reader = command.ExecuteReader();
                int fnameoridinal = reader.GetOrdinal("fname");
                int lnameordinal = reader.GetOrdinal("lname");
                int idordinal = reader.GetOrdinal("id");
                int pwordordinal = reader.GetOrdinal("password");

                while (reader.Read())
                {
                    employeeList.Add(new LoggedEmployee
                    {
                        Fname = reader.GetFieldValueCheckNull<string>(fnameoridinal),
                        Lname = reader.GetFieldValueCheckNull<string>(lnameordinal),
                        Id = reader.GetFieldValueCheckNull<int>(idordinal),
                        Pword = reader.GetFieldValueCheckNull<string>(pwordordinal),
                    });

                }

            }
            return employeeList.Count > 0;

        }
    }
}
