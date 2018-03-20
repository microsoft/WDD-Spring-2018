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
    public sealed partial class GenderIcon : UserControl
    {
        public GenderIcon()
        {
            InitializeComponent();
        }

        public GenderType Type
        {
            get => (GenderType)GetValue(TypeProperty);
            set => SetValue(TypeProperty, value);
        }

        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register("Type", typeof(GenderType), typeof(GenderIcon), 
                new PropertyMetadata(GenderType.Unspecified, (d, e) =>
                {
                    var self = (GenderIcon)d;
                    var type = (GenderType)e.NewValue;

                    switch (type)
                    {
                        case GenderType.Female:
                            self.FindName(nameof(FemaleIcon));
                            break;
                        case GenderType.Male:
                            self.FindName(nameof(MaleIcon));
                            break;
                    }
                }));
    }
}