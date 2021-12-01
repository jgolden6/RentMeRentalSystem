using RentMeRentalSystem.Model;
using RentMeRentalSystem.ViewModel;
using System;
using System.Collections.Generic;
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
    public sealed partial class AdminReportMenu : Page
    {
        AdminDAL DataAccess = new AdminDAL();

        public AdminReportMenu()
        {
            this.InitializeComponent();
        }

        private void ViewReportButton_Click(object sender, RoutedEventArgs e)
        {
            ReportDataGrid.ItemsSource = null;
            string startDate = '\'' + StartDatePicker.Date.Year.ToString() + '-' +
                               StartDatePicker.Date.Month.ToString() + '-' +
                               StartDatePicker.Date.Day.ToString() + '\'';
            string endDate = '\'' + EndDatePicker.Date.Year.ToString() + '-' +
                               EndDatePicker.Date.Month.ToString() + '-' +
                               EndDatePicker.Date.Day.ToString() + '\'';

            DataGridFiller.FillDataGrid(DataAccess.ViewReportBetweenTwoDates(startDate, endDate), ReportDataGrid);

        }

        private void QueryMenuButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AdminQueryMenu));
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentUser.Logout();
            this.Frame.Navigate(typeof(LoginMenu));
        }
    }
}
