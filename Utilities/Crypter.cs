using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UWG_CS3230_FurnitureRental.DAL;
using UWG_CS3230_FurnitureRental.Model;

namespace UWG_CS3230_FurnitureRental.Utilities
{
    // Credit to https://www.youtube.com/watch?v=EEItNLDw0-A
    public class Crypter
    {
        public static bool VerifyLogin(String username, String password)
        {
            EmployeeDAL dal = new EmployeeDAL();
            String encryptedPassword = Encrypt(password);
            LoggedEmployee.CurrentLoggedEmployee = dal.VerifyEmployeeLogin(username, encryptedPassword);

            return LoggedEmployee.CurrentLoggedEmployee != null;
        }
        public static string Encrypt(String decrypted)
        {
            string hash = "Bbouwmanlmfao@2021$";
            byte[] data = UTF8Encoding.UTF8.GetBytes(decrypted);

            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            TripleDESCryptoServiceProvider tripDES = new TripleDESCryptoServiceProvider();

            tripDES.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
            tripDES.Mode = CipherMode.ECB;

            ICryptoTransform transform = tripDES.CreateEncryptor();
            byte[] result = transform.TransformFinalBlock(data, 0, data.Length);

            return Convert.ToBase64String(result);
        }

        public static string Decrypt(String encrypted)
        {
            string hash = "Bbouwmanlmfao@2021$";
            byte[] data = Convert.FromBase64String(encrypted);

            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            TripleDESCryptoServiceProvider tripDES = new TripleDESCryptoServiceProvider();

            tripDES.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
            tripDES.Mode = CipherMode.ECB;

            ICryptoTransform transform = tripDES.CreateDecryptor();
            byte[] result = transform.TransformFinalBlock(data, 0, data.Length);

            return UTF8Encoding.UTF8.GetString(result);
        }
    }
}
