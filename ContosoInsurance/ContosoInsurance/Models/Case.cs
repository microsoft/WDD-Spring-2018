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
    public class Case
    {
        public string Id { get; set; }
        public string AvatarUri { get; set; }
        public string Date { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Phone { get; set; }
        public string DriversLicense { get; set; }
        public string PlateNumber { get; set; }
        public CaseType Type { get; set; }
        public bool IsNew { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public bool IsExisting => !IsNew;

        public Case(string id, string uri, string date, string first, string last, string address1, string address2, string phone, string license,
            string plate, CaseType type, bool isNew = false)
        {
            Id = id;
            AvatarUri = uri;
            Date = date;
            FirstName = first;
            LastName = last;
            Address1 = address1;
            Address2 = address2;
            Phone = phone;
            DriversLicense = license;
            PlateNumber = plate;
            Type = type;
            IsNew = isNew;
        }
    }
}