using System;
using System.Collections.ObjectModel;
using System.Linq;
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
        private bool edit;

        private readonly EmployeeDAL dal = new EmployeeDAL();
        private readonly MemberDAL mDal = new MemberDAL();
        private readonly Customer editCustomer;

        #endregion

        #region Constructors

        public RegisterCustomer()
        {
            this.InitializeComponent();
            this.initStates();
            this.edit = false;
        }

        public RegisterCustomer(Customer customer)
        {
            this.editCustomer = customer;
            this.InitializeComponent();
            this.initStates();
            this.fNameTextBox.Text = customer.fName;
            this.lNameTextBox.Text = customer.lName;
            if (customer.gender == "M")
            {
                this.genderComboBox.SelectedIndex = 0;
            }
            else
            {
                this.genderComboBox.SelectedIndex = 1;
            }

            this.phoneTextBox.Text = customer.phoneNumber;
            this.bdayDatePicker.SelectedDate = customer.birthday;
            this.addr1TextBox.Text = customer.address.address1;
            this.addr2TextBox.Text = customer.address.address2;
            this.cityTextBox.Text = customer.address.city;
            var stateIndex = this.states.IndexOf(customer.address.state);
            this.stateComboBox.SelectedIndex = stateIndex;
            this.zipTextBox.Text = customer.address.zip;
            this.edit = true;
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

            if (this.edit)
            {
                this.editMemberSave(args);
            }
            else
            {
                this.newMemberSave(args);
            }
            
        }

        private void newMemberSave(ContentDialogButtonClickEventArgs args)
        {
            if (!string.IsNullOrEmpty(this.errorTextBox.Text))
            {
                args.Cancel = true;
            }
            if (string.IsNullOrEmpty(this.fNameTextBox.Text))
            {
                args.Cancel = true;
            }
            else if (string.IsNullOrEmpty(this.lNameTextBox.Text))
            {
                args.Cancel = true;
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

        private void editMemberSave(ContentDialogButtonClickEventArgs args)
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
                this.mDal.UpdateMember(newCustomer, id);
            }
        }

        private Address createAddress()
        {
            var customerAddress = new Address {
                address1 = this.addr1TextBox.Text,
                address2 = this.addr2TextBox.Text,
                city = this.cityTextBox.Text,
                state = this.stateComboBox.SelectedValue.ToString(),
                zip = this.zipTextBox.Text
            };
            if (this.editCustomer != null)
            {
                customerAddress.id = this.editCustomer.address.id;
            }
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
            if (this.editCustomer != null)
            {
                newCustomer.id = this.editCustomer.id;
                newCustomer.registrationDate = this.editCustomer.registrationDate;
            }
            return newCustomer;
        }

        #endregion

        private void fNameTextBox_LostFocus(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.fNameTextBox.Text))
            {
                this.errorTextBox.Text += "First Name Required\n";
            }
            else
            {
                this.errorTextBox.Text = "";
            }
        }

        private void zipTextBox_BeforeTextChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
            args.Cancel = args.NewText.Any(c => !char.IsDigit(c));
        }

        private void lNameTextBox_LostFocus(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.lNameTextBox.Text))
            {
                this.errorTextBox.Text += "Last Name Required\n";
            }
            else
            {
                this.errorTextBox.Text = "";
            }
        }
    }
}