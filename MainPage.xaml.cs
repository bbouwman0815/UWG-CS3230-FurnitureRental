using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using UWG_CS3230_FurnitureRental.DAL;
using UWG_CS3230_FurnitureRental.Model;
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWG_CS3230_FurnitureRental
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.verifyLoginCredentials())
            {
                this.setLoggedEmployee();
                Frame.Navigate(typeof(Home));
            }
        }

        private void setLoggedEmployee()
        {
            String username = usernameTextBox.Text;
            String password = passwordTextBox.Text;
            EmployeeDAL dal = new EmployeeDAL();
            LoggedEmployee.CurrentLoggedEmployee = dal.GetEmployeeByLoginInformation(username, password);
        }

        private bool verifyLoginCredentials()
        {
            String username = usernameTextBox.Text;
            String password = passwordTextBox.Text;
            EmployeeDAL dal = new EmployeeDAL();
            bool isValid = dal.VerifyEmployeeLogin(username, password);

            return isValid;
        }
    }
}
