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
using System.Threading.Tasks;
using Windows.ApplicationModel.UserActivities;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System;
using Windows.UI.Shell;
using Windows.ApplicationModel;
using System.IO;
using Windows.UI.Xaml;

namespace ContosoInsurance.Pages
{
    public sealed partial class CasePage : Page
    {
        public Case Case { get; set; }

        public CasePage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 4. Create adaptive card from timeline.json and send it to Windows Timeline.
        /// </summary>
        private UserActivityChannel _userActivityChannel;
        private UserActivity _userActivity;
        private UserActivitySession _userActivitySession;

        private async Task CreateAdaptiveCardForTimelineAsync()
        {
            // Fetch the adaptive card JSON.
            var adaptiveCard = File.ReadAllText(
                $@"{Package.Current.InstalledLocation.Path}\AdaptiveCards\timeline.json");

            // Create the protocol, so when the clicks the Adaptive Card on the Timeline, 
            // it will directly launch to the correct image.
            _userActivity.ActivationUri = new Uri("contoso-insurance://case?#1703542");

            // Set the display text to the User Activity.
            _userActivity.VisualElements.DisplayText = "NEW CASE";

            // Assign the Adaptive Card to the user activity. 
            _userActivity.VisualElements.Content = 
                AdaptiveCardBuilder.CreateAdaptiveCardFromJson(adaptiveCard);

            // Save the details user activity.
            await _userActivity.SaveAsync();

            // Dispose of the session and create a new one ready for the next activity.
            _userActivitySession?.Dispose();
            _userActivitySession = _userActivity.CreateSession();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            var selectedCase = e.Parameter as Case;
            Case = selectedCase;
            Bindings.Update();

            if (App.IsDeepLink) return;

            _userActivityChannel = UserActivityChannel.GetDefault();
            _userActivity = await _userActivityChannel.GetOrCreateUserActivityAsync("Case");
            await Task.Delay(500);

            MessagePanel.Visibility = Visibility.Visible;
            MessageText.Text = "Saving case to Windows Timeline...";
            await CreateAdaptiveCardForTimelineAsync();
            await Task.Delay(3000);
            MessageText.Text = "Done and you are good to go!";
            await Task.Delay(2000);
            MessagePanel.Visibility = Visibility.Collapsed;
        }

        private void OnBackButtonClick(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }
    }
}