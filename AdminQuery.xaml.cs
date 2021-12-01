using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UWG_CS3230_FurnitureRental.DAL;
using UWG_CS3230_FurnitureRental.Utilities;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UWG_CS3230_FurnitureRental
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AdminQuery : Page
    {
        public AdminQuery()
        {
            this.InitializeComponent();
        }

        private void handleReturn(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Admin));
        }

        private void handleSendQuery(object sender, RoutedEventArgs e)
        {
            AdminDAL adal = new AdminDAL();
            DataTable table = new DataTable();
            table = adal.AdminQuery(this.queryTextBox.Text);
            OrderFormatter.FillDataGrid(table, this.queryDataGrid);
        }
    }
}
