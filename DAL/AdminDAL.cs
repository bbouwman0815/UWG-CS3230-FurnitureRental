using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWG_CS3230_FurnitureRental.Model;

namespace UWG_CS3230_FurnitureRental.DAL
{
    public class AdminDAL
    {
        public DataTable ReturnTransactionDateQuery(string startDate, string endDate)
        {

            DataTable dataTable = new DataTable();

            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection.connectionString))
                {
                    String query = "Select m.id, m.fname, m.lname, r.id, r.transactionDate, i.quantity, f.description from member as m left outer join rentaltransaction as r on r.memberId = m.id left outer join rentalitem as i on i.rentalId = r.id left outer join furniture as f on f.id = i.furnitureId where r.id is not null and r.transactionDate >= @startDate and r.transactionDate <= @endDate;";
                    using (MySqlCommand command = new MySqlCommand(query, con))
                    {
                        command.Parameters.Add("@startDate", MySqlDbType.VarChar).Value = startDate;
                        command.Parameters.Add("@endDate", MySqlDbType.VarChar).Value = endDate;
                        command.CommandType = CommandType.Text;
                        using (MySqlDataAdapter sda = new MySqlDataAdapter(command))
                        {
                            sda.Fill(dataTable);
                        }
                    }
                }
            }
            catch
            {

            }
            return dataTable;
        }

        public DataTable AdminQuery(string query)
        {
            DataTable dataTable = new DataTable();

            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection.connectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                        {
                            sda.Fill(dataTable);
                        }
                    }
                }
            }
            catch
            {

            }
            return dataTable;
        }

        public DataTable RentalTransactionDateQuery(string startDate, string endDate)
        {

            DataTable dataTable = new DataTable();

            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection.connectionString))
                {
                    String query = "Select m.id as memberId, m.fname as firstName, m.lname as lastName, r.id as rentalId, r.transactionDate, i.quantity, f.description from member as m left outer join rentaltransaction as r on r.memberId = m.id left outer join rentalitem as i on i.rentalId = r.id left outer join furniture as f on f.id = i.furnitureId where r.id is not null and r.transactionDate >= @startDate and r.transactionDate <= @endDate;";
                    using (MySqlCommand command = new MySqlCommand(query, con))
                    {
                        command.Parameters.Add("@startDate", MySqlDbType.VarChar).Value = startDate;
                        command.Parameters.Add("@endDate", MySqlDbType.VarChar).Value = endDate;
                        command.CommandType = CommandType.Text;
                        using (MySqlDataAdapter sda = new MySqlDataAdapter(command))
                        {
                            sda.Fill(dataTable);
                        }
                    }
                }
            }
            catch
            {

            }
            return dataTable;
        }
    }
}
