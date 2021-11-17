using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using UWG_CS3230_FurnitureRental.DAL;
using UWG_CS3230_FurnitureRental.Model;
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

        private ObservableCollection<Furniture> inventory { get; set; }
        private ObservableCollection<string> styles { get; set; }
        private ObservableCollection<string> categories { get; set; }

        private Furniture selectedFurniture { get; set; }

        private String selectedStyle { get; set; }

        private String selectedCategory { get; set; }

        public Admin()
        {
            this.InitializeComponent();
            this.initializeCollections();
            this.setupAdminHeader();
        }

        private void initializeCollections()
        {
            this.inventory = new ObservableCollection<Furniture>();
            this.styles = new ObservableCollection<string>();
            this.categories = new ObservableCollection<string>();
            this.inventory = fdal.GetFurnitureInventory();
            this.styles = fdal.GetStyles();
            this.categories = fdal.GetCategories();
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


        private void handleChangeFurnitureQuantity(object sender, RoutedEventArgs e)
        {

        }

        private void addFurnitureButton_Click(object sender, RoutedEventArgs e)
        {
            _ = setupAddItemAsync();
        }

        private void handleRemoveFurniture(object sender, RoutedEventArgs e)
        {

        }

        private void handleFurnitureSelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void handleFilterSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.applyFilters();
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
                if (furnitureByID.Id != 0)
                {
                    this.inventory.Clear();
                    this.inventory.Add(furnitureByID);
       
                    this.furnitureListView.ItemsSource = this.inventory;
                    return;
                }
            }

            this.inventory = this.fdal.SearchFurnitureByDescription(search);
        
            this.furnitureListView.ItemsSource = this.inventory;

            this.applyFilters();
        }

        private void handleResetFilters()
        {
            this.styleComboBox.SelectedIndex = -1;
            this.categoryComboBox.SelectedIndex = -1;
            this.selectedFurniture = new Furniture();
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
    }
}
