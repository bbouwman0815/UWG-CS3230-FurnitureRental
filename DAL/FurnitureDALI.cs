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
        public ObservableCollection<Furniture> getFurnitureInventory()
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

                while (reader.Read())
                {
                    furnitureList.Add(new Furniture
                    {
                        Id = reader.GetFieldValueCheckNull<int>(idordinal),
                        StyleId = reader.GetFieldValueCheckNull<int>(styleordinal),
                        CategoryId = reader.GetFieldValueCheckNull<int>(categoryordinal),
                        Description = reader.GetFieldValueCheckNull<string>(descriptionordinal)
                    });

                }

            }
            return furnitureList;

        }

        public ObservableCollection<Furniture> getFurnitureByCategory(int categoryId)
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

                while (reader.Read())
                {
                    furnitureList.Add(new Furniture
                    {
                        Id = reader.GetFieldValueCheckNull<int>(idordinal),
                        StyleId = reader.GetFieldValueCheckNull<int>(styleordinal),
                        CategoryId = reader.GetFieldValueCheckNull<int>(categoryordinal),
                        Description = reader.GetFieldValueCheckNull<string>(descriptionordinal)
                    });

                }

            }
            return furnitureList;

        }

        public string getCategoryTypeById(int categoryId)
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

        public int getCategoryIdByType(string type)
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

        public int getStyleIdByType(string type)
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

        public ObservableCollection<Furniture> getFurnitureByStyle(int styleId)
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

                while (reader.Read())
                {
                    furnitureList.Add(new Furniture
                    {
                        Id = reader.GetFieldValueCheckNull<int>(idordinal),
                        StyleId = reader.GetFieldValueCheckNull<int>(styleordinal),
                        CategoryId = reader.GetFieldValueCheckNull<int>(categoryordinal),
                        Description = reader.GetFieldValueCheckNull<string>(descriptionordinal)
                    });

                }

            }
            return furnitureList;

        }

        public ObservableCollection<Furniture> getFurnitureBySearch(string search)
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

                while (reader.Read())
                {
                    furnitureList.Add(new Furniture
                    {
                        Id = reader.GetFieldValueCheckNull<int>(idordinal),
                        StyleId = reader.GetFieldValueCheckNull<int>(styleordinal),
                        CategoryId = reader.GetFieldValueCheckNull<int>(categoryordinal),
                        Description = reader.GetFieldValueCheckNull<string>(descriptionordinal)
                    });

                }

            }
            return furnitureList;

        }
    }
}
