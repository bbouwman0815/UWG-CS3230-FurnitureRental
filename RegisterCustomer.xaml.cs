using System;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using Windows.UI.Xaml.Controls;
using UWG_CS3230_FurnitureRental.DAL;
using UWG_CS3230_FurnitureRental.Model;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UWG_CS3230_FurnitureRental
{
    public sealed partial class RegisterCustomer : ContentDialog
    {
        #region Data members

        private const string phoneRegex = @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$";
        private const string zipRegex = @"^\d{5}-?(\d{4})?$";
        private readonly ObservableCollection<string> states = new ObservableCollection<string>();

        private readonly EmployeeDAL dal = new EmployeeDAL();

        #endregion

        #region Constructors

        public RegisterCustomer()
        {
            this.InitializeComponent();
            this.initStates();
        }

        #endregion

        #region Methods

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
            if (string.IsNullOrEmpty(this.fNameTextBox.Text))
            {
                args.Cancel = true;
                this.errorTextBox.Text = "First Name Required";
            }
            else if (string.IsNullOrEmpty(this.lNameTextBox.Text))
            {
                args.Cancel = true;
                this.errorTextBox.Text = "Last Name Required";
            }
            else if (this.genderComboBox.SelectedValue == null)
            {
                args.Cancel = true;
                this.errorTextBox.Text = "Gender Required";
            }
            else if (!Regex.IsMatch(this.phoneTextBox.Text, phoneRegex))
            {
                args.Cancel = true;
                this.errorTextBox.Text = "Phone Format Invalid";
            }
            else if (this.bdayDatePicker.SelectedDate == null)
            {
                args.Cancel = true;
                this.errorTextBox.Text = "Birthday Required";
            }
            else if (string.IsNullOrEmpty(this.addr1TextBox.Text))
            {
                args.Cancel = true;
                this.errorTextBox.Text = "Address 1 Required";
            }
            else if (string.IsNullOrEmpty(this.cityTextBox.Text))
            {
                args.Cancel = true;
                this.errorTextBox.Text = "City Required";
            }
            else if (this.stateComboBox.SelectedValue == null)
            {
                args.Cancel = true;
                this.errorTextBox.Text = "State Required";
            }
            else if (!Regex.IsMatch(this.zipTextBox.Text, zipRegex))
            {
                args.Cancel = true;
                this.errorTextBox.Text = "Zip Format Invalid";
            }
            else
            {
                var address = this.createAddress();
                var newCustomer = this.createCustomer(address);
                this.dal.CreateNewAddress(address);
                var id = this.dal.GetAddressId(address);
                this.dal.CreateNewCustomer(newCustomer, id);
            }
        }

        private Address createAddress()
        {
            var customerAddress = new Address {
                id = null,
                address1 = this.addr1TextBox.Text,
                address2 = this.addr2TextBox.Text,
                city = this.cityTextBox.Text,
                state = this.stateComboBox.SelectedValue.ToString(),
                zip = this.zipTextBox.Text
            };
            return customerAddress;
        }

        private Customer createCustomer(Address customerAddress)
        {
            var newCustomer = new Customer {
                fName = this.fNameTextBox.Text,
                lName = this.lNameTextBox.Text,
                address = customerAddress,
                birthday = this.bdayDatePicker.Date.DateTime,
                gender = this.genderComboBox.SelectedValue.ToString(),
                phoneNumber = this.phoneTextBox.Text,
                registrationDate = DateTime.Now
            };
            return newCustomer;
        }

        #endregion
    }
}