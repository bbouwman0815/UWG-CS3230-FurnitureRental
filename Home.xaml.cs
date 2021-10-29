using System;
using Windows.UI.Xaml.Controls;
using UWG_CS3230_FurnitureRental.Model;
using UWG_CS3230_FurnitureRental.DAL;
using System.Collections.Generic;
using System.Collections.ObjectModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UWG_CS3230_FurnitureRental
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Home : Page
    {
        private readonly EmployeeDAL dal = new EmployeeDAL();
        private readonly FurnitureDAL fdal = new FurnitureDAL();

        private ObservableCollection<Furniture> furniture;

        public Home()
        {
            this.InitializeComponent();
            this.setupEmployeeHeader();
        }

        private void setupEmployeeHeader()
        {
            String identification = "Employee Name: ";
            identification += LoggedEmployee.CurrentLoggedEmployee.Fname + " ";
            identification += LoggedEmployee.CurrentLoggedEmployee.Lname;
            identification += Environment.NewLine;
            identification += "Employee Number: " + LoggedEmployee.CurrentLoggedEmployee.Id;
            identification += Environment.NewLine;
            identification += "Employee UserName: " + LoggedEmployee.CurrentLoggedEmployee.Uname;
           
            this.EmployeeInfoTextBlock.Text = identification;

            ObservableCollection<Furniture> furniture = this.fdal.getFurnitureInventory();
            this.furnitureListView.ItemsSource = furniture;
        }

        private async void onRegisterCustomerClick(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ContentDialog registerDialog = new RegisterCustomer();
            await registerDialog.ShowAsync();
        }

        private void logoutButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            _ = this.setupLogoutDialogAsync();
        }

        private async System.Threading.Tasks.Task setupLogoutDialogAsync()
        {
            ContentDialog deleteFileDialog = new ContentDialog
            {
                Title = "Are you sure you wish to logout?",
                PrimaryButtonText = "Logout",
                CloseButtonText = "Cancel"
            };
            ContentDialogResult result = await deleteFileDialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                Frame.Navigate(typeof(MainPage));
            }
            else
            {
            }
        }

        private async System.Threading.Tasks.Task notifyInvalidSearchResult()
        {
            ContentDialog noWifiDialog = new ContentDialog()
            {
                Title = "Invalid search result.",
                Content = "Use the Help option to see the search possibilities.",
                CloseButtonText = "Ok lol"
            };

            await noWifiDialog.ShowAsync();
        }

        private void searchButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            string search = this.searchInputTextBox.Text;

            ObservableCollection<Furniture> furniture = this.fdal.getFurnitureBySearch(search);
            this.furnitureListView.ItemsSource = furniture;
        }
    }
}
