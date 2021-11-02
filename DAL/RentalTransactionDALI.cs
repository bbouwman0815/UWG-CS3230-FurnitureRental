using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWG_CS3230_FurnitureRental.Model;

namespace UWG_CS3230_FurnitureRental.DAL
{
    public class RentalTransactionDAL
    {
        public int CreateNewRentalTransaction(RentalTransaction rentalTransaction)
        {
            using (MySqlConnection connection = new MySqlConnection(Connection.connectionString))
            {
                connection.Open();
                String query = "insert into rentaltransaction values(null, @cost, @transactionDate, @dueDate, @employeeId, @memberId);";

                using MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.Add("@cost", MySqlDbType.Double).Value = rentalTransaction.cost;
                command.Parameters.Add("@transactionDate", MySqlDbType.Date).Value = rentalTransaction.transactionDate.Date;
                command.Parameters.Add("@dueDate", MySqlDbType.Date).Value = rentalTransaction.dueDate.Date;
                command.Parameters.Add("@employeeId", MySqlDbType.Int32).Value = rentalTransaction.employeeId;
                command.Parameters.Add("@memberId", MySqlDbType.Int32).Value = rentalTransaction.memberId;
                command.ExecuteNonQuery();

                if (command.LastInsertedId != null) command.Parameters.Add(
                    new MySqlParameter("newId", command.LastInsertedId));

                return Convert.ToInt32(command.Parameters["@newId"].Value);
            }

        
        }

        public void CreateNewRentalItem(RentalItem rentalItem)
        {
            using (MySqlConnection connection = new MySqlConnection(Connection.connectionString))
            {
                connection.Open();
                String query = "insert into rentalitem values(@rentalId, @furnitureId, @quantity, @daily_rental_rate);";

                using MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.Add("@rentalId", MySqlDbType.Int32).Value = rentalItem.RentalId;
                command.Parameters.Add("@furnitureId", MySqlDbType.Int32).Value = rentalItem.FurnitureId;
                command.Parameters.Add("@quantity", MySqlDbType.Int32).Value = rentalItem.Quantity;
                command.Parameters.Add("@daily_rental_rate", MySqlDbType.Double).Value = rentalItem.DailyRentalRate;
                command.ExecuteNonQuery();
            }
        }
    }
}
