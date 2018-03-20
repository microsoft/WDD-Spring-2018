// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using ContosoInsurance.Models;
using System;
using System.Globalization;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace ContosoInsurance.Pages
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();

            MainNav.SelectionChanged += OnMainNavSelectionChanged;

            void OnMainNavSelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
            {
                if (args.SelectedItem is NavigationViewItem navViewItem)
                {
                    var pageName = $"ContosoInsurance.Pages.{navViewItem.Tag}";
                    var pageType = Type.GetType(pageName);

                    ContentFrame.Navigate(pageType);
                }
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // From deep linking, redirect to the case page.
            if (e.Parameter is Case selectedCase)
            {
                App.IsDeepLink = true;

                ProfileImage.UriSource = new Uri("ms-appx:///Assets/Avatars/11.jpg");
                ContentFrame.Navigate(typeof(CasePage), selectedCase);
            }
            // On a normal app launch, select the first menu item (i.e. Home) as default.
            else
            {
                MainNav.SelectedItem = MainNav.MenuItems.FirstOrDefault();
            }
        }

        private void OnBackgroundImageOpened(object sender, RoutedEventArgs e) =>
            BackgroundImage.Visibility = Visibility.Visible;            

        private void OnUserAvatarTapped(object sender, TappedRoutedEventArgs e)
        {
            MainNav.SelectedItem = MainNav.MenuItems[1];
            ProfileImage.UriSource = new Uri("ms-appx:///Assets/Avatars/11.jpg");
            NotificationIndicator.Visibility = Visibility.Visible;
            NotificationDialog.GoToEmptyState();
            NotificationDialog.Visibility = Visibility.Collapsed;
            ContentFrame.Navigate(typeof(CasesPage));
        }

        private string GetMenuItemName(object selectedItem)
        {
            if (selectedItem is NavigationViewItem navViewItem)
            {
                string content;

                if (navViewItem.Content is StackPanel panel)
                {
                    content = ((TextBlock)panel.Children.First()).Text;
                }
                else
                {
                    content = navViewItem.Content.ToString();
                }

                return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(content.ToLower());
            }

            return string.Empty;
        }
    }
}
