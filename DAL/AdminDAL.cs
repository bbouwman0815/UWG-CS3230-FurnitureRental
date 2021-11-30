using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWG_CS3230_FurnitureRental.Model;

namespace UWG_CS3230_FurnitureRental.DAL
{
    public class AdminDAL
    {
        public String RentalTransactionDateQuery(string startDate, string endDate)
        {
  
            List<AdminDateQuery> rows = new List<AdminDateQuery>();
            using (MySqlConnection connection = new MySqlConnection(Connection.connectionString))
            {
                connection.Open();
                String query = "Select m.id, m.fname, m.lname, r.id, r.transactionDate, i.quantity, f.description from member as m left outer join rentaltransaction as r on r.memberId = m.id left outer join rentalitem as i on i.rentalId = r.id left outer join furniture as f on f.id = i.furnitureId where r.id is not null and r.transactionDate >= @startDate and r.transactionDate <= @endDate;";

                using MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.Add("@startDate", MySqlDbType.VarChar).Value = startDate;
                command.Parameters.Add("@endDate", MySqlDbType.VarChar).Value = endDate;
    

                using MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    StringBuilder sb = new StringBuilder();

                    while (reader.Read())
                    {
                        if (sb.Length > 0)
                            sb.Append(Environment.NewLine);

                        for (int i = 0; i < reader.FieldCount; i++)
                            sb.AppendFormat("{0} ", reader[i]);
                    }

                    return sb.ToString();
                }

            }

            return "";
        }

        public String ReturnTransactionDateQuery(string startDate, string endDate)
        {
            List<AdminDateQuery> rows = new List<AdminDateQuery>();
            using (MySqlConnection connection = new MySqlConnection(Connection.connectionString))
            {
                connection.Open();
                String query = "Select m.id, m.fname, m.lname, r.id, r.returnDate, i.quantity, f.description from member as m left outer join returntransaction as r on r.memberId = m.id left outer join rentalitem as i on i.rentalId = r.id left outer join furniture as f on f.id = i.furnitureId where r.id is not null and r.transactionDate >= @startDate and r.transactionDate <= @endDate;";

                using MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.Add("@startDate", MySqlDbType.VarChar).Value = startDate;
                command.Parameters.Add("@endDate", MySqlDbType.VarChar).Value = endDate;

                using MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    StringBuilder sb = new StringBuilder();

                    while (reader.Read())
                    {
                        if (sb.Length > 0)
                            sb.Append(Environment.NewLine);

                        for (int i = 0; i < reader.FieldCount; i++)
                            sb.AppendFormat("{0} ", reader[i]);
                    }
                    return sb.ToString();
                }
            }
            return "";
        }
    }
}
