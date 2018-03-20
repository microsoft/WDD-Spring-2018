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

namespace ContosoInsurance.Models
{
    public class Adjuster
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public double Distance { get; set; }
        public GenderType Gender { get; set; }
        public StatusType Status { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public Adjuster(string first, double distance, GenderType type, StatusType status = StatusType.Available)
        {
            FirstName = first;
            Distance = distance;
            Gender = type;
            Status = status;
        }
    }
}