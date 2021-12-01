using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using UWG_CS3230_FurnitureRental.Model;

namespace UWG_CS3230_FurnitureRental.DAL
{
    public class ReturnTransactionDAL
    {
        public int CreateNewReturnTransaction(List<ReturnItem> returnItems, int employeeId)
        {
            int transactionIdReturn = 0;
            using (MySqlConnection connection = new MySqlConnection(Connection.connectionString))
            {
                connection.Open();
                MySqlTransaction transaction = connection.BeginTransaction();

                String query = "insert into returntransaction values(null, @returnDate, @employeeId);";

                try
                {
                    using MySqlCommand command = new MySqlCommand(query, connection);
                    command.Transaction = transaction;
                    command.Parameters.Add("@returnDate", MySqlDbType.Date).Value = DateTime.Today.Date;
                    command.Parameters.Add("@employeeId", MySqlDbType.Int32).Value = employeeId;
                    command.ExecuteNonQuery();

                    if (command.LastInsertedId != null) command.Parameters.Add(
                        new MySqlParameter("newId", command.LastInsertedId));

                    transactionIdReturn = Convert.ToInt32(command.Parameters["@newId"].Value);

                    foreach (ReturnItem currentReturnItem in returnItems)
                    {
                        currentReturnItem.ReturnId = transactionIdReturn;

                        String rquery = "insert into returnitem values(@rentalId, @furnitureId, @returnId, @quantity);";
                        using MySqlCommand rcommand = new MySqlCommand(rquery, connection);
                        rcommand.Transaction = transaction;
                        rcommand.Parameters.Add("@rentalId", MySqlDbType.Int32).Value = currentReturnItem.RentalId;
                        rcommand.Parameters.Add("@furnitureId", MySqlDbType.Int32).Value = currentReturnItem.FurnitureId;
                        rcommand.Parameters.Add("@returnId", MySqlDbType.Int32).Value = currentReturnItem.ReturnId;
                        rcommand.Parameters.Add("@quantity", MySqlDbType.Int32).Value = currentReturnItem.Quantity;
                        rcommand.ExecuteNonQuery();

                        String uquery = "UPDATE furniture SET available = available + @available where id = @id;";
                        using MySqlCommand ucommand = new MySqlCommand(uquery, connection);
                        ucommand.Transaction = transaction;
                        ucommand.Parameters.Add("@id", MySqlDbType.Int32).Value = currentReturnItem.FurnitureId;
                        ucommand.Parameters.AddWithValue("@available", MySqlDbType.Int32).Value = currentReturnItem.Quantity;
                        ucommand.ExecuteNonQuery();

                        String fquery = "UPDATE furniture SET rented = rented - @rented where id = @id;";
                        using MySqlCommand fcommand = new MySqlCommand(fquery, connection);
                        fcommand.Transaction = transaction;
                        fcommand.Parameters.Add("@id", MySqlDbType.Int32).Value = currentReturnItem.FurnitureId;
                        fcommand.Parameters.AddWithValue("@rented", MySqlDbType.Int32).Value = currentReturnItem.Quantity;
                        fcommand.ExecuteNonQuery();
                    }
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (MySqlException ex)
                    {
                    }
                }
            }
            return transactionIdReturn;
        }
    }
}