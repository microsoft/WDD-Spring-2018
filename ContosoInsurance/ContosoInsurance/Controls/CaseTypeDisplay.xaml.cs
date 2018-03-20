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

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ContosoInsurance.Controls
{
    public sealed partial class CaseTypeDisplay : UserControl
    {
        public CaseTypeDisplay()
        {
            InitializeComponent();
        }

        public CaseType CaseType
        {
            get => (CaseType)GetValue(CaseTypeProperty);
            set => SetValue(CaseTypeProperty, value);
        }

        public static readonly DependencyProperty CaseTypeProperty =
            DependencyProperty.Register("CaseType", typeof(CaseType), typeof(CaseTypeDisplay), new PropertyMetadata(CaseType.Car,
                (s, e) =>
                {
                    var self = (CaseTypeDisplay)s;
                    var type = (CaseType)e.NewValue;

                    switch (type)
                    {
                        case CaseType.Car:
                            self.FindName(nameof(CarIcon));
                            break;
                        case CaseType.House:
                            self.FindName(nameof(HouseIcon));
                            break;
                        case CaseType.Human:
                            self.FindName(nameof(HumanIcon));
                            break;
                    }
                }));
    }
}