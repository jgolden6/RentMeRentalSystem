using Microsoft.Toolkit.Uwp.UI.Controls;
using MySql.Data.MySqlClient;
using RentMeRentalSystem.Model;
using RentMeRentalSystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace RentMeRentalSystem.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AdminQueryMenu : Page
    {
        AdminDAL DataAccess = new AdminDAL();

        public AdminQueryMenu()
        {
            this.InitializeComponent();
        }

        private void InputQueryButton_Click(object sender, RoutedEventArgs e)
        {
            QueryDataGrid.ItemsSource = null;
            QueryErrorTextBlock.Text = "";

            try
            {
                DataGridFiller.FillDataGrid(DataAccess.PoseQuery(QueryTextBox.Text), QueryDataGrid);
            }
            catch (MySqlException)
            {
                QueryErrorTextBlock.Text = "Invalid query.";
            }
            
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentUser.Logout();
            this.Frame.Navigate(typeof(LoginMenu));
        }
    }
}
