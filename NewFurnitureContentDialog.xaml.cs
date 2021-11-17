using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UWG_CS3230_FurnitureRental
{
    public sealed partial class NewFurnitureContentDialog : ContentDialog
    {
        private readonly EmployeeDAL dal = new EmployeeDAL();
        private readonly FurnitureDAL fdal = new FurnitureDAL();
        private readonly MemberDAL mdal = new MemberDAL();

        private ObservableCollection<Furniture> inventory { get; set; }
        private ObservableCollection<string> styles { get; set; }
        private ObservableCollection<string> categories { get; set; }

        private ObservableCollection<int> quantities { get; set; }

        private Furniture selectedFurniture { get; set; }

        private String selectedStyle { get; set; }

        private String selectedCategory { get; set; }

        private int selectedQuantity { get; set; }

        public NewFurnitureContentDialog()
        {
            this.InitializeComponent();
            this.initializeCollections();
        }

        private void initializeCollections()
        {
            this.inventory = new ObservableCollection<Furniture>();
            this.styles = new ObservableCollection<string>();
            this.categories = new ObservableCollection<string>();
            this.inventory = fdal.GetFurnitureInventory();
            this.styles = fdal.GetStyles();
            this.categories = fdal.GetCategories();
            this.quantities = Utilities.OrderFormatter.GetQuantityRange(100);
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            try
            {
             
                if (Utilities.OrderFormatter.VerifyPrice(this.rentalRateTextBox.Text) && this.categoryComboBox.SelectedIndex > -1 && this.quantityComboBox.SelectedIndex > -1 && this.styleComboBox.SelectedIndex > -1)
                {
                    double rentalRate = Convert.ToDouble(this.rentalRateTextBox.Text);
                    Furniture furniture = new Furniture
                    {
                        Description = this.descriptionTextBox.Text,
                        Style = this.selectedStyle,
                        Category = this.selectedCategory,
                        Available = this.selectedQuantity,
                        Rented = 0,
                        RentalRate = rentalRate
                    };
                    this.fdal.AddFurnitureToInventory(furniture);
                }
                else
                {
                    args.Cancel = true;
                }
            }
            catch (Exception e)
            {
            }  
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
