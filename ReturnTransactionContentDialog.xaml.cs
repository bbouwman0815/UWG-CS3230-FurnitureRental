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
        private readonly FurnitureDAL fdal = new FurnitureDAL();

        public List<ReturnItem> itemsToReturn { get; set; }

        public ReturnTransactionContentDialog(int rentalId)
        {
            this.InitializeComponent();
            this.initilizeCollections();
            this.selectedRentalItem = new RentalItem();
            this.selectedReturnItem = new RentalItem();
            this.transactionItems = this.rdal.GetRemainingRentedTransactionItems(rentalId);
            this.checkForItemsAlreadyInReturns();
            this.rentalItemsListView.ItemsSource = this.transactionItems;
        }

        private void checkForItemsAlreadyInReturns()
        {
            foreach(var item in this.transactionItems)
            {
                foreach(var returnItem in ReturnItem.itemsToBeReturned)
                {
                    if (item.FurnitureId == returnItem.FurnitureId && item.RentalId == returnItem.RentalId)
                    {
                        item.Quantity -= returnItem.Quantity;
                    }
                }
            }
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
            int qty = Int32.Parse(this.addQtyComboBox.SelectedValue.ToString());

            var added = false;
            var newCopy = new RentalItem();
            foreach (var item in this.returnItems)
            {
                if (item.FurnitureId == this.selectedRentalItem.FurnitureId)
                {
                    item.Quantity += qty;
                    newCopy = item;
                    added = true;
                }
            }
            if (added)
            {
                this.returnItems.Remove(newCopy);
                this.returnItems.Add(newCopy);
            }
            else
            {
                RentalItem selectedItem = new RentalItem()
                {
                    RentalId = this.selectedRentalItem.RentalId,
                    FurnitureId = this.selectedRentalItem.FurnitureId,
                    Quantity = qty,
                    DailyRentalRate = this.selectedRentalItem.DailyRentalRate
                };
                this.returnItems.Add(selectedItem);
            }
            this.returnItemsListView.ItemsSource = this.returnItems;

            this.selectedRentalItem.Quantity -= qty;
            var selected = this.selectedRentalItem;
            this.transactionItems.Remove(this.selectedRentalItem);
            this.transactionItems.Add(selected);
            if (selected.Quantity == 0)
            {
                this.transactionItems.Remove(selected);
            }
            this.rentalItemsListView.ItemsSource = this.transactionItems;
            this.IsSecondaryButtonEnabled = true;
        }

        private void removeItemButton_Click(object sender, RoutedEventArgs e)
        {
            int qty = Int32.Parse(this.removeQtyComboBox.SelectedValue.ToString());

            var added = false;
            var newCopy = new RentalItem();
            foreach (var item in this.transactionItems)
            {
                if (item.FurnitureId == this.selectedReturnItem.FurnitureId)
                {
                    item.Quantity += qty;
                    newCopy = item;
                    added = true;
                }
            }
            if (added)
            {
                this.transactionItems.Remove(newCopy);
                this.transactionItems.Add(newCopy);
            }
            else
            {
                RentalItem selectedItem = new RentalItem()
                {
                    RentalId = this.selectedReturnItem.RentalId,
                    FurnitureId = this.selectedReturnItem.FurnitureId,
                    Quantity = qty,
                    DailyRentalRate = this.selectedReturnItem.DailyRentalRate
                };
                this.transactionItems.Add(selectedItem);
            }
            this.rentalItemsListView.ItemsSource = this.transactionItems;

            this.selectedReturnItem.Quantity -= qty;
            var selected = this.selectedReturnItem;
            this.returnItems.Remove(this.selectedReturnItem);
            this.returnItems.Add(selected);
            if (selected.Quantity == 0)
            {
                this.returnItems.Remove(selected);
            }
            this.returnItemsListView.ItemsSource = this.returnItems;
            this.IsSecondaryButtonEnabled = true;
        }

        private void rentalItemsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.selectedRentalItem != null)
            {
                this.addQtyComboBox.ItemsSource = this.selectedRentalItem.GetQuantityRange();
                this.addQtyComboBox.IsEnabled = true;
            }
            else
            {
                this.addQtyComboBox.ItemsSource = null;
                this.addQtyComboBox.IsEnabled = false;
            }

        }

        private void returnItemsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.selectedReturnItem != null)
            {
                this.removeQtyComboBox.ItemsSource = this.selectedReturnItem.GetQuantityRange();
                this.removeQtyComboBox.IsEnabled = true;
            }
            else
            {
                this.removeQtyComboBox.ItemsSource = null;
                this.removeQtyComboBox.IsEnabled = false;
            }

        }

        private void setupReturn()
        {
            this.itemsToReturn = this.createReturnItems();
            var newList = new List<ReturnItem>();
            newList.AddRange(ReturnItem.itemsToBeReturned);
            newList.AddRange(this.itemsToReturn);
            ReturnItem.itemsToBeReturned = newList;

            this.returnItems.Clear();
            this.transactionItems.Clear();

        }

        private List<ReturnItem> createReturnItems()
        {
            List<ReturnItem> returns = new List<ReturnItem>();
            foreach (var item in this.returnItems)
            {
                returns.Add(new ReturnItem
                {
                    RentalId = (int)item.RentalId,
                    FurnitureId = item.FurnitureId,
                    Quantity = item.Quantity
                });
            }

            return returns;
        }

        private void addQtyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.addQtyComboBox.SelectedValue == null)
            {
                this.addItemButton.IsEnabled = false;
            }
            else
            {
                this.addItemButton.IsEnabled = true;
            }
        }

        private void removeQtyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.removeQtyComboBox.SelectedValue == null)
            {
                this.removeItemButton.IsEnabled = false;
            }
            else
            {
                this.removeItemButton.IsEnabled = true;
            }
        }
    }
}