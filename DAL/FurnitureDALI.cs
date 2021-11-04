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
                int styleordinal = reader.GetOrdinal("styleId");
                int categoryordinal = reader.GetOrdinal("categoryId");
                int descriptionordinal = reader.GetOrdinal("description");
                int availableordinal = reader.GetOrdinal("available");
                int rentedordinal = reader.GetOrdinal("rented");

                while (reader.Read())
                {
                    furnitureList.Add(new Furniture
                    {
                        Id = reader.GetFieldValueCheckNull<int>(idordinal),
                        StyleId = reader.GetFieldValueCheckNull<int>(styleordinal),
                        CategoryId = reader.GetFieldValueCheckNull<int>(categoryordinal),
                        Description = reader.GetFieldValueCheckNull<string>(descriptionordinal),
                        Available = reader.GetFieldValueCheckNull<int>(availableordinal),
                        Rented = reader.GetFieldValueCheckNull<int>(rentedordinal)
                    });
                }
            }
            return furnitureList;
        }

        public ObservableCollection<Furniture> SearchFurnitureByCategory(int categoryId)
        {
            ObservableCollection<Furniture> furnitureList = new ObservableCollection<Furniture>();
            using (MySqlConnection connection = new MySqlConnection(Connection.connectionString))
            {
                connection.Open();
                String query = "select * from furniture where categoryId = @categoryId;";

                using MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.Add("@categoryId", MySqlDbType.Int32).Value = categoryId;

                using MySqlDataReader reader = command.ExecuteReader();
                int idordinal = reader.GetOrdinal("id");
                int styleordinal = reader.GetOrdinal("styleId");
                int categoryordinal = reader.GetOrdinal("categoryId");
                int descriptionordinal = reader.GetOrdinal("description");
                int availableordinal = reader.GetOrdinal("available");
                int rentedordinal = reader.GetOrdinal("rented");

                while (reader.Read())
                {
                    furnitureList.Add(new Furniture
                    {
                        Id = reader.GetFieldValueCheckNull<int>(idordinal),
                        StyleId = reader.GetFieldValueCheckNull<int>(styleordinal),
                        CategoryId = reader.GetFieldValueCheckNull<int>(categoryordinal),
                        Description = reader.GetFieldValueCheckNull<string>(descriptionordinal),
                        Available = reader.GetFieldValueCheckNull<int>(availableordinal),
                        Rented = reader.GetFieldValueCheckNull<int>(rentedordinal)
                    });
                }
            }
            return furnitureList;
        }

        public string GetCategoryTypeById(int categoryId)
        {
            String category = "";
            using (MySqlConnection connection = new MySqlConnection(Connection.connectionString))
            {
                connection.Open();
                String query = "select type from category where id = @categoryId;";

                using MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.Add("@categoryId", MySqlDbType.Int32).Value = categoryId;

                using MySqlDataReader reader = command.ExecuteReader();
                int typeordinal = reader.GetOrdinal("type");

                while (reader.Read())
                {
                    category = reader.GetFieldValueCheckNull<string>(typeordinal);
                }
            }
            return category;
        }

        public string GetStyleTypeById(int styleId)
        {
            String category = "";
            using (MySqlConnection connection = new MySqlConnection(Connection.connectionString))
            {
                connection.Open();
                String query = "select type from style where id = @styleId;";

                using MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.Add("@styleId", MySqlDbType.Int32).Value = styleId;

                using MySqlDataReader reader = command.ExecuteReader();
                int typeordinal = reader.GetOrdinal("type");

                while (reader.Read())
                {
                    category = reader.GetFieldValueCheckNull<string>(typeordinal);
                }
            }
            return category;
        }

        public int GetCategoryIdByType(string type)
        {
            int id = -1;
            using (MySqlConnection connection = new MySqlConnection(Connection.connectionString))
            {
                connection.Open();
                String query = "select id from category where type = @type;";

                using MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.Add("@type", MySqlDbType.VarChar).Value = type;

                using MySqlDataReader reader = command.ExecuteReader();
                int idordinal = reader.GetOrdinal("id");

                while (reader.Read())
                {
                    id = reader.GetFieldValueCheckNull<int>(idordinal);
                }

            }
            return id;
        }

        public List<int> GetCategoriesBySearch(string search)
        {
            List<int> categoryList = new List<int>();
            using (MySqlConnection connection = new MySqlConnection(Connection.connectionString))
            {
                connection.Open();
                String query = "select id from category where type LIKE @search";

                using MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@search", "%" + search + "%");

                using MySqlDataReader reader = command.ExecuteReader();
                int idordinal = reader.GetOrdinal("id");

                while (reader.Read())
                {
                    int category = reader.GetFieldValueCheckNull<int>(idordinal);
                    categoryList.Add(category);
                    };
                }
            return categoryList;
        }

        public List<int> GetStylesBySearch(string search)
        {
            List<int> stylesList = new List<int>();
            using (MySqlConnection connection = new MySqlConnection(Connection.connectionString))
            {
                connection.Open();
                String query = "select id from style where type LIKE @search";

                using MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@search", "%" + search + "%");

                using MySqlDataReader reader = command.ExecuteReader();
                int idordinal = reader.GetOrdinal("id");

                while (reader.Read())
                {
                    int category = reader.GetFieldValueCheckNull<int>(idordinal);
                    stylesList.Add(category);
                };
            }
            return stylesList;
        }

        public int GetStyleIdByType(string type)
        {
            int id = -1;
            using (MySqlConnection connection = new MySqlConnection(Connection.connectionString))
            {
                connection.Open();
                String query = "select id from style where type = @type;";

                using MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.Add("@type", MySqlDbType.VarChar).Value = type;

                using MySqlDataReader reader = command.ExecuteReader();
                int idordinal = reader.GetOrdinal("id");

                while (reader.Read())
                {
                    id = reader.GetFieldValueCheckNull<int>(idordinal);
                }
            }
            return id;
        }

        public ObservableCollection<Furniture> GetFurnitureByStyle(int styleId)
        {
            ObservableCollection<Furniture> furnitureList = new ObservableCollection<Furniture>();
            using (MySqlConnection connection = new MySqlConnection(Connection.connectionString))
            {
                connection.Open();
                String query = "select * from furniture where styleId = @styleId;";

                using MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.Add("@styleId", MySqlDbType.Int32).Value = styleId;

                using MySqlDataReader reader = command.ExecuteReader();
                int idordinal = reader.GetOrdinal("id");
                int styleordinal = reader.GetOrdinal("styleId");
                int categoryordinal = reader.GetOrdinal("categoryId");
                int descriptionordinal = reader.GetOrdinal("description");
                int availableordinal = reader.GetOrdinal("available");
                int rentedordinal = reader.GetOrdinal("rented");

                while (reader.Read())
                {
                    furnitureList.Add(new Furniture
                    {
                        Id = reader.GetFieldValueCheckNull<int>(idordinal),
                        StyleId = reader.GetFieldValueCheckNull<int>(styleordinal),
                        CategoryId = reader.GetFieldValueCheckNull<int>(categoryordinal),
                        Description = reader.GetFieldValueCheckNull<string>(descriptionordinal),
                        Available = reader.GetFieldValueCheckNull<int>(availableordinal),
                        Rented = reader.GetFieldValueCheckNull<int>(rentedordinal)
                    });
                }
            }
            return furnitureList;
        }

        public ObservableCollection<Furniture> GetFurnitureByCategory(int categoryId)
        {
            ObservableCollection<Furniture> furnitureList = new ObservableCollection<Furniture>();
            using (MySqlConnection connection = new MySqlConnection(Connection.connectionString))
            {
                connection.Open();
                String query = "select * from furniture where categoryId = @categoryId;";

                using MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.Add("@categoryId", MySqlDbType.Int32).Value = categoryId;

                using MySqlDataReader reader = command.ExecuteReader();
                int idordinal = reader.GetOrdinal("id");
                int styleordinal = reader.GetOrdinal("styleId");
                int categoryordinal = reader.GetOrdinal("categoryId");
                int descriptionordinal = reader.GetOrdinal("description");
                int availableordinal = reader.GetOrdinal("available");
                int rentedordinal = reader.GetOrdinal("rented");

                while (reader.Read())
                {
                    furnitureList.Add(new Furniture
                    {
                        Id = reader.GetFieldValueCheckNull<int>(idordinal),
                        StyleId = reader.GetFieldValueCheckNull<int>(styleordinal),
                        CategoryId = reader.GetFieldValueCheckNull<int>(categoryordinal),
                        Description = reader.GetFieldValueCheckNull<string>(descriptionordinal),
                        Available = reader.GetFieldValueCheckNull<int>(availableordinal),
                        Rented = reader.GetFieldValueCheckNull<int>(rentedordinal)
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
                int styleordinal = reader.GetOrdinal("styleId");
                int categoryordinal = reader.GetOrdinal("categoryId");
                int descriptionordinal = reader.GetOrdinal("description");
                int availableordinal = reader.GetOrdinal("available");
                int rentedordinal = reader.GetOrdinal("rented");

                while (reader.Read())
                {
                    furniture = (new Furniture
                    {
                        Id = reader.GetFieldValueCheckNull<int>(idordinal),
                        StyleId = reader.GetFieldValueCheckNull<int>(styleordinal),
                        CategoryId = reader.GetFieldValueCheckNull<int>(categoryordinal),
                        Description = reader.GetFieldValueCheckNull<string>(descriptionordinal),
                        Available = reader.GetFieldValueCheckNull<int>(availableordinal),
                        Rented = reader.GetFieldValueCheckNull<int>(rentedordinal)
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
                int styleordinal = reader.GetOrdinal("styleId");
                int categoryordinal = reader.GetOrdinal("categoryId");
                int descriptionordinal = reader.GetOrdinal("description");
                int availableordinal = reader.GetOrdinal("available");
                int rentedordinal = reader.GetOrdinal("rented");

                while (reader.Read())
                {
                    furnitureList.Add(new Furniture
                    {
                        Id = reader.GetFieldValueCheckNull<int>(idordinal),
                        StyleId = reader.GetFieldValueCheckNull<int>(styleordinal),
                        CategoryId = reader.GetFieldValueCheckNull<int>(categoryordinal),
                        Description = reader.GetFieldValueCheckNull<string>(descriptionordinal),
                        Available = reader.GetFieldValueCheckNull<int>(availableordinal),
                        Rented = reader.GetFieldValueCheckNull<int>(rentedordinal)
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
    }
}
