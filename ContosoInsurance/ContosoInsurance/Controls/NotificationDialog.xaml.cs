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

using AdaptiveCards.Rendering.Uwp;
using ContosoInsurance.Models;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Numerics;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Hosting;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using ListViewBase = Windows.UI.Xaml.Controls.ListViewBase;

namespace ContosoInsurance.Controls
{
    public sealed partial class NotificationDialog : UserControl
    {
        public ObservableCollection<Adjuster> Adjusters { get; } = new ObservableCollection<Adjuster>();

        private static readonly AnimationCollection OffsetImplicitAnimations = new AnimationCollection
        {
            new Vector3Animation { Target = "Offset", Duration = TimeSpan.FromMilliseconds(600) }
        };

        private static readonly AnimationCollection IndicatorShowAnimations = new AnimationCollection
        {
            new TranslationAnimation { From = new Vector3(0, 8, 0).ToString(), To = Vector3.Zero.ToString(), Duration = TimeSpan.FromMilliseconds(600) }
        };

        private static readonly AnimationCollection CardElementShowAnimations = new AnimationCollection
        {
            new TranslationAnimation { From = new Vector3(0, 16, 0).ToString(), To = Vector3.Zero.ToString(), Duration = TimeSpan.FromMilliseconds(600) },
            new OpacityAnimation { From = 0, To = 1, Duration = TimeSpan.FromMilliseconds(600) }
        };

        private static readonly AnimationCollection CardElementHideAnimations = new AnimationCollection
        {
            new TranslationAnimation { To = new Vector3(0, -16, 0).ToString(), Duration = TimeSpan.FromMilliseconds(600) },
            new OpacityAnimation { To = 0, Duration = TimeSpan.FromMilliseconds(600) }
        };

        public NotificationDialog()
        {
            InitializeComponent();

            // Set the initial state.
            VisualStateManager.GoToState(this, "EmptyState", false);

            // Clip the top-level panel so nothing goes outside will be visible.
            VisualExtensions.GetVisual(LayoutRoot).Clip = Window.Current.Compositor.CreateInsetClip();

            Loaded += async (s, e) =>
            {
                if (App.IsDeepLink) return;

                // Simulate a network request so the message dialog will appear after 2.5s.
                await Task.Delay(2500);

                // Show the message dialog on the bottom right of the screen.
                VisualStateManager.GoToState(this, "MinimizedState", true);
            };

            AddDummyAdjusters();

            ShowMapAndPolyline();

            void AddDummyAdjusters()
            {
                Adjusters.Add(new Adjuster("Emma", 7, GenderType.Female));
                Adjusters.Add(new Adjuster("Max", 3.4, GenderType.Male, StatusType.Busy));
                Adjusters.Add(new Adjuster("Charlotte", 1.2, GenderType.Female));
                Adjusters.Add(new Adjuster("Chloe", 8.9, GenderType.Female));
                Adjusters.Add(new Adjuster("Giselle", 15.6, GenderType.Female, StatusType.Busy));
            }

            void ShowMapAndPolyline()
            {
                Map.LoadingStatusChanged += ActivityMapLoadingStatusChanged;

                async void ActivityMapLoadingStatusChanged(MapControl sender, object args)
                {
                    Map.LoadingStatusChanged -= ActivityMapLoadingStatusChanged;

                    await ShowMapContents();

                    async Task ShowMapContents()
                    {
                        var spaceNeedlePoint = new Geopoint(new BasicGeoposition()
                        {
                            Latitude = 47.6204,
                            Longitude = -122.3491
                        });

                        MapScene spaceNeedleScene = MapScene.CreateFromLocationAndRadius(spaceNeedlePoint,
                                                                                            240, /* show this many meters around */
                                                                                            45, /* looking at it to the south east*/
                                                                                            75   /* degrees pitch */);

                        await Map.TrySetSceneAsync(spaceNeedleScene);

                        var ownerAddressPoint = new Button { Style = (Style)App.Current.Resources["ButtonMapPinStyle"] };
                        Map.Children.Add(ownerAddressPoint);
                        MapControl.SetLocation(ownerAddressPoint, new Geopoint(new BasicGeoposition
                        {
                            Latitude = 47.6204,
                            Longitude = -122.3491
                        }));
                        MapControl.SetNormalizedAnchorPoint(ownerAddressPoint, new Point(0.5, 0.5));

                        var branchPoint1 = new Button { Style = (Style)App.Current.Resources["ButtonMapBuildingStyle"] };
                        Map.Children.Add(branchPoint1);
                        MapControl.SetLocation(branchPoint1, new Geopoint(new BasicGeoposition
                        {
                            Latitude = 47.6208148,
                            Longitude = -122.350805
                        }));
                        MapControl.SetNormalizedAnchorPoint(branchPoint1, new Point(0.5, 0.5));

                        var branchPoint2 = new Button { Style = (Style)App.Current.Resources["ButtonMapBuildingStyle"] };
                        Map.Children.Add(branchPoint2);
                        MapControl.SetLocation(branchPoint2, new Geopoint(new BasicGeoposition
                        {
                            Latitude = 47.619535,
                            Longitude = -122.349163
                        }));
                        MapControl.SetNormalizedAnchorPoint(branchPoint2, new Point(0.5, 0.5));
                    }
                }
            }
        }

