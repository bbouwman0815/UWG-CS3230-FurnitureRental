using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using UWG_CS3230_FurnitureRental.DAL;
using UWG_CS3230_FurnitureRental.Model;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UWG_CS3230_FurnitureRental
{
    public sealed partial class ReturnTransactionContentDialog : ContentDialog
    {
        private int rentalId { get; set; }

        private ObservableCollection<RentalItem> transactionItems { get; set; }
        private ObservableCollection<RentalItem> returnItems { get; set; }
        private RentalItem selectedRentalItem { get; set; }
        private RentalItem selectedReturnItem { get; set; }

        private readonly RentalTransactionDAL rdal = new RentalTransactionDAL();
        private readonly ReturnTransactionDAL returndal = new ReturnTransactionDAL();

        public ReturnTransactionContentDialog(int rentalId)
        {
            this.rentalId = rentalId;
            this.InitializeComponent();
            this.initilizeCollections();
            this.selectedRentalItem = new RentalItem();
            this.selectedReturnItem = new RentalItem();
            this.transactionItems = this.rdal.GetRemainingRentedTransactionItems(rentalId);
            this.rentalItemsListView.ItemsSource = this.transactionItems;
        }

        private void initilizeCollections()
        {
            this.transactionItems = new ObservableCollection<RentalItem>();
            this.returnItems = new ObservableCollection<RentalItem>();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (this.returnItems.Count != 0)
            {
                this.setupReturn();
            }
            else
            {
            }
        }

        private void addItemButton_Click(object sender, RoutedEventArgs e)
        {
            int qtyRemaining = Int32.Parse(this.addQtyComboBox.SelectedValue.ToString());
            var selectedItem = this.selectedRentalItem;
            selectedItem.Quantity -= qtyRemaining;
            this.returnItems.Add(this.selectedRentalItem);
            this.transactionItems.Remove(this.selectedRentalItem);
            if (qtyRemaining != 0)
            {
                this.transactionItems.Add(selectedItem);
            }
            
            this.addQtyComboBox.ItemsSource = null;
            this.rentalItemsListView.SelectedIndex = -1;
            this.returnItemsListView.ItemsSource = this.returnItems;
            this.rentalItemsListView.ItemsSource = this.transactionItems;
            this.IsSecondaryButtonEnabled = true;
        }

        private void removeItemButton_Click(object sender, RoutedEventArgs e)
        {
            int qtyRemaining = Int32.Parse(this.removeQtyComboBox.SelectedValue.ToString());
            var selectedItem = this.selectedReturnItem;
            this.selectedReturnItem.Quantity -= qtyRemaining;

            this.transactionItems.Add(this.selectedReturnItem);
            this.returnItems.Remove(this.selectedReturnItem);
            selectedItem.Quantity -= qtyRemaining;
            if (qtyRemaining != 0)
            {
                this.returnItems.Add(selectedItem);
            }

            this.removeQtyComboBox.ItemsSource = null;
            this.returnItemsListView.SelectedIndex = -1;
            this.returnItemsListView.ItemsSource = this.returnItems;
            this.rentalItemsListView.ItemsSource = this.transactionItems;
            if (this.returnItems.Count == 0)
            {
                this.IsSecondaryButtonEnabled = false;
            }
        }

        private void rentalItemsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.selectedRentalItem != null)
            {
                this.addQtyComboBox.ItemsSource = this.selectedRentalItem.GetQuantityRange();
            }
            
        }

        private void returnItemsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.selectedReturnItem != null)
            {
                this.removeQtyComboBox.ItemsSource = this.selectedReturnItem.GetQuantityRange();
            }
            
        }

        private void setupReturn()
        {
            this.returndal.CreateNewReturnTransaction(new List<ReturnItem>(this.createReturnItems()));
            this.returnItems.Clear();
            this.transactionItems.Clear();
            
        }

        private List<ReturnItem> createReturnItems()
        {
            List<ReturnItem> returns = new List<ReturnItem>();
            foreach (var item in this.returnItems)
            {
                returns.Add(new ReturnItem {
                    RentalId = (int)item.RentalId,
                    FurnitureId = item.FurnitureId,
                    Quantity = item.Quantity
                });
            }

            return returns;
        }
    }
}
