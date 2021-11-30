using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWG_CS3230_FurnitureRental.Model;

namespace UWG_CS3230_FurnitureRental.DAL
{
    public class RentalTransactionDAL
    {
        public int CreateNewRentalTransaction(RentalTransaction rentalTransaction, List<RentalItem> rentalItems)
        {
            int transactionIdReturn = 0;
            using (MySqlConnection connection = new MySqlConnection(Connection.connectionString))
            {
                connection.Open();
                MySqlTransaction transaction = connection.BeginTransaction();

                String query = "insert into rentaltransaction values(null, @cost, @transactionDate, @dueDate, @employeeId, @memberId);";

                try
                {
                    using MySqlCommand command = new MySqlCommand(query, connection);
                    command.Transaction = transaction;
                    command.Parameters.Add("@cost", MySqlDbType.Double).Value = rentalTransaction.cost;
                    command.Parameters.Add("@transactionDate", MySqlDbType.Date).Value = rentalTransaction.transactionDate.Date;
                    command.Parameters.Add("@dueDate", MySqlDbType.Date).Value = rentalTransaction.dueDate.Date;
                    command.Parameters.Add("@employeeId", MySqlDbType.Int32).Value = rentalTransaction.employeeId;
                    command.Parameters.Add("@memberId", MySqlDbType.Int32).Value = rentalTransaction.memberId;
                    command.ExecuteNonQuery();
            
                    if (command.LastInsertedId != null) command.Parameters.Add(
                        new MySqlParameter("newId", command.LastInsertedId));

                    transactionIdReturn = Convert.ToInt32(command.Parameters["@newId"].Value);

                    foreach (RentalItem currentRentalItem in rentalItems)
                    {
                        FurnitureDAL fdal = new FurnitureDAL();
                        int id = currentRentalItem.FurnitureId;
                        currentRentalItem.RentalId = transactionIdReturn;
                        int availableQuantity = fdal.GetFurnitureById(currentRentalItem.FurnitureId).Available - currentRentalItem.Quantity;
                        int rentedQuantity = fdal.GetFurnitureById(currentRentalItem.FurnitureId).Rented + currentRentalItem.Quantity;
                        fdal.UpdateAvailableFurnitureQuantity(currentRentalItem.FurnitureId, availableQuantity);
                        fdal.UpdateRentedFurnitureQuantity(currentRentalItem.FurnitureId, rentedQuantity);

                        String rquery = "insert into rentalitem values(@rentalId, @furnitureId, @Quantity, @daily_rental_rate);";               
                        using MySqlCommand rcommand = new MySqlCommand(rquery, connection);
                        rcommand.Transaction = transaction;
                        rcommand.Parameters.Add("@rentalId", MySqlDbType.Int32).Value = currentRentalItem.RentalId;
                        rcommand.Parameters.Add("@furnitureId", MySqlDbType.Int32).Value = currentRentalItem.FurnitureId;
                        rcommand.Parameters.Add("@Quantity", MySqlDbType.Int32).Value = currentRentalItem.Quantity;
                        rcommand.Parameters.Add("@daily_rental_rate", MySqlDbType.Double).Value = currentRentalItem.DailyRentalRate;               
                        rcommand.ExecuteNonQuery();

                        String uquery = "UPDATE furniture SET available = @available where id = @id;";
                        using MySqlCommand ucommand = new MySqlCommand(uquery, connection);
                        ucommand.Transaction = transaction;
                        ucommand.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                        ucommand.Parameters.AddWithValue("@available", availableQuantity);
                        ucommand.ExecuteNonQuery();

                        String fquery = "UPDATE furniture SET rented = @rented where id = @id;";
                        using MySqlCommand fcommand = new MySqlCommand(fquery, connection);
                        fcommand.Transaction = transaction;
                        fcommand.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                        fcommand.Parameters.AddWithValue("@rented", rentedQuantity);
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

        public ObservableCollection<RentalTransaction> GetMemberTransactions(int memberId)
        {
            var rentalTransactions = new ObservableCollection<RentalTransaction>();
            using (MySqlConnection connection = new MySqlConnection(Connection.connectionString))
            {
                connection.Open();
                using (var cmd = new MySqlCommand("uspGetMemberTransactions", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@memId", MySqlDbType.Int32);
                    cmd.Parameters["@memId"].Value = memberId;
                    cmd.Parameters["@memId"].Direction = ParameterDirection.Input;

                    using MySqlDataReader reader = cmd.ExecuteReader();
                    int transactionidordinal = reader.GetOrdinal("id");
                    int costordinal = reader.GetOrdinal("cost");
                    int transactiondateordinal = reader.GetOrdinal("transactionDate");
                    int duedateordinal = reader.GetOrdinal("dueDate");
                    int employeeidordinal = reader.GetOrdinal("employeeId");

                    while (reader.Read())
                    {
                        rentalTransactions.Add( new RentalTransaction 
                        {
                            id = reader.GetFieldValueCheckNull<int>(transactionidordinal),
                            cost = (double)reader.GetFieldValueCheckNull<decimal>(costordinal),
                            transactionDate = reader.GetFieldValueCheckNull<DateTime>(transactiondateordinal),
                            dueDate = reader.GetFieldValueCheckNull<DateTime>(duedateordinal),
                            memberId = memberId,
                            employeeId = reader.GetFieldValueCheckNull<int>(employeeidordinal),
                        });
                    }
                }
            }
            return rentalTransactions;
        }

        public ObservableCollection<RentalItem> GetTransactionRentalItems(int transactionId)
        {
            var rentalItems = new ObservableCollection<RentalItem>();
            using (MySqlConnection connection = new MySqlConnection(Connection.connectionString))
            {
                connection.Open();
                using (var cmd = new MySqlCommand("uspGetRentalItems", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@transactionId", MySqlDbType.Int32);
                    cmd.Parameters["@transactionId"].Value = transactionId;
                    cmd.Parameters["@transactionId"].Direction = ParameterDirection.Input;

                    using MySqlDataReader reader = cmd.ExecuteReader();
                    int furnitureidordinal = reader.GetOrdinal("furnitureId");
                    int quantityordinal = reader.GetOrdinal("Quantity");
                    int dailyrateordinal = reader.GetOrdinal("daily_rental_rate");

                    while (reader.Read())
                    {
                        rentalItems.Add(new RentalItem
                        {
                            FurnitureId = reader.GetFieldValueCheckNull<int>(furnitureidordinal),
                            Quantity = reader.GetFieldValueCheckNull<int>(quantityordinal),
                            DailyRentalRate = (double)reader.GetFieldValueCheckNull<decimal>(dailyrateordinal),
                            RentalId = transactionId,
                        });
                    }
                }
            }
            return rentalItems;
        }

        public ObservableCollection<RentalItem> GetRemainingRentedTransactionItems(int transactionId)
        {
            var rentalItems = new ObservableCollection<RentalItem>();
            using (MySqlConnection connection = new MySqlConnection(Connection.connectionString))
            {
                connection.Open();
                using (var cmd = new MySqlCommand("uspGetUnreturnedTransactionItems", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@transactionId", MySqlDbType.Int32);
                    cmd.Parameters["@transactionId"].Value = transactionId;
                    cmd.Parameters["@transactionId"].Direction = ParameterDirection.Input;

                    using MySqlDataReader reader = cmd.ExecuteReader();
                    int furnitureidordinal = reader.GetOrdinal("furnitureId");
                    int originalqtyordinal = reader.GetOrdinal("quantity");
                    int returnedsumordinal = reader.GetOrdinal("returnedSum");
                    int dailyrateordinal = reader.GetOrdinal("daily_rental_rate");

                    while (reader.Read())
                    {
                        rentalItems.Add(new RentalItem
                        {
                            FurnitureId = reader.GetFieldValueCheckNull<int>(furnitureidordinal),
                            Quantity = reader.GetFieldValueCheckNull<int>(originalqtyordinal) - (int)reader.GetFieldValueCheckNull<decimal>(returnedsumordinal),
                            DailyRentalRate = (double)reader.GetFieldValueCheckNull<decimal>(dailyrateordinal),
                            RentalId = transactionId,
                        });
                    }
                }
            }
            return rentalItems;
        }
    }
}