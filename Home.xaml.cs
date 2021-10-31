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

        private ObservableCollection<Furniture> inventory { get; set; }
        private ObservableCollection<RentalItem> rentalItems { get; set; }

        private ObservableCollection<int> quantityAv { get; set; }

        private Furniture selectedFurniture { get; set; }

        private RentalItem selectedRentalItem { get; set; }

        private int selectedQuantity { get; set; }

        public Home()
        {
            this.InitializeComponent();
            this.initializeCollections();
            this.setupEmployeeHeader();
            this.hideOrder();
        }

        private void initializeCollections()
        {
            this.inventory = new ObservableCollection<Furniture>();
            this.rentalItems = new ObservableCollection<RentalItem>();
            this.quantityAv = new ObservableCollection<int>();
            this.quantityComboBox.ItemsSource = this.quantityAv;
            this.selectedFurniture = new Furniture();
            this.selectedRentalItem = new RentalItem();
            this.selectedQuantity = 0;
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

            this.inventory = this.fdal.getFurnitureInventory();
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

        private async System.Threading.Tasks.Task setupCancelOrderDialogAsync()
        {
            ContentDialog cancelOrderDialog = new ContentDialog
            {
                Title = "Are you sure you want to cancel the order?",
                PrimaryButtonText = "Yes",
                CloseButtonText = "No"
            };
            ContentDialogResult result = await cancelOrderDialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                this.placeNewOrderButton.Visibility = Windows.UI.Xaml.Visibility.Visible;
                this.priceTextBox.Text = "";
                this.quantityComboBox.SelectedIndex = 0;
                this.hideOrder();
            }
        }

        private async System.Threading.Tasks.Task setupCancelOrderItem()
        {
            RentalItem rentalItem = (RentalItem)this.orderListView.SelectedItems[0];
            ContentDialog cancelOrderDialog = new ContentDialog
            {
                Title = "Are you sure you want to remove the item: " + this.fdal.GetFurnitureById(rentalItem.FurnitureId) + " | Quantity:  " + rentalItem.Quantity,
                PrimaryButtonText = "Yes",
                CloseButtonText = "No"
            };
            ContentDialogResult result = await cancelOrderDialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                this.rentalItems.Remove(rentalItem);
                this.orderTotalTextBox.Text = "Total: " + OrderFormatter.CalculateFormatOrderCost(this.rentalItems);

                foreach (Furniture currentFurniture in this.inventory)
                {
                    if (currentFurniture.Id == rentalItem.FurnitureId)
                    {
                        currentFurniture.UpdateQuantity(rentalItem.Quantity, "remove");
                    }
                }
            }
        }

        private async System.Threading.Tasks.Task setupHelpDialogAsync()
        {
            ContentDialog helpDialog = new ContentDialog
            {
                Title = "Before you can add the item, check that the item, the price, and the quantity are selected.",
                PrimaryButtonText = "Ok",
            };

            ContentDialogResult result = await helpDialog.ShowAsync();
        }

        private void HandleSearchTextChange(object sender, TextChangedEventArgs e)
        {
            string search = this.searchInputTextBox.Text;
            ObservableCollection<Furniture> foundFurniture = this.fdal.getFurnitureBySearch(search);
            this.inventory = foundFurniture;
            this.furnitureListView.ItemsSource = this.inventory;
        }

        private void addFurnitureButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (this.furnitureListView.SelectedItem == null | this.priceTextBox.Text == "" | this.quantityComboBox.SelectedValue == null | !(OrderFormatter.VerifyPrice(this.priceTextBox.Text)))
            {
                _ = this.setupHelpDialogAsync();
                return;
            }

            Furniture furniture = this.selectedFurniture;
            int quantity = Int32.Parse(this.quantityComboBox.SelectedValue.ToString());
            double price = Convert.ToDouble(this.priceTextBox.Text);
            RentalItem rentalItem = new RentalItem(furniture.Id, quantity, price);

            this.rentalItems.Add(rentalItem);
            furniture.UpdateQuantity(quantity, "add");
            this.quantityAv = new ObservableCollection<int>();
            this.selectedFurniture = null;
   
            this.orderTotalTextBox.Text = "Total: " + OrderFormatter.CalculateFormatOrderCost(this.rentalItems);

            this.priceTextBox.Text = "";
            this.quantityComboBox.SelectedIndex = 0;
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

            this.rentalItems.Clear();
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

        private void handleSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.selectedFurniture != null)
            {
                this.quantityAv = this.selectedFurniture.GetQuantityRange();
                this.quantityComboBox.ItemsSource = this.quantityAv;
            }
        }
    }
}
