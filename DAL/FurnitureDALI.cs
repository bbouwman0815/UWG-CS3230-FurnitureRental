using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWG_CS3230_FurnitureRental.Model;

namespace UWG_CS3230_FurnitureRental.DAL
{
    public class FurnitureDAL
    {

        /// <summary>
        /// Gets the furniture inventory.
        /// </summary>
        /// <returns>
        /// furnitureList the list of available furniture
        /// </returns>
        public ObservableCollection<Furniture> GetFurnitureInventory()
        {
            ObservableCollection<Furniture> furnitureList = new ObservableCollection<Furniture>();
            using (MySqlConnection connection = new MySqlConnection(Connection.connectionString))
            {
                connection.Open();
                String query = "select * from furniture;";

                using MySqlCommand command = new MySqlCommand(query, connection);

                using MySqlDataReader reader = command.ExecuteReader();
                int idordinal = reader.GetOrdinal("id");
                int styleordinal = reader.GetOrdinal("style");
                int categoryordinal = reader.GetOrdinal("category");
                int descriptionordinal = reader.GetOrdinal("description");
                int availableordinal = reader.GetOrdinal("available");
                int rentedordinal = reader.GetOrdinal("rented");
                int rentalrateordinal = reader.GetOrdinal("rentalRate");

                while (reader.Read())
                {
                    furnitureList.Add(new Furniture
                    {
                        Id = reader.GetFieldValueCheckNull<int>(idordinal),
                        Style = reader.GetFieldValueCheckNull<string>(styleordinal),
                        Category = reader.GetFieldValueCheckNull<string>(categoryordinal),
                        Description = reader.GetFieldValueCheckNull<string>(descriptionordinal),
                        Available = reader.GetFieldValueCheckNull<int>(availableordinal),
                        Rented = reader.GetFieldValueCheckNull<int>(rentedordinal),
                        RentalRate = reader.GetFieldValueCheckNull<double>(rentalrateordinal)
                    });
                }
            }
            return furnitureList;
        }

        public Furniture GetFurnitureById(int id)
        {
            Furniture furniture = new Furniture();
            using (MySqlConnection connection = new MySqlConnection(Connection.connectionString))
            {
                connection.Open();
                String query = "select * from furniture where id = @id;";

                using MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;

                using MySqlDataReader reader = command.ExecuteReader();
                int idordinal = reader.GetOrdinal("id");
                int styleordinal = reader.GetOrdinal("style");
                int categoryordinal = reader.GetOrdinal("category");
                int descriptionordinal = reader.GetOrdinal("description");
                int availableordinal = reader.GetOrdinal("available");
                int rentedordinal = reader.GetOrdinal("rented");
                int rentalrateordinal = reader.GetOrdinal("rentalRate");

                while (reader.Read())
                {
                    furniture = (new Furniture
                    {
                        Id = reader.GetFieldValueCheckNull<int>(idordinal),
                        Style = reader.GetFieldValueCheckNull<string>(styleordinal),
                        Category = reader.GetFieldValueCheckNull<string>(categoryordinal),
                        Description = reader.GetFieldValueCheckNull<string>(descriptionordinal),
                        Available = reader.GetFieldValueCheckNull<int>(availableordinal),
                        Rented = reader.GetFieldValueCheckNull<int>(rentedordinal),
                        RentalRate = reader.GetFieldValueCheckNull<double>(rentalrateordinal)
                    });
                }
            }
            return furniture;
        }

