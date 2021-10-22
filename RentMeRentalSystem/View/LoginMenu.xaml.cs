using System;
using Windows.Foundation;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using RentMeRentalSystem.DAL;
using RentMeRentalSystem.Model;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

// ReSharper disable once CheckNamespace
namespace RentMeRentalSystem
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginMenu : Page
    {
        #region Properties

        private EmployeeDAL DataAccess { get; set; }

        private Employee Employee { get; set; }

        #endregion

        #region Constructors

        public LoginMenu()
        {
            this.InitializeComponent();
            this.DataAccess = new EmployeeDAL();
            ApplicationView.PreferredLaunchViewSize = new Size(600, 800);
            ApplicationView.GetForCurrentView().SetPreferredMinSize(ApplicationView.PreferredLaunchViewSize);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
        }

        #endregion

        #region Methods

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            String username = this.UsernameTextBox.Text;
            String password = this.PasswordTextBox.Text;
            this.Employee = this.DataAccess.Authenticate(username, password);
            if (this.Employee != null)
            {
                this.Frame.Navigate(typeof(MainMenu));
            }
            else
            {
                this.ErrorTextBlock.Text = "Username/password not found.";
            }
        }

        #endregion
    }
}