        /// <summary>
        /// 3. Use AdaptiveCardRenderer to create a UWP UI element from a JSON payload.
        /// </summary>
        private async Task RenderCardsAsync()
        {
            // Create a new adaptive card renderer.
            var renderer = new AdaptiveCardRenderer();

            // Customize the font sizes via AdaptiveHostConfig.
            var hostConfig = new AdaptiveHostConfig
            {
                FontSizes =
                    {
                        Small = 14,
                        Default = 16,
                        Medium = 16,
                        Large = 20,
                        ExtraLarge= 24
                    }
            };
            renderer.HostConfig = hostConfig;

            // Get our JSON string from a .json file.
            var json1 = File.ReadAllText(Package.Current.InstalledLocation.Path + @"\AdaptiveCards\bot1.json");
            var json2 = File.ReadAllText(Package.Current.InstalledLocation.Path + @"\AdaptiveCards\bot2.json");
            var json3 = File.ReadAllText(Package.Current.InstalledLocation.Path + @"\AdaptiveCards\bot3.json");

            // Parse the JSON string to AdaptiveCardParseResult.
            var card1 = AdaptiveCard.FromJsonString(json1);
            var card2 = AdaptiveCard.FromJsonString(json2);
            var card3 = AdaptiveCard.FromJsonString(json3);

            await RenderCardElementAsync(renderer, card1);
            await AddProgressIndicatorAsync("\"Closest first\"");
            await Task.Delay(1000);

            await RenderCardElementAsync(renderer, card2);
            await AddProgressIndicatorAsync("\"Team selected\"");
            await Task.Delay(1000);

            await RenderCardElementAsync(renderer, card3);
            AdjustersFoundText.Text = "Adjuster found!";
            DimUnavailableAdjusters();
            await AddProgressIndicatorAsync("\"Notification sent!\"", true);
            await Task.Delay(3000);

            VisualStateManager.GoToState(this, "EmptyState", true);
        }

        internal void GoToEmptyState() =>
            VisualStateManager.GoToState(this, "EmptyState", false);

        private async void OnProceedButtonClick(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "MapState", true);
            await Task.Delay(400);

            FindName("AdjusterList");

            // Because we realize the ListView later, we need to manually add this
            // additional setter to the visual state.
            EmptyState.Setters.Add(new Setter
            {
                Target = new TargetPropertyPath
                {
                    Path = new PropertyPath("(ListView.Visibility)"),
                    Target = AdjusterList
                },
                Value = Visibility.Collapsed
            });
        }

        private void DimUnavailableAdjusters()
        {
            foreach (var item in AdjusterList.Items)
            {
                if (AdjusterList.ContainerFromItem(item) is ListViewItem container && container.Content is Adjuster adjuster)
                {
                    if (!adjuster.FirstName.ToLower().Equals("charlotte"))
                    {
                        container.Fade(0.2f, 800).Start();
                    }
                }
            }
        }

