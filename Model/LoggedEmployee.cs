using System;

namespace UWG_CS3230_FurnitureRental.Model
{
    /// <summary>
    /// The LoggedEmployee Class.
    /// </summary>
    public class LoggedEmployee
    {
        public string Fname { get; set; }

        public string Lname { get; set; }

        public string Uname { get; set; }

        public int Id { get; set; }

        public string Pword { get; set; }

        public static LoggedEmployee CurrentLoggedEmployee { get; set; }
    }
}
