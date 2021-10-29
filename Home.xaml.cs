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

        private ObservableCollection<Furniture> customerOrder = new ObservableCollection<Furniture>();

        private Furniture selectedFurniture { get; set; }

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

        private void HandleSearchTextChange(object sender, TextChangedEventArgs e)
        {
            string search = this.searchInputTextBox.Text;

            ObservableCollection<Furniture> furniture = this.fdal.getFurnitureBySearch(search);
            this.furnitureListView.ItemsSource = furniture;
        }

        private void addFurnitureButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (this.furnitureListView.SelectedItem != null)
            {
                this.customerOrder.Add((Furniture)this.furnitureListView.SelectedItems[0]);
                this.orderListView.ItemsSource = this.customerOrder;
                
            }
        }
    }
}
