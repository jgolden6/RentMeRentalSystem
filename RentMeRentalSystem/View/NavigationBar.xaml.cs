using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using RentMeRentalSystem.ViewModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace RentMeRentalSystem.View
{
    /// <summary>
    /// 11/5/2021 The navigation bar
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.Page" />
    /// <author>
    /// Eboni Walker
    /// </author>
    public sealed partial class NavigationBar : Page
    {
        #region Data members

        private readonly List<(string Tag, Type Page)> navigationPages = new() {
            ("Members", typeof(MemberMenu))
            // ("Inventory", typeof(GamesPage)),
            // ("music", typeof(MusicPage)),
        };

        #endregion

        #region Constructors

        public NavigationBar()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Methods

        private void NavView_Loaded(object sender, RoutedEventArgs e)
        {
            NavView.SelectedItem = NavView.MenuItems[0];
            NavView_Navigate("Members", new Windows.UI.Xaml.Media.Animation.EntranceNavigationTransitionInfo());
        }


        private void ContentFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        private void NavView_ItemInvoked(NavigationView sender,
            NavigationViewItemInvokedEventArgs args)
        {
            if (args.InvokedItemContainer != null)
            {
                var navItemTag = args.InvokedItemContainer.Tag.ToString();
                this.NavView_Navigate(navItemTag, args.RecommendedNavigationTransitionInfo);
            }
        }

        private void NavView_Navigate(
            string navItemTag,
            NavigationTransitionInfo transitionInfo)
        {
            Type navigationPage = this.navigationPages.FirstOrDefault(p => p.Tag.Equals(navItemTag)).Page;
            var preNavPageType = this.ContentFrame.CurrentSourcePageType;

            if (!(navigationPage == null) && preNavPageType != navigationPage)
            {
                this.ContentFrame.Navigate(navigationPage, null, transitionInfo);
            }
        }

        private void On_Navigated(object sender, NavigationEventArgs e)
        {
            var item = this.navigationPages.FirstOrDefault(p => p.Page == e.SourcePageType);

            this.NavView.SelectedItem = this.NavView.MenuItems
                                            .OfType<NavigationViewItem>()
                                            .First(navigationViewItem => navigationViewItem.Tag.Equals(item.Tag));
            if (item.Tag.Equals("Home"))
            {
                this.NavView.Visibility = Visibility.Collapsed;
            }
        }
        #endregion
    }
}