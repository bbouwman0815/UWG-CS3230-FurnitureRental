using System;
using Windows.UI.Xaml.Controls;
using UWG_CS3230_FurnitureRental.Model;
using UWG_CS3230_FurnitureRental.DAL;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Windows.UI.Xaml;
using UWG_CS3230_FurnitureRental.Utilities;
using System.Linq;
using System.Text.RegularExpressions;

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
        private readonly MemberDAL mdal = new MemberDAL();
        private readonly RentalTransactionDAL rdal = new RentalTransactionDAL();

        private ObservableCollection<Furniture> inventory { get; set; }
        private ObservableCollection<Customer> members { get; set; }
        private ObservableCollection<RentalItem> rentalItems { get; set; }

        private ObservableCollection<RentalTransaction> selectedMemberTransactions{ get; set; }
        private ObservableCollection<string> styles { get; set; }
        private ObservableCollection<string> categories { get; set; }

        private ObservableCollection<int> quantityAv { get; set; }

        private Furniture selectedFurniture { get; set; }
        private Customer selectedMember { get; set; }

        private RentalItem selectedRentalItem { get; set; }

        private RentalTransaction selectedTransaction { get; set; }
        private ObservableCollection<RentalItem> selectedTransactionItems { get; set; }

        private int selectedQuantity { get; set; }

        private int rentalPeriod { get; set; }

        private ObservableCollection<ReturnItem> itemsToReturn { get; set; }

        private ReturnItem selectedItemToReturn { get; set; }

        public Home()
        {
            this.InitializeComponent();
            this.initializeCollections();
            this.setupTypeComboBoxes();
            this.setupMemberSearchByComboBox();
            this.setupEmployeeHeader();
            this.hideOrder();
        }

        private void initializeCollections()
        {
            this.inventory = new ObservableCollection<Furniture>();
            this.members = new ObservableCollection<Customer>();
            this.rentalItems = new ObservableCollection<RentalItem>();
            this.quantityAv = new ObservableCollection<int>();
            this.selectedMemberTransactions = new ObservableCollection<RentalTransaction>();
            this.selectedTransactionItems = new ObservableCollection<RentalItem>();
            this.itemsToReturn = new ObservableCollection<ReturnItem>();
            
            this.quantityComboBox.ItemsSource = this.quantityAv;
            this.selectedFurniture = new Furniture();
            this.selectedTransaction = new RentalTransaction();
            this.selectedMember = null;
            this.selectedRentalItem = new RentalItem();
            this.selectedItemToReturn = new ReturnItem();
            this.selectedQuantity = 0;
            this.rentalPeriod = 0;
        }

        private void setupTypeComboBoxes()
        {
            FurnitureDAL fdal = new FurnitureDAL();
            this.categories = fdal.GetCategories();
            this.styles = fdal.GetStyles();

            this.styleComboBox.ItemsSource = this.styles;
            this.categoryComboBox.ItemsSource = this.categories;
        }

        private void setupMemberSearchByComboBox()
        {
            this.searchMemberByComboBox.ItemsSource = new ObservableCollection<string>() { "Id", "Phone", "Name" };
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

            this.inventory = this.fdal.GetFurnitureInventory();
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
                this.quantityComboBox.SelectedIndex = 0;
                this.hideOrder();
            }
        }

        private async System.Threading.Tasks.Task setupConfirmOrderDialogAsync()
        {
            ContentDialog confirmOrderDialog = new ContentDialog
            {
                Title = "Are you sure you want to place the following order?",
                PrimaryButtonText = "Yes",
                CloseButtonText = "No"
            };
            ContentDialogResult result = await confirmOrderDialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                RentalTransaction rentalTransaction = new RentalTransaction
                {
                    id = null,
                    cost = OrderFormatter.CalculateOrderCost(this.rentalItems, this.rentalPeriod),
                    transactionDate = DateTime.Now,
                    dueDate = this.orderDatePicker.Date.DateTime,
                    employeeId = LoggedEmployee.CurrentLoggedEmployee.Id,
                    memberId = this.selectedMember.id.Value
                };

                this.rdal.CreateNewRentalTransaction(rentalTransaction, new List<RentalItem>(this.rentalItems));
                this.rentalItems.Clear();
                this.hideOrder();
            }
        }

        private async System.Threading.Tasks.Task setupHelpDialogAsync()
        {
            ContentDialog helpDialog = new ContentDialog
            {
                Title = "Before you can add the item, check that the item, the price, and the Quantity are selected.",
                PrimaryButtonText = "Ok",
            };

            ContentDialogResult result = await helpDialog.ShowAsync();
        }

        private void HandleSearchTextChange(object sender, TextChangedEventArgs e)
        {
            string search = this.searchInputTextBox.Text;
            if (search == "reset")
            {
                this.handleResetFilters();
                this.searchInputTextBox.Text = "";
            }
            if (Regex.IsMatch(search, @"^\d"))
            {
                Furniture furnitureByID = this.fdal.GetFurnitureById(Int32.Parse(search));
                if (furnitureByID.Id != 0) {
                    this.inventory.Clear();
                    this.inventory.Add(furnitureByID);
                    this.ConfigureQuantities();
                    this.furnitureListView.ItemsSource = this.inventory;

                    this.applyFilters();

                    return;
                }
            }
            this.inventory = this.fdal.SearchFurnitureByDescription(search);
            this.ConfigureQuantities();
            this.furnitureListView.ItemsSource = this.inventory;

            this.applyFilters();

        }

        private void ConfigureQuantities()
        {
            foreach (RentalItem currentItem in this.rentalItems)
            {
                foreach (Furniture currentFurniture in this.inventory)
                {
                    if (currentItem.FurnitureId == currentFurniture.Id)
                    {
                        currentFurniture.RemoveQuantity(currentItem.Quantity);
                    }
                }
            }
        }

        private void addFurnitureButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (this.furnitureListView.SelectedItem == null | this.quantityComboBox.SelectedValue == null)
            {
                _ = this.setupHelpDialogAsync();
                return;
            }
            this.addFurniture();
        }

        private void addFurniture()
        {
            Furniture furniture = this.selectedFurniture;
            int quantity = Int32.Parse(this.quantityComboBox.SelectedValue.ToString());
            double price = furniture.RentalRate;
            RentalItem rentalItem = new RentalItem(furniture.Id, quantity, price);

            this.rentalItems.Add(rentalItem);
            furniture.RemoveQuantity(quantity);
            this.quantityAv = furniture.GetQuantityRange();
            this.quantityComboBox.ItemsSource = this.quantityAv;
            this.refreshDisplay();

            this.updateTotal();
            this.searchInputTextBox.Text = "";
            this.quantityComboBox.SelectedIndex = 0;
        }

        private void addEditedFurniture()
        {
            RentalItem rentalItem = RentalItem.SelectedRentalItem;
            Furniture furniture = this.fdal.GetFurnitureById(rentalItem.FurnitureId);
            this.rentalItems.Add(rentalItem);
            this.quantityAv = furniture.GetQuantityRange();
            this.quantityComboBox.ItemsSource = this.quantityAv;
            this.refreshDisplay();

            this.updateTotal();
            this.searchInputTextBox.Text = "";
            this.quantityComboBox.SelectedIndex = 0;
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
                this.removeOrderItem(rentalItem);
            }
        }

        private void removeOrderItem(RentalItem rentalItem)
        {
            this.rentalItems.Remove(rentalItem);
            this.updateTotal();

            Furniture furniture = new Furniture();
            foreach (Furniture currentFurniture in this.inventory)
            {
                if (currentFurniture.Id == rentalItem.FurnitureId)
                {
                    furniture = currentFurniture;
                }
            }
            furniture.AddQuantity(rentalItem.Quantity);
            this.quantityAv = furniture.GetQuantityRange();
            this.quantityComboBox.ItemsSource = this.quantityAv;
            this.refreshDisplay();
        }

        private void displayOrder()
        {
            if (this.furniturePanel.Visibility == Visibility.Visible)
            {
                this.quantityComboBox.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }

            this.addFurnitureButton.Visibility = Windows.UI.Xaml.Visibility.Visible;
            this.cancelOrderButton.Visibility = Windows.UI.Xaml.Visibility.Visible;
            this.orderBorder.Visibility = Windows.UI.Xaml.Visibility.Visible;
            this.orderDetailsTextBox.Visibility = Windows.UI.Xaml.Visibility.Visible;
            this.orderTotalTextBox.Visibility = Windows.UI.Xaml.Visibility.Visible;
            this.orderListView.Visibility = Windows.UI.Xaml.Visibility.Visible;
            this.removeFurnitureButton.Visibility = Windows.UI.Xaml.Visibility.Visible;
            this.orderDatePicker.Visibility = Windows.UI.Xaml.Visibility.Visible;
            this.orderDueDateTextBlock.Visibility = Visibility.Visible;
            this.editItemButton.Visibility = Visibility.Visible;
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
            this.removeFurnitureButton.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            this.orderDatePicker.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            this.orderDueDateTextBlock.Visibility = Visibility.Collapsed;
            this.placeNewOrderButton.Visibility = Windows.UI.Xaml.Visibility.Visible;
            this.editItemButton.Visibility = Visibility.Collapsed;

            this.rentalItems.Clear();
            this.orderDetailsTextBox.Text = "";
            this.orderDatePicker.SelectedDate = null;
            this.searchInputTextBox.Text = "";
            this.orderTotalTextBox.Text = "";
            this.rentalPeriod = 0;
            this.ConfigureQuantities();
            this.inventory.Clear();
            this.inventory = fdal.GetFurnitureInventory();
            this.furnitureListView.ItemsSource = this.inventory;
        }

        private void placeNewOrderButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            this.displayOrder();
            this.placeNewOrderButton.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            this.rentalTransactionsBorder.Visibility = Visibility.Collapsed;
        }

        private void placeOrderButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (this.selectedMember != null)
            {
                _ = this.setupConfirmOrderDialogAsync();
            }
            else
            {
                this.selectMemberDialog();
            }
        }

        private async void selectMemberDialog()
        {
            ContentDialog helpDialog = new ContentDialog
            {
                Title = "Before you can place the order, check that a member has been selected.",
                PrimaryButtonText = "Ok",
            };
            ContentDialogResult result = await helpDialog.ShowAsync();
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

        private void refreshDisplay()
        {
            this.inventory = this.fdal.GetFurnitureInventory();
            this.ConfigureQuantities();
            this.furnitureListView.ItemsSource = this.inventory;
            this.searchInputTextBox.Text = "";
        }

        private void handleDateChanged(object sender, DatePickerValueChangedEventArgs e)
        {
            DateTime currentDate = DateTime.Now;
            DateTime returnDate = this.orderDatePicker.Date.DateTime;
            if (returnDate < currentDate)
            {
                return;
            }
            this.rentalPeriod = (returnDate.Date - currentDate.Date).Days;
            this.updateTotal();
        }

        private void updateTotal()
        {
            this.orderTotalTextBox.Text = "Total: " + OrderFormatter.CalculateFormatOrderCost(this.rentalItems, this.rentalPeriod);
            this.updateOrderDetails();
            if (OrderFormatter.CalculateOrderCost(this.rentalItems, this.rentalPeriod) > 0)
            {
                this.placeOrderButton.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
        }

        private void updateOrderDetails()
        {
            string details = "";
            details += "Rental Period: " + this.rentalPeriod + " days";
            details += "   Total: " + OrderFormatter.CalculateFormatOrderCost(this.rentalItems, this.rentalPeriod);
            if (this.selectedMember != null)
            {
                details += "   Member Id: " + this.selectedMember.id;
            }
            
            this.orderDetailsTextBox.Text = details;
        }

        private void handleFilterSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.applyFilters();
        }

        private void applyFilters()
        {
         
            ObservableCollection<Furniture> furnitureToDisplay = new ObservableCollection<Furniture>();
            FurnitureDAL fdal = new FurnitureDAL();
            this.inventory = this.fdal.SearchFurnitureByDescription(this.searchInputTextBox.Text);
            if (this.categoryComboBox.SelectedIndex > -1 && this.styleComboBox.SelectedIndex > -1)
            {
             
                string styleType = this.styleComboBox.SelectedValue.ToString();
         
                string categoryType = this.categoryComboBox.SelectedValue.ToString();
  
                foreach (var currentFurniture in this.inventory.Where(currentFurniture => currentFurniture.Category == categoryType && currentFurniture.Style == styleType))
                {
                    furnitureToDisplay.Add(currentFurniture);
                }
                this.inventory = furnitureToDisplay;
                this.furnitureListView.ItemsSource = this.inventory;
                return;
            }
            if (this.categoryComboBox.SelectedIndex > -1)
            {
                string categoryType = this.categoryComboBox.SelectedValue.ToString();
                foreach (var currentFurniture in this.inventory.Where(currentFurniture => currentFurniture.Category == categoryType))
                {
                    furnitureToDisplay.Add(currentFurniture);
                }
                this.inventory = furnitureToDisplay;
                this.furnitureListView.ItemsSource = this.inventory;
                return;
            }
            if (this.styleComboBox.SelectedIndex > -1)
            {
                string styleType = this.styleComboBox.SelectedValue.ToString();
                foreach (var currentFurniture in this.inventory.Where(currentFurniture => currentFurniture.Style == styleType))
                {
                    furnitureToDisplay.Add(currentFurniture);
                }
                this.inventory = furnitureToDisplay;
                this.furnitureListView.ItemsSource = this.inventory;
                return;
            }
        }

        private void handleResetFilters()
        {
            this.styleComboBox.SelectedIndex = -1;
            this.categoryComboBox.SelectedIndex = -1;
            this.quantityComboBox.SelectedIndex = -1;
            this.selectedFurniture = new Furniture();
        }
        
        private void toggleFurnitureAndMembers_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (this.furniturePanel.Visibility == Visibility.Visible)
            {
                this.furniturePanel.Visibility = Visibility.Collapsed;
                this.quantityComboBox.Visibility = Visibility.Collapsed;
                if (this.orderBorder.Visibility == Visibility.Collapsed)
                {
                    this.placeNewOrderButton.Visibility = Visibility.Visible;
                }
                
                this.memberPanel.Visibility = Visibility.Visible;
                this.toggleFurnitureAndMembers.Content = "Toggle Furniture";
            }

            else
            {
                this.memberPanel.Visibility = Visibility.Collapsed;
                this.toggleFurnitureAndMembers.Content = "Toggle Members";
                this.furniturePanel.Visibility = Visibility.Visible;
                if (this.orderBorder.Visibility == Visibility.Visible)
                {
                    this.quantityComboBox.Visibility = Visibility.Visible;
                }
            }

            this.categoryComboBox.SelectedIndex = -1;
            this.styleComboBox.SelectedIndex = -1;
            this.searchMemberByComboBox.SelectedIndex = -1;
            this.quantityComboBox.SelectedIndex = -1;
        }

        private void memberSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.memberListView.ItemsSource = new ObservableCollection<Customer>();
            var content = this.searchMemberByComboBox.SelectedValue;
            if (content != null)
            {
                var searchType = content.ToString();
                if (searchType == "Id")
                {
                    try
                    {
                        int id = Convert.ToInt32(this.memberSearchTextBox.Text);
                        var match = this.mdal.GetMemberById(id);
                        if (match != null && match.fName != null)
                        {
                            this.members.Add(match);
                            this.memberListView.ItemsSource = this.members;
                        }
                   
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Input string is not a sequence of digits.");
                    }
                    catch (OverflowException)
                    {
                        Console.WriteLine("The number cannot fit in an Int32.");
                    }
                }

                else if (searchType == "Name")
                {
                    string name = this.memberSearchTextBox.Text;
                    var members = this.mdal.GetMemberByName(name);
                    if (members != null)
                    { 
                        this.memberListView.ItemsSource = members;
                    }

                }
                else if (searchType == "Phone")
                {
                    string phone = this.memberSearchTextBox.Text;
                    var members = this.mdal.GetMemberByPhone(phone);
                    if (members != null)
                    {
                        this.memberListView.ItemsSource = members;
                    }

                }
            }
        }

        private void memberListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.updateOrderDetails();
            this.editMemberButton.Visibility = Visibility.Visible;
            this.viewMemberRentalsButton.Visibility = Visibility.Visible;
        }

        private async void editMemberButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.selectedMember != null)
            {
                ContentDialog registerDialog = new RegisterCustomer(this.selectedMember);
                await registerDialog.ShowAsync();
            }
            
        }

        private void editItemButton_Click(object sender, RoutedEventArgs e)
        {
            _ = setupEditItemAsync();
        }

        private async System.Threading.Tasks.Task setupEditItemAsync()
        {
            RentalItem.SelectedRentalItem = this.selectedRentalItem;

            ContentDialog editItemDialog = new EditItem();
            _ = await editItemDialog.ShowAsync();

            this.selectedRentalItem.Quantity = RentalItem.SelectedRentalItem.Quantity;
            foreach (var rentalItem in this.rentalItems.Where(rentalItem => rentalItem == this.selectedRentalItem))
            {
                this.removeOrderItem(rentalItem);
                this.addEditedFurniture();
            }

            this.refreshDisplay();
        }

        private void viewMemberRentalsButton_Click(object sender, RoutedEventArgs e)
        {
            this.hideOrder();
            this.displayMemberRentals();
            var selectedMemberId = this.selectedMember.id;
            if (selectedMemberId != null)
            {
                this.selectedMemberTransactions = this.rdal.GetMemberTransactions((int)selectedMemberId);
                this.rentalsListView.ItemsSource = this.selectedMemberTransactions;
            }
        }

        private void displayMemberRentals()
        {
            this.rentalTransactionsBorder.Visibility = Visibility.Visible;
        }

        private void hideMemberRentals()
        {
            this.rentalTransactionsBorder.Visibility = Visibility.Collapsed;
        }

        private void rentalsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.selectedTransaction != null)
            {
                this.viewTransactionButton.Visibility = Visibility.Visible;
                this.newReturnButton.Visibility = Visibility.Visible;
            }
            else
            {
                this.viewTransactionButton.Visibility = Visibility.Collapsed;
                this.newReturnButton.Visibility = Visibility.Collapsed;
            }
            
        }

        private async void newReturnButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.selectedTransaction != null)
            {
                ContentDialog registerDialog = new ReturnTransactionContentDialog((int)this.selectedTransaction.id);
                ContentDialogResult result = await registerDialog.ShowAsync();
                if (result == ContentDialogResult.Secondary)
                {
                    this.itemsToReturn = new ObservableCollection<ReturnItem>(ReturnItem.itemsToBeReturned);
                    this.itemsToReturnListView.ItemsSource = this.itemsToReturn;
                }
                this.rentalsListView.SelectedIndex = -1;
            }
        }

        private void viewTransactionButton_Click(object sender, RoutedEventArgs e)
        {
            this.selectedTransactionItems = this.rdal.GetTransactionRentalItems((int)this.selectedTransaction.id);
            this.transactionItemsListView.ItemsSource = this.selectedTransactionItems;
        }

        private void itemsToReturnListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
