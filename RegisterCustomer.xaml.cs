using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using UWG_CS3230_FurnitureRental.Model;
using UWG_CS3230_FurnitureRental.DAL;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UWG_CS3230_FurnitureRental
{
    public sealed partial class RegisterCustomer : ContentDialog
    {
        ObservableCollection<String> states = new ObservableCollection<String>();
        EmployeeDAL dal = new EmployeeDAL();

        public RegisterCustomer()
        {
            this.InitializeComponent();
            this.initStates();
        }

        private void initStates()
        {
            foreach (var state in Enum.GetValues(typeof(States)))
            {
                this.states.Add(state.ToString());
            }
        }

        private void ContentDialog_CancelButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            
        }

        private void ContentDialog_SaveButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var address = this.createAddress();
            var newCustomer = this.createCustomer(address);
            dal.CreateNewAddress(address);
            var id = dal.GetAddressId(address);
            dal.CreateNewCustomer(newCustomer, id);
        }

        private Address createAddress()
        {
            Address customerAddress = new Address()
            {
                id = null,
                address1 = this.addr1TextBox.Text,
                address2 = this.addr2TextBox.Text,
                city = this.cityTextBox.Text,
                state = this.stateComboBox.Text,
                zip = this.zipTextBox.Text
            };
            return customerAddress;
        }

        private Customer createCustomer(Address customerAddress)
        {
            Customer newCustomer = new Customer()
            {
                fName = this.fNameTextBox.Text,
                lName = this.lNameTextBox.Text,
                address = customerAddress,
                birthday = this.bdayDatePicker.Date.DateTime,
                gender = this.genderComboBox.Text,
                phoneNumber = this.phoneTextBox.Text,
                registrationDate = DateTime.Now
            };
            return newCustomer;
        }
    }
}
