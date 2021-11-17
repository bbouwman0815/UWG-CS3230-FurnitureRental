using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using UWG_CS3230_FurnitureRental.DAL;
using UWG_CS3230_FurnitureRental.Model;
using UWG_CS3230_FurnitureRental.Utilities;
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
                if (LoggedEmployee.CurrentLoggedEmployee.Uname == "admin")
                {
                    Frame.Navigate(typeof(Admin));
                } else
                {
                    Frame.Navigate(typeof(Home));
                }
            }
            else
            {
                _ = this.notifyInvalidCredentialsAsync();
                this.passwordTextBox.Password = "";
            }
        }

        private bool verifyLoginCredentials()
        {
            String username = usernameTextBox.Text;
            String password = passwordTextBox.Password.ToString();
            return Crypter.VerifyLogin(username, password);
        }

        private async System.Threading.Tasks.Task notifyInvalidCredentialsAsync()
        {
            ContentDialog noWifiDialog = new ContentDialog()
            {
                Title = "Invalid Username or Password",
                Content = "Slow down, ol' timer.",
                CloseButtonText = "Ok lol"
            };

            await noWifiDialog.ShowAsync();
        }
    }
}