        private Task RenderCardElementAsync(AdaptiveCardRenderer renderer, AdaptiveCardParseResult card)
        {
            var taskSource = new TaskCompletionSource<bool>();

            // Get the RenderedAdaptiveCard from the parse result.
            var renderResult = renderer.RenderAdaptiveCard(card.AdaptiveCard);
            renderResult.Action += OnRenderResultAction;

            // Add the AdaptiveCard UIElement to the Visual Tree.
            if (renderResult.FrameworkElement is FrameworkElement cardElement)
            {
                cardElement.Loaded += OnCardElementLoaded;
                cardElement.Visibility = Visibility.Collapsed;
                cardElement.Margin = new Thickness(12, 0, 12, 0);

                Implicit.SetAnimations(cardElement, OffsetImplicitAnimations);
                Implicit.SetShowAnimations(cardElement, CardElementShowAnimations);
                Implicit.SetHideAnimations(cardElement, CardElementHideAnimations);

                CardsPanel.Children.Add(cardElement);
            }

            void OnCardElementLoaded(object sender, RoutedEventArgs e)
            {
                cardElement.Loaded -= OnCardElementLoaded;

                cardElement.Visibility = Visibility.Visible;
            }

            async void OnRenderResultAction(RenderedAdaptiveCard sender, AdaptiveActionEventArgs args)
            {
                renderResult.Action -= OnRenderResultAction;

                if (args.Action.ActionType == ActionType.Submit)
                {
                    sender.FrameworkElement.Visibility = Visibility.Collapsed;
                    await Task.Delay(600);

                    taskSource.TrySetResult(true);
                }
            }

            return taskSource.Task;
        }

        private Task AddProgressIndicatorAsync(string message, bool isLast = false)
        {
            var taskSource = new TaskCompletionSource<bool>();

            var indicator = new ProgressIndicator();
            indicator.Loaded += OnProgressIndicatorLoaded;
            indicator.Text = message;
            indicator.IsLast = isLast;
            indicator.Visibility = Visibility.Collapsed;
            indicator.Margin = new Thickness(40, 0, 40, 0);

            Implicit.SetAnimations(indicator, OffsetImplicitAnimations);
            Implicit.SetShowAnimations(indicator, IndicatorShowAnimations);

            CardsPanel.Children.Add(indicator);

            void OnProgressIndicatorLoaded(object sender, RoutedEventArgs e)
            {
                indicator.Loaded -= OnProgressIndicatorLoaded;

                indicator.Visibility = Visibility.Visible;
                taskSource.TrySetResult(true);
            }

            return taskSource.Task;
        }

        private void OnAdjusterListContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            if (!args.InRecycleQueue)
            {
                args.ItemContainer.Loaded += OnItemContainerLoaded;
            }

            void OnItemContainerLoaded(object s, RoutedEventArgs e)
            {
                args.ItemContainer.Loaded -= OnItemContainerLoaded;

                // Don't animate if it's not in the visible viewport.
                if (AdjusterList.ItemsPanelRoot is ItemsStackPanel itemsPanel &&
                    args.ItemContainer.FindDescendant<ListViewItemPresenter>() is ListViewItemPresenter itemPresenter &&
                    args.ItemIndex >= itemsPanel.FirstVisibleIndex && args.ItemIndex <= itemsPanel.LastVisibleIndex)
                {
                    var delay = (args.ItemIndex - itemsPanel.FirstVisibleIndex) * 100;
                    var centerX = (float)itemPresenter.RenderSize.Width / 2;
                    var centerY = (float)itemPresenter.RenderSize.Height / 2;

                    itemPresenter.Fade(0.0f, 0).Then().Fade(1.0f, 600, delay).Start();
                    itemPresenter.Scale(0.6f, 0.6f, centerX, centerY, 0).Then().Scale(1.0f, 1.0f, centerX, centerY, 600, delay).Start();
                }
            }
        }

        private async void OnChooseBotButtonClick(object sender, RoutedEventArgs e)
        {
            ChooseBotButton.IsEnabled = false;

            VisualStateManager.GoToState(this, "BotState", true);
            await Task.Delay(2600);

            await RenderCardsAsync();
        }

        private void OnAdjusterListLoaded(object sender, RoutedEventArgs e) =>
            Implicit.SetAnimations(AdjusterList, OffsetImplicitAnimations);
    }
}