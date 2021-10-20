using System;
using MySql.Data.MySqlClient;

namespace UWG_CS3230_FurnitureRental.DAL
{/// <summary>
///  Helper class define an Extension method that checks if a column is null before retturn its value
/// </summary>
    public static class DataReaderHelper
    {
        /// <summary>
        /// Extension method that checks if a column is null before return its value.
        /// 
        /// </summary>
        /// <typeparam name="T"> column type</typeparam>
        /// <param name="reader">DataReader object</param>
        /// <param name="columnOrdinal">column ordinal</param>
        /// <returns></returns>
        public static T GetFieldValueCheckNull<T>(this MySqlDataReader reader, int columnOrdinal)
        {
            T returnValue = default;

            if (!reader[columnOrdinal].Equals(DBNull.Value)){
                returnValue = (T)reader[columnOrdinal];
            }
            return returnValue;
        }
    }
}
