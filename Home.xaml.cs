using System;
using Windows.UI.Xaml.Controls;
using UWG_CS3230_FurnitureRental.Model;
using UWG_CS3230_FurnitureRental.DAL;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UWG_CS3230_FurnitureRental.Utilities;

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
        private ObservableCollection<RentalItem> rentalItems = new ObservableCollection<RentalItem>();

        private Furniture selectedFurniture { get; set; }

        public Home()
        {
            this.InitializeComponent();
            this.setupEmployeeHeader();
            this.hideOrder();
            this.setupQuantity();
        }

        private void setupQuantity()
        {
            ObservableCollection<int> rentalItems = new ObservableCollection<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            this.quantityComboBox.ItemsSource = rentalItems;
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

        private async System.Threading.Tasks.Task setupCancelOrderDialogAsync()
        {
            ContentDialog cancelOrderDialog = new ContentDialog
            {
                Title = "Are you sure you wish to cancel the order?",
                PrimaryButtonText = "Yes",
                CloseButtonText = "No"
            };
            ContentDialogResult result = await cancelOrderDialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                this.placeNewOrderButton.Visibility = Windows.UI.Xaml.Visibility.Visible;
                this.hideOrder();
            }
            else
            {
             
            }
        }

        private async System.Threading.Tasks.Task setupCancelOrderItem()
        {
            RentalItem rentalItem = (RentalItem)this.orderListView.SelectedItems[0];
            ContentDialog cancelOrderDialog = new ContentDialog
            {
                Title = "Are you sure you wish to remove the item: " + this.fdal.GetFurnitureById(rentalItem.FurnitureId) + " Quantity:  " + rentalItem.Quantity,
                PrimaryButtonText = "Yes",
                CloseButtonText = "No"
            };
            ContentDialogResult result = await cancelOrderDialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                this.rentalItems.Remove(rentalItem);
                this.orderListView.ItemsSource = this.rentalItems;
                this.orderTotalTextBox.Text = "Total: " + OrderFormatter.CalculateFormatOrderCost(this.rentalItems);
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
                Furniture furniture = (Furniture)this.furnitureListView.SelectedItems[0];
                int quantity = Int32.Parse(this.quantityComboBox.SelectedValue.ToString());
                double price = Convert.ToDouble(this.priceTextBox.Text);
                RentalItem rentalItem = new RentalItem(furniture.Id, quantity, price);

                this.rentalItems.Add(rentalItem);
                this.orderListView.ItemsSource = this.rentalItems;
                this.orderTotalTextBox.Text = "Total: " + OrderFormatter.CalculateFormatOrderCost(this.rentalItems);

                this.priceTextBox.Text = "";
                this.quantityComboBox.SelectedIndex = 0;
            }
        }

        private void displayOrder()
        {
            this.addFurnitureButton.Visibility = Windows.UI.Xaml.Visibility.Visible;
            this.placeOrderButton.Visibility = Windows.UI.Xaml.Visibility.Visible;
            this.cancelOrderButton.Visibility = Windows.UI.Xaml.Visibility.Visible;
            this.orderBorder.Visibility = Windows.UI.Xaml.Visibility.Visible;
            this.orderDetailsTextBox.Visibility = Windows.UI.Xaml.Visibility.Visible;
            this.orderTotalTextBox.Visibility = Windows.UI.Xaml.Visibility.Visible;
            this.orderListView.Visibility = Windows.UI.Xaml.Visibility.Visible;
            this.quantityComboBox.Visibility = Windows.UI.Xaml.Visibility.Visible;
            this.priceTextBox.Visibility = Windows.UI.Xaml.Visibility.Visible;
            this.removeFurnitureButton.Visibility = Windows.UI.Xaml.Visibility.Visible;
        }

        private void hideOrder()
        {
            this.addFurnitureButton.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            this.placeOrderButton.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            this.cancelOrderButton.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            this.orderBorder.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            this.orderDetailsTextBox.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            this.orderTotalTextBox.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            this.orderListView.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            this.quantityComboBox.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            this.priceTextBox.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            this.removeFurnitureButton.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

            this.customerOrder.Clear();
            this.orderListView.ItemsSource = this.customerOrder;
        }

        private void placeNewOrderButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            this.displayOrder();
            this.placeNewOrderButton.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        private void placeOrderButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {

        }

        private void cancelOrderButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            _ = this.setupCancelOrderDialogAsync();
        }

        private void removeFurnitureButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (this.orderListView.SelectedItem != null)
            {
                _ = this.setupCancelOrderItem();
            }    
        }
    }
}
