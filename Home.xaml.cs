using System;
using Windows.UI.Xaml.Controls;
using UWG_CS3230_FurnitureRental.Model;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UWG_CS3230_FurnitureRental
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Home : Page
    {
        public Home()
        {
            this.InitializeComponent();
            this.setupEmployeeHeader();
        }

        private void setupEmployeeHeader()
        {
            String identification = LoggedEmployee.CurrentLoggedEmployee.Fname + " ";
            identification += LoggedEmployee.CurrentLoggedEmployee.Lname;
            identification += Environment.NewLine;
            identification += LoggedEmployee.CurrentLoggedEmployee.Id;
            identification += Environment.NewLine;
            identification += LoggedEmployee.CurrentLoggedEmployee.Pword;
           
            this.EmployeeInfoTextBlock.Text = identification;
        }

        private void onRegisterCustomerClick(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {

        }
    }
}
