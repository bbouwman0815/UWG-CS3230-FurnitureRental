using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using UWG_CS3230_FurnitureRental.DAL;
using UWG_CS3230_FurnitureRental.Model;
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
    public sealed partial class Admin : Page
    {
        private readonly EmployeeDAL dal = new EmployeeDAL();
        private readonly FurnitureDAL fdal = new FurnitureDAL();
        private readonly MemberDAL mdal = new MemberDAL();

        private ObservableCollection<string> transactionTypes { get; set; }

        public Admin()
        {
            this.InitializeComponent();
            this.initializeCollections();
            this.setupAdminHeader();
        }

        private void initializeCollections()
        {
            List<string> transactions = new List<string>();
            transactions.Add("Rental Transaction");
            transactions.Add("Return Transaction");
            this.transactionTypes = new ObservableCollection<string>(transactions);
            this.transactionComboBox.ItemsSource = this.transactionTypes;
            this.queryButton.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        private async System.Threading.Tasks.Task setupAddItemAsync()
        {
            ContentDialog NewFurnitureContentDialog = new NewFurnitureContentDialog();
            _ = await NewFurnitureContentDialog.ShowAsync();
        }

        private void setupAdminHeader()
        {
            String identification = "Admin: ";
            identification += LoggedEmployee.CurrentLoggedEmployee.Fname + " ";
            identification += LoggedEmployee.CurrentLoggedEmployee.Lname;
            identification += Environment.NewLine;
            identification += "Employee Number: " + LoggedEmployee.CurrentLoggedEmployee.Id;
            identification += Environment.NewLine;
            identification += "Employee UserName: " + LoggedEmployee.CurrentLoggedEmployee.Uname;

            this.AdminInfoTextBlock.Text = identification;
        }

        private void logoutButton_Click(object sender, RoutedEventArgs e)
        {
            _ = this.setupLogoutDialogAsync();
        }

        private async System.Threading.Tasks.Task setupLogoutDialogAsync()
        {
            ContentDialog deleteFileDialog = new ContentDialog
            {
                Title = "Are you sure you want to logout?",
                PrimaryButtonText = "Logout",
                CloseButtonText = "Cancel"
            };
            ContentDialogResult result = await deleteFileDialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                Frame.Navigate(typeof(MainPage));
            }
        }

        private void handlePopulateTable(object sender, RoutedEventArgs e)
        {
            AdminDAL adal = new AdminDAL();
            DataTable table = new DataTable();
            DateTime starttime = this.startDatePicker.Date.DateTime;
            DateTime endTime = this.endDatePicker.Date.DateTime;
            var formattedStartDate = starttime.ToString("yyyy-MM-dd");
            var formattedEndDate = endTime.ToString("yyyy-MM-dd");
            if (this.transactionComboBox.SelectedValue.ToString().Equals("Rental Transaction"))
            {
                table = adal.RentalTransactionDateQuery(formattedStartDate, formattedEndDate);
            }
            else
            {
                table = adal.ReturnTransactionDateQuery(formattedStartDate, formattedEndDate);
            }
            OrderFormatter.FillDataGrid(table, this.queryDataGrid);
        }

        private void handleValidateSelections(object sender, DatePickerValueChangedEventArgs e)
        {
            this.validateQuery();
        }

        private void validateQuery()
        {
            DateTime starttime = this.startDatePicker.Date.DateTime;
            DateTime endTime = this.endDatePicker.Date.DateTime;

            this.queryButton.Visibility = Windows.UI.Xaml.Visibility.Visible;

            if (starttime >= endTime)
            {
                this.queryButton.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }

            if (starttime == null | endTime == null | this.transactionComboBox.SelectedIndex == -1)
            {
                this.queryButton.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }    
        }

        private void handleValidateSelections(object sender, SelectionChangedEventArgs e)
        {
            this.validateQuery();
        }

        private void handleWriteQuery(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AdminQuery));
        }
    }
}