        public ObservableCollection<Furniture> SearchFurnitureByDescription(string search)
        {
            ObservableCollection<Furniture> furnitureList = new ObservableCollection<Furniture>();
            using (MySqlConnection connection = new MySqlConnection(Connection.connectionString))
            {
                connection.Open();
                String query = "select * from furniture where description LIKE @search";

                using MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@search", "%" + search + "%");

                using MySqlDataReader reader = command.ExecuteReader();
                int idordinal = reader.GetOrdinal("id");
                int styleordinal = reader.GetOrdinal("style");
                int categoryordinal = reader.GetOrdinal("category");
                int descriptionordinal = reader.GetOrdinal("description");
                int availableordinal = reader.GetOrdinal("available");
                int rentedordinal = reader.GetOrdinal("rented");
                int rentalrateordinal = reader.GetOrdinal("rentalRate");

                while (reader.Read())
                {
                    furnitureList.Add(new Furniture
                    {
                        Id = reader.GetFieldValueCheckNull<int>(idordinal),
                        Style = reader.GetFieldValueCheckNull<string>(styleordinal),
                        Category = reader.GetFieldValueCheckNull<string>(categoryordinal),
                        Description = reader.GetFieldValueCheckNull<string>(descriptionordinal),
                        Available = reader.GetFieldValueCheckNull<int>(availableordinal),
                        Rented = reader.GetFieldValueCheckNull<int>(rentedordinal),
                        RentalRate = reader.GetFieldValueCheckNull<double>(rentalrateordinal)

                    });

                }
            }
            return furnitureList;
        }

        public void UpdateAvailableFurnitureQuantity(int id, int available)
        {
            using MySqlConnection connection = new MySqlConnection(Connection.connectionString);
            connection.Open();
            String query = "UPDATE furniture SET available = @available where id = @id;";

            using MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
            command.Parameters.AddWithValue("@available", available);
            command.ExecuteNonQuery();
        }

        public void UpdateRentedFurnitureQuantity(int id, int rented)
        {
            using MySqlConnection connection = new MySqlConnection(Connection.connectionString);
            connection.Open();
            String query = "UPDATE furniture SET rented = @rented where id = @id;";

            using MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
            command.Parameters.AddWithValue("@rented", rented);
            command.ExecuteNonQuery();
        }

        public ObservableCollection<string> GetCategories()
        {
            ObservableCollection<string> categoryList = new ObservableCollection<string>();
            using (MySqlConnection connection = new MySqlConnection(Connection.connectionString))
            {
                connection.Open();
                String query = "select type from category";

                using MySqlCommand command = new MySqlCommand(query, connection);
                using MySqlDataReader reader = command.ExecuteReader();
                int typeordinal = reader.GetOrdinal("type");

                while (reader.Read())
                {
                    string category = reader.GetFieldValueCheckNull<string>(typeordinal);
                    categoryList.Add(category);
                };
            }
            return categoryList;
        }

        public ObservableCollection<string> GetStyles()
        {
            ObservableCollection<string> stylesList = new ObservableCollection<string>();
            using (MySqlConnection connection = new MySqlConnection(Connection.connectionString))
            {
                connection.Open();
                String query = "select type from style";

                using MySqlCommand command = new MySqlCommand(query, connection);
                using MySqlDataReader reader = command.ExecuteReader();
                int typeordinal = reader.GetOrdinal("type");

                while (reader.Read())
                {
                    string category = reader.GetFieldValueCheckNull<string>(typeordinal);
                    stylesList.Add(category);
                };
            }
            return stylesList;
        }

        public void AddFurnitureToInventory(Furniture furniture)
        {
            using MySqlConnection connection = new MySqlConnection(Connection.connectionString);
            connection.Open();
            String query = "call uspInsertFurniture (@style, @category, @description, @available, @rented, @rentalRate)";

            using MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.Add(new MySqlParameter("category", furniture.Category));
            command.Parameters.Add(new MySqlParameter("style", furniture.Style));
            command.Parameters.Add(new MySqlParameter("description", furniture.Description));
            command.Parameters.Add(new MySqlParameter("available", furniture.Available));
            command.Parameters.Add(new MySqlParameter("rented", furniture.Rented));
            command.Parameters.Add(new MySqlParameter("rentalRate", furniture.RentalRate));         
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
}
