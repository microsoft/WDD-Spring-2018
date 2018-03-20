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

using Windows.UI.Xaml.Controls;

namespace ContosoInsurance.Pages
{
    public sealed partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();

            Loaded += (s, e) =>
            {
                // Just some dummy data for the Telerik chart.
                BarChart.DataContext = new double[] 
                {
                    28, 31, 35, 32, 27, 33, 47, 45, 43, 55,
                    47, 40, 32, 28, 33, 39, 47, 45, 43, 55,
                    56, 58, 61, 44, 37, 33, 28, 40, 43, 47,
                    48, 50, 54, 59, 58, 55, 49, 44, 43, 44,
                    53, 60, 59, 56, 54, 48, 58, 49, 44, 40,
                    50, 58, 55, 58, 56, 37, 44, 37, 30, 28,
                    33, 37, 60, 58, 44, 20, 22, 24, 29, 37
                };
            };
        }
    }
}
