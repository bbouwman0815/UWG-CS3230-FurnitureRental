using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWG_CS3230_FurnitureRental.Model
{
    public class AdminDateQuery
    {
        public string memberID { get; set; }

        public string memberFirstName { get; set; }

        public string memberLastName { get; set; }

        public string rentalID { get; set; }

        public string furnitureDescription { get; set; }

        public string rentalItemQuantity { get; set; }

        public string transactionDate { get; set; }

        public static List<AdminDateQuery> convertToRows(string data)
        {
            List<AdminDateQuery> rows = new List<AdminDateQuery>();
            string[] parsedData = data.Split('\n');
            List<string> editedRows = new List<string>();

            foreach (string row in parsedData)
            {
                string editedRow = row.Replace("\r", "");
                editedRows.Add(editedRow);
            }

            foreach (string row in editedRows)
            {
                string[] rowData = row.Split(' ');
                for (int i = 8; i < rowData.Length; i++)
                {
                    rowData[8] = rowData[8] + " " + rowData[i];
                }
                rows.Add(
                    new AdminDateQuery
                    {
                        memberID = rowData[0],
                        memberFirstName = rowData[1],
                        memberLastName = rowData[2],
                        rentalID = rowData[3],
                        transactionDate = rowData[4],
                        rentalItemQuantity = rowData[7],
                        furnitureDescription = rowData[8]         
                    });

            }

            return rows;
        }
    }
}
