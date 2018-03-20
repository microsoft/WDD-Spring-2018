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
using ContosoInsurance.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace ContosoInsurance
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        public static bool IsDeepLink { get; set; }

        public static List<Case> Cases { get; } = new List<Case>
        {
            new Case("#1703542", "ms-appx:///Assets/Avatars/2.jpg", "2 MINUTES AGO", "Alex", "Zollinger Chohfi",
                "4567 Main St", "WA 98052", "(200) 123 4567", "", "", CaseType.Car, isNew: true),

            new Case("#1703548", "ms-appx:///Assets/Avatars/9.jpg", "2 WEEKS AGO", "Freya", "Hoppe",
                "2345 Contoso Rd", "NY 98052", "(789) 012 3456", "", "", CaseType.House),

            new Case("#1703543", "ms-appx:///Assets/Avatars/3.jpg", "5 HOURS AGO", "Nikola", "Metulev",
                "78901 Contoso Way", "WA 50001", "(123) 456 7890", "", "", CaseType.Car),

            new Case("#1703544", "ms-appx:///Assets/Avatars/4.jpg", "3 DAYS AGO", "Shen", "Chauhan",
                "1234 Contoso Ave", "NY 98052", "(234) 567 8901", "", "", CaseType.House),

            new Case("#1703545", "ms-appx:///Assets/Avatars/5.jpg", "3 DAYS AGO", "Vlad", "Kolesnikov",
                "1000 Contoso Pl", "WA 50001", "(345) 678 9012", "", "", CaseType.Car),

            new Case("#1703547", "ms-appx:///Assets/Avatars/8.jpg", "A WEEK AGO", "Cherry", "Wang",
                "2345 Alfred St", "NY 98052", "(678) 901 2345", "", "", CaseType.Human),

            new Case("#17834", "ms-appx:///Assets/Avatars/6.png", "5 DAYS AGO", "Dave", "Crowford",
                "45678 Oxford St", "SY 98052", "(456) 789 0123", "", "", CaseType.Car),

            new Case("#1703546", "ms-appx:///Assets/Avatars/7.jpg", "A WEEK AGO", "Chris", "Barker",
                "10000 Xinhua St", "WA 50001", "(567) 890 1234", "", "", CaseType.Human),

            new Case("#1703549", "ms-appx:///Assets/Avatars/10.jpg", "2 WEEKS AGO", "Richard", "Fricks",
                "2345 Paulo Rd", "NY 98052", "(890) 123 4567", "", "", CaseType.Car)
        };

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            InitializeComponent();
            Suspending += OnSuspending;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            CustomizeTitleBar();

            void CustomizeTitleBar()
            {
                var titleBar = ApplicationView.GetForCurrentView().TitleBar;
                titleBar.ButtonBackgroundColor = titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
                titleBar.ButtonForegroundColor = Colors.White;

                var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
                coreTitleBar.ExtendViewIntoTitleBar = true;
            }

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (!(Window.Current.Content is Frame rootFrame))
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }
                // Ensure the current window is active
                Window.Current.Activate();
            }
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }

        protected override void OnActivated(IActivatedEventArgs args)
        {
            if (args.Kind == ActivationKind.Protocol && args is ProtocolActivatedEventArgs protocolEventArgs)
            {
                var root = new Frame();
                Window.Current.Content = root;

                root.Navigate(typeof(MainPage), Cases.Single(c => c.Id.Equals(protocolEventArgs.Uri.Fragment)));

                Window.Current.Activate();
            }
        }
    }
}