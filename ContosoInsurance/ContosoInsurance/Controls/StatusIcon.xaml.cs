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
    public sealed partial class StatusIcon : UserControl
    {
        public StatusIcon()
        {
            InitializeComponent();
        }

        public StatusType Type
        {
            get => (StatusType)GetValue(TypeProperty);
            set => SetValue(TypeProperty, value);
        }

        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register("Type", typeof(StatusType), typeof(StatusIcon), 
                new PropertyMetadata(StatusType.Unspecified, (d, e) =>
                {
                    var self = (StatusIcon)d;
                    var type = (StatusType)e.NewValue;

                    switch (type)
                    {
                        case StatusType.Available:
                            self.FindName(nameof(AvailableIcon));
                            break;
                        case StatusType.Busy:
                            self.FindName(nameof(BusyIcon));
                            break;
                    }
                }));
    }
}