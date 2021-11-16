using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public sealed partial class EditItem : ContentDialog
    {
        public EditItem()
        {
            this.InitializeComponent();
            this.setupDisplay();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (this.QuantityComboBox.SelectedValue != null)
            {
                RentalItem.SelectedRentalItem.Quantity = (int)this.QuantityComboBox.SelectedValue;
            }
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void setupDisplay()
        {
            ObservableCollection<int> available = RentalItem.SelectedRentalItem.GetQuantityRange();
            int itemDescription = RentalItem.SelectedRentalItem.Quantity;
            this.QuantityComboBox.ItemsSource = available;
            this.ItemDescriptionTextBlock.Text = "Current Quantity: " + itemDescription;
        }
    }
}
