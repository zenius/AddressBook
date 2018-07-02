using System;
//using Microsoft.SharePoint.Client; 

namespace AddressBook
{
    class Contact
    {   
        public int Id { get; set;  }
        public string Name { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; } 
        public string Department { get; set; }
        public string MaritalStatus { get; set; }
        public double Salary { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool Happy { get; set; }
        public string Company { get; set; }
        public string SiteMember { get; set; }
        public string WebPageUrl { get; set; }

        public Contact() { }

        public Contact( 
                        int Id, 
                        string Name, 
                        string MobileNumber, 
                        string Email, 
                        string Address,
                        string Department,
                        string MaritalStatus, 
                        double Salary,
                        DateTime DateOfBirth,
                        bool Happy,
                        string Company, 
                        string SiteMember,
                        string WebPageUrl
                      
        )
        {
            this.Id = Id;
            this.Name = Name;
            this.MobileNumber = MobileNumber;
            this.Email = Email;
            this.Address = Address;
            this.Department = Department;
            this.MaritalStatus = MaritalStatus;
            this.Salary = Salary;
            this.DateOfBirth = DateOfBirth;
            this.Happy = Happy;
            this.Company = Company;
            this.SiteMember = SiteMember;
            this.WebPageUrl = WebPageUrl;
        }
    }
}
