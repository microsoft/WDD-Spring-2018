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
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Linq;

namespace ContosoInsurance.Pages
{
    public sealed partial class CasesPage : Page
    {
        public ObservableCollection<Case> Cases { get; private set; }

        private Case _caseToAdd;

        private static bool _upToDate;

        public CasesPage()
        {
            InitializeComponent();

            Loaded += (s, e) =>
            {
                if (!_upToDate)
                {
                    _caseToAdd = App.Cases.First();
                    Cases = new ObservableCollection<Case>(App.Cases.Skip(1));
                }
                else
                {
                    Cases = new ObservableCollection<Case>(App.Cases);
                }

                Bindings.Update();
            };
        }

        private void OnAcceptButtonClick(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is Button button && button.DataContext is Case selectedCase)
            {
                selectedCase.IsNew = false;

                Frame.Navigate(typeof(CasePage), selectedCase);
            }
        }

        private async void OnRefreshRequested(RefreshContainer sender, RefreshRequestedEventArgs args)
        {
            if (!_upToDate)
            {
                _upToDate = true;

                var deferral = args.GetDeferral();

                await Task.Delay(1000);

                Cases.Insert(0, _caseToAdd);

                deferral.Complete();
            }
        }

        private void OnRefreshButtonClick(object sender, RoutedEventArgs e)
        {
            if (!_upToDate)
            {
                _upToDate = true;

                Cases.Insert(0, _caseToAdd);
            }
        }
    }
}